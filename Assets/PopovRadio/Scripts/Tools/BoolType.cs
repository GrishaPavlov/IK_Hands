using UnityEngine;

namespace PopovRadio.Scripts.Tools
{
    [CreateAssetMenu]
    public class BoolType : ScriptableObject
    {
        [SerializeField] private bool value;

        public bool Value
        {
            get => value;
            set => this.value = value;
        }
    }
}