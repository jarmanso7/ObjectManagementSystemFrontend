﻿namespace ObjectManagementSystemFrontend.Models
{
	public class Relationship
	{
        public string Id { get; set; }
        public string Type { get; set; }
        public GeneralObject From { get; set; }
        public GeneralObject To { get; set; }
    }
}