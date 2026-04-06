//using PizzaGrandiosa.Models;

using PizzaModels.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaWebAPI.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        public String? UserName { get; set; } = String.Empty;

        public String? Name { get; set; } = String.Empty;

        public bool IsAuthorized { get; set; }

        public string? Email { get; set; }

        public int? CustomerId { get; set; }

        public virtual Customer? Customer { get; set; }

        public UserDTO()
        { }

        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            UserName = user.UserName;
            IsAuthorized = user.IsAuthorized;
            CustomerId = user.CustomerId;
            Customer =  user.Customer;
        }

        public User GetAsUser()
        {
            return new User{ 
                Id = Id,
                Name = Name,
                UserName= UserName,
                IsAuthorized = IsAuthorized,
                Email= Email,
                Password = "",
                EmailConfirmed=false,
                rating = 0,
                CustomerId = CustomerId
            };
        }
    }
}
