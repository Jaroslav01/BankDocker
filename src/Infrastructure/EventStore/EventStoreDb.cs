using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using EventStore.Client;

namespace CleanArchitecture.Infrastructure.EventStore;

public class EventStoreDb : IEventStoreDb
{
    private readonly static EventStoreClientSettings Settings = EventStoreClientSettings
    .Create("esdb://138.68.105.205:2113?tls=false");
    private readonly EventStoreClient Client = new(Settings);

    public async Task Save(string type, string streamName, object entity, CancellationToken cancellationToken)
    {
        var eventData = new EventData(
            Uuid.NewUuid(),
            type, // "TestEvent"
            JsonSerializer.SerializeToUtf8Bytes(entity)
        );
        await Client.AppendToStreamAsync(
            streamName, // "some-stream"
            StreamState.Any,
            new[] { eventData },
            cancellationToken: cancellationToken
        );
    }
    public async Task Read(object entity, CancellationToken cancellationToken)
    {
        
    }
}
