namespace AEChallenge.Application.DTOs
{
    public class UserLoginDto
    {
        private const int MaxUsernameLength = 50;
        private const int MinPasswordLength = 6;
        private const int MaxPasswordLength = 100;


        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public (bool IsValid, string ErrorMessage) Validate()
        {

            if (string.IsNullOrEmpty(Username))
            {
                return (false, "Username is required.");
            }
            if (Username.Length > MaxUsernameLength)
            {
                return (false, $"Username cannot exceed {MaxUsernameLength} characters.");
            }


            if (string.IsNullOrEmpty(Password))
            {
                return (false, "Password is required.");
            }
            if (Password.Length < MinPasswordLength || Password.Length > MaxPasswordLength)
            {
                return (false, $"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.");
            }




            return (true, string.Empty);
        }
    }
}