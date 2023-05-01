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
        Task<IEnumerable<Drone>> GetAllDrones();
        /// <summary>
        /// Returns a List instance with all drones available for load medications.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Drone>> GetAvailableDrones();
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
        Task<Drone> GetDrone(int id);
        /// <summary>
        /// Updates the drone corresponding to the given id with the provided instane.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <param name="drone">Drone Instance</param>
        /// <returns></returns>
        Task<Drone> UpdDrone(int id, Drone drone);
        /// <summary>
        /// Adds a new drone. 
        /// </summary>
        /// <param name="drone">Drone instance</param>
        /// <returns></returns>
        Task<Drone> AddDrone(Drone drone);
        /// <summary>
        /// Deletes the drone corresponding to the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        Task<bool> DelDrone(int id);
    }
}
