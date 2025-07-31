using System;
using Grpc.Net.Client;

namespace GrpcClient;

public class BaseGrpcClient(string? address = null) : IDisposable
{
    protected readonly GrpcChannel Channel = GrpcChannel.ForAddress(address ?? "http://localhost:9090");

    public void Dispose()
    {
        Channel.Dispose();
    }
}