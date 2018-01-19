using System.Collections.Generic;

namespace UrlShortener.Models
{
    public partial class AspNetRole
    {
        public AspNetRole()
        {
            AspNetUserRoles = new HashSet<AspNetUserRole>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string NormalizedName { get; set; }

        public ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
    }
}
