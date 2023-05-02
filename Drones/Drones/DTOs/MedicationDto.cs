using Drones.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.DTOs
{
    public class AddMedicationDto
    {
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
    }
    public class GetMedicationDto
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
    }
}
