﻿using ColossalFramework.UI;
using HideItBobby.Translation;
using ICities;
using System;
using System.Reflection;
using UnityEngine;

namespace HideItBobby.UserInterface
{
    internal static class UIHelperExtensions
    {
        #region UIHelper template consts
        private const string kCheckBoxTemplate = "OptionsCheckBoxTemplate";
        private const string kSliderTemplate = "OptionsSliderTemplate";
        private const string kDropdownTemplate = "OptionsDropdownTemplate";
        //private const string kTextfieldTemplate = "OptionsTextfieldTemplate";
        private const string kButtonTemplate = "OptionsButtonTemplate";
        private const string kGroupTemplate = "OptionsGroupTemplate";
        #endregion

        private static readonly Lazy<FieldInfo> rootField = new Lazy<FieldInfo>(() => typeof(UIHelper).GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance));

        public static UIComponent Root(this UIHelperBase helper) => (UIComponent)rootField.Value.GetValue(helper);

        public static TranslatedComponent<UILabel> AddLabel(this UIHelperBase helper, Template textTemplate, float textScale = 1.0f, Color32? textColor = null, Action<UILabel, string> onTranslate = null)
        {
            var uiLabel = helper.Root().AddUIComponent<UILabel>();
            uiLabel.autoSize = true;
            uiLabel.autoHeight = false;
            uiLabel.wordWrap = false;
            uiLabel.textScale = textScale;
            uiLabel.text = textTemplate?.Translate() ?? "";
            if (textColor.HasValue) uiLabel.textColor = textColor.Value;
            if (!(onTranslate is null)) onTranslate(uiLabel, Language.Key);
            return new TranslatedComponent<UILabel>(uiLabel, (component, languageKey) =>
            {
                if (!(textTemplate is null)) component.text = textTemplate.Translate(languageKey);
                if (!(onTranslate is null)) onTranslate(component, languageKey);
            });
        }

        public static TranslatedComponent<UICheckBox> AddCheckbox(this UIHelperBase helper, Template textTemplate, bool value, Action<UICheckBox, bool> eventCallback, float textScale = 1.0f, Color32? textColor = null, bool enabled = true, Action<UICheckBox, string> onTranslate = null)
        {
            var uiCheckBox = helper.Root().AttachUIComponent(UITemplateManager.GetAsGameObject(kCheckBoxTemplate)) as UICheckBox;
            var uiLabel = uiCheckBox.label;
            uiLabel.autoSize = true;
            uiLabel.autoHeight = false;
            uiLabel.wordWrap = false;
            uiLabel.textScale = textScale;
            uiLabel.text = textTemplate?.Translate() ?? "";
            if (textColor.HasValue) uiLabel.textColor = textColor.Value;
            uiCheckBox.isChecked = value;
            uiCheckBox.eventCheckChanged += (c, isChecked) => eventCallback((UICheckBox)c, isChecked);
            uiCheckBox.isEnabled = enabled;
            if (!(onTranslate is null)) onTranslate(uiCheckBox, Language.Key);
            return new TranslatedComponent<UICheckBox>(uiCheckBox, (component, languageKey) =>
            {
                if (!(textTemplate is null)) component.label.text = textTemplate.Translate(languageKey);
                if (!(onTranslate is null)) onTranslate(component, languageKey);
            });
        }

        public static TranslatedComponent<UIButton> AddButton(this UIHelperBase helper, Template textTemplate, Action<UIButton> eventCallback, float textScale = 1.0f, Color32? textColor = null, bool enabled = true, Action<UIButton, string> onTranslate = null)
        {
            var uiButton = helper.Root().AttachUIComponent(UITemplateManager.GetAsGameObject(kButtonTemplate)) as UIButton;
            uiButton.text = textTemplate?.Translate() ?? "";
            uiButton.autoSize = true;
            uiButton.wordWrap = false;
            uiButton.textScale = textScale;
            if (textColor.HasValue) uiButton.textColor = textColor.Value;
            uiButton.eventClick += (c, sel) => eventCallback((UIButton)c);
            uiButton.isEnabled = enabled;
            if (!(onTranslate is null)) onTranslate(uiButton, Language.Key);
            return new TranslatedComponent<UIButton>(uiButton, (component, languageKey) =>
            {
                if (!(textTemplate is null)) uiButton.text = textTemplate.Translate(languageKey);
                if (!(onTranslate is null)) onTranslate(uiButton, languageKey);
            });
        }

        public static TranslatedComponent<UIPanel, UIDropDown> AddDropdown(this UIHelperBase helper, Template textTemplate, string[] options, int defaultSelection, Action<UIDropDown, int> eventCallback, float textScale = 1.0f, Color32? textColor = null, Color32? color = null, bool enabled = true, Action<UIPanel, string> onTranslate = null)
        {
            var uiPanel = helper.Root().AttachUIComponent(UITemplateManager.GetAsGameObject(kDropdownTemplate)) as UIPanel;
            var uiLabel = uiPanel.Find<UILabel>("Label");
            uiLabel.autoSize = true;
            uiLabel.autoHeight = false;
            uiLabel.wordWrap = false;
            uiLabel.textScale = textScale;
            uiLabel.text = textTemplate?.Translate() ?? "";
            if (textColor.HasValue) uiLabel.textColor = textColor.Value;
            var uiDropDown = uiPanel.Find<UIDropDown>("Dropdown");
            uiDropDown.items = options;
            uiDropDown.selectedIndex = defaultSelection;
            uiDropDown.eventSelectedIndexChanged += (c, sel) => eventCallback((UIDropDown)c, sel);
            if (textColor.HasValue) uiDropDown.textColor = textColor.Value;
            if (color.HasValue) uiDropDown.color = color.Value;
            uiDropDown.isInteractive = enabled;
            if (!(onTranslate is null)) onTranslate(uiPanel, Language.Key);
            return new TranslatedComponent<UIPanel, UIDropDown>(uiPanel, uiDropDown, (parent, component, languageKey) =>
            {
                if (!(textTemplate is null)) uiLabel.text = textTemplate.Translate(languageKey);
                if (!(onTranslate is null)) onTranslate(parent, languageKey);
            });
        }

        public static TranslatedComponent<UIPanel, UISlider> AddSlider(this UIHelperBase helper, Template textTemplate, float minValue, float maxValue, float stepSize, float value, Action<UIPanel, float> eventCallback, float textScale = 1.0f, float width = 227.0f, Color32? textColor = null, bool enabled = true, Action<UIPanel, string> onTranslate = null)
        {
            UIPanel uiPanel = helper.Root().AttachUIComponent(UITemplateManager.GetAsGameObject(kSliderTemplate)) as UIPanel;
            var uiSlider = uiPanel.Find<UISlider>("Slider");
            var uiLabel = uiPanel.Find<UILabel>("Label");
            uiLabel.autoSize = true;
            uiLabel.autoHeight = false;
            uiLabel.wordWrap = false;
            uiLabel.textScale = textScale;
            uiLabel.text = textTemplate?.Translate() ?? "";
            uiLabel.isVisible = !(textTemplate is null);
            if (textColor.HasValue) uiLabel.textColor = textColor.Value;
            uiSlider.minValue = minValue;
            uiSlider.maxValue = maxValue;
            uiSlider.stepSize = stepSize;
            uiSlider.value = value;
            uiSlider.width = width;
            uiSlider.eventValueChanged += (c, isChecked) => eventCallback(uiPanel, isChecked);
            uiSlider.isEnabled = enabled;
            if (!(onTranslate is null)) onTranslate(uiPanel, Language.Key);
            return new TranslatedComponent<UIPanel, UISlider>(uiPanel, uiSlider, (parent, component, languageKey) =>
            {
                if (!(textTemplate is null)) uiLabel.text = textTemplate.Translate(languageKey);
                if (!(onTranslate is null)) onTranslate(parent, languageKey);
            });
        }

        public static TranslatedGroup AddGroup(this UIHelperBase helper, Template textTemplate, float textScale = 1.0f, Color32? textColor = null, Action<UIPanel, string> onTranslate = null)
        {
            var uiPanel = helper.Root().AttachUIComponent(UITemplateManager.GetAsGameObject(kGroupTemplate)) as UIPanel;
            var uiLabel = uiPanel.Find<UILabel>("Label");
            uiLabel.textScale = textScale;
            uiLabel.text = textTemplate?.Translate() ?? "";
            if (textColor.HasValue) uiLabel.textColor = textColor.Value;
            return new TranslatedGroup(new UIHelper(uiPanel.Find("Content")), uiPanel, (component, languageKey) =>
            {
                if (!(textTemplate is null)) uiLabel.text = textTemplate.Translate(languageKey);
                if (!(onTranslate is null)) onTranslate(component, languageKey);
            });
        }

        public static object AddSpace(this TranslatedGroup group, int height)
            => group.UIHelper.AddSpace(height);

        public static TranslatedComponent<UILabel> AddLabel(this TranslatedGroup group, Template textTemplate, float textScale = 1.0f, Color32? textColor = null, Action<UILabel, string> onTranslate = null)
            => AddLabel(group.UIHelper, textTemplate, textScale, textColor, onTranslate);

        public static TranslatedComponent<UICheckBox> AddCheckbox(this TranslatedGroup group, Template textTemplate, bool value, Action<UICheckBox, bool> eventCallback, float textScale = 1.0f, Color32? textColor = null, bool enabled = true, Action<UICheckBox, string> onTranslate = null)
            => AddCheckbox(group.UIHelper, textTemplate, value, eventCallback, textScale, textColor, enabled, onTranslate);

        public static TranslatedComponent<UIButton> AddButton(this TranslatedGroup group, Template textTemplate, Action<UIButton> eventCallback, float textScale = 1.0f, Color32? textColor = null, bool enabled = true, Action<UIButton, string> onTranslate = null)
            => AddButton(group.UIHelper, textTemplate, eventCallback, textScale, textColor, enabled, onTranslate);

        public static TranslatedComponent<UIPanel, UIDropDown> AddDropdown(this TranslatedGroup group, Template textTemplate, string[] options, int defaultSelection, Action<UIDropDown, int> eventCallback, float textScale = 1.0f, Color32? textColor = null, Color32? color = null, bool enabled = true, Action<UIPanel, string> onTranslate = null)
            => AddDropdown(group.UIHelper, textTemplate, options, defaultSelection, eventCallback, textScale, textColor, color, enabled, onTranslate);

        public static TranslatedComponent<UIPanel, UISlider> AddSlider(this TranslatedGroup group, Template textTemplate, float minValue, float maxValue, float stepSize, float value, Action<UIPanel, float> eventCallback, float textScale = 1.0f, float width = 227.0f, Color32? textColor = null, bool enabled = true, Action<UIPanel, string> onTranslate = null)
            => AddSlider(group.UIHelper, textTemplate, minValue, maxValue, stepSize, value, eventCallback, textScale, width, textColor, enabled, onTranslate);

    }
}