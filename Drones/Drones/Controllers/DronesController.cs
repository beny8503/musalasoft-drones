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
        public async Task<ActionResult<IEnumerable<Drone>>> GetDrones()
        {
            return Ok(await _droneService.GetAllDrones() );
        }

        [HttpGet("Available")]
        public async Task<ActionResult<IEnumerable<Drone>>> GetAvailableDrones()
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
        public async Task<ActionResult<Drone>> GetDrone(int id)
        {
            return Ok(await _droneService.GetDrone(id));
        }

        // PUT: api/Drones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Drone>> PutDrone(int id, Drone drone)
        {
            if (id != drone.DroneId)
            {
                return BadRequest();
            }

            return Ok(await _droneService.UpdDrone(id, drone));
        }

        // POST: api/Drones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Drone>> PostDrone(Drone drone)
        {
            await _droneService.AddDrone(drone);

            return CreatedAtAction("GetDrone", new { id = drone.DroneId }, drone);
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
            return NoContent();
        }
    }
}
