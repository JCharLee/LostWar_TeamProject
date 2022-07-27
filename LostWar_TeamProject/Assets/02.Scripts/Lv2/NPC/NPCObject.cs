using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObject : MonoBehaviour, IInteraction
{
    public int objectId;
    public string npcName;
    public string prompt;
    public bool hasQuest;

    private UIManager uiManager;

    protected virtual void Awake()
    {
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        prompt = "[F] 대화하기";
    }

    public virtual string interactionPrompt => prompt;

    public virtual bool Action(PlayerInteraction interactor)
    {
        uiManager.Action(this.gameObject);
        return true;
    }
}