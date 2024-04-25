using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharma.Models
{
    [Table("districts")]
    public class DistrictModel
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Sort { get; set; }

        public bool Active { get; set; }

        [StringLength(20)]
        public string? Prefix { get; set; }

        public int CityId { get; set; }

        [NotMapped]
        public bool Selected { get; set; }
        public virtual CityModel City { get; set; }


    }
}
