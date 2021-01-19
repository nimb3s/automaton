using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nimb3s.Automaton.Pocos.Models
{
    public class HttpRequestBaseModel
    {
        [Required]
        public string Url { get; set; }
        [Required]
        public string ContentType { get; set; }
        [Required]
        public string Method { get; set; }
        public string Content { get; set; }
        public string UserAgent { get; set; }
        public Dictionary<string, string> RequestHeaders { get; set; }
        public Dictionary<string, string> ContentHeaders { get; set; }
        [Required]
        public HttpAuthenticationConfig AuthenticationConfig { get; set; }
    }
}
