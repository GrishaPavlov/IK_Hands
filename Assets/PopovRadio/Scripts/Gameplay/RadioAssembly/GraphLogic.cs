using System.Collections.Generic;
using System.Linq;
using PopovRadio.Scripts.Gameplay.EventSystem;
using PopovRadio.Scripts.Gameplay.EventSystem.xNodeEditor;
using PopovRadio.Scripts.Gameplay.EventSystem.xNodeEditor.Nodes;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.RadioAssembly
{
    public class GraphLogic : MonoBehaviour
    {
        #region Fields

        [Tooltip("Маска для 'всех' слоев")] [SerializeField]
        private LayerMask everythingMask;

        [Tooltip("Граф с последовательностью установки объектов")] [SerializeField]
        private AssemblyTree graph;

        [Tooltip("Материал 'призрак' для подсветки слотов радио")] [SerializeField]
        private Material ghostMat;

        [Tooltip("Список 'слотов' куда надо ставить объекты")] [SerializeField]
        private List<Slot> slots;


        private List<EventNode> _availableNodes = new List<EventNode>();
        private List<GameObject> _selectedObjects = new List<GameObject>();

        private Material _oldMat;

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _availableNodes = graph.GetRootNodes();
        }

        private void Start()
        {
            GameEvents.current.OnStopShowingChildren += DeSelectObjects;
            GameEvents.current.OnShowCurrentPart += SelectCurrent;
            GameEvents.current.OnPutCurrentPart += OnPutNode;
            GameEvents.current.OnSecondAct += OnSecondActStart;
        }

        private void OnDisable()
        {
            GameEvents.current.OnStopShowingChildren -= DeSelectObjects;
            GameEvents.current.OnShowCurrentPart -= SelectCurrent;
            GameEvents.current.OnPutCurrentPart -= OnPutNode;
            GameEvents.current.OnSecondAct -= OnSecondActStart;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Функция, обновляющая список доступных деталей для сборки радио. Вызывается когда какая-либо деталь ставится в свой слот в радио
        /// </summary>
        /// <param name="index">Индекс детали, которую мы ставим в слот</param>
        public void OnPutNode(int index)
        {
            var children = graph.GetChildren(index, _availableNodes);
            if (!children.Any()) return;

            HandleNewNodes(children);

            _availableNodes = children;
        }

        #endregion

        #region Private Methods

        private void DeSelectObjects()
        {
            foreach (var go in _selectedObjects)
            {
                go.GetComponent<Renderer>().material = _oldMat;
                go.GetComponent<MeshRenderer>().enabled = false;
            }
        }

        private void SelectCurrent(int index)
        {
            foreach (var availableNode in _availableNodes)
            {
                foreach (var slot in slots)
                {
                    if (slot.Index == availableNode.index && slot.Index == index)
                    {
                        _selectedObjects.Add(slot.gameObject);
                        slot.GetComponent<MeshRenderer>().enabled = true;
                        var slotRenderer = slot.GetComponent<Renderer>();
                        _oldMat = slotRenderer.material;
                        slotRenderer.material = ghostMat;
                    }
                }
            }
        }

        private void HandleNewNodes(IEnumerable<EventNode> nodes)
        {
            foreach (var node in nodes)
            {
                foreach (var slot in slots)
                {
                    if (slot.Index != node.index) continue;
                    ActivateSocketInteractor(slot);

                    node.StartEvent?.Invoke();
                }
            }
        }

        private void ActivateSocketInteractor(Slot slot)
        {
            if (slot.TryGetComponent<XRSocketInteractorByIndex>(out var socketInteractor))
            {
                socketInteractor.interactionLayerMask = everythingMask;
            }
        }

        private void OnSecondActStart()
        {
            HandleNewNodes(_availableNodes);
        }

        #endregion
    }
}