namespace Nimb3s.Automaton.Pocos
{
    public class AuthResponseBase
    {

    }

    public class OAuth20AuthResponse : AuthResponseBase
    {
        public string access_token { get; set; }
        public long expires_in { get; set; }
        public string refresh_token { get; set; }
    }

    public class OAuth20AuthenticationConfig : HttpAuthenticationOptions
    {
        public OAuth20HttpRequestConfig HttpRequestConfig { get; set; }

        public OAuth20GrantBase Grant { get; set; }
    }

    public class OAuth20HttpRequestConfig
    {
        public string AuthUrl { get; set; }
        public string RevocationUrl { get; set; }
        public HttpRequestContentType ContentType { get; set; }
    }

    public class OAuth20GrantBase
    {
        public GrantType GrantType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }
    }

    public class OAuth20ClientGrant : OAuth20GrantBase
    {

    }

    public class OAuth20PasswordGrant : OAuth20GrantBase
    {
        public string Username { get; set; }
        public string UserPassword { get; set; }
    }
}
