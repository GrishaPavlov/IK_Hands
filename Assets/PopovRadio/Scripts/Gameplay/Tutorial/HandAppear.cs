using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace PopovRadio.Scripts.Gameplay.Tutorial
{
    public class HandAppear : MonoBehaviour
    {
        #region Constant Fields

        private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");

        #endregion

        #region Fields

        [Tooltip("Модели контроллеров")] [SerializeField]
        private GameObject[] controllerModels;

        [Tooltip("Модели рук")] [SerializeField]
        private GameObject[] handModels;

        [Tooltip("Продолжительность перехода")] [SerializeField]
        private float dissolveDuration;

        private List<Material> _controllerDissolveMaterials = new List<Material>();
        private List<Material> _handDissolveMaterials = new List<Material>();

        #endregion

        #region LifeCycle

        private void Awake()
        {
            _controllerDissolveMaterials = GetModelsChildMaterials(controllerModels);
            _handDissolveMaterials = GetModelsChildMaterials(handModels);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Запускает смену контроллеров на руки
        /// </summary>
        public void StartHandAppearing()
        {
            var showHandsAnimation = DOTween.Sequence();
            showHandsAnimation
                .Append(HideControllersAnimation())
                .Append(ShowHandsAnimation());
        }

        #endregion

        #region Private Methods

        private void HideControllers()
        {
            foreach (var model in controllerModels)
            {
                model.SetActive(false);
            }
        }

        private Sequence HideControllersAnimation()
        {
            var animationSequence = DOTween.Sequence();
            foreach (var dissolveMaterial in _controllerDissolveMaterials)
            {
                animationSequence.Join(dissolveMaterial.DOFloat(1f, Dissolve, dissolveDuration));
            }

            animationSequence.OnComplete(HideControllers);

            return animationSequence;
        }

        private void ShowHands()
        {
            foreach (var model in handModels)
            {
                model.SetActive(true);
            }
        }

        private Sequence ShowHandsAnimation()
        {
            ShowHands();

            var animationSequence = DOTween.Sequence();
            foreach (var dissolveMaterial in _handDissolveMaterials)
            {
                animationSequence.Join(dissolveMaterial.DOFloat(0f, Dissolve, dissolveDuration));
            }

            return animationSequence;
        }

        private List<Material> GetModelsChildMaterials(GameObject[] models)
        {
            return (
                from model in models
                from objectRenderer in model.GetComponentsInChildren<Renderer>()
                select objectRenderer.material
            ).ToList();
        }

        #endregion
    }
}