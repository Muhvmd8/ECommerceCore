using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Features.Products.Queries.GetProductById;
public sealed record GetProductByIdQuery(int Id)
    : IRequest<Result<GetProductByIdResponse>>;