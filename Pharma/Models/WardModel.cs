using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharma.Models
{
    [Table("wards")]
    public class WardModel
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public bool Active { get; set; }
        [StringLength(20)]
        public string? Prefix { get; set; }

        public int DistrictID { get; set; }
        public virtual DistrictModel District { get; set; }

    }
}
