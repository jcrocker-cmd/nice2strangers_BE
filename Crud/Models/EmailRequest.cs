    namespace Crud.Models
    {
        public class EmailRequest
        {
            public Guid Id { get; set; }
            public string ToEmail { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public string Name { get; set; }
            public string CreatedDate { get; set; }
            public bool IsReplied { get; set; }
        }

    }
