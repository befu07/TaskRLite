using TaskRLite.Data;
using static TaskRLite.Services.ToDoListService;

namespace TaskRLite.Models;
public class TDLDetailsVm
{
    public int Id { get; set; }
    public TaskFilter Filter { get; set; } = 0;
    public string Query { get; set; } = string.Empty;
    public string Name { get; set; } = null!;
    public virtual IEnumerable<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}