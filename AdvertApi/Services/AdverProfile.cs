using AdvertApi.Models;
using AutoMapper;

namespace AdvertApi.Services
{
    public class AdverProfile : Profile
    {
        public AdverProfile()
        {
            CreateMap<AdvertModel, AdvertDbModel>();
        }
    }
}
