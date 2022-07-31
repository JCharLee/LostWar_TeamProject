using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class GetDropItem : MonoBehaviour
{
    public int itemIdx;
    public UIManager uiManager;
    private QuestManager questManager;
    private PlayerInteraction player;
    public GameDataObject gameDataObject;

    private void Awake()
    {
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        player = FindObjectOfType<PlayerInteraction>();
    }

    public void GetItem()
    {
        if (questManager.MainQuestList[questManager.MainQuestId].Goal.GoalType == GoalType.GATHERING)
        {
            if (questManager.MainQuestId == 10)
            {
                if (uiManager.items[itemIdx].name == "Sword" || uiManager.items[itemIdx].name == "Pistol")
                    player.Collect();
            }
        }

        if (uiManager.items[itemIdx].itemType == ItemType.shortWeapon)
        {
            gameDataObject.shortWeapon.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.longWeapon)
        {
            gameDataObject.longWeapon.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.shoes)
        {
            gameDataObject.shoes.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.top)
        {
            gameDataObject.top.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }
        else if (uiManager.items[itemIdx].itemType == ItemType.bottoms)
        {
            gameDataObject.bottoms.Add(uiManager.items[itemIdx]);
            uiManager.items[itemIdx] = null;
        }

        Destroy(gameObject);
    }
}