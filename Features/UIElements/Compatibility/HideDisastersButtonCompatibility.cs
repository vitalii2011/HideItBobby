namespace HideItBobby.Features.UIElements.Compatibility
{
    internal sealed class HideDisastersButtonCompatibility : CompatibilityCheckBase
    {
        #region Instance
        private static readonly Lazy<HideDisastersButtonCompatibility> _instance = new Lazy<HideDisastersButtonCompatibility>(() => new HideDisastersButtonCompatibility());
        public static HideDisastersButtonCompatibility Instance { get => _instance.Value; }
        #endregion

        protected override bool CheckCompatibility() => SteamHelper.IsDLCOwned(SteamHelper.DLC.NaturalDisastersDLC);
    }
}
