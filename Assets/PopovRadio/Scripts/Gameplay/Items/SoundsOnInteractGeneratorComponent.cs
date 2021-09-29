using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Items
{
    public class SoundsOnInteractGeneratorComponent : MonoBehaviour
    {
        #region Settings

        [Tooltip("Маски объектов, при взаимодействии с которыми издаётся звук.")] [SerializeField]
        private LayerMask interactableMask;

        [Space] [Tooltip("Звук, издаваемый при взаимодействии предмета с другим объектом")] [SerializeField]
        private AudioClip audioClip;

        [Tooltip("Громкость звука, издаваемого при взаимодействии предмета с другим объектом")]
        [SerializeField]
        [Range(0, 1)]
        private float volume = 0.5f;

        #endregion

        #region LifeCycle

        private void OnCollisionEnter(Collision collision)
        {
            if (IsAudioClipNotNull() && IsIteractable(collision.gameObject))
            {
                PlayAudioClip();
            }
        }

        #endregion

        #region Methods

        private bool IsIteractable(GameObject other) => (interactableMask & 1 << other.layer) == 1 << other.layer;

        private bool IsAudioClipNotNull() => audioClip != null;

        private void PlayAudioClip() => AudioSource.PlayClipAtPoint(audioClip, transform.position, volume);

        #endregion
    }
}