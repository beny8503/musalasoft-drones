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
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationService _medicationService;

        public MedicationsController(IMedicationService medicationService)
        {
            _medicationService = medicationService;
        }

        // GET: api/Medications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMedicationDto>>> GetMedications()
        {
            return Ok(await _medicationService.GetAllMedications());
        }

        // GET: api/Medications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMedicationDto>> GetMedication(int id)
        {
            var medication = await _medicationService.GetMedication(id);

            if (medication == null)
            {
                return NotFound();
            }

            return medication;
        }

        // PUT: api/Medications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedication(int id, GetMedicationDto medication)
        {
            if (id != medication.MedicationId)
            {
                return BadRequest();
            }

            var respMedication = await _medicationService.UpdMedication(id, medication);
            if (respMedication == null)
            {
                return NotFound();
            }

            return Ok(respMedication);
        }

        // POST: api/Medications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetMedicationDto>> PostMedication(AddMedicationDto medication)
        {
           
            GetMedicationDto newMedication = await _medicationService.AddMedication(medication);

            return CreatedAtAction("GetMedication", new { id = newMedication.MedicationId }, newMedication);
        }

        // DELETE: api/Medications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedication(int id)
        {
            var result = await _medicationService.DelMedication(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
