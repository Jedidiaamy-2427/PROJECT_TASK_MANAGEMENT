using System.Text.Json.Serialization;

namespace TaskManagerAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Duration { get; set; }
        public int IsCompleted { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? ProjectId { get; set; } 
        [JsonIgnore]
        public Project? Project { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
