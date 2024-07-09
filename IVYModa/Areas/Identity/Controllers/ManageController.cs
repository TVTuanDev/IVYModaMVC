using AutoMapper;
using UAParser;
using IVYModa.Areas.Identity.Models.Manage;
using IVYModa.EF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace IVYModa.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Route("/customer/")]
    [Authorize]
    public class ManageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public ManageController
            (
                AppDbContext context,
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager,
                IMapper mapper
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet("info")]
        public async Task<IActionResult> Infomation()
        {
            var user = await _userManager.GetUserAsync(User);

            var userVM = _mapper.Map<UserVM>(user);

            return View(userVM);
        }

        [HttpPost("info")]
        public async Task<IActionResult> Infomation(UserVM model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy người dùng");
                return View(model);
            }

            // Nếu email mới đã được đăng ký rồi báo lỗi
            var userDb = await _userManager.FindByEmailAsync(model.Email);
            if (userDb != null)
            {
                ModelState.AddModelError(string.Empty, "Email đã được sử dụng");
                return View(model);
            }

            await _userManager.SetEmailAsync(user, model.Email);
            user.Gender = model.Gender;
            user.EmailConfirmed = true;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Cập nhật thành công";

            return View(model);
        }

        [HttpPost("change-password")]
        public async Task<JsonResult> ChangePassword(ChangePasswordVM model)
        {
            if (model.NewPassword.Length < 7 || model.NewPassword.Length > 32)
                return Json(new { Status = "error", Message = "Mật khẩu mới phải có độ dài từ 7 đến 32 ký tự" });

            if (model.NewPassword != model.ConfirmNewPassword)
                return Json(new { Status = "error", Message = "Mật khẩu nhập lại không chính xác" });

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Json(new { Status = "error", Message = "Không tìm thấy người dùng" });

            if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
                return Json(new { Status = "error", Message = "Mật khẩu cũ không đúng" });

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
                return Json(new { Status = "error", Message = "Lỗi không xác định" });

            return Json(new { Status = "success", Message = "Đổi mật khẩu thành công" });
        }

        [HttpGet("login-log")]
        public async Task<IActionResult> LoginLog()
        {
            var user = await _userManager.GetUserAsync(User);
            if(user is null)
            {
                TempData["Message"] = "Không tìm thấy thông tin người dùng";
                return View();
            }

            var loginLogs = _context.LoginLogs.Where(ll => ll.IdUser == user.Id).ToList();    

            return View(loginLogs);
        }

        [HttpGet("order-list")]
        public IActionResult OrderList()
        {
            return View();
        }

        [HttpGet("address-list")]
        public async Task<IActionResult> AddressList()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                TempData["Message"] = "Không tìm thấy thông tin người dùng";
                return View();
            }

            var addressList = _context.Locations.Where(l => l.IdUser == user.Id)
                                                .OrderByDescending(l => l.Default)
                                                .ToList();

            return View(addressList);
        }

        [HttpPost("create-address")]
        public async Task<JsonResult> CreateLocation(LocationVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                TempData["Message"] = "error: Không tìm thấy thông tin người dùng";
                return Json(new { Status = "success" });
            }

            if (model.Default)
            {
                var locationDb = _context.Locations.FirstOrDefault(l =>  l.IdUser == user.Id && l.Default);
                if (locationDb != null)
                {
                    locationDb.Default = false;
                    _context.Locations.Update(locationDb);
                }
            }

            var location = _mapper.Map<Location>(model);
            location.IdUser = user.Id;
            _context.Locations.Update(location);
            await _context.SaveChangesAsync();

            TempData["Message"] = "success: Thêm địa chỉ thành công";

            return Json(new { Status = "success" });
        }

        [HttpPut("update-address")]
        public async Task<JsonResult> UpdateLocation(LocationVM model, int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                TempData["Message"] = "error: Không tìm thấy địa chỉ";
                return Json(new { Status = "success" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                TempData["Message"] = "error: Không tìm thấy thông tin người dùng";
                return Json(new { Status = "success" });
            }

            if (location.IdUser != user.Id)
            {
                TempData["Message"] = "error: Không thể sửa địa chỉ không phải của bạn";
                return Json(new { Status = "success" });
            }

            if (model.Default)
            {
                var locationDb = _context.Locations.FirstOrDefault(l => l.IdUser == user.Id && l.Default);
                if (locationDb != null)
                {
                    locationDb.Default = false;
                    _context.Locations.Update(locationDb);
                }
            }

            location = _mapper.Map<Location>(model);
            location.IdUser = user.Id;

            _context.Locations.Update(location);
            await _context.SaveChangesAsync();

            TempData["Message"] = "success: Sửa địa chỉ thành công";

            return Json(new { Status = "success" });
        }

        [HttpDelete("delete-address")]
        public async Task<JsonResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                TempData["Message"] = "error: Không tìm thấy địa chỉ cần xóa";
                return Json(new { Status = "success" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                TempData["Message"] = "error: Không tìm thấy thông tin người dùng";
                return Json(new { Status = "success" });
            }

            if (location.IdUser !=  user.Id)
            {
                TempData["Message"] = "error: Không thể xóa địa chỉ không phải của bạn";
                return Json(new { Status = "success" });
            }

            _context.Remove(location);
            await _context.SaveChangesAsync();

            TempData["Message"] = "success: Xóa địa chỉ thành công";

            return Json(new { Status = "success" });
        }

        [HttpPost("default-location")]
        public async Task<JsonResult> SetDefaultLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                TempData["Message"] = "error: Không tìm thấy địa chỉ";
                return Json(new { Status = "success" });
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                TempData["Message"] = "error: Không tìm thấy thông tin người dùng";
                return Json(new { Status = "success" });
            }

            if (location.IdUser != user.Id)
            {
                TempData["Message"] = "error: Không thể sửa địa chỉ không phải của bạn";
                return Json(new { Status = "success" });
            }

            var locationDb = await _context.Locations.FirstOrDefaultAsync(l => l.Default);
            if (locationDb != null)
            {
                locationDb.Default = false;
                _context.Locations.Update(locationDb);
            }

            location.Default = true;
            _context.Locations.Update(location);
            await _context.SaveChangesAsync();

            TempData["Message"] = "success: Sửa địa chỉ thành công";

            return Json(new { Status = "success" });
        }

        [HttpGet("view-products")]
        public IActionResult ViewProducts()
        {
            return View();
        }

        [HttpGet("like-products")]
        public IActionResult LikeProducts()
        {
            return View();
        }

        [HttpGet("question")]
        public IActionResult Question()
        {
            return View();
        }

        [HttpGet("add-question")]
        public IActionResult AddQuestion()
        {
            return View();
        }

        [HttpGet("oder-find")]
        public IActionResult OderFind()
        {
            return View();
        }
    }
}
