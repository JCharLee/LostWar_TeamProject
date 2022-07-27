using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteraction
{
    private string prompt;
    private string alertText;

    private UIManager uiManager;

    public string interactionPrompt => prompt;

    void Start()
    {
        prompt = "[F] 획득하기";
        alertText = "카드키를 획득했습니다!";

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    public bool Action(PlayerInteraction interactor)
    {
        var key = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        key.hasKey = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.layer = 0;
        Destroy(gameObject, 3.1f);
        StartCoroutine(uiManager.NoticeText(true, alertText));
        return true;
    }
}
