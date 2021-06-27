namespace SharedTrip.Services
{
    using System.Collections.Generic;

    using SharedTrip.Models.Users;
    using SharedTrip.Models.Trips;

    public interface IValidator
    {
        ICollection<string> ValidateUser(RegisterUserFormModel model);

        ICollection<string> ValidateTrip(AddTripFormModel model);
    }
}
