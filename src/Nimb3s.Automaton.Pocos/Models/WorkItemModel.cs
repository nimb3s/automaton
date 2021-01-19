using System;
using System.ComponentModel.DataAnnotations;

namespace Nimb3s.Automaton.Pocos.Models
{
    public class WorkItemModelBase
    {
        /// <summary>
        /// The automation job id.
        /// </summary>
        [Required]
        public Guid JobId { get; set; }

    }
}
