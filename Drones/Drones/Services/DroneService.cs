using Drones.Context;
using Drones.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Services
{
    public class DroneService : IDroneService
    {
        private readonly DronesContext _droneContext;

        public DroneService(DronesContext dronesContext)
        {
            _droneContext = dronesContext;
        }
        public async Task<Drone> AddDrone(Drone drone)
        {
            _droneContext.Drones.Add(drone);
            await _droneContext.SaveChangesAsync();

            return drone;
        }

        public async Task<bool> DelDrone(int id)
        {
            var drone = await _droneContext.Drones.FindAsync(id);
            if (drone == null)
            {
                return false;
            }

            _droneContext.Drones.Remove(drone);
            await _droneContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Drone>> GetAvailableDrones()
        {
            return await _droneContext.Drones.Where(x => x.BatteryCapacity > 25 && x.Medications.Sum(m => m.Weight) < x.WeightLimit).ToListAsync();
        }

        public async Task<Drone> GetDrone(int id)
        {
            var drone = await _droneContext.Drones.FindAsync(id);
            return drone;
        }

        public async Task<IEnumerable<Medication>> GetDroneMedications(int id)
        {
            var drone = await _droneContext.Drones.FindAsync(id);

            if (drone == null)
                return new List<Medication>();
            return drone.Medications;
        }
  
        public async Task<IEnumerable<Drone>> GetAllDrones()
        {
            return await _droneContext.Drones.ToListAsync();
        }

        public async Task<Drone> UpdDrone(int id, Drone drone)
        {
            if (id != drone.DroneId)
            {
                return null;
            }

            _droneContext.Entry(drone).State = EntityState.Modified;

            try
            {
                await _droneContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DroneExists(id))
                {
                    return drone;
                }
                else
                {
                    throw;
                }
            }

            return drone;
        }

        private bool DroneExists(int id)
        {
            return _droneContext.Drones.Any(e => e.DroneId == id);
        }
    }
}
