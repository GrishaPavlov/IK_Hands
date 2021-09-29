using System;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.EventSystem
{
    public class GameEvents : MonoBehaviour
    {
        #region Singleton

        //Статический объект GameEvents для Singleton паттерна
        public static GameEvents current;

        #endregion

        #region Events

        public event Action<int> OnShowChildren;
        public event Action OnStopShowingChildren;
        public event Action<int> OnShowCurrentPart;
        public event Action<int> OnPutCurrentPart;
        public event Action OnItemsCollected;
        public event Action OnFirstAct;
        public event Action OnSecondAct;

        #endregion

        #region Public Methods

        #region Radio Assembly

        /// <summary>
        /// Ивент для подсвечивания всех "детей" данного элемента дерева
        /// </summary>
        /// <param name="index">Индекс элемента</param>
        public void ShowChildren(int index)
        {
            OnShowChildren?.Invoke(index);
        }

        /// <summary>
        /// Ивент для снятия выделения со всех элементов дерева
        /// </summary>
        public void StopShowingChildren()
        {
            OnStopShowingChildren?.Invoke();
        }

        /// <summary>
        /// Ивент для выделения слота для взятого предмета
        /// </summary>
        /// <param name="index">Индекс слота</param>
        public void ShowCurrentPart(int index)
        {
            OnShowCurrentPart?.Invoke(index);
        }

        /// <summary>
        /// Ивент для постановки элемента радио в его слот
        /// </summary>
        /// <param name="index">Индекс объекта</param>
        public void PutCurrentPart(int index)
        {
            OnPutCurrentPart?.Invoke(index);
        }

        #endregion

        #region Act Transition

        /// <summary>
        /// Ивент, выполняющийся когда все предметы в первом этапе были собраны
        /// </summary>
        public void ItemsCollected()
        {
            OnItemsCollected?.Invoke();
        }

        /// <summary>
        /// Ивент, выполняющийся когда начинается сборка радио
        /// </summary>
        public void FirstAct()
        {
            OnFirstAct?.Invoke();
        }

        /// <summary>
        /// Ивент, выполняющийся когда начинается сборка радио
        /// </summary>
        public void SecondAct()
        {
            OnSecondAct?.Invoke();
        }

        #endregion

        #endregion

        #region LifeCycle

        private void Awake() => current = this;

        #endregion
    }
}