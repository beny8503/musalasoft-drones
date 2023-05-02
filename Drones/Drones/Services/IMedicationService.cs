using Drones.DTOs;
using Drones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Services
{
    public interface IMedicationService
    {
        public Task<IEnumerable<GetMedicationDto>> GetAllMedications();

        public Task<GetMedicationDto> GetMedication(int id);

        public Task<GetMedicationDto> UpdMedication(int id, GetMedicationDto medication);

        public Task<GetMedicationDto> AddMedication(AddMedicationDto medication);

        public Task<bool> DelMedication(int id);
    }
}
