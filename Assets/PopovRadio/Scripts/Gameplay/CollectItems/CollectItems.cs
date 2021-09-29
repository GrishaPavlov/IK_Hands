using System.Collections.Generic;
using PopovRadio.Scripts.Gameplay.Backpack;
using PopovRadio.Scripts.Gameplay.EventSystem;
using PopovRadio.Scripts.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.CollectItems
{
    public class CollectItems : MonoBehaviour
    {
        [SerializeField] private InteractableStackSet inventoryItems;
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private Transform textsContainer;
        [SerializeField] private HotAndCold.HotAndCold hotAndColdManager;

        public UnityEvent OnAllItemsCollected;

        private int _itemsCollected;
        private CollectItem[] _itemsToCollect;
        private readonly Dictionary<string, CollectItemText> _itemTexts = new Dictionary<string, CollectItemText>();

        private void Awake()
        {
            _itemsToCollect = FindObjectsOfType<CollectItem>(false);
            _itemsCollected = 0;

            foreach (var item in _itemsToCollect)
            {
                var newTextItem = Instantiate(textPrefab, textsContainer).GetComponentInChildren<CollectItemText>();
                newTextItem.ItemName = item.UIName;
                _itemTexts.Add(item.name, newTextItem);
            }
        }

        private void OnEnable()
        {
            inventoryItems.OnItemAdded.AddListener(OnItemPut);
            inventoryItems.OnItemRemoved.AddListener(OnItemTaken);
        }

        private void OnDisable()
        {
            inventoryItems.OnItemAdded.RemoveListener(OnItemPut);
            inventoryItems.OnItemRemoved.RemoveListener(OnItemTaken);
        }

        private void OnItemPut(XRBaseInteractable putObject)
        {
            _itemsCollected++;
            if (_itemsCollected == _itemsToCollect.Length)
                GameEvents.current.ItemsCollected();
            _itemTexts[putObject.name].StrikethroughItemName();
            hotAndColdManager.ClearCurrentItem();
        }

        private void OnItemTaken(XRBaseInteractable takenObject)
        {
            _itemTexts[takenObject.name].UnstrikethroughItemName();
        }
    }
}