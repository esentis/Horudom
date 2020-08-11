namespace Esentis.Horudom.Web.Api.Helpers
{
	public static class HorudomLogTemplates
	{
		public const string CreatedEntity = "Created {Entity} {@Value}";

		public const string FoundEntity = "Found {Entity} {@Value}";

		public const string FoundEntities = "List of {Entity}s : {@Value}";

		public const string Conflict = "{Entity} with {Id} has assignments";

		public const string Deleted = "{Entity} with {Id} has been deleted";

		public const string Updated = "{Entity} has been updated to {@NewEntity}";
	}
}
