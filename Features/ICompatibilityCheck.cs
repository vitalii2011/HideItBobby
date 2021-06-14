namespace HideItBobby.Features
{
    internal interface ICompatibilityCheck
    {
        bool IsCompatible { get; }

        void Reset();
    }
}
