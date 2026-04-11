namespace _Project.Runtime.Menu.Main
{
    public interface IMenuListener
    {
    }

    public interface IGameStartListener : IMenuListener
    {
        void OnGameStart();
    }
}