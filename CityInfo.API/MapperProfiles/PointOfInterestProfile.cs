using AutoMapper;

namespace CityInfo.API.MapperProfiles;

public class PointOfInterestProfile : Profile
{
    public PointOfInterestProfile()
    {
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestDTO>();
        CreateMap<Models.PointOfInterestForCreationDTO, Entities.PointOfInterest>();
        CreateMap<Models.PointOfInterestForUpdateDTO, Entities.PointOfInterest>();
        CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDTO>();
    }
}