namespace Esentis.Horudom.Web.Api.Helpers
{
	using System.Text;

	public static class StringExtensions
	{
		public static string NormalizeSearch(this string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return value;
			}

			var builder = new StringBuilder(value);

			return builder.ToString()
				.Normalize(NormalizationForm.FormC)
				.Trim()
				.ToUpperInvariant();
		}
	}
}
