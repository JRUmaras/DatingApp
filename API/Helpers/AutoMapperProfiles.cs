using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            this
                .CreateAppUserToMemberDtoMap()
                .CreatePhotoToPhotoDtoMap()
                .CreateMemberUpdateDtoToAppUserMap();
        }
    }
}
