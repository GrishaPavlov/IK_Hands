using PopovRadio.Scripts.Characters.Popov;
using PopovRadio.Scripts.Gameplay.EventSystem;
using PopovRadio.Scripts.Gameplay.Backpack;
using PopovRadio.Scripts.Tools;
using PopovRadio.Scripts.UI.Tooltip;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.StageTransition
{
    public class StageTransition : MonoBehaviour
    {
        [SerializeField] private InteractableStackSet inventoryItems;
        [SerializeField] private InventoryInteractable inventoryInteractable;
        [SerializeField] private PopovStateSystem popovStateSystem;

        [SerializeField] private AudioClip secondActStartAudio;
        [SerializeField] private Transform secondActTable;
        [SerializeField] private Outline secondActTableOutline;

        private Tooltip _tableTooltip;

        private void Start()
        {
            GameEvents.current.OnFirstAct += StartFirstAct;
            // GameEvents.current.OnSecondAct += StartSecondAct;
            GameEvents.current.OnItemsCollected += StartSecondAct;
            inventoryItems.OnItemRemoved.AddListener(CheckBackpack);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.End))
            {
                // GameEvents.current.SecondAct();
                StartSecondAct();
            }
        }

        private void StartFirstAct()
        {
            Debug.Log("Начинаем сборку предметов");
        }

        private void StartSecondAct()
        {
            Debug.Log("Ты собрал все предметы, молодец");
            inventoryInteractable.enabled = true;

            popovStateSystem.StartSpeakingState();
            popovStateSystem.AppendAudioClipToQueue(secondActStartAudio);
            popovStateSystem.PlayQueue();

            secondActTableOutline.enabled = true;
            _tableTooltip =
                TooltipSystem.Instance.AddTooltip(secondActTable, "Выложи предметы на стол, чтобы начать сборку радио",
                    new Vector3(0, 1.2f));
        }

        private void CheckBackpack(XRBaseInteractable item)
        {
            if (inventoryInteractable.enabled && inventoryItems.Items.Count == 0)
            {
                Debug.Log("Начинается сборка радио");

                secondActTableOutline.enabled = false;
                _tableTooltip.Destroy();

                GameEvents.current.SecondAct();
            }
        }
    }
}