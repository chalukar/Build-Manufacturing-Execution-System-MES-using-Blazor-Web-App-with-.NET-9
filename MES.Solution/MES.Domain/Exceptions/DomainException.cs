using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Domain.Exceptions
{
    public class DomainException(string message) : Exception(message);
    public class NotFoundException(string entity, object id)
        : Exception($"{entity} with id '{id}' was not found.");
}
