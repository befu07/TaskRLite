using System.ComponentModel.DataAnnotations;
using TaskRLite.Data;

namespace TaskRLite.Models;
public class ToDoCreateVm
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = null!;
}
