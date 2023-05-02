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
        public Task<ServiceResponse<IEnumerable<GetMedicationDto>>> GetAllMedications();

        public Task<ServiceResponse<GetMedicationDto>> GetMedication(int id);

        public Task<ServiceResponse<GetMedicationDto>> UpdMedication(int id, GetMedicationDto medication);

        public Task<ServiceResponse<GetMedicationDto>> AddMedication(AddMedicationDto medication);

        public Task<ServiceResponse<bool>> DelMedication(int id);
    }
}
