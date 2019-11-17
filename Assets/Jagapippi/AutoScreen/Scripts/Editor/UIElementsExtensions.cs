#if UNITY_EDITOR
using UnityEditor;
#if !UNITY_2019_1_OR_NEWER
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
#else
using UnityEngine.UIElements;
using PopupWindow = UnityEngine.UIElements.PopupWindow;
#endif

namespace Jagapippi.AutoScreen
{
    public static class UIElementsExtensions
    {
        public static VisualElement GetRootVisualElement(this EditorWindow editorWindow)
        {
#if !UNITY_2019_1_OR_NEWER
            return editorWindow.GetRootVisualContainer();
#else
            return editorWindow.rootVisualElement;
#endif
        }

        public static void SetPositionRelative(this IStyle style)
        {
#if !UNITY_2019_1_OR_NEWER
            style.positionType = PositionType.Relative;
#else
            style.position = Position.Relative;
#endif
        }

        public static void SetPositionAbsolute(this IStyle style)
        {
#if !UNITY_2019_1_OR_NEWER
            style.positionType = PositionType.Absolute;
#else
            style.position = Position.Absolute;
#endif
        }

        public static void SetTop(this IStyle style, float value)
        {
#if !UNITY_2019_1_OR_NEWER
            style.positionTop = value;
#else
            style.top = value;
#endif
        }

        public static void SetRight(this IStyle style, float value)
        {
#if !UNITY_2019_1_OR_NEWER
            style.positionRight = value;
#else
            style.right = value;
#endif
        }

        public static void SetBottom(this IStyle style, float value)
        {
#if !UNITY_2019_1_OR_NEWER
            style.positionBottom = value;
#else
            style.bottom = value;
#endif
        }

        public static void SetLeft(this IStyle style, float value)
        {
#if !UNITY_2019_1_OR_NEWER
            style.positionLeft = value;
#else
            style.left = value;
#endif
        }

#if !UNITY_2019_1_OR_NEWER
        public static void RegisterValueChangedCallback(this BaseField<bool> baseField, EventCallback<ChangeEvent<bool>> e)
        {
            baseField.OnValueChanged(e);
        }
#endif
    }
}
#endif