using AutoMapper;

namespace CityInfo.API.MapperProfiles;

public class CityProfile : Profile
{
    public CityProfile()
    {
        // map City Entity -> CityWithoutPointsOfInterestDTO
        CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
        CreateMap<Entities.City, Models.CityDTO>();
    }
}