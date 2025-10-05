namespace Crud.Models.Entities.Services
{
    public class WebAppViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Subject { get; set; }
        public string ProjectType { get; set; }
        public List<string> Platform { get; set; }
        public string Budget { get; set; }
        public string Timeline { get; set; }
        public string Message { get; set; }
        public bool? IsReplied { get; set; }
        public string? CreatedDate { get; set; }
    }
}
