using System;

public interface IDashProvider
{
    float DashProgress { get; }
    float RemainingDashProgress { get; }
    bool IsDashReady { get; }
    event Action OnDashFailed;
}