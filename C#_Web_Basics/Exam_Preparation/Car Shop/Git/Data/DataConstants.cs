namespace Git.Data
{
    public class DataConstants
    {
        public const int IdMaxLength = 40;
        public const int DefaultMaxLength = 20;

        public const int UserNameMinlength = 5;
        public const int PasswordMinlength = 6;
        public const string UserEmailRegularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public const int RepositoryNameMaxLength = 10;
        public const int RepositoryNameMinLength = 3;
        public const string PublicRepository = "Public";

        public const int CommitDescriptionMinLength = 5;

    }
}
