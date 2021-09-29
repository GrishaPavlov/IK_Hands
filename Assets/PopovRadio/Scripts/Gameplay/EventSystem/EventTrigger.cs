using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.EventSystem
{
    public class EventTrigger : MonoBehaviour
    {
        #region Public Variables

        [Tooltip("Индекс триггера")] public int index;

        [Tooltip("Клавиша, по которой вызывается ивент")]
        public KeyCode key;

        #endregion

        #region Private Methods
        
        private void PrintIndex(int i)
        {
            if (i == index)
                Debug.Log(i);
        }

        #endregion
    }
}