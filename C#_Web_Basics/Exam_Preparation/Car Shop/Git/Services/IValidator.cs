namespace Git.Services
{
    using System.Collections.Generic;

    using Git.Models.Users;
    using Git.Models.Repositories;

    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterViewModel model);

        ICollection<string> ValidateRepository(CreateRepositoryViewModel model);

        ICollection<string> ValidateCommit(string description);
    }
}
