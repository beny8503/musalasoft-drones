using Drones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.DTOs
{
    public class AddDroneDto
    {
        public string SN { get; set; }

        public DroneModel Model { get; set; }

        public int WeightLimit { get; set; }

        public int BatteryCapacity { get; set; }

        public DroneState State { get; set; }
    }
    public class GetDroneDto
    {
        public int DroneId { get; set; }
        public string SN { get; set; }

        public DroneModel Model { get; set; }

        public int WeightLimit { get; set; }

        public int BatteryCapacity { get; set; }

        public DroneState State { get; set; }
    }
}
