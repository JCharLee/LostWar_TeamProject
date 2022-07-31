using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private int mainQuestId;
    [SerializeField] private int mainQuestActionIdx;
    [SerializeField] private int subQuestId;
    [SerializeField] private int subQuestActionIdx;

    private UIManager uiManager;
    private QuestGiver questGiver;
    private PlayerInteraction player;
    [SerializeField] private QuestData mainQuestData;
    [SerializeField] private QuestData subQuestData;

    Dictionary<int, QuestData> mainQuestList;
    Dictionary<int, QuestData> subQuestList;

    public int MainQuestId => mainQuestId;
    public int MainQuestActionIdx => mainQuestActionIdx;
    public int SubQuestId { get; set; }
    public int SubQuestActionIdx => subQuestActionIdx;
    public Dictionary<int, QuestData> MainQuestList => mainQuestList;
    public Dictionary<int, QuestData> SubQuestList => subQuestList;

    public static QuestManager instance = null;

    private bool isStarting = false;
    public bool IsStarting => isStarting;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        mainQuestList = new Dictionary<int, QuestData>();
        subQuestList = new Dictionary<int, QuestData>();
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questGiver = GameObject.Find("QuestGiver").GetComponent<QuestGiver>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteraction>();
        GenerateData();
    }

    private void Start()
    {
        isStarting = true;
        StartCoroutine(QuestStart());
    }

    IEnumerator QuestStart()
    {
        yield return new WaitForSeconds(3.0f);
        questGiver.gameObject.SetActive(true);
        uiManager.Action(questGiver.gameObject);
        isStarting = false;
    }

    void GenerateData()
    {
        // 메인 퀘스트
        mainQuestList.Add(0, new QuestData("시작", "게임 스타트", new int[] { 0 }, true, 1, GoalType.TALK, 0f));
        mainQuestList.Add(10, new QuestData("아파트 탈출", "장비를 찾아서 챙긴다.", new int[] { 0, 0 }, true, 2, GoalType.GATHERING, 5f));
        mainQuestList.Add(20, new QuestData("아파트 탈출", "집 밖으로 나가자.", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 5f));
        mainQuestList.Add(30, new QuestData("아파트 탈출", "출몰하는 적 처치", new int[] { 0, 0 }, true, 10, GoalType.KILL, 5f));
        mainQuestList.Add(40, new QuestData("아파트 탈출", "건물을 빠져나간다.", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 5f));

        mainQuestList.Add(50, new QuestData("호위 임무", "민간인을 공격하고 있는 적 처치", new int[] { 0, 2000 }, true, 3, GoalType.KILL, 10f));
        mainQuestList.Add(60, new QuestData("호위 임무", "목적지에 도착할 때까지 엘리사 보호", new int[] { 0, 2000, 2000 }, true, 1, GoalType.LOCATION, 10f));
        mainQuestList.Add(70, new QuestData("가족의 행방을 찾아서", "단장에게 말을 건다.", new int[] { 3000 }, true, 1, GoalType.TALK, 5f));
        mainQuestList.Add(80, new QuestData("가족의 행방을 찾아서", "적 처치", new int[] { 0, 3000 }, true, 10, GoalType.KILL, 20f));
        mainQuestList.Add(90, new QuestData("가족의 행방을 찾아서", "연구소에 들어간다.", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 5f));

        mainQuestList.Add(100, new QuestData("연구소 심층부로", "첫 번째 열쇠를 찾아서 우측 방으로 이동", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 5f));
        mainQuestList.Add(110, new QuestData("연구소 심층부로", "안쪽 방으로 이동해서 두 번째 열쇠 획득", new int[] { 0, 0 }, true, 1, GoalType.GATHERING, 5f));
        mainQuestList.Add(120, new QuestData("연구소 심층부로", "아래 층으로 이동해서 밖으로 나가본다", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 5f));
        mainQuestList.Add(130, new QuestData("연구소 심층부로", "나타난 적들 처치", new int[] { 0, 0 }, true, 5, GoalType.KILL, 20f));
        mainQuestList.Add(140, new QuestData("연구소 심층부로", "연구소 심층부에 들어간다.", new int[] { 0, 0 }, true, 1, GoalType.LOCATION, 10f));
        mainQuestList.Add(150, new QuestData("연구소장 처치", "연구소장 처치", new int[] { 0, 0 }, true, 1, GoalType.KILL, 30f));
        mainQuestList.Add(160, new QuestData("끝", "게임 클리어", new int[] { 0 }, true, 1, GoalType.TALK, 0f));

        // 서브 퀘스트
        subQuestList.Add(10, new QuestData("잃어버린 OO를 찾아서", "OO 수집", new int[] { 4000 }, false, 5, GoalType.GATHERING, 10f));
    }

    public int GetQuestTalkIdx(int id)
    {
        return mainQuestId + mainQuestActionIdx;
    }

    public void CheckQuest(int id)
    {
        if (id == mainQuestList[mainQuestId].NpcId[mainQuestActionIdx])
        {
            mainQuestActionIdx++;
        }

        if (mainQuestActionIdx == mainQuestList[mainQuestId].NpcId.Length)
            NextMainQuest();
    }

    void NextMainQuest()
    {
        uiManager.UpdateExp(mainQuestList[mainQuestId].ExpReward);
        mainQuestId += 10;
        mainQuestActionIdx = 0;

        //if (mainQuestId == 60)
        //    StartCoroutine(SetEscortQuest());

        if (mainQuestId == 80)
            subQuestId = 10;

        mainQuestData = mainQuestList[mainQuestId];
        player.QuestData = mainQuestData;
        mainQuestData.isActive = true;
        uiManager.QuestListPanel.SetActive(true);
        Instantiate(uiManager.QuestPrefab, uiManager.QuestListPanel.transform);
        QuestContents.contents.QuestData = mainQuestData;
    }

    public void Complete()
    {
        mainQuestData.isActive = false;
        if (mainQuestList[mainQuestId].NpcId[mainQuestActionIdx + 1] == 0)
        {
            mainQuestActionIdx++;
            questGiver.gameObject.SetActive(true);
            uiManager.Action(questGiver.gameObject);
        }
        else
            mainQuestActionIdx++;
    }

    //IEnumerator SetEscortQuest()
    //{
    //    yield return new WaitForSeconds(0.5f);
    //    NPCObject npc = GameObject.Find("FirstElisaNPC").GetComponent<NPCObject>();
    //    EscortedNPC esNpc = GameObject.Find("EscortedElisaNPC").GetComponent<EscortedNPC>();
    //    npc.gameObject.SetActive(false);
    //    esNpc.gameObject.SetActive(true);
    //}
}