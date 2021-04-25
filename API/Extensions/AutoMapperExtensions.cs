using System;
using System.Linq;
using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Extensions
{
    public static class AutoMapperExtensions
    {
        public static Profile CreateAppUserToMemberDtoMap(this Profile profile)
        {
            profile.CreateMap<AppUser, MemberDto>()
                .ForMember(memberDto => memberDto.PhotoUrl, opt =>
                {
                    opt.MapFrom(appUser => appUser.Photos.FirstOrDefault(photo => photo.IsMain).Url);
                })
                .ForMember(memberDto => memberDto.Age, opt =>
                {
                    opt.MapFrom(appUser => appUser.DateOfBirth.CalculateAge(DateTime.Today));
                });

            return profile;
        }

        public static Profile CreatePhotoToPhotoDtoMap(this Profile profile)
        {
            profile.CreateMap<Photo, PhotoDto>();
            
            return profile;
        }

        public static Profile CreateMemberUpdateDtoToAppUserMap(this Profile profile)
        {
            profile.CreateMap<MemberUpdateDto, AppUser>();
            
            return profile;
        }
    }
}
