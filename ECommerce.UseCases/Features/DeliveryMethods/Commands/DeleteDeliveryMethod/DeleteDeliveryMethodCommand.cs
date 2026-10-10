using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.DeleteDeliveryMethod;

public sealed record DeleteDeliveryMethodCommand(Guid Id) : IRequest<Result>;
