using Drones.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.DTOs
{
    public class AddDroneDto
    {
        [Required, MaxLength(100)]
        public string SN { get; set; }
        [Required, EnumDataType(typeof(DroneModel))]
        public DroneModel Model { get; set; }
        [Required, Range(0, 500)]
        public int WeightLimit { get; set; }
        [Required, Range(0, 100)]
        public int BatteryCapacity { get; set; }
        [Required, EnumDataType(typeof(DroneState))]
        public DroneState State { get; set; }
    }
    public class GetDroneDto
    {
        [Key]
        public int DroneId { get; set; }
        [Required, MaxLength(100)]
        public string SN { get; set; }
        [Required, EnumDataType(typeof(DroneModel))]
        public DroneModel Model { get; set; }
        [Required, Range(0, 500)]
        public int WeightLimit { get; set; }
        [Required, Range(0, 100)]
        public int BatteryCapacity { get; set; }
        [Required, EnumDataType(typeof(DroneState))]
        public DroneState State { get; set; }
    }
    public class FullDroneDto
    {
        [Key]
        public int DroneId { get; set; }
        [Required, MaxLength(100)]
        public string SN { get; set; }
        [Required, EnumDataType(typeof(DroneModel))]
        public DroneModel Model { get; set; }
        [Required, Range(0, 500)]
        public int WeightLimit { get; set; }
        [Required, Range(0, 100)]
        public int BatteryCapacity { get; set; }
        [Required, EnumDataType(typeof(DroneState))]
        public DroneState State { get; set; }

        public List<GetMedicationDto> Medications { get; set; }
    }
}
