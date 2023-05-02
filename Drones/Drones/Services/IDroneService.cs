using Drones.DTOs;
using Drones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Services
{
    public interface IDroneService
    {
        /// <summary>
        /// Returns a List instance with all the drones.
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<GetDroneDto>>> GetAllDrones();
        /// <summary>
        /// Returns a List instance with all drones available for load medications.
        /// </summary>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<GetDroneDto>>> GetAvailableDrones();
        /// <summary>
        /// Returns a List instance with all the meditacions of the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<ServiceResponse<IEnumerable<GetMedicationDto>>> GetDroneMedications(int id);
        /// <summary>
        /// Returns the drone instance for the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<ServiceResponse<FullDroneDto>> GetDrone(int id);
        /// <summary>
        /// Updates the drone corresponding to the given id with the provided instane.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <param name="drone">Drone Instance</param>
        /// <returns></returns>
        Task<ServiceResponse<GetDroneDto>> UpdDrone(int id, GetDroneDto drone);
        /// <summary>
        /// Adds a new drone. 
        /// </summary>
        /// <param name="drone">Drone instance</param>
        /// <returns></returns>
        Task<ServiceResponse<GetDroneDto>> AddDrone(AddDroneDto drone);
        /// <summary>
        /// Deletes the drone corresponding to the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<ServiceResponse<bool>> DelDrone(int id);
        /// <summary>
        /// Returns the drone's battery level.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<ServiceResponse<int>> GetDroneBatteryLevel(int id);
        /// <summary>
        /// Loads the given drone with one medication at a time. If the medication does not exist it will be created.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <param name="medication">Medication</param>
        /// <returns></returns>
        Task<ServiceResponse<FullDroneDto>> LoadDrone(int id, GetMedicationDto medication);
        /// <summary>
        /// Unload all the medications of the given drone.
        /// </summary>
        /// <param name="id">Drone ID</param>
        /// <returns>The drone with its load</returns>
        Task<ServiceResponse<FullDroneDto>> UnloadDrone(int id);
        /// <summary>
        /// Updates the drone state for the given drone id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<ServiceResponse<GetDroneDto>> ChangeDroneState(int id, DroneState state);


    }
}
