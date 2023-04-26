using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drones.Models
{
    public enum DroneState { IDLE, LOADING, LOADED, DELIVERING, DELIVERED, RETURNING }
    public enum DroneModel { Lightweight, Middleweight, Cruiserweight, Heavyweight }
    [Table("Drones")]
    public class Drone
    {
        [Key]
        public int DroneId { get; set; }
        [MaxLength(100)]
        public string SN { get; set; }
        [Required]
        public DroneModel Model { get; set; }
        [Required, Range(0, 500)]
        public int WeightLimit { get; set; }
        [Required, Range(0, 100)]
        public int BatteryCapacity { get; set; }
        [Required]
        public DroneState State { get; set; }

        public List<Medication> Medications { get; set; }
    }
}
