using CarRentingSystem.Core.Models.CarRoute;

namespace CarRentingSystem.Models
{
    public class AllCarRoutesQueryModel
    {
        public const int CarRoutesPerPage = 3;

        public string? Category { get; set; }

        public string? SearchTerm { get; set; }

        public CarRouteSorting Sorting { get; set; }

        public int CurrentPage { get; set; } = 1;


        public int TotalCarRoutesCount { get; set; }

        public IEnumerable<string> Categories { get; set; } = Enumerable.Empty<string>();

        public IEnumerable<CarRouteServiceModel> CarRoutes { get; set; } = Enumerable.Empty<CarRouteServiceModel>();
    }
}
