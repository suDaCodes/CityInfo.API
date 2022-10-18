namespace CityInfo.API.Models
{
    public class CityDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        // you know it's a good idea to always initialize them to an empty collection instead of leaving them null,
        // as to avoid null reference issues.
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();

        public int NumberOfPointsOfInterest
        {
            get { return PointsOfInterest.Count; }
        }
    }
}