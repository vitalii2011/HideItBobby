namespace HideItBobby.Features.UIElements.Compatibility
{
    internal sealed class HideThermometerCompatibility : CompatibilityCheckBase
    {
        #region Instance
        private static readonly Lazy<HideThermometerCompatibility> _instance = new Lazy<HideThermometerCompatibility>(() => new HideThermometerCompatibility());
        public static HideThermometerCompatibility Instance { get => _instance.Value; }
        #endregion

        protected override bool CheckCompatibility() => SteamHelper.IsDLCOwned(SteamHelper.DLC.SnowFallDLC);
    }
}
