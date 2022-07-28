using System.Security;
using Newtonsoft.Json;
using TeisterMask.Data.Models;

namespace TeisterMask.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    using Shared;

    public class ImportEmployeeDto
    {
        [Required]
        [MinLength(GlobalConstants.EmployeeUsernameMinLength)]
        [MaxLength(GlobalConstants.EmployeeUsernameMaxLength)]
        [RegularExpression(GlobalConstants.EmployeeUsernameReggex)]
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(GlobalConstants.EmployeePhoneReggex)]
        public string Phone { get; set; }


        public int[] Tasks { get; set; }
    }
}
