using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Drones.Context;
using Drones.Models;
using Drones.Services;
using Drones.DTOs;

namespace Drones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DronesController : ControllerBase
    {
        private readonly IDroneService _droneService;
        public DronesController(IDroneService droneService)
        {
            _droneService = droneService;
        }
        /// <summary>
        /// Return the list of the drones registered in the fleet.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetDroneDto>>>> GetDrones()
        {
            return Ok(await _droneService.GetAllDrones());
        }
        /// <summary>
        /// Return the list of drones available for load medication items.
        /// </summary>
        /// <returns></returns>
        [HttpGet("Available")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetDroneDto>>>> GetAvailableDrones()
        {
            return Ok(await _droneService.GetAvailableDrones());
        }
        /// <summary>
        /// Return the list of medication items loaded into the given drone.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Medications")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetMedicationDto>>>> GetDroneMedications(int id)
        {
            var response = await _droneService.GetDroneMedications(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Returns the drone details for the diven drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<FullDroneDto>>> GetDrone(int id)
        {
            var response = await _droneService.GetDrone(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        /// <summary>
        /// Returns the battery capacity for the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns></returns>
        [HttpGet("{id}/Battery")]
        public async Task<ActionResult<ServiceResponse<int>>> GetDroneBatteryLevel(int id)
        {
            var response = await _droneService.GetDroneBatteryLevel(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        /// <summary>
        /// Updates the drone data for the given drone id. It does not update the load info.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <param name="drone">Drone Data</param>
        /// <returns>Returns a Drone instance</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetDroneDto>>> PutDrone(int id, GetDroneDto drone)
        {
            if (id != drone.DroneId)
            {
                return BadRequest();
            }

            var respDrone = await _droneService.UpdDrone(id, drone);
            if (respDrone.Data == null)
            {
                return NotFound(respDrone);
            }

            return Ok(respDrone);
        }
        /// <summary>
        /// Add a new drone to the fleet.
        /// </summary>
        /// <param name="drone">Drone data</param>
        /// <returns>Returns the newly created drone</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetDroneDto>>> PostDrone(AddDroneDto drone)
        {
            var response = await _droneService.AddDrone(drone);

            return CreatedAtAction("GetDrone", new { id = response.Data.DroneId }, response);
        }
        /// <summary>
        /// Loads a medication item into the drone for the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <param name="medication">Medication Item</param>
        /// <returns>Drone details</returns>
        [HttpPost("{id}/Medication")]
        public async Task<ActionResult<ServiceResponse<FullDroneDto>>> LoadMedication(int id, GetMedicationDto medication)
        {
            var response = await _droneService.LoadDrone(id, medication);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        /// <summary>
        /// Unload all medication items for the given drone id.
        /// </summary>
        /// <param name="id">Drone Id</param>
        /// <returns>Return the Drone instance with its load.</returns>
        [HttpDelete("{id}/Medications")]
        public async Task<ActionResult<ServiceResponse<FullDroneDto>>> UnloadMedications(int id)
        {
            var response = await _droneService.UnloadDrone(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Remove a drone from the fleet for the given drone id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteDrone(int id)
        {
            var result = await _droneService.DelDrone(id);
            if (!result.Data)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpPut("{id}/State")]
        public async Task<ActionResult<ServiceResponse<GetDroneDto>>> ChangeDroneStatus(int id, DroneState state)
        {
            var response = await _droneService.ChangeDroneState(id, state);
            if (response.Data==null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
