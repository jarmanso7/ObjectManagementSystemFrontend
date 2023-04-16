using ObjectManagementSystemFrontend.DTOs;

namespace ObjectManagementSystemFrontend.Models
{
    /// <summary>
    ///     Represents a Relationship between two objects in the Object Management System. While <see cref="GeneralObjectDTO" /> matches <see cref="GeneralObject" /> perfectly field by field,
    ///     <see cref="Relationship" > is more complex than <see cref="RelationshipDTO" > because it includes full  <see cref="GeneralObject" /> instead of string references. This is necessary
    ///     to display the data properly in the UI DataGrid components.
    /// </summary>
    public class Relationship
	{
        public string Id { get; set; }
        public string Type { get; set; }
        public GeneralObject From { get; set; }
        public GeneralObject To { get; set; }
    }
}