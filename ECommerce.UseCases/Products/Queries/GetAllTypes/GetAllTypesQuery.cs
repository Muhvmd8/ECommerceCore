using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.DTOs;
using MediatR;
namespace ECommerce.UseCases.Products.Queries.GetAllTypes;
public sealed class GetAllTypesQuery
    : IRequest<Result<IReadOnlyList<GetAllTypesResponse>>>;