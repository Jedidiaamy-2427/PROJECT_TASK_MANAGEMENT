using System.Text.Json.Serialization;

namespace TaskManagerAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public User? User { get; set; }
        [JsonIgnore]
        public List<TaskItem> TaskItems { get; set; } = [];
    }
}
