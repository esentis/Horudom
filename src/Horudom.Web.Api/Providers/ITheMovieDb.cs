namespace Esentis.Horudom.Web.Api.Providers
{
	using System.Threading.Tasks;

	using Esentis.Horudom.Web.Api.Providers.Models;

	using Refit;

	[Headers("Authorization: Bearer")]
	public interface ITheMovieDb
	{
		[Get("/search/person")]
		Task<SearchResult<PersonSearch>> SearchPeople(string query, int page = 1);

		[Get("/person/{person_id}")]
		Task<Person> GetPerson(long person_id);
	}
}
