namespace ECommerce.Domain.Enums;

public enum OrderStatus
{
    Pending = 0,
    PaymentReceived = 1,
    PaymentFailed = 2,
    Shipped = 3,
    Delivered = 4,
    Cancelled = 5
}
