using AutoMapper;
using Drones.DTOs;
using Drones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drones
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Drone, GetDroneDto>();
            CreateMap<AddDroneDto, Drone>();
            CreateMap<FullDroneDto, Drone>();
            CreateMap<Drone, FullDroneDto>();
            CreateMap<Medication, GetMedicationDto>();
            CreateMap<GetMedicationDto, Medication>();
            CreateMap<AddMedicationDto, Medication>();
            CreateMap<AddMedicationDto, GetMedicationDto>();
            CreateMap<GetMedicationDto, AddMedicationDto>();
        }
    }
}
