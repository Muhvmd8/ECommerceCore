using ECommerce.Domain.Shared;
using ECommerce.UseCases.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Products.Queries;
public sealed class GetAllTypesQuery
    : IRequest<Result<IReadOnlyList<GetAllTypesResponse>>>;