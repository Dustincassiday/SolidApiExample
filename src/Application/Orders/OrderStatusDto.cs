using System.Text.Json.Serialization;

namespace SolidApiExample.Application.Orders;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatusDto
{
    New,
    Paid,
    Shipped
}
