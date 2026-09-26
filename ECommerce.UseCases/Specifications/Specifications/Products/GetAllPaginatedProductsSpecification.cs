using ECommerce.Domain.Entities.ProductModule;
using ECommerce.UseCases.Products.DTOs;
using ECommerce.UseCases.Products.Enums;

namespace ECommerce.UseCases.Specifications.Specifications.Products
{
    public class GetAllPaginatedProductsSpecification
        : Specification<Product, GetAllProductsResponse>
    {
        public GetAllPaginatedProductsSpecification(string? search = null, int? brandId = null, int? typeId = null,
            ProductsSortField? orderBy = null, bool sortType = false, int? pageNumber = null, int? pageSize = null)
        {
            var query = Query;

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim();

                query = query.Where(p =>
                    p.Name.Contains(term) ||
                    p.Description.Contains(term));
            }

            if (brandId.HasValue)
                query = query.Where(p => p.ProductBrandId == brandId.Value);

            if (typeId.HasValue)
                query = query.Where(p => p.ProductTypeId == typeId.Value);

            if (orderBy is ProductsSortField sortField)
                _ApplySort(orderBy, sortType);

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query
                    .Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
            }

            query = query.AsNoTracking()
                 .Select(p => new GetAllProductsResponse
                 {
                     Id = p.Id,
                     Name = p.Name,
                     Description = p.Description,
                     Price = p.Price,
                     BrandName = p.ProductBrand.Name,
                     TypeName = p.ProductType.Name,
                     PictureUrl = p.PictureUrl
                 });
        }

        private void _ApplySort(
            ProductsSortField? sortField,
            bool sortType)
        {
            if (!sortField.HasValue)
            {
                Query.OrderByDescending(p => p.Id);
                return;
            }

            switch (sortField.Value)
            {
                case ProductsSortField.Name:
                    if (sortType)
                        Query.OrderByDescending(p => p.Name);
                    else
                        Query.OrderBy(p => p.Name);
                    break;

                case ProductsSortField.Price:
                    if (sortType)
                        Query.OrderByDescending(p => p.Price);
                    else
                        Query.OrderBy(p => p.Price);
                    break;

                case ProductsSortField.Brand:
                    if (sortType)
                        Query.OrderByDescending(p => p.ProductBrand);
                    else
                        Query.OrderBy(p => p.ProductBrand);
                    break;

                case ProductsSortField.Type:
                    if (sortType)
                        Query.OrderByDescending(p => p.ProductType);
                    else
                        Query.OrderBy(p => p.ProductType);
                    break;

                default:
                    Query.OrderByDescending(p => p.Id);
                    break;
            }
        }
    }
}