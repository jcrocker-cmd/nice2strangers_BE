namespace Crud.Models.Entities.Services
{
    public class WebAppModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string ProjectType { get; set; }
        public string Platform { get; set; }
        public string Budget { get; set; }
        public string Timeline { get; set; }
        public string Message { get; set; }
        public bool IsReplied { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
