namespace Esentis.Horudom.Web.Api.Helpers
{
	using System;
	using System.ComponentModel.DataAnnotations;

	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field)]
	public class PositiveNumberValidator : ValidationAttribute
	{
		public override bool IsValid(object value) =>
			   value is int input && input > 0;
	}
}
