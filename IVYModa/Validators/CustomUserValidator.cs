using Microsoft.AspNetCore.Identity;

namespace IVYModa.Validators
{
    public class CustomUserValidator<User> : UserValidator<User> where User : class
    {
        public CustomUserValidator(IdentityErrorDescriber errors) : base(errors)
        {
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            var result = await base.ValidateAsync(manager, user);
            var errors = new List<IdentityError>(result.Errors);

            // Remove the error related to duplicate UserName
            errors.RemoveAll(e => e.Code == nameof(IdentityErrorDescriber.DuplicateUserName));

            // Return the result without the duplicate UserName error
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }
    }
}
