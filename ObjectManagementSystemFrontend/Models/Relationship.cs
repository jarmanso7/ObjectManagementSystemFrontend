namespace ObjectManagementSystemFrontend.Models
{
	public class Relationship
	{
        public string Id { get; set; }
        public string Type { get; set; }
        public string FromId { get; set; }
        public string ToId { get; set; }
    }
}