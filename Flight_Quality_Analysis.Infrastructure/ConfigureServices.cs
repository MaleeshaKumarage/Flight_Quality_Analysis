using AutoMapper;
using Flight_Quality_Analysis.Domain.Repository;
using Flight_Quality_Analysis.Infrastructure.Repository;
using Flight_Quality_Analysis.Infrastructure.Services.FileReadingService;
using Flight_Quality_Analysis.Infrastructure.Services.FlightAnalysisService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Quality_Analysis.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICsvReadingService, CsvReadingService>();
            services.AddScoped<IFlightInconsistancyAnalysisService, FlightInconsistancyAnalysisService>();
            services.AddScoped<IFlightRepository, FlightRepository>();
            return services;
        }
    }
}
