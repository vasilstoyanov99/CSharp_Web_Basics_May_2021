namespace Git.Controllers
{
    using System.Globalization;
    using System.Linq;

    using Git.Services;
    using Git.Data;
    using Git.Data.Models;
    using Git.Models.Repositories;
    using MyWebServer.Controllers;
    using MyWebServer.Http;

    using static Git.Data.DataConstants;

    public class RepositoriesController : Controller
    {
        private readonly GitDbContext data;
        private readonly IValidator validator;

        public RepositoriesController(GitDbContext data,
            IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        public HttpResponse All()
        {
            var publicRepos = this.data
                    .Repositories
                    .Where(x => x.IsPublic == true)
                    .Select(x => new RepositoryListingViewModel()
                    {
                        CommitsCount = x.Commits.Count,
                        CreatedOn = x.CreatedOn
                            .ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture),
                        Name = x.Name,
                        Owner = x.Owner.Username,
                        Id = x.Id
                    })
                    .ToList();

                return View(publicRepos);
            }

        [Authorize]
        public HttpResponse Create() => View();


        [Authorize]
        [HttpPost]
        public HttpResponse Create(CreateRepositoryViewModel model)
        {
            var modelErrors = this.validator.ValidateRepository(model);

            if (this.data.Repositories.Any(x => x.Name == model.Name))
            {
                modelErrors.Add($"Repository with '{model.Name}' name already exists.");
            }

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var repository = new Repository()
            {
                Name = model.Name,
                IsPublic = model.RepositoryType == PublicRepository,
                OwnerId = this.User.Id
            };

            this.data.Repositories.Add(repository);
            this.data.SaveChanges();

            return Redirect("/Repositories/All");
        }
    }
}
