using AutoMapper;
using Drones.Context;
using Drones.DTOs;
using Drones.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly DronesContext _droneContext;
        private readonly IMapper _mapper;

        public MedicationService(DronesContext dronesContext, IMapper mapper)
        {
            _droneContext = dronesContext;
            _mapper = mapper;
        }

        public async Task<GetMedicationDto> AddMedication(AddMedicationDto medication)
        {
            Medication newMedication = _mapper.Map<Medication>(medication);
            await _droneContext.Medications.AddAsync(newMedication);
            await _droneContext.SaveChangesAsync();

            return _mapper.Map<GetMedicationDto>(newMedication);
        }

        public async Task<bool> DelMedication(int id)
        {
            var medication = await _droneContext.Medications.FindAsync(id);
            if (medication == null)
            {
                return false;
            }

            _droneContext.Medications.Remove(medication);
            await _droneContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<GetMedicationDto>> GetAllMedications()
        {
            return await _droneContext.Medications.Select(m => _mapper.Map<GetMedicationDto>(m)).ToListAsync();
        }

        public async Task<GetMedicationDto> GetMedication(int id)
        {
            return _mapper.Map<GetMedicationDto>(await _droneContext.Medications.FindAsync(id));
        }

        public async Task<GetMedicationDto> UpdMedication(int id, GetMedicationDto medication)
        {
            if (id != medication.MedicationId)
            {
                return null;
            }

            var updMedication = await _droneContext.Medications.FindAsync(medication.MedicationId);
            if (updMedication == null)
            {
                return null;
            }
            updMedication.Code = medication.Code;
            updMedication.Name = medication.Name;
            updMedication.Weight = updMedication.Weight;
            updMedication.Image = updMedication.Image;
            _droneContext.Entry(updMedication).State = EntityState.Modified;
            try
            {
                await _droneContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {

                if (!MedicationExists(id))
                {
                    return null;
                }
                else
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Medication)
                        {
                            var databaseValues = await entry.GetDatabaseValuesAsync();
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                    }
                }
            }

            return _mapper.Map<GetMedicationDto>(updMedication);
        }
        private bool MedicationExists(int id)
        {
            return _droneContext.Medications.Any(e => e.MedicationId == id);
        }
    }
}
