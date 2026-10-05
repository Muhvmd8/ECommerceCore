using ECommerce.Domain.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Domain.Entities.Security
{
    [Table("LoginAudits", Schema = "Security")]
    public class LoginAudit : BaseEntity<int>
    {
        public string Username { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string Endpoint { get; set; } = string.Empty;
        public string Status { get; set; } = "Failed"; // Success / Failed
        public string? RequestBody { get; set; }
    }
}
