using CitiesHarmony.API;
using HarmonyLib;
using ICities;
using System.Reflection;

namespace HideItBobby
{
    public class ModInfo : IUserMod
    {
        private const string HarmonyId = "local.csl.HideItBobbyby";

        public string Name => "Hide it, Bobby!";
        public string Description => "BOB compatible fork of Hide It!. This version does NOT allow to hide props. Please use BOB for that.";

        private static bool patched = false;

        public void OnEnabled()
        {
            if (!patched)
            {
                var harmony = new Harmony(HarmonyId);
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
        }

        public void OnDisabled()
        {
            if (patched) { }
            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            UIHelperBase group;
            bool selected;

            group = helper.AddGroup(Name);

            selected = ModConfig.Instance.InfoViewsButton;
            group.AddCheckbox("Info Views Button", selected, sel =>
            {
                ModConfig.Instance.InfoViewsButton = sel;
                ModConfig.Instance.Save();
            });

            if (SteamHelper.IsDLCOwned(SteamHelper.DLC.NaturalDisastersDLC))
            {
                selected = ModConfig.Instance.DisastersButton;
                group.AddCheckbox("Disasters Button", selected, sel =>
                {
                    ModConfig.Instance.DisastersButton = sel;
                    ModConfig.Instance.Save();
                });
            }

            selected = ModConfig.Instance.ChirperButton;
            group.AddCheckbox("Chirper Button", selected, sel =>
            {
                ModConfig.Instance.ChirperButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.RadioButton;
            group.AddCheckbox("Radio Button", selected, sel =>
            {
                ModConfig.Instance.RadioButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.GearButton;
            group.AddCheckbox("Gear Button", selected, sel =>
            {
                ModConfig.Instance.GearButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.ZoomButton;
            group.AddCheckbox("Zoom Button", selected, sel =>
            {
                ModConfig.Instance.ZoomButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.UnlockButton;
            group.AddCheckbox("Unlock Button", selected, sel =>
            {
                ModConfig.Instance.UnlockButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.AdvisorButton;
            group.AddCheckbox("Advisor Button", selected, sel =>
            {
                ModConfig.Instance.AdvisorButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.BulldozerButton;
            group.AddCheckbox("Bulldozer Button", selected, sel =>
            {
                ModConfig.Instance.BulldozerButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.CinematicCameraButton;
            group.AddCheckbox("Cinematic Camera Button", selected, sel =>
            {
                ModConfig.Instance.CinematicCameraButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.FreeCameraButton;
            group.AddCheckbox("Free Camera Button", selected, sel =>
            {
                ModConfig.Instance.FreeCameraButton = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.CongratulationPanel;
            group.AddCheckbox("Congratulation Panel", selected, sel =>
            {
                ModConfig.Instance.CongratulationPanel = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.AdvisorPanel;
            group.AddCheckbox("Advisor Panel", selected, sel =>
            {
                ModConfig.Instance.AdvisorPanel = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.TimePanel;
            group.AddCheckbox("Time Panel", selected, sel =>
            {
                ModConfig.Instance.TimePanel = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.ZoomAndUnlockBackground;
            group.AddCheckbox("Zoom and Unlock Background", selected, sel =>
            {
                ModConfig.Instance.ZoomAndUnlockBackground = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.Separators;
            group.AddCheckbox("Separators in Toolbar", selected, sel =>
            {
                ModConfig.Instance.Separators = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Objects & Props");

            selected = ModConfig.Instance.Seagulls;
            group.AddCheckbox("Seagulls", selected, sel =>
            {
                ModConfig.Instance.Seagulls = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.Wildlife;
            group.AddCheckbox("Wildlife", selected, sel =>
            {
                ModConfig.Instance.Wildlife = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Sprites");

            selected = ModConfig.Instance.CliffDecorations;
            group.AddCheckbox("Cliff Decorations", selected, sel =>
            {
                ModConfig.Instance.CliffDecorations = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.GrassDecorations;
            group.AddCheckbox("Grass Decorations", selected, sel =>
            {
                ModConfig.Instance.GrassDecorations = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.FertileDecorations;
            group.AddCheckbox("Fertile Decorations", selected, sel =>
            {
                ModConfig.Instance.FertileDecorations = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Ground and Water Colors");

            selected = ModConfig.Instance.GrassFertilityGroundColor;
            group.AddCheckbox("Grass Fertility Ground Color", selected, sel =>
            {
                ModConfig.Instance.GrassFertilityGroundColor = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.GrassFieldGroundColor;
            group.AddCheckbox("Grass Field Ground Color", selected, sel =>
            {
                ModConfig.Instance.GrassFieldGroundColor = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.GrassForestGroundColor;
            group.AddCheckbox("Grass Forest Ground Color", selected, sel =>
            {
                ModConfig.Instance.GrassForestGroundColor = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.GrassPollutionGroundColor;
            group.AddCheckbox("Grass Pollution Ground Color", selected, sel =>
            {
                ModConfig.Instance.GrassPollutionGroundColor = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.DirtyWaterColor;
            group.AddCheckbox("Dirty Water Color", selected, sel =>
            {
                ModConfig.Instance.DirtyWaterColor = sel;
                ModConfig.Instance.Save();
            });

            group = helper.AddGroup("Effects");

            selected = ModConfig.Instance.OreArea;
            group.AddCheckbox("Ore Area", selected, sel =>
            {
                ModConfig.Instance.OreArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.OilArea;
            group.AddCheckbox("Oil Area", selected, sel =>
            {
                ModConfig.Instance.OilArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.SandArea;
            group.AddCheckbox("Sand Area", selected, sel =>
            {
                ModConfig.Instance.SandArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.FertilityArea;
            group.AddCheckbox("Fertility Area", selected, sel =>
            {
                ModConfig.Instance.FertilityArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.ForestArea;
            group.AddCheckbox("Forest Area", selected, sel =>
            {
                ModConfig.Instance.ForestArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.ShoreArea;
            group.AddCheckbox("Shore Area", selected, sel =>
            {
                ModConfig.Instance.ShoreArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.PollutedArea;
            group.AddCheckbox("Polluted Area", selected, sel =>
            {
                ModConfig.Instance.PollutedArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.BurnedArea;
            group.AddCheckbox("Burned Area", selected, sel =>
            {
                ModConfig.Instance.BurnedArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.DestroyedArea;
            group.AddCheckbox("Destroyed Area", selected, sel =>
            {
                ModConfig.Instance.DestroyedArea = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.PollutionFog;
            group.AddCheckbox("Pollution Fog", selected, sel =>
            {
                ModConfig.Instance.PollutionFog = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.VolumeFog;
            group.AddCheckbox("Volume Fog", selected, sel =>
            {
                ModConfig.Instance.VolumeFog = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.DistanceFog;
            group.AddCheckbox("Distance Fog", selected, sel =>
            {
                ModConfig.Instance.DistanceFog = sel;
                ModConfig.Instance.Save();
            });

            selected = ModConfig.Instance.EdgeFog;
            group.AddCheckbox("Edge Fog", selected, sel =>
            {
                ModConfig.Instance.EdgeFog = sel;
                ModConfig.Instance.Save();
            });
        }
    }
}