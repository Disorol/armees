using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Services
{
    public class ExceptionValidationErrorHandler : IValidationErrorHandler
    {
        public void Handle(string message)
        {
            throw new Exception(message);
        }

        public Task HandleAsync(string message)
        {
            throw new Exception(message);
        }
    }
}
