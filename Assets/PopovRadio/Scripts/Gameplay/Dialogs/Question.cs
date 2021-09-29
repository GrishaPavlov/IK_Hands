using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Dialogs
{
    [CreateAssetMenu]
    public class Question : Command
    {
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private bool clipInHoldingObject;

        public AudioClip AudioClip => audioClip;
        public bool ClipInHoldingObject => clipInHoldingObject;
    }
}