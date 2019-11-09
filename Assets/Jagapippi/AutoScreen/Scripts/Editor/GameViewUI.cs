#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.UIElements;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using PopupWindow = UnityEngine.Experimental.UIElements.PopupWindow;

namespace Jagapippi.AutoScreen
{
    public static class GameViewUI
    {
        private static readonly GearImage gearImage = new GearImage();
        private static readonly SettingsWindow settingsWindow = new SettingsWindow();

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            GameViewEvent.opened += OnOpen;
            GameViewEvent.closed += OnClose;
            CurrentGameViewScreen.changed += OnScreenChanged;
            OnScreenChanged(CurrentGameViewScreen.value);
        }

        private static VisualElement GetRoot() => GameViewProxy.instance.GetRootVisualContainer();

        private static void OnOpen()
        {
            var root = GetRoot();

            if (gearImage.image.parent == null)
            {
                root.Add(gearImage.image);

                gearImage.style.positionType = PositionType.Absolute;
                gearImage.style.positionTop = 15;
                gearImage.style.positionLeft = 3;

                gearImage.image.RegisterCallback<MouseDownEvent>(e =>
                {
                    gearImage.image.visible = false;
                    settingsWindow.visible = true;

                    settingsWindow.deviceFrameToggle.Focus();
                });
            }

            if (settingsWindow.window.parent == null)
            {
                settingsWindow.AddTo(root);

                var style = settingsWindow.window.style;
                style.positionType = PositionType.Absolute;
                style.positionTop = 12;
                style.positionLeft = -4;

                settingsWindow.background.RegisterCallback<MouseDownEvent>(e =>
                {
                    gearImage.image.visible = true;
                    settingsWindow.visible = false;
                });
            }
        }

        private static void OnClose()
        {
        }

        private static void OnScreenChanged(GameViewScreen screen)
        {
            if (screen != null)
            {
                gearImage.image.visible = true;
                settingsWindow.visible = false;
            }
            else
            {
                gearImage.image.visible = false;
                settingsWindow.visible = false;
            }
        }

        private class GearImage
        {
            private static readonly Texture texture = EditorGUIUtility.IconContent("_Popup").image;

            public Image image { get; }
            public IStyle style => this.image.style;

            public GearImage()
            {
                this.image = new Image {image = texture};
                this.style.width = texture.width;
                this.style.height = texture.height;
            }
        }

        private class SettingsWindow
        {
            public VisualElement background { get; }
            public PopupWindow window { get; }
            public Toggle deviceFrameToggle { get; }
            public Toggle safeAreaBorderToggle { get; }

            public bool visible
            {
                set { this.background.visible = this.window.visible = value; }
            }

            public SettingsWindow()
            {
                this.background = new VisualElement();
                this.background.style.flexGrow = 1;

                this.window = new PopupWindow {text = "AutoScreen"};
                this.window.style.width = 140;

                this.deviceFrameToggle = new Toggle
                {
                    text = "Device Frame",
                    value = AutoScreenSettings.deviceFrame.enabled
                };
                this.deviceFrameToggle.OnValueChanged(e => AutoScreenSettings.deviceFrame.enabled = e.newValue);

                this.safeAreaBorderToggle = new Toggle
                {
                    text = "Safe Area Border",
                    value = AutoScreenSettings.safeAreaBorder.enabled
                };
                this.safeAreaBorderToggle.OnValueChanged(e => AutoScreenSettings.safeAreaBorder.enabled = e.newValue);

                this.window.Add(deviceFrameToggle);
                this.window.Add(safeAreaBorderToggle);
            }

            public void AddTo(VisualElement parent)
            {
                parent.Add(this.background);
                parent.Add(this.window);
            }
        }
    }
}
#endif