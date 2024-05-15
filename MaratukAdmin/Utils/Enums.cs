using Bogus.DataSets;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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

        public enum enumBookStatusForClient
        {
            Waiting = 1,
            Canceled = 2,
            InProcess = 3,
            Confirmed = 4,
            Rejected = 5,
            InvoiceSent = 6,
            PartiallyPaid = 7,
            FullyPaid = 8,
            TicketSent = 9,
            ConfirmedByAccountant = 10,
            CanceledByAccountant = 11
        }

        public enum enumBookStatusForMaratuk
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
            TicketSent = 11,
            ConfirmedByAccountant = 12,
            CanceledByAccountant = 13
        }

        public enum enumBookPaymentTypes
        {
            [EnumMember(Value = "D")]
            D,
            [EnumMember(Value = "C")]
            C
        }

        public enum enumBookPaymentStatuses
        {
            InProcess = 1,
            Approved = 2,
            Declined = 3,
            Cancelled = 4
        }

        public enum enumPassengerTypes
        {
            [EnumMember(Value = "ADL")]
            Adult = 1,
            [EnumMember(Value = "CHD")]
            Child = 2,
            [EnumMember(Value = "INF")]
            Infant =3
        }
        //public class EnumTypeSchemaFilter : ISchemaFilter
        //{
        //    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        //    {
        //        if (context.Type.IsEnum)
        //        {
        //            var enumValues = Enum.GetNames(context.Type);
        //            schema.Enum = enumValues.Length > 0 ? enumValues.Select(v => new OpenApiString(v)).Cast<IOpenApiAny>().ToList() : null;
        //            schema.Type = "string";
        //        }
        //    }
        //}
    }
}
