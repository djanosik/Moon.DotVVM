// ReSharper disable once CheckNamespace

namespace DotVVM.Framework.Hosting
{
    public static class DotvvmRequestContextExtensions
    {
        /// <summary>
        /// Ends the request execution and returns the given status code to client.
        /// </summary>
        /// <param name="context">The DotVVM context.</param>
        /// <param name="statusCode">The status code to return.</param>
        public static void FailWithStatusCode(this IDotvvmRequestContext context, int statusCode)
        {
            context.HttpContext.Response.StatusCode = statusCode;
            throw new DotvvmInterruptRequestExecutionException();
        }
    }
}