﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemSpace;

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
    public bool isAction = false;
    public int talkIndex = 0;

    [Header("[Quest]")]
    [SerializeField] private GameObject questListPanel;
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private GameObject questGiver;
    public bool openingPanel = false;
    public GameObject QuestListPanel => questListPanel;
    public GameObject QuestPrefab => questPrefab;

    [Header("[PlayerState]")]
    [SerializeField] private Image hpBar;
    [SerializeField] private Text hpAmount;
    [SerializeField] private Image spBar;
    [SerializeField] private Text spAmount;
    [SerializeField] private Image expBar;
    [SerializeField] private Text lvText;

    [Header("[Drop]")]
    [SerializeField] public GameObject dropPanel;
    [SerializeField] private GameObject[] dropPrefab;
    [SerializeField] public Item[] items;
    public bool dropOn = false;

    [Header("[Inventory]")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Text itemInfoText;
    [SerializeField] private Image itemInfoImage;
    [SerializeField] private Text str;
    [SerializeField] private Text agi;
    [SerializeField] private Text con;
    [SerializeField] private Text vit;
    [SerializeField] private Text dmg;
    [SerializeField] private Text def;
    [SerializeField] private Text hp;
    [SerializeField] private Text sp;
    public bool inventoryOn = false;

    public GameObject fadeObject;

    public Coroutine castRoutine;
    public Coroutine moveRoutine;

    private TalkManager talkManager;
    private QuestManager questManager;
    [SerializeField] private GameDataObject gameDataObject;
    public static UIManager instance = null;
    private BasicBehaviour basicBehaviour;

    private void Awake()
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

        questListPanel = transform.GetChild(4).gameObject;
        questGiver = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;

        hpBar = transform.GetChild(5).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        hpAmount = transform.GetChild(5).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
        spBar = transform.GetChild(5).transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        spAmount = transform.GetChild(5).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
        expBar = transform.GetChild(5).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>();
        lvText = transform.GetChild(5).transform.GetChild(4).GetComponent<Text>();

        dropPanel = transform.GetChild(7).gameObject;

        inventoryPanel = transform.GetChild(6).gameObject;
        itemInfoText = inventoryPanel.transform.GetChild(5).GetComponentInChildren<Text>();
        itemInfoImage = inventoryPanel.transform.GetChild(4).transform.GetChild(0).GetComponent<Image>();
        str = inventoryPanel.transform.GetChild(3).transform.GetChild(0).GetComponent<Text>();
        agi = inventoryPanel.transform.GetChild(3).transform.GetChild(1).GetComponent<Text>();
        con = inventoryPanel.transform.GetChild(3).transform.GetChild(2).GetComponent<Text>();
        vit = inventoryPanel.transform.GetChild(3).transform.GetChild(3).GetComponent<Text>();
        dmg = inventoryPanel.transform.GetChild(3).transform.GetChild(4).GetComponent<Text>();
        def = inventoryPanel.transform.GetChild(3).transform.GetChild(5).GetComponent<Text>();
        hp = inventoryPanel.transform.GetChild(3).transform.GetChild(6).GetComponent<Text>();
        sp = inventoryPanel.transform.GetChild(3).transform.GetChild(7).GetComponent<Text>();

        fadeObject = transform.GetChild(8).gameObject;

        talkManager = GameObject.Find("TalkManager").GetComponent<TalkManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        basicBehaviour = FindObjectOfType<BasicBehaviour>();
    }

    private void Start()
    {
        interactionPanel.SetActive(false);
        castingBar_back.SetActive(false);
        castingBar.fillAmount = 0f;
        noticeText.gameObject.SetActive(false);
        talkPanel.SetActive(false);
        questListPanel.SetActive(false);
        questGiver.SetActive(false);
        inventoryPanel.SetActive(false);
        dropPanel.SetActive(false);
        itemInfoText.enabled = false;
        itemInfoImage.enabled = false;

        expBar.fillAmount = 0f;
    }

    private void Update()
    {
        if (questListPanel.transform.childCount == 0)
            questListPanel.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AllUiClose();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (questManager.IsStarting || isAction || dropOn) return;
            inventoryOn = !inventoryOn;
            Inventory(inventoryOn);
        }

        if (casting)
        {
            if (basicBehaviour.IsMoving())
            {
                StopCoroutine(moveRoutine);
                StopCasting();
                StartCoroutine(NoticeText(false, "중간에 움직여서 취소됐습니다."));
            }
        }

        if (dropOn)
            if (basicBehaviour.IsMoving())
                CloseDropPanel();

        hpBar.fillAmount = (gameDataObject.Hp / gameDataObject.MaxHp);
        hpAmount.text = $"{gameDataObject.Hp} / {gameDataObject.MaxHp}";
        spBar.fillAmount = (gameDataObject.Sp / gameDataObject.MaxSp);
        spAmount.text = $"{gameDataObject.Sp} / {gameDataObject.MaxSp}";

        str.text = $"STR : {gameDataObject.Str}";
        agi.text = $"AGI : {gameDataObject.Agi}";
        con.text = $"CON : {gameDataObject.Con}";
        vit.text = $"VIT : {gameDataObject.Vit}";
        dmg.text = $"DAM : {gameDataObject.Dam}";
        def.text = $"DEF : {gameDataObject.Def}";
        hp.text = $"HP : {gameDataObject.MaxHp}";
        sp.text = $"SP : {gameDataObject.MaxSp}";
    }

    private void AllUiClose()
    {
        CloseDropPanel();
        inventoryOn = false;
        Inventory(inventoryOn);
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
        AllUiClose();
    }

    void Talk(int id)
    {
        int questTalkIdx = questManager.GetQuestTalkIdx(id);
        string talkData = talkManager.GetTalk(id + questTalkIdx, talkIndex);

        if (talkData == null)
        {
            isAction = false;
            talkIndex = 0;
            questManager.CheckQuest(id);
            return;
        }

        npcName.text = talkManager.GetName(id, int.Parse(talkData.Split(':')[0]));
        talkText.text = talkData.Split(':')[1];
        portrait.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[2]));

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

    #region [경험치/레벨업 함수]
    public void UpdateLevel(int level)
    {
        lvText.text = $"Lv {level}";
    }

    public void UpdateExp(float exp)
    {
        gameDataObject.Exp += exp;
        expBar.fillAmount = gameDataObject.Exp / gameDataObject.Exp_require;
    }
    #endregion

    #region [플레이어 HP/SP]
    public void DisplayHpBar()
    {
        hpBar.fillAmount = (gameDataObject.Hp / gameDataObject.MaxHp);
        hpAmount.text = $"{gameDataObject.Hp} / {gameDataObject.MaxHp}";
    }

    public void DisplaySpBar()
    {
        spBar.fillAmount = (gameDataObject.Sp / gameDataObject.MaxSp);
        spAmount.text = $"{gameDataObject.Sp} / {gameDataObject.MaxSp}";
    }
    #endregion

    #region [아이템 드랍]
    public void OpenDropPanel(DropItem obj)
    {
        Inventory(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        dropPanel.SetActive(true);
        dropOn = true;
        items = obj.items;
        dropPrefab = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            if (items[i] != null)
            {
                dropPrefab[i] = Instantiate(Resources.Load<GameObject>("Prefabs/DroppedItem"), dropPanel.transform);
                if (items[i].itemType == ItemType.potion)
                {
                    Potion potion = items[i] as Potion;
                    dropPrefab[i].transform.GetChild(0).GetComponentInChildren<Text>().enabled = true;
                    dropPrefab[i].transform.GetChild(0).GetComponentInChildren<Text>().text = potion.count.ToString();
                }
                dropPrefab[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].img;
                dropPrefab[i].transform.GetChild(1).GetComponent<Text>().text = items[i].name;
                dropPrefab[i].GetComponent<GetDropItem>().itemIdx = i;
            }
        }
    }

    public void CloseDropPanel()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (dropPrefab != null)
        {
            foreach (var prefab in dropPrefab)
                Destroy(prefab);
        }
        dropPanel.SetActive(false);
        dropOn = false;
    }
    #endregion

    #region [인벤토리/스테이터스]
    private void Inventory(bool isOn)
    {
        inventoryPanel.SetActive(isOn);

        if (isOn)
        {
            GameManager.instance.InventoryOn();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            GameManager.instance.EmptyItemObject();
            itemInfoText.enabled = false;
            itemInfoImage.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void ShowItemInfo(Item item)
    {
        itemInfoImage.enabled = true;
        itemInfoText.enabled = true;

        Weapon weapon;
        Clothes clothes;
        Potion potion;
        if (item.itemType == ItemType.shortWeapon || item.itemType == ItemType.longWeapon)
        {
            weapon = item as Weapon;
            itemInfoText.text = $"<{item.name}>\n\n" +
                                $"DAM : {weapon.damage}\n" +
                                $"STR : {item.str}\n" +
                                $"AGI : {item.agi}\n" +
                                $"CON : {item.con}\n" +
                                $"VIT : {item.vit}";
        }
        else if (item.itemType == ItemType.potion)
        {
            potion = item as Potion;
            itemInfoText.text = $"<{item.name}>\n\n" +
                                $"FILL : +{potion.amount}\n" +
                                $"NUM : {potion.count}";
        }
        else
        {
            clothes = item as Clothes;
            itemInfoText.text = $"<{item.name}>\n\n" +
                                $"DEF : {clothes.def}\n" +
                                $"STR : {item.str}\n" +
                                $"AGI : {item.agi}\n" +
                                $"CON : {item.con}\n" +
                                $"VIT : {item.vit}";
        }

        itemInfoImage.sprite = item.img;
        itemInfoImage.preserveAspect = true;
    }
    #endregion
}