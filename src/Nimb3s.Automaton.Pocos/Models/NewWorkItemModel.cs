using System.Collections.Generic;

namespace Nimb3s.Automaton.Pocos.Models
{
    /// <summary>
    /// An automation job work item. A work item is composed of one or more HTTP Requests that neeed to be executed
    /// </summary>
    public class NewWorkItemModel : WorkItemModelBase
    {
        /// <summary>
        /// The collection of <see cref="NewHttpRequestModel"/> to exeute.
        /// </summary>
        //[Required]
        public IEnumerable<NewHttpRequestModel> HttpRequests { get; set; }
    }
}
