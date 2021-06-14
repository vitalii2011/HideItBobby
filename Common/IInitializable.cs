namespace HideItBobby.Common
{
    internal interface IInitializable
    {
        bool IsInitialized { get; }

        void Initialize();
        void Terminate();
    }
    internal interface IInitializable<TResult>: IInitializable
    {
        new TResult Initialize();
        new TResult Terminate();
    }
}