using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Services
{
    public interface IValidationErrorHandler
    {
        void Handle(string message);
        Task HandleAsync(string message);
    }
}
