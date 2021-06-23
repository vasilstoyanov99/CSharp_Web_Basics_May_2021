namespace Git.Controllers
{
    using System.Linq;

    using Git.Models.Users;
    using Git.Services;
    using Git.Data;
    using Git.Data.Models;
    using MyWebServer.Http;

    using MyWebServer.Controllers;

    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext data;
        private readonly IPasswordHasher passwordHasher;


        public UsersController(
            IValidator validator,
            GitDbContext data,
            IPasswordHasher passwordHasher)
        {
            this.validator = validator;
            this.data = data;
            this.passwordHasher = passwordHasher;
        }

        public HttpResponse Register() => View();

        [HttpPost]
        public HttpResponse Register(RegisterViewModel model)
        {
            var modelErrors = this.validator.ValidateUser(model);

            if(this.data.Users.Any(u => u.Username == model.Username))
            {
                modelErrors.Add($"User with '{model.Username}' username already exists.");
            }

            if (this.data.Users.Any(u => u.Email == model.Email))
            {
                modelErrors.Add($"User with '{model.Email}' e-mail already exists.");
            }

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var user = new User()
            {
                Email = model.Email,
                Username = model.Username,
                Password = this.passwordHasher.HashPassword(model.Password)
            };

            data.Users.Add(user);
            data.SaveChanges();

            return Redirect("/Users/Login");
        }

        public HttpResponse Login() => View();

        [HttpPost]
        public HttpResponse Login(LogInViewModel model)
        {
            var hashedPassword = this.passwordHasher.HashPassword(model.Password);

            var userId = this.data
                .Users
                .Where(u => u.Username == model.Username 
                            && u.Password == hashedPassword)
                .Select(u => u.Id)
                .FirstOrDefault();

            if (userId == null)
            {
                return Error("Username and password combination is not valid.");
            }

            this.SignIn(userId);

            return Redirect("/Repositories/All");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return Redirect("/");
        }
    }
}
