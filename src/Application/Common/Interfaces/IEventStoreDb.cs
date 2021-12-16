using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IEventStoreDb
{
    public Task Save(string type, string streamName, object entity, CancellationToken cancellationToken);
    public Task Read(object entity, CancellationToken cancellationToken);
}
