using System;
using System.Linq;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(memberDto => memberDto.PhotoUrl, opt =>
                {
                    opt.MapFrom(appUser => appUser.Photos.FirstOrDefault(photo => photo.IsMain).Url);
                })
                .ForMember(memberDto => memberDto.Age, opt =>
                {
                    opt.MapFrom(appUser => appUser.DateOfBirth.CalculateAge(DateTime.Today));
                });
            CreateMap<Photo, PhotoDto>();
        }
    }
}
