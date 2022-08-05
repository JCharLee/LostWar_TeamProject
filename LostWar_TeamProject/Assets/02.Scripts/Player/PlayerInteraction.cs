﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] Transform interactionPoint;
    [SerializeField] float pointRadius = 1f;
    [SerializeField] LayerMask interactableMask;
    public bool hasKey1 = false;
    public bool hasKey2 = false;

    private readonly Collider[] cols = new Collider[3];
    [SerializeField] int numFound;

    private IInteraction interactable;
    private UIManager uiManager;

    [SerializeField] private QuestData questData;
    private QuestManager questManager;
    public static PlayerInteraction instance = null;
    private BasicBehaviour basicBehaviour;

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

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        interactionPoint = transform.GetChild(0).GetComponent<Transform>();

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        basicBehaviour = GetComponent<BasicBehaviour>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ELEVATOR")
            transform.SetParent(collision.transform);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "ELEVATOR")
        {
            transform.SetParent(null);
            DontDestroyOnLoad(this.gameObject);
        }
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

                if (uiManager.casting || uiManager.isAction || uiManager.dropOn)
                    uiManager.InteractionIconOff();

                if (Input.GetKeyDown(KeyCode.F))
                    if (!uiManager.casting && !uiManager.dropOn)
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

        if (SceneLoader.isLoading)
            GetComponent<Rigidbody>().Sleep();
        else
            GetComponent<Rigidbody>().WakeUp();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionPoint.position, pointRadius);
    }
}