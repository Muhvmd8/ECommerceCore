using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.DTOs;
using MediatR;

namespace ECommerce.UseCases.Products.Queries.GetAllBrands;
public sealed record GetAllBrandsQuery
    : IRequest<Result<IReadOnlyList<GetAllBrandsResponse>>>;