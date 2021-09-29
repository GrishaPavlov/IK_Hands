using System.Linq;
using PopovRadio.Scripts.Gameplay.EventSystem;
using PopovRadio.Scripts.Gameplay.Interact;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace PopovRadio.Scripts.Gameplay.RadioAssembly
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class RadioAssemblyItem : MonoBehaviour
    {
        #region Fields

        private XRGrabInteractable _interactable;

        private int _index;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();
            _index = GetComponent<Item>().index;
        }

        private void Start()
        {
            GameEvents.current.OnSecondAct += EnableRadioAssembly;
        }

        private void OnDestroy()
        {
            _interactable.selectEntered.RemoveListener(ShowParts);
            _interactable.selectExited.RemoveListener(StopShowingParts);

            GameEvents.current.OnSecondAct -= EnableRadioAssembly;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Функция, вызывающаяся когда деталь радио берут из своего сокета (selectEntered). Если данный предмет можно поставить, то включается призрак подсветка места куда можно его поставить 
        /// </summary>
        /// <param name="args">Аргументы ивента selectEntered</param>
        public void ShowParts(SelectEnterEventArgs args)
        {
            if (!args.interactor.tag.Equals("Hand")) return;

            GameEvents.current.ShowCurrentPart(_index);
        }

        /// <summary>
        /// Функция, вызывающаяся когда мы отпускаем деталь радио (selectExited). Когда мы отпускаем деталь, призрак детали, если он был, отключается
        /// </summary>
        /// <param name="args">Аргументы ивента selectExited</param>
        public void StopShowingParts(SelectExitEventArgs args)
        {
            GameEvents.current.StopShowingChildren();
        }

        #endregion

        #region Private Methods

        private void EnableRadioAssembly()
        {
            _interactable.selectEntered.AddListener(ShowParts);
            _interactable.selectExited.AddListener(StopShowingParts);
        }

        #endregion
    }
}