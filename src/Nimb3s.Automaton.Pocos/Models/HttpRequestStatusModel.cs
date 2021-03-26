using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Pocos.Models
{
    public class HttpRequestStatusModel
    {
        public Guid Id { get; set; }
        public HttpRequestStatusType HttpRequestStatus { get; set; }
        public string Url { get; set; }
        public short StatusCode { get; set; }
        public string Body { get; set; }
    }
}
