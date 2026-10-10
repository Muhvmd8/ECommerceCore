using ECommerce.Domain.Shared;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.HandleStripeWebhook;

public sealed record HandleStripeWebhookCommand(
    string Payload,
    string Signature) : IRequest<Result>;