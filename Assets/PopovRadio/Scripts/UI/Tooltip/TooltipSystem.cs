using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PopovRadio.Scripts.UI.Tooltip
{
    public class TooltipSystem : MonoBehaviour
    {
        private static readonly Vector3 DefaultOffset = new Vector3(0, 0.2f);

        public static TooltipSystem Instance;

        [SerializeField] private Transform spawnParent;
        [SerializeField] private GameObject tooltipPrefab;
        [SerializeField] private GameObject tooltipSimplePrefab;

        private readonly List<Tooltip> _tooltips = new List<Tooltip>();

        private void Awake()
        {
            Instance = this;
        }

        public Tooltip AddTooltip(Transform target, string descriptionText, string actionTipText,
            Vector3? offset = null)
        {
            offset ??= DefaultOffset;

            return InstantiateTooltip(target, descriptionText, actionTipText, (Vector3) offset);
        }

        public Tooltip AddTooltip(Transform target, string descriptionText, Vector3? offset = null)
        {
            offset ??= DefaultOffset;

            return InstantiateTooltip(target, descriptionText, (Vector3) offset);
        }

        public void AddTooltip(Transform target, string descriptionText, string actionTipText, float ttl,
            Vector3? offset = null)
        {
            offset ??= DefaultOffset;

            var spawnedTooltip = InstantiateTooltip(target, descriptionText, actionTipText, (Vector3) offset);
            StartCoroutine(DestroyAfterTTL(spawnedTooltip, ttl));
        }

        public void DestroyAllTooltips()
        {
            foreach (var tooltip in _tooltips)
            {
                tooltip.Destroy();
            }

            _tooltips.Clear();
        }

        private Tooltip InstantiateTooltip(Transform target, string descriptionText, string actionTipText,
            Vector3 offset)
        {
            var spawnedTooltip = Instantiate(tooltipPrefab, spawnParent).GetComponent<Tooltip>();
            spawnedTooltip.Target = target;
            spawnedTooltip.Offset = offset;
            spawnedTooltip.DescriptionText = descriptionText;
            spawnedTooltip.ActionTipText = actionTipText;

            _tooltips.Add(spawnedTooltip);

            return spawnedTooltip;
        }

        private Tooltip InstantiateTooltip(Transform target, string descriptionText, Vector3 offset)
        {
            var spawnedTooltip = Instantiate(tooltipSimplePrefab, spawnParent).GetComponent<Tooltip>();
            spawnedTooltip.Target = target;
            spawnedTooltip.Offset = offset;
            spawnedTooltip.DescriptionText = descriptionText;

            _tooltips.Add(spawnedTooltip);

            return spawnedTooltip;
        }

        private IEnumerator DestroyAfterTTL(Tooltip tooltip, float ttl)
        {
            yield return new WaitForSeconds(ttl);
            tooltip.Destroy();
        }
    }
}