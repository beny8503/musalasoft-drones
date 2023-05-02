using AutoMapper;
using Drones.Context;
using Drones.DTOs;
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
        private readonly IMapper _mapper;

        public DroneService(DronesContext dronesContext, IMapper mapper)
        {
            _droneContext = dronesContext;
            _mapper = mapper;
        }
        public async Task<GetDroneDto> AddDrone(AddDroneDto drone)
        {
            Drone newDrone = _mapper.Map<Drone>(drone);
            await _droneContext.Drones.AddAsync(newDrone);
            await _droneContext.SaveChangesAsync();

            return _mapper.Map<GetDroneDto>(newDrone);
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

        public async Task<IEnumerable<GetDroneDto>> GetAvailableDrones()
        {
            return await _droneContext.Drones.Where(x => x.BatteryCapacity > 25 && x.Medications.Sum(m => m.Weight) < x.WeightLimit).Select(d => _mapper.Map<GetDroneDto>(d)).ToListAsync();
        }

        public async Task<GetDroneDto> GetDrone(int id)
        {
            var drone = _mapper.Map<GetDroneDto>(await _droneContext.Drones.FindAsync(id));
            return drone;
        }

        public async Task<IEnumerable<Medication>> GetDroneMedications(int id)
        {
            var drone = await _droneContext.Drones.FindAsync(id);

            if (drone == null)
                return new List<Medication>();
            return drone.Medications;
        }

        public async Task<IEnumerable<GetDroneDto>> GetAllDrones()
        {
            return await _droneContext.Drones.Select(d => _mapper.Map<GetDroneDto>(d)).ToListAsync();
        }

        public async Task<GetDroneDto> UpdDrone(int id, GetDroneDto drone)
        {
            if (id != drone.DroneId)
            {
                return null;
            }

            var updDrone = await _droneContext.Drones.FindAsync(drone.DroneId);
            if (updDrone == null)
            {
                return null;
            }
            updDrone.BatteryCapacity = drone.BatteryCapacity;
            updDrone.Model = drone.Model;
            updDrone.SN = drone.SN;
            updDrone.State = drone.State;

            _droneContext.Entry(updDrone).State = EntityState.Modified;

            try
            {
                await _droneContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DroneExists(id))
                {
                    return null;
                }
                else
                {
                    foreach (var entry in ex.Entries)
                    {
                        //In case of concurrency conflict, database data will prevale
                        if (entry.Entity is Drone)
                        {
                            var databaseValues = await entry.GetDatabaseValuesAsync();
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                    }
                }

            }

            return _mapper.Map<GetDroneDto>(updDrone);
        }

        private bool DroneExists(int id)
        {
            return _droneContext.Drones.Any(e => e.DroneId == id);
        }

        public async Task<Drone> GetDroneDetails(int id)
        {
            return await _droneContext.Drones.Include(d => d.Medications).AsNoTracking().FirstOrDefaultAsync(x => x.DroneId == id);
        }

        public async Task<int> GetDroneBatteryLevel(int id)
        {
            var drone = await _droneContext.Drones.AsNoTracking().FirstOrDefaultAsync(x => x.DroneId == id);
            if (drone == null)
            {
                return -1;
            }

            return drone.BatteryCapacity;
        }

        public async Task<Drone> LoadDrone(int id, GetMedicationDto medication)
        {            
            var drone = await _droneContext.Drones.Include(d => d.Medications).AsNoTracking().FirstOrDefaultAsync(x => x.DroneId == id);
            if (drone == null)
            {
                return null;
            }
            //Check battery level and weght limit
            int loadedCapacity = drone.Medications.Sum(m => m.Weight);
            if(loadedCapacity + medication.Weight <= drone.WeightLimit && drone.BatteryCapacity >= 25)
            {
                if (drone.State == DroneState.IDLE || drone.State == DroneState.LOADING)
                {
                    var exMedication = drone.Medications.FirstOrDefault(m => m.MedicationId == medication.MedicationId);
                    if (exMedication == null)
                    {
                        drone.Medications.Add(_mapper.Map<Medication>(medication));
                        if (loadedCapacity + medication.Weight == drone.WeightLimit)
                        {
                            drone.State = DroneState.LOADED;
                        }
                        else
                        {
                            drone.State = DroneState.LOADING;
                        }
                    }
                    else
                    {
                        //Medication already loaded
                    }
                }
                else
                {
                    //State error
                }
            }
            else
            {
                //Available error
            }
            _droneContext.Entry(drone).State = EntityState.Modified;

            try
            {
                await _droneContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!DroneExists(id))
                {
                    return null;
                }
                else
                {
                    foreach (var entry in ex.Entries)
                    {
                        //In case of concurrency conflict, database data will prevale
                        if (entry.Entity is Drone)
                        {
                            var databaseValues = await entry.GetDatabaseValuesAsync();
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                    }
                }
            }

            return drone;
        }

        public async Task<GetDroneDto> ChangeDroneState(int id, DroneState state)
        {
            var drone = await _droneContext.Drones.Include(d => d.Medications).AsNoTracking().FirstOrDefaultAsync(x => x.DroneId == id);
            if (drone == null)
            {
                return null;
            }

            drone.State = state;

            return _mapper.Map<GetDroneDto>(drone);
        }
    }
}
