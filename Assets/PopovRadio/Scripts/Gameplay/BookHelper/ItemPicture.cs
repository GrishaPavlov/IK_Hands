using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PopovRadio.Scripts.Gameplay.BookHelper
{
    public class ItemPicture : MonoBehaviour
    {
        [SerializeField] private Image ItemImage;

        public void ChangeBookImage(ItemSpriteContainer container)
        {
            if (ItemImage.color.a == 0)
                ItemImage.color = Color.white;
            // ItemImage.sprite = container.ItemSprite;
            DOTween.Sequence()
                .Append(ItemImage.DOColor(new Color(255, 255, 255, 0), .3f))
                .AppendCallback(() =>
                {
                    ItemImage.sprite = container.ItemSprite;
                })
                .Append(ItemImage.DOColor(Color.white, .3f));
        }

        public void ClearBookImage()
        {
            ItemImage.DOColor(new Color(255, 255, 255, 0), .3f);
        }
    }
}