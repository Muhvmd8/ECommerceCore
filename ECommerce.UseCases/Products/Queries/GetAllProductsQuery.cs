using ECommerce.Domain.Shared;
using ECommerce.UseCases.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Products.Queries;
public sealed record GetAllProductsQuery
    : IRequest<Result<IReadOnlyList<GetAllProductsResponse>>>;