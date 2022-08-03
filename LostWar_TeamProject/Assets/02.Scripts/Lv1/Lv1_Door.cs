﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv1_Door : MonoBehaviour, IInteraction
{
    [SerializeField] Transform door;
    private float speed = 150f;
    private float moveTime = 1f;
    private float current;

    [SerializeField] string prompt;
    private string cautionText;

    private UIManager uiManager;
    private QuestManager questManager;

    void Start()
    {
        door = transform.GetChild(3).transform.GetChild(2).GetComponent<Transform>();

        prompt = "[F] 문 열기";
        cautionText = "장비를 먼저 얻어야 합니다.";

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    public string interactionPrompt => prompt;

    public bool Action(PlayerInteraction interactor)
    {
        if (questManager.QuestId >= 20)
        {
            StartCoroutine(DoorOpen());
            return true;
        }

        if (!uiManager.alert)
            StartCoroutine(uiManager.NoticeText(false, cautionText));
        return false;
    }

    IEnumerator DoorOpen()
    {
        this.gameObject.layer = 0;

        current = 0f;
        while (current <= moveTime)
        {
            current += Time.deltaTime;
            door.rotation = Quaternion.RotateTowards(door.rotation, Quaternion.Euler(0f, 190f, 0f), speed * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(3.0f);
        current = 0f;
        while (current <= moveTime)
        {
            current += Time.deltaTime;
            door.rotation = Quaternion.RotateTowards(door.rotation, Quaternion.Euler(0f, 90f, 0f), speed * Time.deltaTime);
            yield return null;
        }

        this.gameObject.layer = 8;
    }
}