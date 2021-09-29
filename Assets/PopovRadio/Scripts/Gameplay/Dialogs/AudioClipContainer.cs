using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Dialogs
{
    public class AudioClipContainer : MonoBehaviour
    {
        [SerializeField] private AudioClip audioClip;

        public AudioClip AudioClip => audioClip;
    }
}