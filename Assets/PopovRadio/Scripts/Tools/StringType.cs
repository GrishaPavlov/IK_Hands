using UnityEngine;

namespace PopovRadio.Scripts.Tools
{
    [CreateAssetMenu]
    public class StringType : ScriptableObject
    {
        [SerializeField] private string value;

        public string Value => value;
    }
}