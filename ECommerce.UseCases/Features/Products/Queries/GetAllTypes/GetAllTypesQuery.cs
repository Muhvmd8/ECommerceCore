using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Features.Products.Queries.GetAllTypes;
public sealed class GetAllTypesQuery
    : IRequest<Result<IReadOnlyList<GetAllTypesResponse>>>;