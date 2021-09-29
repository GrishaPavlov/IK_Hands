using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.HotAndCold
{
    [CreateAssetMenu]
    public class HotAndColdTip : ScriptableObject
    {
        public AudioClip[] Colder;
        public AudioClip[] Warmer;
    }
}