namespace FitnessTracker.WebAPI.Entities.Models
{
    public class User : Entity
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }

        public User(string userName, string email, string password, string name, string surname, DateTime birthDate) : base()
        {
            UserId = new Guid();
            UserName = userName;
            Email = email;
            Password = password;
            Name = name;
            Surname = surname;
            BirthDate = birthDate;
        }
    }

}
