using System.ComponentModel.DataAnnotations;

namespace Persistence.Adapter
{
    public sealed class PersistenceAdapterSettings
    {
        [Required(AllowEmptyStrings = false)]
        public string DataSource { get; private set; }
        
        [Required(AllowEmptyStrings = false)]
        public string UserId { get; private set; }
        
        [Required(AllowEmptyStrings = false)]
        public string Password { get; private set; }
    }
}
