namespace HideItBobby.Properties
{
    internal static class ModProperties
    {
        public const string HarmonyId = "com.github.TheCSUser.HideItBobby";
        public const string VersionNumber = "1.21";
        public const string Name = "Hide it, Bobby!";
        public const string ShortName = "HideItBobby";
#if DEV
        public const string Stream = "Dev";
        public const string LongName = Name + " " + VersionNumber + " " + Stream;
#elif PREVIEW
        public const string Stream = "Preview";
        public const string LongName = Name + " " + VersionNumber + " " + Stream;
#else
        public const string Stream = "";
        public const string LongName = Name + " " + VersionNumber;
#endif
        public const string Description = "BOB compatible fork of Hide It!. This version does NOT allow to hide props. Please use BOB for that.";
    }
}
