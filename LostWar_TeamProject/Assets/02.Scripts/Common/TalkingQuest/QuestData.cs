using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    [SerializeField] private string questName;
    [SerializeField] private string contentsName;
    private int[] npcId;
    [SerializeField] private bool isMain;
    [SerializeField] private QuestGoal goal;

    public bool isActive;

    public string QuestName => questName;
    public string ContentsName => contentsName;
    public int[] NpcId => npcId;
    public bool IsMain => isMain;
    public QuestGoal Goal => goal;

    public QuestData(string name, string content, int[] npc, bool main, int amount, GoalType type)
    {
        questName = name;
        contentsName = content;
        npcId = npc;
        isMain = main;
        goal = new QuestGoal(type, amount);
    }
}