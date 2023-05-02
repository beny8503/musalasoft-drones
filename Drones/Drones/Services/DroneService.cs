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
        public async Task<ServiceResponse<GetDroneDto>> AddDrone(AddDroneDto drone)
        {
            var response = new ServiceResponse<GetDroneDto>();
            try
            {
                Drone newDrone = _mapper.Map<Drone>(drone);
                _droneContext.Drones.Add(newDrone);
                _droneContext.Entry(newDrone).State = EntityState.Added;
                await _droneContext.SaveChangesAsync();
                response.Data = _mapper.Map<GetDroneDto>(newDrone);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DelDrone(int id)
        {
            var response = new ServiceResponse<bool>();
            var drone = await _droneContext.Drones.FindAsync(id);
            if (drone == null)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "Drone not found.";
            }
            else
            {
                try
                {
                    _droneContext.Drones.Remove(drone);
                    await _droneContext.SaveChangesAsync();
                    response.Data = true;
                    response.Message = "Drone successfully removed.";
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = ex.Message;
                }
            }
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<GetDroneDto>>> GetAvailableDrones()
        {
            var response = new ServiceResponse<IEnumerable<GetDroneDto>>();
            response.Data = await _droneContext.Drones.Where(x => x.BatteryCapacity > 25 && x.Medications.Sum(m => m.Weight) < x.WeightLimit).Select(d => _mapper.Map<GetDroneDto>(d)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<FullDroneDto>> GetDrone(int id)
        {
            var response = new ServiceResponse<FullDroneDto>();
            response.Data = _mapper.Map<FullDroneDto>(await _droneContext.Drones.Include(d => d.Medications).AsNoTracking().FirstOrDefaultAsync(x => x.DroneId == id));
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<GetMedicationDto>>> GetDroneMedications(int id)
        {
            var response = new ServiceResponse<IEnumerable<GetMedicationDto>>();
            var drone = await _droneContext.Drones.Include(d => d.Medications).AsNoTracking().FirstOrDefaultAsync(x => x.DroneId == id);

            if (drone == null)
            {
                response.Success = false;
                response.Message = "Drone not found.";
            }
            else
            {
                response.Data = drone.Medications.Select(m => _mapper.Map<GetMedicationDto>(m));
            }
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<GetDroneDto>>> GetAllDrones()
        {
            var response = new ServiceResponse<IEnumerable<GetDroneDto>>();
            response.Data = await _droneContext.Drones.Select(d => _mapper.Map<GetDroneDto>(d)).AsNoTracking().ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<GetDroneDto>> UpdDrone(int id, GetDroneDto drone)
        {
            var response = new ServiceResponse<GetDroneDto>();
            if (id != drone.DroneId)
            {
                response.Success = false;
                response.Message = "Bad request.";
            }
            else
            {
                var updDrone = await _droneContext.Drones.FindAsync(drone.DroneId);
                if (updDrone == null)
                {
                    response.Success = false;
                    response.Message = "Drone not found.";
                }
                else
                {
                    updDrone.BatteryCapacity = drone.BatteryCapacity;
                    updDrone.Model = drone.Model;
                    updDrone.SN = drone.SN;
                    updDrone.State = drone.State;
                    _droneContext.Entry(updDrone).State = EntityState.Modified;
                    try
                    {
                        await _droneContext.SaveChangesAsync();
                        response.Data = _mapper.Map<GetDroneDto>(updDrone);
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (!DroneExists(id))
                        {
                            response.Success = false;
                            response.Message = "Drone not found in database.";
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
                                    response.Data = _mapper.Map<GetDroneDto>(entry);
                                }
                            }
                        }
                    }
                }
            }
            return response;
        }

        private bool DroneExists(int id)
        {
            return _droneContext.Drones.Any(e => e.DroneId == id);
        }

        public async Task<ServiceResponse<int>> GetDroneBatteryLevel(int id)
        {
            var response = new ServiceResponse<int>();
            var drone = await _droneContext.Drones.AsNoTracking().FirstOrDefaultAsync(x => x.DroneId == id);
            if (drone == null)
            {
                response.Success = false;
                response.Message = "Drone not found.";
            }
            else
            {
                response.Data = drone.BatteryCapacity;
            }
            return response;
        }

        public async Task<ServiceResponse<FullDroneDto>> LoadDrone(int id, GetMedicationDto medication)
        {
            var response = new ServiceResponse<FullDroneDto>();
            var drone = await _droneContext.Drones.Include(d => d.Medications).FirstOrDefaultAsync(x => x.DroneId == id);
            if (drone == null)
            {
                response.Success = false;
                response.Message = "Drone not found.";
            }
            else
            {
                //Check battery level and weght limit
                int loadedCapacity = drone.Medications.Sum(m => m.Weight);
                if (loadedCapacity + medication.Weight <= drone.WeightLimit && drone.BatteryCapacity >= 25)
                {
                    if (drone.State == DroneState.IDLE || drone.State == DroneState.LOADING)
                    {
                        var exMedication = drone.Medications.FirstOrDefault(m => m.MedicationId == medication.MedicationId);
                        if (exMedication == null)
                        {
                            var existMedication = await _droneContext.Medications.FirstOrDefaultAsync(m => m.Code == medication.Code);
                            if (existMedication == null)
                            {
                                var newMedication = _mapper.Map<AddMedicationDto>(medication);
                                existMedication = _mapper.Map<Medication>(newMedication);
                                _droneContext.Medications.Add(existMedication);
                            }

                            drone.Medications.Add(existMedication);
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
                            response.Success = false;
                            response.Message = "Medication could not be loaded because is already loaded.";
                        }
                    }
                    else
                    {
                        //State error
                        response.Success = false;
                        response.Message = string.Format("Medication could not be loaded while the drone is in '{0}' state.", drone.State.ToString());
                    }
                }
                else
                {
                    //Available error
                    response.Success = false;
                    response.Message = "The drone could not be loaded because it's at maximum capacity or battery level is under 25%.";
                }
                _droneContext.Entry(drone).State = EntityState.Modified;

                try
                {
                    await _droneContext.SaveChangesAsync();
                    response.Data = _mapper.Map<FullDroneDto>(drone);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!DroneExists(id))
                    {
                        response.Success = false;
                        response.Message = "Drone not found in database.";
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
                                response.Data = _mapper.Map<FullDroneDto>(entry.Entity);
                            }
                        }
                    }
                }
            }
            return response;
        }

        public async Task<ServiceResponse<GetDroneDto>> ChangeDroneState(int id, DroneState state)
        {
            var response = new ServiceResponse<GetDroneDto>();
            var drone = await _droneContext.Drones.Include(d => d.Medications).AsNoTracking().FirstOrDefaultAsync(x => x.DroneId == id);
            if (drone == null)
            {
                response.Success = false;
                response.Message = "Drone not found.";
            }
            else
            {
                if (drone.State != state)
                {
                    bool validState = false;
                    string tplErrorManual = "The drone state can not be changed manually when it is at {0} state.";
                    string tplError = "The drone state can not be changed to {0} while it is at {1} state.";
                    string errorMsg = "";
                    switch (drone.State)
                    {
                        case DroneState.IDLE:
                            errorMsg = string.Format(tplErrorManual, "IDLE");
                            break;
                        case DroneState.LOADING:
                            validState = state == DroneState.LOADED;
                            errorMsg = string.Format(tplError, state.ToString(), drone.State);
                            break;
                        case DroneState.LOADED:
                            validState = state == DroneState.DELIVERING;
                            errorMsg = string.Format(tplError, state.ToString(), drone.State);
                            break;
                        case DroneState.DELIVERING:
                            errorMsg = string.Format(tplErrorManual, "DELIVERING");
                            break;
                        case DroneState.DELIVERED:
                            validState = state == DroneState.RETURNING;
                            errorMsg = string.Format(tplError, state.ToString(), drone.State);
                            break;
                        case DroneState.RETURNING:
                            validState = state == DroneState.IDLE;
                            errorMsg = string.Format(tplError, state.ToString(), drone.State);
                            break;
                    }
                    if (validState)
                    {
                        drone.State = state;
                        _droneContext.Entry(drone).State = EntityState.Modified;
                        await _droneContext.SaveChangesAsync();
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = errorMsg;
                    }
                }
                response.Data = _mapper.Map<GetDroneDto>(drone);
            }
            return response;
        }

        public async Task<ServiceResponse<FullDroneDto>> UnloadDrone(int id)
        {
            var response = new ServiceResponse<FullDroneDto>();
            var drone = await _droneContext.Drones.Include(d => d.Medications).FirstOrDefaultAsync(x => x.DroneId == id);
            if (drone == null)
            {
                response.Success = false;
                response.Message = "Drone not found.";
            }
            else
            {
                _droneContext.Entry(drone).State = EntityState.Modified;
                drone.Medications.Clear();
                drone.State = DroneState.DELIVERED;
                try
                {
                    await _droneContext.SaveChangesAsync();
                    response.Data = _mapper.Map<FullDroneDto>(drone);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!DroneExists(id))
                    {
                        response.Success = false;
                        response.Message = "Drone not found in database.";
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
                                response.Data = _mapper.Map<FullDroneDto>(entry.Entity);
                            }
                        }
                    }
                }
            }
            return response;
        }
    }
}
