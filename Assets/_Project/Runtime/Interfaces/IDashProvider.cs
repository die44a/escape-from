public interface IDashProvider
{
    float DashProgress { get; }
    float RemainingDashProgress { get; }
    bool IsDashReady { get; }
}