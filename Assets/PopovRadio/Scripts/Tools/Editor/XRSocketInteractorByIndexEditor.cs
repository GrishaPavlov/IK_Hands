using PopovRadio.Scripts.Gameplay.RadioAssembly;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEditor.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Tools.Editor
{
    [CustomEditor(typeof(XRSocketInteractorByIndex))]
    public class XRSocketInteractorByIndexEditor : XRSocketInteractorEditor
    {
        private SerializedProperty _targetIndex;

        protected override void OnEnable()
        {
            base.OnEnable();
            _targetIndex = serializedObject.FindProperty("index");
        }

        protected override void DrawProperties()
        {
            base.DrawProperties();
            EditorGUILayout.PropertyField(_targetIndex);
        }
    }
}