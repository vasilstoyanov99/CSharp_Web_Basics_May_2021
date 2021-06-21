using System;
using CarShop.Data.Models;
using MyWebServer.Results;

namespace CarShop.Controllers
{
    using CarShop.Data;
    using CarShop.Models.Issues;
    using CarShop.Services;
    using CarShop.Common;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using System.Linq;

    public class IssuesController : Controller
    {
        private readonly IUserService userService;
        private readonly CarShopDbContext data;

        public IssuesController(IUserService userService, CarShopDbContext data)
        {
            this.userService = userService;
            this.data = data;
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                var userOwnsCar = this.data.Cars
                    .Any(c => c.Id == carId && c.OwnerId == this.User.Id);

                if (!userOwnsCar)
                {
                    return Error("You do not have access to this car.");
                }
            }

            var carWithIssues = this.data
                .Cars
                .Where(c => c.Id == carId)
                .Select(c => new CarIssuesViewModel
                {
                    Id = c.Id,
                    Model = c.Model,
                    Year = c.Year,
                    Issues = c.Issues.Select(i => new IssueListingViewModel
                    {
                        Id = i.Id,
                        Description = i.Description,
                        IsFixed = i.IsFixed
                    })
                })
                .FirstOrDefault();

            if (carWithIssues == null)
            {
                return Error($"Car with ID '{carId}' does not exist.");
            }

            return View(carWithIssues);
        }

        [Authorize]
        public HttpResponse Add(string carId)
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddIssueViewModel issue)
        {
            var car = GetCar(issue.CarId);

            if (car == null)
            {
                return Error($"Car with {issue.CarId} id was not found!"); //.Error should work with string.Format()
            }

            car.Issues.Add(new Issue()
            {
                CarId = issue.CarId,
                Car = car,
                Description = issue.Description
            });

            data.SaveChanges();

            return Redirect($"/Issues/Add?CarId={car.Id}");
        }

        [Authorize]
        public HttpResponse Fix(FixAndDeleteIssueViewModel viewModel)
        {
            if (!userService.IsMechanic(this.User.Id))
            {
                return Unauthorized();
            }

            var issue = GetIssue(viewModel.IssueId);

            issue.IsFixed = true;

            this.data.SaveChanges();

            return Redirect($"/Issues/CarIssues?CarId={viewModel.CarId}");
        }

        [Authorize]
        public HttpResponse Delete(FixAndDeleteIssueViewModel viewModel)
        {
            var issue = GetIssue(viewModel.IssueId);


            var car = GetCar(viewModel.CarId);

            car.Issues.Remove(issue);

            data.SaveChanges();

            return Redirect($"/Issues/CarIssues?CarId={viewModel.CarId}");
        }

        private Issue GetIssue(string issueId) =>
            data
                .Issues
                .FirstOrDefault(x => x.Id == issueId);

        private Car GetCar(string carId) =>
            data
                .Cars
                .FirstOrDefault(x => x.Id == carId);
    }
}
