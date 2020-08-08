namespace Esentis.Horudom.Web.Api.Helpers
{
	using global::Horudom.Data;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Logging;

	public abstract class BaseController<T> : ControllerBase
		where T : BaseController<T>
	{
		public BaseController(HorudomContext ctx, ILogger<T> logger)
		{
			Context = ctx;
			Logger = logger;
		}

		protected HorudomContext Context { get; }

		protected ILogger<T> Logger { get; }
	}
}
