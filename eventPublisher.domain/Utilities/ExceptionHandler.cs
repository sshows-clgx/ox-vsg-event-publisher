using System;
using System.Net;
using System.Threading.Tasks;
using eventPublisher.domain.exceptions;

namespace eventPublisher.domain.utilities
{
	public static class ExceptionHandler
	{
		/// <summary>
		/// Handles the exception asynchronous.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func">The function.</param>
		/// <returns></returns>
		public static async Task<Either<HttpStatusCodeErrorResponse, T>> HandleExceptionAsync<T>(Func<Task<T>> func)
		{
			var either = new EitherFactory<HttpStatusCodeErrorResponse, T>();
			try
			{
				return either.Create(await func().ConfigureAwait(false));
			}
			// catch (ValidationException ex)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			// }
			catch (NotFoundException ex)
			{
				return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.NotFound, ex.Message));
			}
			catch (NotAuthorizedException ex)
			{
				return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.Forbidden, ex.Message));
			}
			// catch (ConflictException ex)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.Conflict, ex.Message));
			// }
			// catch (InfrastructureException ex)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
			// }
			// catch (InvalidCredentialsException ex)
			// {
			// 	return either.Create(new HttpStatusCodeErrorResponse(HttpStatusCode.BadRequest, ex.Message));
			// }
		}
	}
}