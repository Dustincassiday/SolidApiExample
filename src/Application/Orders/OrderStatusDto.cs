using System.Text.Json.Serialization;

namespace SolidApiExample.Application.Orders;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatusDto
{
    Pending,
    Processing,
    Completed,
    Cancelled
}
