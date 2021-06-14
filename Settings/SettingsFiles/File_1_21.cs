using System.Xml.Serialization;

namespace HideItBobby.Settings.SettingsFiles
{
    [XmlType("ModConfig")]
    public sealed class File_1_21
    {
        [XmlIgnore]
        public bool Migrated;

        #region Language
        [XmlElement("UseGameLanguage")]
        public bool UseGameLanguage = true;
        [XmlElement("SelectedLanguage")]
        public string SelectedLanguage;
        #endregion

        #region Main menu features
        [XmlElement("HideMainMenuChirper")]
        public bool HideMainMenuChirper;
        [XmlElement("HideMainMenuDLCPanel")]
        public bool HideMainMenuDLCPanel;
        [XmlElement("HideMainMenuLogo")]
        public bool HideMainMenuLogo;
        [XmlElement("HideMainMenuNewsPanel")]
        public bool HideMainMenuNewsPanel;
        [XmlElement("HideMainMenuParadoxAccountPanel")]
        public bool HideMainMenuParadoxAccountPanel;
        [XmlElement("HideMainMenuVersionNumber")]
        public bool HideMainMenuVersionNumber;
        [XmlElement("HideMainMenuWorkshopPanel")]
        public bool HideMainMenuWorkshopPanel;
        #endregion

        #region In game features
        //Decorations
        [XmlElement("HideCliffDecorations")]
        public bool HideCliffDecorations;
        [XmlElement("HideFertileDecorations")]
        public bool HideFertileDecorations;
        [XmlElement("HideGrassDecorations")]
        public bool HideGrassDecorations;

        //Effects
        [XmlElement("HideOreArea")]
        public bool HideOreArea;
        [XmlElement("HideOilArea")]
        public bool HideOilArea;
        [XmlElement("HideSandArea")]
        public bool HideSandArea;
        [XmlElement("HideFertilityArea")]
        public bool HideFertilityArea;
        [XmlElement("HideForestArea")]
        public bool HideForestArea;
        [XmlElement("HideShoreArea")]
        public bool HideShoreArea;
        [XmlElement("HidePollutedArea")]
        public bool HidePollutedArea;
        [XmlElement("HideBurnedArea")]
        public bool HideBurnedArea;
        [XmlElement("HideDestroyedArea")]
        public bool HideDestroyedArea;
        [XmlElement("HidePollutionFog")]
        public bool HidePollutionFog;
        [XmlElement("HideVolumeFog")]
        public bool HideVolumeFog;
        [XmlElement("HideDistanceFog")]
        public bool HideDistanceFog;
        [XmlElement("HideEdgeFog")]
        public bool HideEdgeFog;

        //Ground and water color
        [XmlElement("DisableGrassFertilityGroundColor")]
        public bool DisableGrassFertilityGroundColor;
        [XmlElement("DisableGrassFieldGroundColor")]
        public bool DisableGrassFieldGroundColor;
        [XmlElement("DisableGrassForestGroundColor")]
        public bool DisableGrassForestGroundColor;
        [XmlElement("DisableGrassPollutionGroundColor")]
        public bool DisableGrassPollutionGroundColor;
        [XmlElement("DisableDirtyWaterColor")]
        public bool DisableDirtyWaterColor;

        //Objects
        [XmlElement("HideSeagulls")]
        public bool HideSeagulls;
        [XmlElement("HideWildlife")]
        public bool HideWildlife;

        //Ruining - Deprecated
        [XmlElement("HideTreeRuining")]
        public bool HideTreeRuining;
        [XmlElement("HidePropRuining")]
        public bool HidePropRuining;

        //In game ui elements
        [XmlElement("HideInfoViewsButton")]
        public bool HideInfoViewsButton;
        [XmlElement("HideDisastersButton")]
        public bool HideDisastersButton;
        [XmlElement("HideChirperButton")]
        public bool HideChirperButton;
        [XmlElement("HideRadioButton")]
        public bool HideRadioButton;
        [XmlElement("HideGearButton")]
        public bool HideGearButton;
        [XmlElement("HideZoomButton")]
        public bool HideZoomButton;
        [XmlElement("HideUnlockButton")]
        public bool HideUnlockButton;
        [XmlElement("HideAdvisorButton")]
        public bool HideAdvisorButton;
        [XmlElement("HideBulldozerButton")]
        public bool HideBulldozerButton;
        [XmlElement("HideCinematicCameraButton")]
        public bool HideCinematicCameraButton;
        [XmlElement("HideFreeCameraButton")]
        public bool HideFreeCameraButton;
        [XmlElement("HideCongratulationPanel")]
        public bool HideCongratulationPanel;
        [XmlElement("HideAdvisorPanel")]
        public bool HideAdvisorPanel;
        [XmlElement("HideTimePanel")]
        public bool HideTimePanel;
        [XmlElement("HideZoomAndUnlockBackground")]
        public bool HideZoomAndUnlockBackground;
        [XmlElement("HideSeparators")]
        public bool HideSeparators;
        [XmlElement("HideCityName")]
        public bool HideCityName;
        [XmlElement("HidePauseOutline")]
        public bool HidePauseOutline;
        [XmlElement("HideBulldozerBar")]
        public bool HideBulldozerBar;
        [XmlElement("HideThermometer")]
        public bool HideThermometer;
        [XmlElement("ModifyToolbarPosition")]
        public bool ModifyToolbarPosition;
        [XmlElement("ToolbarPosition")]
        public float ToolbarPosition;
        #endregion
    }
}