namespace Crud.Models.Entities.Services
{
    public class DroneViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Subject { get; set; }
        public string ServiceType { get; set; }
        public string Location { get; set; }
        public string Budget { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }
        public bool? IsReplied { get; set; }
        public string? CreatedDate { get; set; }
    }
}
