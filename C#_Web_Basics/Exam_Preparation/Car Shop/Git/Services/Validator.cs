namespace Git.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Git.Models.Repositories;
    using Git.Models.Users;

    using static Data.DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterViewModel model)
        {

            var errors = new List<string>();

            if (model.Username.Length < UserNameMinlength || model.Username.Length > DefaultMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid. It must be between {UserNameMinlength} and {DefaultMaxLength} characters long.");
            }

            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} is not a valid e-mail address.");
            }

            if (model.Password.Length < PasswordMinlength || model.Password.Length > DefaultMaxLength)
            {
                errors.Add($"The provided password is not valid. It must be between {PasswordMinlength} and {DefaultMaxLength} characters long.");
            }

            if (model.Password.All(x => x == ' '))
            {
                errors.Add($"The provided password cannot be only whitespaces!");
            }

            if (model.Password != model.ConfirmPassword)
            {
                errors.Add($"Password and its confirmation are different.");
            }

            return errors;
        }

        public ICollection<string> ValidateRepository(CreateRepositoryViewModel model)
        {
            var errors = new List<string>();

            if (model.Name.Length < RepositoryNameMinLength || model.Name.Length > RepositoryNameMaxLength)
            {
                errors.Add($"The repository name '{model.Name}' is not valid. It must be between {RepositoryNameMinLength} and {RepositoryNameMaxLength} characters long.");
            }

            if (model.Name.All(x => x == ' '))
            {
                errors.Add($"The provided repository name cannot be only whitespaces!");
            }

            return errors;
        }

        public ICollection<string> ValidateCommit(string description)
        {
            var errors = new List<string>();

            if (description.Length < CommitDescriptionMinLength)
            {
               errors.Add($"The provided description should be at least {CommitDescriptionMinLength} characters long.");
            }

            return errors;
        }
    }
}
