using System;
using System.Globalization;
using SharedTrip.Models.Trips;

namespace SharedTrip.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using SharedTrip.Models.Users;

    using static Data.DataConstants;

    public class Validator : IValidator
    {
        public ICollection<string> ValidateUser(RegisterUserFormModel model)
        {
            var errors = new List<string>();

            if (model.Username.Length < UserNameMinlength ||
                model.Username.Length > UsernameAndPasswordMaxLength)
            {
                errors.Add($"Username '{model.Username}' is not valid." +
                           $" It must be between {UserNameMinlength}" +
                           $" and {UsernameAndPasswordMaxLength} characters long.");
            }

            if (!Regex.IsMatch(model.Email, UserEmailRegularExpression))
            {
                errors.Add($"Email {model.Email} is not a valid e-mail address.");
            }

            if (model.Password.Length < PasswordMinlength || 
                model.Password.Length > UsernameAndPasswordMaxLength)
            {
                errors.Add($"The provided password is not valid." +
                           $" It must be between {PasswordMinlength} and " +
                           $"{UsernameAndPasswordMaxLength} characters long.");
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

        public ICollection<string> ValidateTrip(AddTripFormModel model)
        {
            var errors = new List<string>();

            if (model.Seats < SeatsMinValue || model.Seats > SeatsMaxValue)
            {
                errors.Add($"The provided seats count is invalid!" +
                           $" It must be between {SeatsMinValue} and {SeatsMaxValue}!");
            }

            if (model.Description.Length > DescriptionMaxLength)
            {
                errors.Add($"The provided description is too long." +
                           $" The maximum length is {DescriptionMaxLength} characters!");
            }

            if (model.Description.All(x => x == ' '))
            {
                errors.Add("The provided description cannot be only whitespaces!");
            }

            if (model.Description.Length <= 0)
            {
                errors.Add("A description is required!");
            }

            return errors;
        }
    }
}
