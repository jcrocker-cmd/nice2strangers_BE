using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models.Entities
{
    public class SocialLinks
    {
        [Key]
        public int Id { get; set; }

        [Column("Facebook_Url")]
        public string Facebook { get; set; }

        [Column("Twitter_Url")]
        public string Twitter { get; set; }

        [Column("Instagram_Url")]
        public string Instagram { get; set; }

        [Column("Youtube_Url")]
        public string Youtube { get; set; }

        [Column("Tiktok_Url")]
        public string Tiktok { get; set; }
    }
}
