using IVYModa.Areas.Identity.Models.Account;
using IVYModa.EF;
using IVYModa.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using IVYModa.Models;
using Microsoft.EntityFrameworkCore;
using IVYModa.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using UAParser;
using System.Net;

namespace IVYModa.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/customer/")]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly SendMailService _sendMailService;

        public AccountController
            (
                AppDbContext context, 
                UserManager<AppUser> userManager, 
                SignInManager<AppUser> signInManager, 
                SendMailService sendMailService
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _sendMailService = sendMailService;
        }

        [HttpGet("login")]
        public IActionResult Login(string? returnUrl)
        {
            if(User.Identity!.IsAuthenticated) 
                return RedirectToAction(nameof(HomeController.Index), "Home");

            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl)
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Lỗi không xác định");
                return View(model);
            }

            // Tìm user theo email
            var user = await _userManager.FindByEmailAsync(model.EmailOrPhoneNumber);
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.EmailOrPhoneNumber);
            if (user is null)
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == model.EmailOrPhoneNumber);

                if (user is null)
                {
                    ModelState.AddModelError(string.Empty, "Tài khoản chưa được đăng ký");
                    return View(model);
                }
            }

            if(!user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng xác thực email ở hòm thư");
                return View(model);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Thông tin đăng nhập chưa chính xác");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, model.RememberMe, lockoutOnFailure: false);
            if(result.Succeeded)
            {
                // Lưu thông tin device đăng nhập
                await SetInfoLogin(HttpContext, user.Id);

                return LocalRedirect(returnUrl);
            }

            if(!result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Lỗi không xác định");

            return View(model);
        }

        [HttpGet("register")]
        public IActionResult Register(string? returnUrl)
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterVM model, string returnUrl)
        {
            if (User.Identity!.IsAuthenticated)
                return RedirectToAction(nameof(HomeController.Index), "Home");

            returnUrl ??= Url.Content("~/");
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Lỗi không xác định");
                return View(model);
            }

            var userDb = _context.Users.FirstOrDefault(u => u.PhoneNumber == model.PhoneNumber);
            if (userDb != null)
            {
                ModelState.AddModelError(string.Empty, "Số điện thoại đã được sử dụng");
                return View(model);
            }

            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = ConvertString.RemoveDiacritics(model.FirstName + " " + model.LastName),
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                BirthDay = ConvertDate(model.BirthDay),
                Gender = model.Gender
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if(error.Code == nameof(IdentityErrorDescriber.DuplicateEmail))
                        error.Description = "Địa chỉ email đã tồn tại";
                    if (error.Code == nameof(IdentityErrorDescriber.DuplicateUserName))
                        error.Description = "Tên người dùng đã tồn tại";

                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            // Tạo location
            var location = new Location
            {
                UserName = model.FirstName + " " + model.LastName,
                PhoneNumber = model.PhoneNumber,
                CityCode = model.CityCode,
                DistrictCode = model.DistrictCode,
                WardCode = model.WardCode,
                Address = model.Address,
                Default = true,
                IdUser = user.Id
            };

            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();

            // Phát sinh token để xác nhận email
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.ActionLink(
                    action: nameof(ConfirmEmail),
                    values:
                        new
                        {
                            area = "Identity",
                            userId = user.Id,
                            code = code
                        },
                    protocol: Request.Scheme);

            await _sendMailService.SendMailAsync(model.Email,
                "Xác nhận địa chỉ email",
                @$"Bạn đã đăng ký tài khoản trên IVYModa, 
                       hãy <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>bấm vào đây</a> 
                       để kích hoạt tài khoản.");

            TempData["Message"] = "Hãy kiểm tra hòm thư để biết cách xác thực tài khoản";

            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            TempData["Message"] = "Có lỗi xảy ra, vui lòng thao tác lại";
            if (userId == null || code == null) return View("Register");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return View("Register");

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                TempData["Message"] = "Xác thực thành công, hãy đăng nhập vào tài khoản";
                return View("Login");
            }
                
            return View("Register");
        }

        [HttpGet("location")]
        public JsonResult GetLocation()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "location.json");
            string jsonData = System.IO.File.ReadAllText(filePath);

            return Json(jsonData);
        }

        [NonAction]
        public string ConvertDate(string dateInput)
        {
            DateTime date;
            if (DateTime.TryParseExact(dateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return date.ToString("dd/MM/yyyy");
            }
            else
            {
                throw new ArgumentException("Invalid date format. Expected format is yyyy-MM-dd.");
            }
        }

        [NonAction]
        public async Task SetInfoLogin(HttpContext context, string idUser)
        {
            // Lấy device
            var userAgent = context.Request.Headers["User-Agent"];
            var uaParser = Parser.GetDefault();
            ClientInfo clientInfo = uaParser.Parse(userAgent);
            var device = clientInfo.OS.Family;

            var host = context.Request.Headers.Host;

            // Lấy Ip
            IPAddress? ipAddress = context.Connection.RemoteIpAddress;

            if (ipAddress != null)
            {
                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    ipAddress = Dns.GetHostEntry(ipAddress).AddressList
                        .First(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                }
            }

            string ipv4 = ipAddress?.ToString() ?? "1.1.1.1";

            // Thời gian đăng nhập
            var loginTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var loginLog = new LoginLog
            {
                Device = device,
                Software = host!,
                LoginStyle = "Mặc định",
                Address = string.Empty,
                IP = ipv4,
                LoginTime = loginTime,
                IdUser = idUser
            };

            _context.LoginLogs.Add(loginLog);
            await _context.SaveChangesAsync();
        }
    }
}
