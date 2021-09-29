using System;
using PopovRadio.Scripts.Characters.Popov;
using PopovRadio.Scripts.Tools;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.CollectItems
{
    public class CollectItem : MonoBehaviour
    {
        [SerializeField] private StringType uiName;
        [SerializeField] private GameObjectListSet itemsSet;
        [SerializeField] private PopovStateSystem popovStateSystem;
        [SerializeField] private AudioClip placeTipAudio;

        public string UIName => uiName.Value;

        private void Awake()
        {
            itemsSet.Add(gameObject);
        }

        public void PlayPlaceTip()
        {
            popovStateSystem.StartSpeakingState();
            popovStateSystem.AppendAudioClipToQueue(placeTipAudio);
            popovStateSystem.PlayQueue();
        }
    }
}