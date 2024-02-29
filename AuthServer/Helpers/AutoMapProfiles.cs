using AuthServer.DTOs;
using AuthServer.Entities;
using AutoMapper;

namespace AuthServer.Helpers
{
    public class AutoMapProfiles : Profile
    {
        public AutoMapProfiles()
        {
            CreateMap<DateTime, DateTime>()
                .ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
            CreateMap<RegisterDto, Korisnik>();
            CreateMap<AuthorizationDto, AuthCodeDto>();

            CreateMap<AuthCodeDto, AuthCode>();
        }
    }
}