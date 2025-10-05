namespace Crud.Models.Entities.Services
{
    public class SocialMediaConsultingViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Subject { get; set; }
        public List<string> Platforms { get; set; }
        public string Goals { get; set; }
        public string Budget { get; set; }
        public string Duration { get; set; }
        public string Message { get; set; }
        public bool? IsReplied { get; set; }
        public string? CreatedDate { get; set; }
    }
}
