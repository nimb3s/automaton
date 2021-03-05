using System;
using System.Collections.Generic;
using System.Text;

namespace Nimb3s.Automaton.Constants
{
    public static partial class AutomatonConstants
    {
        public static class MessageBus
        {
            public static class HttpRequestEndpoint
            {
                public const string ENDPOINT_NAME = "Nimb3s.Automaton.HttpRequest.Endpoint";
                /// <summary>
                /// If null, then max throughput is used
                /// </summary>
                public static readonly int? MessageProcessingConcurrency = 1;
                /// <summary>
                /// If null, then max throughput is used
                /// </summary>
                public static readonly int? RateLimitInSeconds = 30;
            }

            public static class JobEndpoint
            {
                public const string ENDPOINT_NAME = "Nimb3s.Automaton.Job.Endpoint";
            }            
        }
    }
}
