namespace FitnessTracker.WebAPI.Entities.DTO
{
    public class UserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserDto() { }

        public UserDto( string _username, string _password)
        {
            Username = _username;
            Password = _password;
        }
    }
}
