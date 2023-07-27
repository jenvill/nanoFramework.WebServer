

namespace nanoFramework.WebServer
{
    public static class Extensions
    {
        public static string? TryGetValue(this UrlParameter[] urlParameters, string name)
        {
            foreach (var urlParameter in urlParameters)
                if (urlParameter.Name == name) return urlParameter.Value;
            return null;
        }
    }
}
