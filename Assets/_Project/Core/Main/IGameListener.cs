namespace _Project.Core.Runtime.Core.Main
{
    public interface IGameListener
    {
    }

    public interface IPauseStartListener : IGameListener
    {
        void OnStartGame();
    }
    
    public interface IFinishGameListener : IGameListener
    {
        void OnFinishGame();
    }
    
    public interface IPauseGameListener : IGameListener
    {
        void OnPauseGame();
    }

    public interface IResumeGameListener : IGameListener
    {
        void OnResumeGame();
    }
}