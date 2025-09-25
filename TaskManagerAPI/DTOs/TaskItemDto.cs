namespace TaskManagerAPI.Dtos
{
    public class TaskItemDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public DateTime Duration { get; set; }
        public int UserId { get; set; }
    }
}
