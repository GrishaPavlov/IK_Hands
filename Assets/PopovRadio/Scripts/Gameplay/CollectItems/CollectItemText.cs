using TMPro;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Backpack
{
    public class CollectItemText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;

        public string ItemName
        {
            get => itemName.text;
            set => itemName.text = value;
        }

        public void StrikethroughItemName()
        {
            itemName.fontStyle = FontStyles.Strikethrough;
        }

        public void UnstrikethroughItemName()
        {
            itemName.fontStyle = FontStyles.Normal;
        }
    }
}