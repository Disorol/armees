using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class LogicalOrganizerCommand
    {
        public ReactiveCommand<string, Unit>? Command { get; set; }
        public string? CommandParameter { get; set; } 
    }
}
