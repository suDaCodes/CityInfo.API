namespace CityInfo.API.Services;

public class PaginationMetadata
{
    private int TotalItemCount { get; set; }
    private int TotalPageCount { get; set; }
    private int PageSize { get; set; }
    private int CurrentPage { get; set; }

    public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
    {
        TotalItemCount = totalItemCount;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);
    }
}