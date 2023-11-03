using System.Net;

namespace TgyMiddlewareExampleWeb.Middleware
{
    public class WhiteIpAdressControlMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private const string WhiteIpAdress = "::2"; //::1

        public WhiteIpAdressControlMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //IPv4 -> 127.0.01 -> localhost
            //IPv6 -> ::1 -> localhost

            var reqIpAdress = context.Connection.RemoteIpAddress;

            bool anyWhiteIpAdress = IPAddress.Parse(WhiteIpAdress).Equals(reqIpAdress);
            
            if (anyWhiteIpAdress)
            {
                await _requestDelegate(context);
            }
            else
            {
                context.Response.StatusCode =HttpStatusCode.Forbidden.GetHashCode();
                await context.Response.WriteAsync("Forbidden");
            }
        }
    }
}
