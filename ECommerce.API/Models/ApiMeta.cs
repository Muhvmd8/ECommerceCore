namespace ECommerce.API.Models;
public class ApiMeta
{
    public string TraceId { get; set; } = string.Empty;
    public PaginationMeta? Pagination { get; set; }
}