using System.Collections;
using System.Collections.Generic;
using PopovRadio.Scripts.Tools.AppEvents;
using UnityEngine;

namespace PopovRadio.Scripts.Characters.Popov
{
    public class Speaking : PopovState
    {
        private readonly Queue<AudioClip> _clipQueue = new Queue<AudioClip>();

        public Speaking(PopovStateSystem stateSystem) : base(stateSystem)
        {
            StateSystem.AudioSource.Stop();
        }

        public override IEnumerator PlayQueue(AppEvent completeEvent)
        {
            while (_clipQueue.Count > 0)
            {
                var currentClip = _clipQueue.Dequeue();

                StateSystem.AudioSource.PlayOneShot(currentClip);

                yield return new WaitWhile(() => StateSystem.AudioSource.isPlaying);
                yield return new WaitForSeconds(0.5f);
            }

            completeEvent?.Invoke();
        }

        public override IEnumerator AppendClipToQueue(AudioClip clip)
        {
            _clipQueue.Enqueue(clip);
            yield break;
        }
    }
}