using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.DTOs
{
    public class AddMedicationDto
    {
        public string Name { get; set; }
        
        public int Weight { get; set; }
        
        public string Code { get; set; }
        /// <summary>
        /// Store the image as a base64string
        /// </summary>
        public string Image { get; set; }
    }
    public class GetMedicationDto
    {
        public int MedicationId { get; set; }
        public string Name { get; set; }

        public int Weight { get; set; }

        public string Code { get; set; }
        /// <summary>
        /// Store the image as a base64string
        /// </summary>
        public string Image { get; set; }
    }
}
