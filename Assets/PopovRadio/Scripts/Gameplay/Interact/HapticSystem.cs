using System;
using PopovRadio.Scripts.Gameplay.Hands;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.Interact
{
    public class HapticSystem : MonoBehaviour
    {
        #region Fields

        public static HapticSystem Instance;

        [Tooltip("Левый контроллер")] [SerializeField]
        private XRBaseController leftHandController;

        [Tooltip("Правый контроллер")] [SerializeField]
        private XRBaseController rightHandController;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Запускает вибрацию контроллера
        /// </summary>
        /// <param name="handSide">Тип руки (правая, левая)</param>
        /// <param name="amplitude">Мощность вибрации</param>
        /// <param name="duration">Продолжительность вибрации</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SendImpulse(HandType handSide, float amplitude, float duration)
        {
            switch (handSide)
            {
                case HandType.Left:
                    leftHandController.SendHapticImpulse(amplitude, duration);
                    break;
                case HandType.Right:
                    rightHandController.SendHapticImpulse(amplitude, duration);
                    break;
                case HandType.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(handSide), handSide, null);
            }
        }

        #endregion
    }
}