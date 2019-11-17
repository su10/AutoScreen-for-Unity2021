#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
#if !UNITY_2019_1_OR_NEWER
using UnityEngine.Experimental.UIElements;
using PopupWindow = UnityEngine.Experimental.UIElements.PopupWindow;
#else
using UnityEngine.UIElements;
using PopupWindow = UnityEngine.UIElements.PopupWindow;
#endif

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

        private static VisualElement GetRoot() => GameViewProxy.instance.GetRootVisualElement();
        private static VisualElement root;

        private static void OnOpen()
        {
            root = GetRoot();

            if (gearImage.image.parent == null)
            {
                root.Add(gearImage.image);

                var style = gearImage.style;
                style.SetPositionAbsolute();
                style.SetTop(15);
#if !UNITY_2019_1_OR_NEWER
                style.SetLeft(3);
#elif UNITY_2019_3_OR_NEWER
                style.SetTop(20);
#endif

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
                style.SetPositionAbsolute();
                style.SetTop(12);
#if !UNITY_2019_1_OR_NEWER
                style.SetLeft(-4);
#elif !UNITY_2019_3_OR_NEWER
                style.SetLeft(-6);
#else
                style.SetTop(21);
                style.SetLeft(1);
#endif

                settingsWindow.background.RegisterCallback<MouseDownEvent>(e =>
                {
                    gearImage.image.visible = true;
                    settingsWindow.visible = false;
                });
            }
        }

        private static void OnClose()
        {
            root.Remove(gearImage.image);
            root.Remove(settingsWindow.window);
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
                this.style.width = 15;
#if !UNITY_2019_3_OR_NEWER
                this.style.height = 14;
#else
                this.style.height = 15;
#endif
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
                this.deviceFrameToggle.RegisterValueChangedCallback(e => AutoScreenSettings.deviceFrame.enabled = e.newValue);

                this.safeAreaBorderToggle = new Toggle
                {
                    text = "Safe Area Border",
                    value = AutoScreenSettings.safeAreaBorder.enabled
                };
                this.safeAreaBorderToggle.RegisterValueChangedCallback(e => AutoScreenSettings.safeAreaBorder.enabled = e.newValue);

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