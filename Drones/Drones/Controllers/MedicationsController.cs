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

        /// <summary>
        /// Gets a list of medication items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<GetMedicationDto>>>> GetMedications()
        {
            return Ok(await _medicationService.GetAllMedications());
        }

        /// <summary>
        /// Gets a medication item for the given medication item id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMedicationDto>> GetMedication(int id)
        {
            var response = await _medicationService.GetMedication(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        /// <summary>
        /// Updates the medication item data for the given medication item id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="medication"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<GetMedicationDto>>> PutMedication(int id, GetMedicationDto medication)
        {
            if (id != medication.MedicationId)
            {
                return BadRequest();
            }

            var respMedication = await _medicationService.UpdMedication(id, medication);
            if (respMedication.Data == null)
            {
                return NotFound(respMedication);
            }

            return Ok(respMedication);
        }

        /// <summary>
        /// Add a new medication item.
        /// </summary>
        /// <param name="medication"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetMedicationDto>>> PostMedication(AddMedicationDto medication)
        {
           
            var response = await _medicationService.AddMedication(medication);

            return CreatedAtAction("GetMedication", new { id = response.Data.MedicationId }, response);
        }

        /// <summary>
        /// Deletes a medication item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteMedication(int id)
        {
            var result = await _medicationService.DelMedication(id);
            if (!result.Data)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

    }
}
