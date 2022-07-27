using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondRoomDoor : Lv3_Door
{
    private string cautionText;

    private UIManager uiManager;

    protected override void Awake()
    {
        base.Awake();
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    protected override void Start()
    {
        speed = 5f;
        prompt = "[F] 문 열기";
        base.Start();
        cautionText = "카드키를 구해야 합니다.";
    }

    public override bool Action(PlayerInteraction interactor)
    {
        var inventory = interactor.GetComponent<Inventory>();

        if (inventory == null) return false;

        if (inventory.hasKey)
        {
            return base.Action(interactor);
        }

        if (!uiManager.alert)
            StartCoroutine(uiManager.NoticeText(false, cautionText));
        return false;
    }

    public override IEnumerator DoorAction()
    {
        return base.DoorAction();
    }
}
