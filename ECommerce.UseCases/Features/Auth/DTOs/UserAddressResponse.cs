namespace ECommerce.UseCases.Features.Auth.DTOs
{
    public sealed record UserAddressResponse(
        Guid Id,
        string Label,
        string RecipientFirstName,
        string RecipientLastName,
        string PhoneNumber,
        string Country,
        string City,
        string Street,
        bool IsDefault);
}