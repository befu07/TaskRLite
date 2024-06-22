using TaskRLite.Data;

//namespace TaskRLite.Views.Admin
namespace TaskRLite.Models
{
    public class UserOverViewVm
    {
        public List<AppUser> AppUsers { get; set; }
        public Dictionary<int,string> AppRoleDict { get; set; } = new();
    }
}
