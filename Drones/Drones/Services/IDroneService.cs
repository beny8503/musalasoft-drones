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
        Task<IEnumerable<GetDroneDto>> GetAllDrones();
        /// <summary>
        /// Returns a List instance with all drones available for load medications.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<GetDroneDto>> GetAvailableDrones();
        /// <summary>
        /// Returns a List instance with all the meditacions of the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<IEnumerable<Medication>> GetDroneMedications(int id);
        /// <summary>
        /// Returns the drone instance for the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<GetDroneDto> GetDrone(int id);
        /// <summary>
        /// Returns the drone instance with its load for the given drone id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Drone> GetDroneDetails(int id);
        /// <summary>
        /// Updates the drone corresponding to the given id with the provided instane.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <param name="drone">Drone Instance</param>
        /// <returns></returns>
        Task<GetDroneDto> UpdDrone(int id, GetDroneDto drone);
        /// <summary>
        /// Adds a new drone. 
        /// </summary>
        /// <param name="drone">Drone instance</param>
        /// <returns></returns>
        Task<GetDroneDto> AddDrone(AddDroneDto drone);
        /// <summary>
        /// Deletes the drone corresponding to the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<bool> DelDrone(int id);
        /// <summary>
        /// Returns the drone's battery level.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<int> GetDroneBatteryLevel(int id);
        /// <summary>
        /// Loads the given drone with one medication at a time. If the medication does not exist it will be created.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <param name="medication">Medication</param>
        /// <returns></returns>
        Task<Drone> LoadDrone(int id, GetMedicationDto medication);
        /// <summary>
        /// Updates the drone' state fro the given drone id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        Task<GetDroneDto> ChangeDroneState(int id, DroneState state);


    }
}
