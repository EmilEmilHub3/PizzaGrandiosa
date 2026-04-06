
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaGrandiosa.Models
{/*
    
    public class Userbackup 
    {
        ////[Required]
        //public string Id { get; set; } = $"user:{Guid.NewGuid().ToString()}";

        [Required]
        public String? UserName { get; set; } = String.Empty;

        [Required]
        public String? Name { get; set; } = String.Empty;

        [Required]
        public bool IsAuthorized { get; set; } = false;

        [Required]
        public string? Email { get; set; } = String.Empty;

        [Required]
        public string? Password { get; set; } = String.Empty;

        [Required]
        public bool EmailConfirmed { get; set; } = false;

        [Required]
        public int rating { get; set; } = 0;

        /// <summary>
        /// CDefault constructor for ORM
        /// </summary>
        private Userbackup()
        {
            UserName = String.Empty;
            Name = String.Empty;

        }

        public Userbackup(string? userName, string? name, bool isAuthorized, string? email, string? password, bool emailConfirmed, int rating)
        {
            UserName = userName;
            Name = name;
            IsAuthorized = isAuthorized;
            Email = email;
            Password = password;
            EmailConfirmed = emailConfirmed;
            this.rating = rating;

            //UpdateLastModified();
        }

        public static Userbackup Create(string name, string userName)
        {
            ValidateInputs(name, userName);
            return new Userbackup { Name=name, UserName=userName };
        }

        private static void ValidateInputs(string name, string userName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("UserName cannot be null or empty.", nameof(userName));

        }
    }*/
}
