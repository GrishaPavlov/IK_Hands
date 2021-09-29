using System.Collections;
using PopovRadio.Scripts.Common;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;

namespace PopovRadio.Scripts.Characters.Popov
{
    public class PopovState : IState
    {
        protected PopovStateSystem StateSystem;

        public PopovState(PopovStateSystem stateSystem)
        {
            StateSystem = stateSystem;
        }

        public virtual IEnumerator Start()
        {
            yield break;
        }

        public virtual IEnumerator AppendClipToQueue(AudioClip clip)
        {
            yield break;
        }
        
        public virtual IEnumerator PlayQueue(AppEvent completeEvent)
        {
            yield break;
        }
    }
}