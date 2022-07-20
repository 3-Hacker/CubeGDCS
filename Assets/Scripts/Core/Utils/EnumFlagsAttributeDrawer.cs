using UnityEditor;
using UnityEngine;

namespace Core.Utils
{
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }
    }

    public class EnumFlagsAttribute : PropertyAttribute
    {
        public EnumFlagsAttribute()
        {
        }
    }
}
