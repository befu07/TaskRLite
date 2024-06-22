using System.ComponentModel.DataAnnotations;
using TaskRLite.Data;
using TaskRLite.Helper;

namespace TaskRLite.Models
{
    public class TagIndexVm
    {
        public int? Id{ get; set; }
        [Required]
        [MaxLength(10)]
        public string Name { get; set; } = null!;
        [Required]
        [HexColor]
        public string HexColor { get; set; } = null!;
        public Dictionary<int, Tag> Tags { get; set; } = new();
    }
}
