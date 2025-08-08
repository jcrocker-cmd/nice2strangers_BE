using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.ViewModel
{
    public class NewsletterViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string CreatedDate { get; set; }
    }
}
