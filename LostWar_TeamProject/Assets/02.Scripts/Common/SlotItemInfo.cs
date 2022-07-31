using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ItemSpace;

public class SlotItemInfo : MonoBehaviour, IPointerClickHandler
{
    public Item item;
    public bool isEquip = false;
    public static SlotItemInfo instance;
    private float clickTime = 0;

    public void ClickItem()
    {
        instance = this;
        UIManager.instance.ShowItemInfo(item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((Time.time - clickTime) < 0.25f)
        {
            if (!isEquip)
                UIManager.instance.OnDoubleClickEquip();
            else
                UIManager.instance.OnDoubleClickEquipOff();
        }
        else
            clickTime = Time.time;
    }
}
