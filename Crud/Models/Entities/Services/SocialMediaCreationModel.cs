namespace Crud.Models.Entities.Services
{
    public class SocialMediaCreationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Platforms { get; set; }
        public string ContentType { get; set; }
        public string Frequency { get; set; }
        public string Budget { get; set; }
        public string Duration { get; set; }
        public string Message { get; set; }
        public bool IsReplied { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
