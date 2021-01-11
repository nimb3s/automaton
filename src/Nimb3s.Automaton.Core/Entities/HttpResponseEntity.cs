﻿using Nimb3s.Data.Abstractions;
using System;

namespace Nimb3s.Automaton.Core.Entities
{
    public class HttpResponseEntity : IEntity<Guid>, IDisposable
    {
        public Guid Id { get; set; }
        public Guid HttpRequestId { get; set; }
        public int StatusCode { get; set; }
        public string Body { get; set; }

        public void Dispose()
        {
            
        }
    }
}