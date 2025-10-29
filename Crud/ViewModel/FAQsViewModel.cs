namespace Crud.ViewModel
{
    public class FAQsViewModel
    {
        public Guid Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool? isActive { get; set; }
        public string? CreatedDate { get; set; }
    }
}
