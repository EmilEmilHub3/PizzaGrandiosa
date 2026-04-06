using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Text;

namespace PizzaModels.Models
{
    [Table("User")]
    public class User : EntityBase
    {
        ////[Required]
        //public string Id { get; set; } = $"user:{Guid.NewGuid().ToString()}";

        [Required]
        public String? UserName { get; set; } = String.Empty;

        public String? Name { get; set; } = String.Empty;

        public bool IsAuthorized { get; set; } = false;

        public string? Email { get; set; } = String.Empty;

        public string? Password { get; set; } = String.Empty;

        public bool EmailConfirmed { get; set; } = false;

        public int rating { get; set; } = 0;

        public int? CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        /*
        /// <summary>
        /// CDefault constructor for ORM
        /// </summary>
        private User()
        {
            UserName = String.Empty;
            Name = String.Empty;

        }
        

        public User(string? userName, string? name, bool isAuthorized, string? email, string? password, bool emailConfirmed, int rating)
        {
            UserName = userName;
            Name = name;
            IsAuthorized = isAuthorized;
            Email = email;
            Password = password;
            EmailConfirmed = emailConfirmed;
            this.rating = rating;

            UpdateLastModified();
        }
        */


        public User Update(User userUpdateInfo)
        {
            this.Name = userUpdateInfo.Name;
            this.Email = userUpdateInfo.Email;
            this.Password = userUpdateInfo.Password;
            this.rating = userUpdateInfo.rating;
            this.CustomerId = userUpdateInfo.CustomerId;

            UpdateLastModified();
            return this;
        }

        public static User Create(string name, string userName)
        {
            ValidateInputs(name, userName);
            return new User { Name = name, UserName = userName };
        }

        private static void ValidateInputs(string name, string userName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.", nameof(name));

            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("UserName cannot be null or empty.", nameof(userName));

        }
    }

}
