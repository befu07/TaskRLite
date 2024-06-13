namespace TaskRLite.Data
{
    public partial class Tag
    {
        public int Id { get; set; }

        public int? AppUserId { get; set; }

        public string Name { get; set; } = null!;

        public string HexColor { get; set; } = null!;

        public virtual AppUser? AppUser { get; set; }

        public virtual ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }

}
