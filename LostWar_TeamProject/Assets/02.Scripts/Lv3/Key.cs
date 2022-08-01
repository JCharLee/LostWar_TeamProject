﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteraction
{
    public int keyNumber;
    private string prompt;
    private string alertText;

    private UIManager uiManager;
    private QuestManager questManager;
    private PlayerInteraction player;

    public string interactionPrompt => prompt;

    void Start()
    {
        prompt = "[F] 획득하기";

        switch(keyNumber)
        {
            case 1:
                alertText = "첫 번째 카드키를 획득했습니다!";
                break;
            case 2:
                alertText = "두 번째 카드키를 획득했습니다!";
                break;
        }

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        player = FindObjectOfType<PlayerInteraction>();
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 60f * Time.deltaTime, Space.World);
    }

    public bool Action(PlayerInteraction interactor)
    {
        switch(keyNumber)
        {
            case 1:
                player.hasKey1 = true;
                break;
            case 2:
                player.hasKey2 = true;
                break;
        }
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.layer = 0;
        Destroy(gameObject, 3.1f);
        StartCoroutine(uiManager.NoticeText(true, alertText));
        if (questManager.QuestId == 90 && keyNumber == 1)
            player.Collect();
        if (questManager.QuestId == 120 && keyNumber == 2)
            player.Collect();
        return true;
    }
}