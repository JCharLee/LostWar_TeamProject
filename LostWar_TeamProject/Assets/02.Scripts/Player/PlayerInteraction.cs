﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Transform interactionPoint;
    [SerializeField] float pointRadius = 1f;
    [SerializeField] LayerMask interactableMask;

    private readonly Collider[] cols = new Collider[3];
    [SerializeField] int numFound;

    private IInteraction interactable;
    private UIManager uiManager;

    [SerializeField] private QuestData questData;
    private QuestManager questManager;
    public static PlayerInteraction instance = null;

    public string currentMapName;

    public QuestData QuestData
    {
        get
        {
            return questData;
        }
        set
        {
            questData = value;
        }

    }

    public void Kill()
    {
        if (questData.isActive)
        {
            questData.Goal.EnemyKilled();
            if (questData.Goal.IsReached())
            {
                questManager.Complete();
            }
        }
    }

    public void Collect()
    {
        if (questData.isActive)
        {
            questData.Goal.ItemCollected();
            if (questData.Goal.IsReached())
            {
                questManager.Complete();
            }
        }
    }

    public void Locate()
    {
        if (questData.isActive)
        {
            questData.Goal.Locate();
            if (questData.Goal.IsReached())
            {
                questManager.Complete();
            }
        }
    }

    public void Talk()
    {
        if (questData.isActive)
        {
            questData.Goal.Talking();
            if (questData.Goal.IsReached())
            {
                questManager.Complete();
            }
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        interactionPoint = transform.GetChild(2).GetComponent<Transform>();

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, pointRadius, cols, interactableMask);

        if (numFound > 0)
        {
            interactable = cols[0].GetComponent<IInteraction>();

            if (interactable != null)
            {
                if (!uiManager.onIcon)
                    uiManager.InteractionIconOn(interactable.interactionPrompt);

                if (uiManager.casting || uiManager.isAction)
                    uiManager.InteractionIconOff();

                if (Input.GetKeyDown(KeyCode.F))
                    interactable.Action(this);
            }
        }
        else
        {
            if (interactable != null)
                interactable = null;

            if (uiManager.onIcon)
                uiManager.InteractionIconOff();
        }
    }

    public void OnClickQuest()
    {
        uiManager.goQuest = true;
        uiManager.talkIndex = 0;
        interactable.Action(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint.position, pointRadius);
    }
}