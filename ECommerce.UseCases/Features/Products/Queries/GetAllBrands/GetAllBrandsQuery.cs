using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.Products.DTOs;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Queries.GetAllBrands;
public sealed record GetAllBrandsQuery
    : IRequest<Result<IReadOnlyList<GetAllBrandsResponse>>>;