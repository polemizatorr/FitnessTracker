namespace FitnessTracker.WebAPI.Entities.Models
{
    public class User : Entity
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PasswordSalt { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

/*        public IList<StrengthTraining> StrenghtTrainings { get; set; }
        public IList<AerobicTraining> AerobicTrainings { get; set; }*/

        public User(string userName, string email, string password, string firstName, string lastName) : base()
        {
            UserId = Guid.NewGuid();
            UserName = userName;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
/*            StrenghtTrainings = new List<StrengthTraining>();
            AerobicTrainings = new List<AerobicTraining>();*/

        }

        public User(string userName, string email, string password, string salt, string firstName, string lastName) : base()
        {
            UserId = Guid.NewGuid();
            UserName = userName;
            Email = email;
            Password = password;
            PasswordSalt = salt;
            FirstName = firstName;
            LastName = lastName;
        }

        public User() { }
    }

}
