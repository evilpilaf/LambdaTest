using System.ComponentModel.DataAnnotations;

namespace Persistence.Adapter
{
    public sealed class PersistenceAdapterSettings
    {
        [Required(AllowEmptyStrings = false)]
        public string DataSource { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string UserId { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }

        [Required]
        public bool LoadBalancing { get; set; }
    }
}
