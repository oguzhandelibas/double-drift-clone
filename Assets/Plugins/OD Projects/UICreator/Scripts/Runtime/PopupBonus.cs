using UnityEngine;
using UnityEngine.UI;

public class PopupBonus : MonoBehaviour
{
    [SerializeField] private Image giftIcon;
    
    public void SetPopupBonusGiftIcon(Sprite giftIconSprite)
    {
        giftIcon.sprite = giftIconSprite;
    }
}
