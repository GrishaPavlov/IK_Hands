using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.HotAndCold
{
    public class ItemToFind : MonoBehaviour
    {
        #region Settings

        public float warm;

        #endregion

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, warm);
        }
    }
}