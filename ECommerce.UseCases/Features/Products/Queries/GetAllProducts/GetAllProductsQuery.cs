using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Features.Products.Queries.GetAllProducts;
public sealed record GetAllProductsQuery
    : IRequest<Result<IReadOnlyList<GetAllProductsResponse>>>;