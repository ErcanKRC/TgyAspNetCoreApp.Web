namespace TgyAspNetCoreApp.Web.Helpers
{
    public class Helper : IHelper
    {
        public string Upper(string value)
        {
            return value.ToUpper();
        }
    }
}
