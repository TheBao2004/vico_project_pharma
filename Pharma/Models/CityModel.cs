using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pharma.Models
{
    [Table("cities")]
    public class CityModel
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }

        public bool Active { get; set; }

        [StringLength(20)]
        public string? Prefix { get; set; }

        [NotMapped]
        public bool Selected { get; set; }

        public virtual List<DistrictModel> Districts { get; set; }

    }
}
