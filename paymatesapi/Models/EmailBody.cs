namespace paymatesapi.Models
{
    public class EmailBody 
    {
        public required string From { get; set; }

        public required string To { get; set; }

        public required string Subject { get; set; }
        
        public required string Body { get; set; }

    }
}