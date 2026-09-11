using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Products.Queries.GetProductById;
public sealed record GetProductByIdQuery(int Id)
    : IRequest<Result<GetProductByIdResponse>>;