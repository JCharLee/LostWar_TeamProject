﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour, IInteraction
{
    [SerializeField] private QuestManager questManager;
    [SerializeField] private PlayerInteraction player;
    [SerializeField] private int objectID = 0;

    private UIManager uiManager;

    public int ObjectID => objectID;

    public string interactionPrompt => null;

    private void Awake()
    {
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
    }

    public bool Action(PlayerInteraction interactor)
    {
        uiManager.Action(this.gameObject);
        return true;
    }
}
