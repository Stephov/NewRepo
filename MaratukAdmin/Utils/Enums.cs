using Bogus.DataSets;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace MaratukAdmin.Utils
{
    public class Enums
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum enumRoles
        {
            //[Display(Name = "All")]
            All,
            //[Display(Name = "Accountant")]
            Accountant,
            //[Display(Name = "Admin")]
            Admin,
            //[Display(Name = "HotelManager")]
            HotelManager,
            //[Display(Name = "FligthManager")]
            FligthManager
        }

        public enum BookStatusForClient
        {
            Waiting = 1,
            Canceled = 2,
            InProcess = 3,
            Confirmed = 4,
            Rejected = 5,
            InvoiceSent = 6,
            PartiallyPaid = 7,
            FullyPaid = 8,
            TicketSent = 9
        }

        public enum BookStatusForMaratuk
        {
            Waiting = 1,
            Canceled = 2,
            TicketIsConfirmed = 3,
            TicketIsRejected = 4,
            HotelIsConfirmed = 5,
            HotelIsRejected = 6,
            Confirmed = 7,
            InvoiceSent = 8,
            PaidPartially = 9,
            PaidInFull = 10,
            TicketSent = 11
        }
    }
}
