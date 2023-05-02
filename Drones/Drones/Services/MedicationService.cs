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

        public async Task<ServiceResponse<GetMedicationDto>> AddMedication(AddMedicationDto medication)
        {
            var response = new ServiceResponse<GetMedicationDto>();
            try
            {
                Medication newMedication = _mapper.Map<Medication>(medication);
                _droneContext.Medications.Add(newMedication);
                _droneContext.Entry(newMedication).State = EntityState.Added;
                await _droneContext.SaveChangesAsync();
                response.Data = _mapper.Map<GetMedicationDto>(newMedication);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<bool>> DelMedication(int id)
        {
            var response = new ServiceResponse<bool>();
            var medication = await _droneContext.Medications.FindAsync(id);
            if (medication == null)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "Medication item not found.";
            }
            else
            {
                try
                {
                    _droneContext.Medications.Remove(medication);
                    await _droneContext.SaveChangesAsync();
                    response.Data = true;
                    response.Message = "Medication item successfully removed.";
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = ex.Message;
                }
            }
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<GetMedicationDto>>> GetAllMedications()
        {
            var response = new ServiceResponse<IEnumerable<GetMedicationDto>>();
            response.Data = await _droneContext.Medications.Select(m => _mapper.Map<GetMedicationDto>(m)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<GetMedicationDto>> GetMedication(int id)
        {
            var response = new ServiceResponse<GetMedicationDto>();
            response.Data = _mapper.Map<GetMedicationDto>(await _droneContext.Medications.FindAsync(id));
            return response;
        }

        public async Task<ServiceResponse<GetMedicationDto>> UpdMedication(int id, GetMedicationDto medication)
        {
            var response = new ServiceResponse<GetMedicationDto>();
            if (id != medication.MedicationId)
            {
                response.Success = false;
                response.Message = "Bad request";
            }
            else
            {
                var updMedication = await _droneContext.Medications.FindAsync(medication.MedicationId);
                if (updMedication == null)
                {
                    response.Success = false;
                    response.Message = "Medication item not found.";
                }
                else
                {
                    updMedication.Code = medication.Code;
                    updMedication.Name = medication.Name;
                    updMedication.Weight = updMedication.Weight;
                    updMedication.Image = updMedication.Image;
                    _droneContext.Entry(updMedication).State = EntityState.Modified;
                    try
                    {
                        await _droneContext.SaveChangesAsync();
                        response.Data = _mapper.Map<GetMedicationDto>(updMedication);
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                        if (!MedicationExists(id))
                        {
                            response.Success = false;
                            response.Message = "Medication item not found in database.";
                        }
                        else
                        {
                            foreach (var entry in ex.Entries)
                            {
                                if (entry.Entity is Medication)
                                {
                                    var databaseValues = await entry.GetDatabaseValuesAsync();
                                    entry.OriginalValues.SetValues(databaseValues);
                                    response.Data = _mapper.Map<GetMedicationDto>(entry);
                                }
                            }
                        }
                    }
                }
            }
            return response;
        }
        private bool MedicationExists(int id)
        {
            return _droneContext.Medications.Any(e => e.MedicationId == id);
        }
    }
}
