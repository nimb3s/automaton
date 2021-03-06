﻿using Nimb3s.Data.Abstractions;
using System;

namespace Nimb3s.Automaton.Core.Entities
{
    public class HttpRequestEntity : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid WorkItemId { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public string Method { get; set; }
        public string Content { get; set; }
        public string UserAgent { get; set; }
        public string RequestHeadersInJson { get; set; }
        public string ContentHeadersInJson { get; set; }
        public string AuthenticationConfigInJson { get; set; }
        public DateTimeOffset InsertTimeStamp { get; set; }
    }
}
