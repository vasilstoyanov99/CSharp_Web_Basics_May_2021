namespace SharedTrip.Controllers
{
    using System.Linq;

    using MyWebServer.Controllers;
    using MyWebServer.Http;

    using SharedTrip.Data;
    using SharedTrip.Models.Trips;
    using SharedTrip.Data.Models;
    using SharedTrip.Services;

    public class TripsController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext data;

        public TripsController(IValidator validator, ApplicationDbContext data)
        {
            this.validator = validator;
            this.data = data;
        }

        [Authorize]
        public HttpResponse All()
        {
            var trips = this.data
                .Trips
                .Select(x => new TripListingViewModel()
                {
                    DepartureTime = x.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    StartPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    Seats = x.Seats,
                    Id = x.Id
                })
                .ToList();

            return View(trips);
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            var trip = this.data
                .Trips
                .Where(x => x.Id == tripId)
                .Select(x => new TripViewModel()
                {
                    Description = x.Description,
                    DepartureTime = x.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    EndPoint = x.EndPoint,
                    StartPoint = x.StartPoint,
                    Seats = x.Seats,
                    ImagePath = x.ImagePath,
                    Id = x.Id
                })
                .FirstOrDefault();

            if (trip == null)
            {
                return Error("The trip was not found! Try again.");
            }

            return View(trip);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var trip = this.data
                .Trips
                .FirstOrDefault(x => x.Id == tripId);

            if (trip == null)
            {
                return Error("The trip was not found! Try again.");
            }

            if (trip.Seats == 0)
            {
                return Error("There are no more free seats for this trip!");
            }

            var userTrip = this.data.UsersTrips.FirstOrDefault(x => x.UserId == this.User.Id
                                                          && x.TripId == tripId);

            if (userTrip != null)
            {
                return Redirect($"/Trips/Details?tripId={tripId}");
            }

            this.data.UsersTrips.Add(new UserTrip()
            {
                TripId = tripId,
                UserId = this.User.Id,
            });

            this.data.SaveChanges();

            trip.Seats -= 1;

            this.data.SaveChanges();

            return Redirect("/");
        }

        [Authorize]
        public HttpResponse Add() => View();

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddTripFormModel model)
        {
            var modelErrors = this.validator.ValidateTrip(model);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var user = this.data
                .Users
                .FirstOrDefault(x => x.Id == this.User.Id);

            var trip = new Trip()
            {
                DepartureTime = model.DepartureTime,
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                Seats = model.Seats,
                Description = model.Description,
                ImagePath = model.ImagePath
            };

            this.data.Trips.Add(trip);
            this.data.SaveChanges();

            return Redirect("/Trips/All");
        }
    }
}
