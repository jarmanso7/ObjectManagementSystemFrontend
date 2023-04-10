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

/*
    "id": "2c285894-ca6e-4e35-87e1-816f9d5139a2",
    "label": "knows",
    "type": "edge",
    "inVLabel": "person",
    "outVLabel": "person",
    "inV": "thomas",
    "outV": "xavi"
*/