namespace Git.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Git.Data.Models;
    using Git.Models.Commits;
    using Git.Data;
    using Git.Services;
    using MyWebServer.Http;
    using MyWebServer.Controllers;

    public class CommitsController : Controller
    {
        private readonly GitDbContext data;
        private readonly IValidator validator;
        private const string InvalidIdErrorMessage =
            "Repository with id '{0}' was not found.";

        public CommitsController(GitDbContext data,
            IValidator validator)
        {
            this.data = data;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var rep = this.data
                .Repositories
                .FirstOrDefault(x => x.Id == id);

            if (rep == null)
            {
                return Error(String.Format(InvalidIdErrorMessage, id));
            }

            return View(rep);
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Create(CommitViewModel model)
        {
            var rep = this.data
                .Repositories
                .FirstOrDefault(x => x.Id == model.Id);

            var modelErrors = this.validator.ValidateCommit(model.Description);

            if (rep == null)
            {
                modelErrors
                    .Add(String.Format(InvalidIdErrorMessage, model.Id));
            }

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var commit = new Commit()
            {
                Description = model.Description,
                RepositoryId = model.Id,
                CreatorId = this.User.Id
            };

            this.data.Commits.Add(commit);
            this.data.SaveChanges();

            return Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse Delete(string id)
        {
            var commit = this.data
                .Commits
                .FirstOrDefault(x => x.Id == id);

            this.data.Commits.Remove(commit);
            this.data.SaveChanges();

            return Redirect("/Commits/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = this.data
                .Commits
                .Where(x => x.CreatorId == this.User.Id)
                .Select(x => new CommitListingViewModel()
                {
                    CreatedOn = x.CreatedOn.ToString("MM/dd/yyyy HH:mm",
                        CultureInfo.InvariantCulture),
                    Description = x.Description,
                    Repository = x.Repository.Name,
                    Id = x.Id
                })
                .ToList();

            return View(commits);
        }
    }
}
