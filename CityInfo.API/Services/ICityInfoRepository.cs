using CityInfo.API.Entities;

namespace CityInfo.API.Services;

public interface ICityInfoRepository
{
    Task<IEnumerable<City>> GetCitiesAsync();

    Task<IEnumerable<City>> GetCitiesAsync(string? name, string? searchQuery, int pageNumber, int pageSize);

    Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);

    Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId);

    Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId);

    Task<bool> CityExistsAsync(int cityId);

    // why we use async here even if we can use void?
    // because in the implementation we call into GetCityAsync.
    Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);

    // it isn't an async method, just like "add" method, it's an in-memory action, not an I/O operation
    // so async does not make sense here.
    void RemovePointOfInterest(PointOfInterest pointOfInterest);

    Task<bool> SaveChangesAsync();
    
}