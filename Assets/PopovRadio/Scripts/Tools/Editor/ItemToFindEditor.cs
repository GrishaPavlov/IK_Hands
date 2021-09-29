using UnityEngine;
using UnityEditor;
using PopovRadio.Scripts.Gameplay.HotAndCold;

namespace PopovRadio.Scripts.Tools.Editor
{
    [CustomEditor(typeof(ItemToFind))]
    public class ItemToFindEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var item = (ItemToFind) target;

            item.warm = EditorGUILayout.Slider(new GUIContent("Warm Radius", "Радиус, в котором будет 'тепло'"),
                item.warm, 0f, 3f);

            EditorUtility.SetDirty(target);
        }
    }
}