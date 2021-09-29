using PopovRadio.Scripts.Characters.Popov;
using PopovRadio.Scripts.Tools;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Dialogs
{
    public class QuestionsHandler : ACommandsHandler<Question>
    {
        [SerializeField] private PlayerInfo playerInfo;
        [SerializeField] private PopovStateSystem popovStateSystem;

        protected override void HandleCommand(Question command)
        {
            var clip = command.AudioClip;
            if (command.ClipInHoldingObject)
            {
                if (!playerInfo.IsHoldingSomething) return;

                var holdingObject = playerInfo.RightHandObject ? playerInfo.RightHandObject : playerInfo.LeftHandObject;
                if (!holdingObject.TryGetComponent(out AudioClipContainer clipContainer)) return;
                clip = clipContainer.AudioClip;
            }

            popovStateSystem.StartSpeakingState();
            popovStateSystem.AppendAudioClipToQueue(clip);
            popovStateSystem.PlayQueue();

            base.HandleCommand(command);
        }
    }
}