using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<FakturaDto, Faktura>();
            CreateMap<Faktura, FakturaDto>();

            CreateMap<StavkaFakture, StavkaFaktureDto>();
            CreateMap<StavkaFaktureDto, StavkaFakture>();
            CreateMap<DateTime, DateTime>()
                .ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        }
    }
}