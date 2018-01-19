using System.Collections.Generic;

namespace UrlShortener.Models
{
    public partial class AspNetRoleClaim
    {
        public AspNetRoleClaim()
        {
            AspNetRoles = new HashSet<AspNetRole>();
        }

        public string Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string RoleId { get; set; }

        public ICollection<AspNetRole> AspNetRoles { get; set; }
    }
}
