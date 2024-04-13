namespace FitnessTracker.WebAPI.Entities.DTO
{
    public class RegisterUserDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public RegisterUserDTO(string userName, string email, string password, string firstName, string lastName)
        {
            UserName = userName ?? string.Empty;
            Email = email ?? string.Empty;
            Password = password ?? string.Empty;
            FirstName = firstName ?? string.Empty;
            LastName = lastName ?? string.Empty;
        }

        public bool isvalidUserData()
        {
            if (this == null || this.Email.Equals(string.Empty) || this.UserName.Equals(string.Empty) || this.Password.Equals(string.Empty)
                || this.FirstName.Equals(string.Empty) || this.LastName.Equals(string.Empty)) return false;

            return true;
        }
    }
}
