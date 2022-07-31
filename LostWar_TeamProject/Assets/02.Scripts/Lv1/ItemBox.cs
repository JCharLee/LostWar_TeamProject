using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSpace;

public class ItemBox : DropItem, IInteraction
{
    private float castTime = 3f;

    [SerializeField] string prompt;

    private UIManager uiManager;

    public string interactionPrompt => prompt;

    void Start()
    {
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();

        prompt = "[F] 상자 열기";
        InitItem();
    }

    private void Update()
    {
        if (items[0] == null && items[1] == null && items[2] == null)
        {
            gameObject.layer = 0;
            GameObject.FindGameObjectWithTag("Player").transform.GetChild(9).gameObject.SetActive(true);
            uiManager.CloseDropPanel();
            return;
        }
    }

    public bool Action(PlayerInteraction interactor)
    {
        StartCoroutine(ItemboxOpen());
        return true;
    }

    IEnumerator ItemboxOpen()
    {
        StartCoroutine(uiManager.InteractionCasting(castTime));
        yield return new WaitForSeconds(castTime);
        uiManager.OpenDropPanel(this);
        Debug.Log("Item box open!");
    }

    public override void InitItem()
    {
        items[0] = ItemList.instance.Get("Sword");
        items[1] = ItemList.instance.Get("Pistol");
        items[2] = null;
    }
}