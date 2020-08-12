namespace Esentis.Horudom.Web.Api.Helpers
{
	public static class HorudomLogTemplates
	{
		public const string CreatedEntity = "Created {Entity} {@Value}";

		public const string RequestEntity = "User requested {Entity} with ID {Id}";

		public const string RequestEntities = "User requested collection of {Entity}. Found {Count} records";

		public const string Conflict = "{Entity} with {Id} has assignments";

		public const string Deleted = "{Entity} with {Id} has been deleted";

		public const string Updated = "{Entity} has been updated";
	}
}
