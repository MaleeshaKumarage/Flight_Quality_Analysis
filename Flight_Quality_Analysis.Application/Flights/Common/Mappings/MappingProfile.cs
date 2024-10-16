﻿using AutoMapper;
using Flight_Quality_Analysis.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Application.Flights.Common.Mappings
{
    public class MappingProfile : Profile
    {
        //Copied from Previous project
        public MappingProfile()
        {

            ApplyMappingFromAssembly(Assembly.GetExecutingAssembly());
            CreateMap<KeyValuePair<Flight, string>, FlightInconsitancyDto>()
               .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Value))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Key.Id))
               .ForMember(dest => dest.AircraftRegistrationNumber, opt => opt.MapFrom(src => src.Key.AircraftRegistrationNumber))
               .ForMember(dest => dest.AircraftType, opt => opt.MapFrom(src => src.Key.AircraftType))
               .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Key.FlightNumber))
               .ForMember(dest => dest.DepartureAirport, opt => opt.MapFrom(src => src.Key.DepartureAirport))
               .ForMember(dest => dest.DepartureDateTime, opt => opt.MapFrom(src => src.Key.DepartureDateTime))
               .ForMember(dest => dest.ArrivalAirport, opt => opt.MapFrom(src => src.Key.ArrivalAirport))
               .ForMember(dest => dest.ArrivalDateTime, opt => opt.MapFrom(src => src.Key.ArrivalDateTime));
        }
        private void ApplyMappingFromAssembly(Assembly assembly)
        {
            var mapFromType = typeof(IMapFrom<>);
            var mappingMethodName = nameof(IMapFrom<object>.Mapping);
            bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

            var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

            var argumentTypes = new Type[] { typeof(Profile) };

            foreach (var type in types)
            {

                var instance = Activator.CreateInstance(type);

                var methodInfo = type.GetMethod(mappingMethodName);

                if (methodInfo != null)
                {

                    methodInfo.Invoke(instance, new object[] { this });

                }
                else
                {

                    var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

                    if (interfaces.Count > 0)
                    {

                        foreach (var @interface in interfaces)
                        {
                            var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);
                            interfaceMethodInfo?.Invoke(instance, new object[] { this });
                        }

                    }
                }
            }
        }
    }
}
