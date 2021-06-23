namespace Git.Models.Repositories
{
    public class RepositoryListingViewModel
    {
        public string Name { get; init; }

        public string Owner { get; init; }

        public string CreatedOn { get; init; }

        public string Id { get; init; }

        public int CommitsCount { get; init; }
    }
}
