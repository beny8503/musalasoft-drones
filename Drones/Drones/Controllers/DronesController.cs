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

        // GET: api/Drones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDroneDto>>> GetDrones()
        {
            return Ok(await _droneService.GetAllDrones() );
        }

        [HttpGet("Available")]
        public async Task<ActionResult<IEnumerable<GetDroneDto>>> GetAvailableDrones()
        {
            return Ok(await _droneService.GetAvailableDrones());
        }

        [HttpGet("{id}/Medications")]
        public async Task<ActionResult<IEnumerable<Medication>>> GetDroneMedications(int id)
        {
            return Ok(await _droneService.GetDroneMedications(id));
        }

        // GET: api/Drones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDroneDto>> GetDrone(int id)
        {
            return Ok(await _droneService.GetDrone(id));
        }

        [HttpGet("{id}/Battery")]
        public async Task<ActionResult<int>> GetDroneBatteryLevel(int id)
        {
            return Ok(await _droneService.GetDroneBatteryLevel(id));
        }

        // PUT: api/Drones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<GetDroneDto>> PutDrone(int id, GetDroneDto drone)
        {
            if (id != drone.DroneId)
            {
                return BadRequest();
            }

            var respDrone = await _droneService.UpdDrone(id, drone);
            if (respDrone == null)
            {
                return NotFound();
            }

            return Ok(respDrone);
        }

        // POST: api/Drones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetDroneDto>> PostDrone(AddDroneDto drone)
        {
            GetDroneDto resultDrone = await _droneService.AddDrone(drone);

            return CreatedAtAction("GetDrone", new { id = resultDrone.DroneId }, resultDrone);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Drone>> PostMedication(int id, GetMedicationDto medication)
        {
            Drone resultDrone = await _droneService.LoadDrone(id, medication);

            return Ok(resultDrone);
        }

        // DELETE: api/Drones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDrone(int id)
        {
            var result = await _droneService.DelDrone(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
