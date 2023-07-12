namespace sports_store_files.Infrastructure {
    public static class UrlExtensions {
        public static string PathAndQuery(this HttpRequest request) {
            if (request.QueryString.HasValue && !request.QueryString.Value.Contains("?returnUrl=%2F")) {
                return $"{request.Path}{request.QueryString}";
            }
            else return request.Path.ToString();
        }
    }
}
