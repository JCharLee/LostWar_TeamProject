using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;
using UnityEngine.UI;
public class EquipItem : MonoBehaviour
{
    public static EquipItem focused;
    public Item item;
    public Color origin_color;
    public bool isEquiped = false;
    private void Start()
    {
        origin_color = new Color(115f / 255f, 178f / 255f, 229f / 255f, 146f / 255f);
    }
    public void Clicked_In_Inventory()
    {
        if (focused != null)
            focused.GetComponent<Image>().color = origin_color;
        focused = this;
        gameObject.GetComponent<Image>().color = Color.blue;
        if(item!=null)
        GameManager.instance.ShowInventoryStatus(item);
        GameManager.instance.InventoryImageChange();
    }
}
