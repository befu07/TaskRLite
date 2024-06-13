﻿namespace TaskRLite.Data
{

    public partial class AppUser
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public byte[] PasswordHash { get; set; } = null!;

        public byte[] Salt { get; set; } = null!;

        public DateTime RegisteredOn { get; set; }

        public string? Email { get; set; }

        public int AppRoleId { get; set; }

        public virtual AppUserRole AppRole { get; set; } = null!;

        public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();

        public virtual ICollection<ToDoList> ToDoLists { get; set; } = new List<ToDoList>();
    }
}
