using SolidApiExample.Application.Validation;
using SolidApiExample.Domain.Shared;

namespace SolidApiExample.Application.Shared;

public static class MoneyMappings
{
    public static Money ToDomain(this MoneyDto dto)
    {
        if (dto is null)
        {
            throw new ValidationException(new[] { "Total must be provided." });
        }

        try
        {
            return Money.Create(dto.Amount, dto.Currency);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new ValidationException(new[] { ex.Message });
        }
        catch (ArgumentException ex)
        {
            throw new ValidationException(new[] { ex.Message });
        }
    }

    public static MoneyDto ToDto(this Money money) =>
        new()
        {
            Amount = money.Amount,
            Currency = money.Currency
        };
}
