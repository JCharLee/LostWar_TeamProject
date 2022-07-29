using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("[Interaction Icon]")]
    [SerializeField] GameObject interactionPanel;
    [SerializeField] Text promptText;
    public bool onIcon = false;

    [Header("[Casting Bar]")]
    [SerializeField] GameObject castingBar_back;
    [SerializeField] Image castingBar;
    [SerializeField] Text castingTimeText;
    public bool casting = false;

    [Header("[Notice]")]
    [SerializeField] Text noticeText;
    private float cautionDurationTime = 3f;
    public bool alert = false;

    [Header("[Dialogue]")]
    [SerializeField] private GameObject talkPanel;
    [SerializeField] private Text npcName;
    [SerializeField] private Text talkText;
    [SerializeField] private Image portrait;
    [SerializeField] public GameObject buttonPanel;
    [SerializeField] private Text functionText;
    public bool isAction = false;
    public bool goQuest = false;
    public bool acceptQuest = false;
    public int talkIndex = 0;

    [Header("[Quest]")]
    [SerializeField] private GameObject questListPanel;
    [SerializeField] private GameObject questPanel;
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentsText;
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private GameObject questGiver;
    public bool openingPanel = false;
    public GameObject QuestListPanel => questListPanel;
    public GameObject QuestPanel => questPanel;
    public GameObject QuestPrefab => questPrefab;

    public Coroutine castRoutine;

    private TalkManager talkManager;
    private QuestManager questManager;
    public static UIManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        interactionPanel = transform.GetChild(0).gameObject;
        promptText = interactionPanel.GetComponentInChildren<Text>();

        castingBar_back = transform.GetChild(1).gameObject;
        castingBar = castingBar_back.transform.GetChild(0).GetComponent<Image>();
        castingTimeText = castingBar_back.transform.GetChild(1).GetComponent<Text>();

        noticeText = transform.GetChild(2).GetComponent<Text>();

        talkPanel = transform.GetChild(3).gameObject;
        npcName = talkPanel.transform.GetChild(0).GetComponent<Text>();
        talkText = talkPanel.transform.GetChild(1).GetComponent<Text>();
        portrait = talkPanel.transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        buttonPanel = talkPanel.transform.GetChild(3).gameObject;
        functionText = buttonPanel.transform.GetChild(0).GetComponentInChildren<Text>();

        questListPanel = transform.GetChild(4).gameObject;
        questPanel = transform.GetChild(5).gameObject;
        titleText = questPanel.transform.GetChild(0).GetComponent<Text>();
        contentsText = questPanel.transform.GetChild(1).GetComponentInChildren<Text>();
        questGiver = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;

        talkManager = GameObject.Find("TalkManager").GetComponent<TalkManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    void Start()
    {
        interactionPanel.SetActive(false);
        castingBar_back.SetActive(false);
        castingBar.fillAmount = 0f;
        noticeText.gameObject.SetActive(false);
        talkPanel.SetActive(false);
        buttonPanel.SetActive(false);
        questListPanel.SetActive(false);
        questPanel.SetActive(false);
        questGiver.SetActive(false);
    }

    #region [상호작용 아이콘 함수]
    public void InteractionIconOn(string _text)
    {
        promptText.text = _text;
        interactionPanel.SetActive(true);
        onIcon = true;
    }

    public void InteractionIconOff()
    {
        interactionPanel.SetActive(false);
        onIcon = false;
    }
    #endregion

    #region [캐스팅 바 함수]
    public IEnumerator InteractionCasting(float time)
    {
        casting = true;
        castingBar_back.SetActive(true);

        float rate = 1f / time;
        float progress = 0.0f;
        float curTime = Time.deltaTime;
        while (progress <= 1f)
        {
            castingBar.fillAmount = Mathf.Lerp(0f, 1f, progress);
            progress += rate * Time.deltaTime;
            curTime += Time.deltaTime;
            castingTimeText.text = (time - curTime).ToString("0.0");
            yield return null;
        }
        StopCasting();
    }

    public void StopCasting()
    {
        casting = false;
        castingBar_back.SetActive(false);
        castingBar.fillAmount = 0f;

        if (castRoutine != null)
        {
            StopCoroutine(castRoutine);
            castRoutine = null;
        }
    }
    #endregion

    #region [알림 함수]
    public IEnumerator NoticeText(bool _inform, string _text)
    {
        if (_inform)
            noticeText.color = Color.green;
        else
            noticeText.color = Color.red;

        alert = true;
        noticeText.text = _text;
        noticeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(cautionDurationTime);
        noticeText.gameObject.SetActive(false);
        alert = false;
    }
    #endregion

    #region [대화/퀘스트 함수]
    public void Action(GameObject obj)
    {
        NPCObject npcObject = obj.GetComponent<NPCObject>();
        QuestGiver quest = obj.GetComponent<QuestGiver>();
        if (npcObject != null && quest == null)
            Talk(npcObject.objectId);
        if (npcObject == null && quest != null)
            SelfTalk(quest.ObjectID);

        talkPanel.SetActive(isAction);
    }

    void Talk(int id)
    {
        int questTalkIdx = questManager.GetQuestTalkIdx(id);
        string talkData = talkManager.GetTalk(id + questTalkIdx, talkIndex);

        if (talkData == null)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            isAction = false;
            talkIndex = 0;
            questManager.CheckQuest(id);
            return;
        }

        npcName.text = talkManager.GetName(id, int.Parse(talkData.Split(':')[0]));
        talkText.text = talkData.Split(':')[1];
        portrait.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[2]));

        //Cursor.lockState = CursorLockMode.None;
        isAction = true;
        talkIndex++;
    }

    void SelfTalk(int id)
    {
        int questTalkIdx = questManager.GetQuestTalkIdx(id);
        string talkData = talkManager.GetTalk(id + questTalkIdx, talkIndex);

        if (talkData == null)
        {
            //Cursor.lockState = CursorLockMode.Locked;
            isAction = false;
            talkIndex = 0;
            questManager.CheckQuest(id);
            questGiver.SetActive(false);
            return;
        }

        npcName.text = talkManager.GetName(id, int.Parse(talkData.Split(':')[0]));
        talkText.text = talkData.Split(':')[1];
        portrait.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[2]));

        //Cursor.lockState = CursorLockMode.None;
        isAction = true;
        talkIndex++;
    }
    #endregion
}