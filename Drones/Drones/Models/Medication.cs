using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drones.Models
{

    [Table("Medications")]
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        public string Code { get; set; }
        /// <summary>
        /// Store the image as a base64string
        /// </summary>
        public string Image { get; set; }

        public List<Drone> Drone { get; set; }
    }
}
