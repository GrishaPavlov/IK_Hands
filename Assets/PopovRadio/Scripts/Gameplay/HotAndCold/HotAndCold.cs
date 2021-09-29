using System;
using System.Collections.Generic;
using PopovRadio.Scripts.Characters.Popov;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace PopovRadio.Scripts.Gameplay.HotAndCold
{
    public class HotAndCold : MonoBehaviour
    {
        #region Settings

        [Tooltip("Объект игровой камеры")] [SerializeField]
        private Transform playerCamera;

        [Tooltip("Максимальный угол для проверки, находится ли искомый предмет в поле зрения")] [SerializeField]
        private float angleTreshold = 30;

        [Tooltip("Расстояние, при преодолении которого вызывается ответ")] [SerializeField]
        private float distanceDelta = 1;

        [SerializeField] private PopovStateSystem popovStateSystem;

        [SerializeField] private HotAndColdTip coldTips;
        [SerializeField] private HotAndColdTip warmTips;

        #endregion

        #region Fields

        private float _previousDistance;
        private bool _previousLookingState;

        private ItemToFind _currentItemToFind;

        #endregion

        #region Properties

        private Vector3 ItemPosition => _currentItemToFind.transform.position;

        private float DistanceToItem => playerCamera ? Vector3.Distance(playerCamera.position, ItemPosition) : 0;

        private float AngleToItem => Vector3.Angle(ItemPosition - playerCamera.position, playerCamera.forward);

        private bool IsLookingAtItem => Mathf.Abs(AngleToItem) < angleTreshold;

        #endregion

        #region LifeCycle

        private void Update()
        {
            if (_currentItemToFind == null) return;

            var distanceDiff = _previousDistance - DistanceToItem;

            var currentTip = GetTipAudio(distanceDiff);
            if (currentTip == null) return;

            PlayTip(currentTip);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Изменяет текущий объект поиска
        /// </summary>
        /// <param name="item">Новый объект для поиска</param>
        public void SetNewItem(GameObject item)
        {
            _currentItemToFind = item.GetComponent<ItemToFind>();
            _previousDistance = DistanceToItem;
            _previousLookingState = IsLookingAtItem;
        }

        /// <summary>
        /// Очищает текущий объект поиска
        /// </summary>
        public void ClearCurrentItem()
        {
            _currentItemToFind = null;
            _previousDistance = 0;
            _previousLookingState = false;
            // tipText.text = "";
        }

        #endregion

        #region Private Methods

        private FindZone GetZoneTips(float distance)
        {
            return distance <= _currentItemToFind.warm ? FindZone.Warm : FindZone.Cold;
        }

        private AudioClip GetRandomTip(IReadOnlyList<AudioClip> tips)
        {
            var random = new Random();
            var randomTipIndex = random.Next(0, tips.Count);
            return tips[randomTipIndex];
        }

        private AudioClip GetTipAudio(float distanceDiff)
        {
            var zone = GetZoneTips(DistanceToItem);
            var zoneTips = zone == FindZone.Cold ? coldTips : warmTips;

            if (Math.Abs(distanceDiff) >= distanceDelta)
            {
                _previousDistance = DistanceToItem;

                var tips = zoneTips.Colder;
                if (distanceDiff >= distanceDelta)
                {
                    tips = zoneTips.Warmer;
                }

                return GetRandomTip(tips);
            }

            if (zone == FindZone.Warm && _previousLookingState != IsLookingAtItem)
            {
                _previousLookingState = IsLookingAtItem;

                var tips = IsLookingAtItem ? zoneTips.Warmer : zoneTips.Colder;
                return GetRandomTip(tips);
            }

            return null;
        }

        private void PlayTip(AudioClip clip)
        {
            popovStateSystem.StartSpeakingState();
            popovStateSystem.AppendAudioClipToQueue(clip);
            popovStateSystem.PlayQueue();
        }

        #endregion
    }
}