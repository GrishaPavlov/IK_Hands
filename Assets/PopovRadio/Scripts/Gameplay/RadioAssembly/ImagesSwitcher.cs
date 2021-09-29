using System;
using UnityEngine;
using UnityEngine.UI;

namespace PopovRadio.Scripts.Gameplay.RadioAssembly
{
    public class ImagesSwitcher : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Sprite[] images;
        [SerializeField] private Image imageComponent;

        private int _currentIndex;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            SetNextImage();
            if (images.Length > 0) imageComponent.enabled = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Переключает изображение на следующее
        /// </summary>
        public void SetNextImage()
        {
            try
            {
                imageComponent.sprite = images[_currentIndex++];
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogWarning($"{name}: Изображения закончились");
            }
        }

        #endregion
    }
}