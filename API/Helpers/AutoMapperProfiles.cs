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
            // DTOs -> Entities
            CreateMemberUpdateDtoToAppUserMap(this);
            CreateUserRegisterDtoToAppUserMap(this);

            // Entities -> DTOs
            CreateAppUserToMemberDtoMap(this);
            CreatePhotoToPhotoDtoMap(this);
            CreateAppUserToUserDtoMap(this);
            CreateAppUserToLikeDtoMap(this);
        }

        #region DTO->Entities

        private static void CreateMemberUpdateDtoToAppUserMap(IProfileExpression profile)
        {
            profile.CreateMap<MemberUpdateDto, AppUser>();
        }

        private static void CreateUserRegisterDtoToAppUserMap(IProfileExpression profile)
        {
            profile.CreateMap<UserRegistrationDto, AppUser>()
                .ForMember(appUser => appUser.UserName, opt =>
                {
                    opt.MapFrom(userRegistrationDto => userRegistrationDto.Username.ToLower());
                });
        }

        #endregion

        #region Entities->DTOs

        private static void CreateAppUserToMemberDtoMap(IProfileExpression profile)
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
        }

        private static void CreatePhotoToPhotoDtoMap(IProfileExpression profile)
        {
            profile.CreateMap<Photo, PhotoDto>();
        }

        private static void CreateAppUserToUserDtoMap(IProfileExpression profile)
        {
            profile.CreateMap<AppUser, UserDto>()
                .ForMember(userDto => userDto.Username, opt =>
                {
                    opt.MapFrom(appUser => appUser.UserName);
                })
                .ForMember(userDto => userDto.PhotoUrl, opt =>
                {
                    opt.MapFrom((appUser, _) => appUser.Photos?.FirstOrDefault(photo => photo.IsMain)?.Url ?? "");
                });
        }

        private static void CreateAppUserToLikeDtoMap(IProfileExpression profile)
        {
            profile.CreateMap<AppUser, LikeDto>()
                .ForMember(likeDto => likeDto.Username, opt =>
                {
                    opt.MapFrom(appUser => appUser.UserName);
                })
                .ForMember(likeDto => likeDto.Age, opt =>
                {
                    opt.MapFrom(appUser => appUser.DateOfBirth.CalculateAge(DateTime.Today));
                })
                .ForMember(likeDto => likeDto.PhotoUrl, opt =>
                {
                    opt.MapFrom(appUser => 
                        appUser.Photos != null 
                            ? appUser.Photos.FirstOrDefault(photo => photo.IsMain) != null 
                                ? appUser.Photos.First(photo => photo.IsMain).Url
                                : ""
                            : "");
                });
        }

        #endregion
    }
}
