using AEChallenge.Application.Constants;

namespace AEChallenge.Application.DTOs.Request
{
    public class UserCreateDto
    {
        private const int MinUserNameLength = 5;
        private const int MaxUserNameLength = 100;
        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 50;
        private const int MaxUserRoleLength = 50;

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string UserRole { get; set; } = "User";

        public (bool IsValid, string ErrorMessage) Validate()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                return (false, "Username is required.");
            }
            if (UserName.Length < MinUserNameLength || UserName.Length > MaxUserNameLength)
            {
                return (false, $"Username must be between {MinUserNameLength} and {MaxUserNameLength} characters.");
            }

            if (string.IsNullOrEmpty(Password))
            {
                return (false, "Password is required.");
            }
            if (Password.Length < MinPasswordLength || Password.Length > MaxPasswordLength)
            {
                return (false, $"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.");
            }

            if (string.IsNullOrEmpty(UserRole))
            {
                return (false, "User role is required.");
            }
            if (UserRole.Length > MaxUserRoleLength)
            {
                return (false, $"User role cannot exceed {MaxUserRoleLength} characters.");
            }

            var allowedRoles = new[] { UserRoles.Administrator, UserRoles.Finance, UserRoles.Operations, UserRoles.RegularUser };
            bool isRoleValid = false;
            foreach (var role in allowedRoles)
            {
                if (string.Equals(role, UserRole, StringComparison.InvariantCultureIgnoreCase))
                {
                    isRoleValid = true;
                    break;
                }
            }

            if (!isRoleValid)
            {
                return (false, $"Invalid UserRole. Allowed roles are: {string.Join(", ", allowedRoles)}");
            }

            if (UserName.Equals(Password, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "Username and Password cannot be the same.");
            }

            return (true, string.Empty);
        }
    }
}
