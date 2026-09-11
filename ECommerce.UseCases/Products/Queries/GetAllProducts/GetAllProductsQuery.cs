using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Products.Queries.GetAllProducts;
public sealed record GetAllProductsQuery
    : IRequest<Result<IReadOnlyList<GetAllProductResponse>>>;