namespace HideItBobby.Common
{
    internal interface IToggleable
    {
        bool IsEnabled { get; }

        void Enable();
        void Disable();
    }

    internal interface IForceToggleable : IToggleable
    {
        void Enable(bool force);
        void Disable(bool force);
    }

    internal interface IToggleable<TResult>: IToggleable
    {
        new TResult Enable();
        new TResult Disable();
    }
    internal interface IForceToggleable<TResult> : IToggleable<TResult>, IForceToggleable
    {
        new TResult Enable(bool force);
        new TResult Disable(bool force);
    }
}