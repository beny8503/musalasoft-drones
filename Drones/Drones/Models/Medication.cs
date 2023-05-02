using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Drones.Models
{
    public class NameAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\-_]+$");
            if (rg.IsMatch(value.ToString()))
                return ValidationResult.Success;
            return new ValidationResult(string.Format("The value {0} is not valid, only letters, numbers, hyphen and underscore are allowed.", value.ToString()));
        }
    }

    public class CodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Regex rg = new Regex(@"^[A-Z0-9_]+$");
            if (rg.IsMatch(value.ToString()))
                return ValidationResult.Success;
            return new ValidationResult(string.Format("The value {0} is not valid, only upper case letters, underscore and numbers are allowed.", value.ToString()));
        }
    }

    public class RequiredGreaterThanZero : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int i;
            if (value != null && int.TryParse(value.ToString(), out i) && i > 0)
                return ValidationResult.Success;
            return new ValidationResult("The value is required and must be greater than 0.");
        }
    }

    [Table("Medications")]
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }
        [Required, Name]
        public string Name { get; set; }
        [RequiredGreaterThanZero]
        public int Weight { get; set; }
        [Required, Code]
        public string Code { get; set; }
        /// <summary>
        /// Store the image as a base64string
        /// </summary>
        public string Image { get; set; }

        public List<Drone> Drone { get; set; }
    }
}
