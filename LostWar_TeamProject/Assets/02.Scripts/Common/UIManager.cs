using System.Collections;
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
    [SerializeField] private Transform[] itemSlots;
    [SerializeField] private Transform equiped_shortWeapon;
    [SerializeField] private Transform equiped_longWeapon;
    [SerializeField] private Transform equiped_shoes;
    [SerializeField] private Transform equiped_top;
    [SerializeField] private Transform equiped_bottoms;
    private GameObject shortWeapon_C;
    private GameObject longWeapon_C;
    private GameObject shoes_C;
    private GameObject top_C;
    private GameObject bottoms_C;
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
    private List<GameObject> list_inventory = new List<GameObject>();
    private bool inventoryOn = false;
    private float clickTime = 0;

    public Coroutine castRoutine;

    private TalkManager talkManager;
    private QuestManager questManager;
    [SerializeField] private GameDataObject gameDataObject;
    public static UIManager instance = null;

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
        buttonPanel = talkPanel.transform.GetChild(3).gameObject;
        functionText = buttonPanel.transform.GetChild(0).GetComponentInChildren<Text>();

        questListPanel = transform.GetChild(4).gameObject;
        questPanel = transform.GetChild(5).gameObject;
        titleText = questPanel.transform.GetChild(0).GetComponent<Text>();
        contentsText = questPanel.transform.GetChild(1).GetComponentInChildren<Text>();
        questGiver = GameObject.FindGameObjectWithTag("Player").transform.GetChild(1).gameObject;

        hpBar = transform.GetChild(6).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>();
        hpAmount = transform.GetChild(6).transform.GetChild(1).transform.GetChild(2).GetComponent<Text>();
        spBar = transform.GetChild(6).transform.GetChild(2).transform.GetChild(0).GetComponent<Image>();
        spAmount = transform.GetChild(6).transform.GetChild(2).transform.GetChild(2).GetComponent<Text>();
        expBar = transform.GetChild(6).transform.GetChild(3).transform.GetChild(0).GetComponent<Image>();
        lvText = transform.GetChild(6).transform.GetChild(4).GetComponent<Text>();

        dropPanel = transform.GetChild(8).gameObject;

        inventoryPanel = transform.GetChild(7).gameObject;
        itemSlots = inventoryPanel.transform.GetChild(2).GetComponentsInChildren<Transform>();
        equiped_top = inventoryPanel.transform.GetChild(1).transform.GetChild(1).GetComponent<Transform>();
        equiped_bottoms = inventoryPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<Transform>();
        equiped_shoes = inventoryPanel.transform.GetChild(1).transform.GetChild(3).GetComponent<Transform>();
        equiped_shortWeapon = inventoryPanel.transform.GetChild(1).transform.GetChild(4).GetComponent<Transform>();
        equiped_longWeapon = inventoryPanel.transform.GetChild(1).transform.GetChild(5).GetComponent<Transform>();
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

        talkManager = GameObject.Find("TalkManager").GetComponent<TalkManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
    }

    private void Start()
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
        inventoryPanel.SetActive(false);
        dropPanel.SetActive(false);
        itemInfoText.enabled = false;
        itemInfoImage.enabled = false;

        expBar.fillAmount = 0f;
        shortWeapon_C = null;
        longWeapon_C = null;
        shoes_C = null;
        top_C = null;
        bottoms_C = null;
        Reset();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AllUiClose();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryOn = !inventoryOn;
            Inventory(inventoryOn);
        }

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

    private void Reset()
    {
        gameDataObject.shortWeapon.Clear();
        gameDataObject.longWeapon.Clear();
        gameDataObject.shoes.Clear();
        gameDataObject.top.Clear();
        gameDataObject.bottoms.Clear();

        gameDataObject.shortWeapon_C = null;
        gameDataObject.longWeapon_C = null;
        gameDataObject.shoes_C = null;
        gameDataObject.top_C = null;
        gameDataObject.bottoms_C = null;

        gameDataObject.Level = 1;
        gameDataObject.Exp = 0;
        gameDataObject.Exp_require = 10;

        gameDataObject.Str = 0;
        gameDataObject.Agi = 0;
        gameDataObject.Con = 0;
        gameDataObject.Vit = 0;
        gameDataObject.Status_own = 7;
        gameDataObject.MaxHp = 1000;
        gameDataObject.Hp = 1000;
        gameDataObject.MaxSp = 100;
        gameDataObject.Sp = 100;
        gameDataObject.Dam = 5;
        gameDataObject.Def = 5;
        gameDataObject.Weight = 0;
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

    #region [아이템 드랍]
    public void OpenDropPanel(DropItem obj)
    {
        dropPanel.SetActive(true);
        dropOn = true;
        items = obj.items;
        dropPrefab = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            if (items[i] != null)
            {
                dropPrefab[i] = Instantiate(Resources.Load<GameObject>("Prefabs/DroppedItem"), dropPanel.transform);
                dropPrefab[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].img;
                dropPrefab[i].transform.GetChild(1).GetComponent<Text>().text = items[i].name;
                dropPrefab[i].GetComponent<GetDropItem>().itemIdx = i;
            }
        }
    }

    public void CloseDropPanel()
    {
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
            AddInventory(gameDataObject.shortWeapon);
            AddInventory(gameDataObject.longWeapon);
            AddInventory(gameDataObject.shoes);
            AddInventory(gameDataObject.top);
            AddInventory(gameDataObject.bottoms);
            shortWeapon_C = AddEquip(equiped_shortWeapon, gameDataObject.shortWeapon_C);
            longWeapon_C = AddEquip(equiped_longWeapon, gameDataObject.longWeapon_C);
            shoes_C = AddEquip(equiped_shoes, gameDataObject.shoes_C);
            top_C = AddEquip(equiped_top, gameDataObject.top_C);
            bottoms_C = AddEquip(equiped_bottoms, gameDataObject.bottoms_C);
        }
        else
        {
            EmptyItemObject();
            itemInfoText.enabled = false;
            itemInfoImage.enabled = false;
        }
    }

    private void EmptyItemObject()
    {
        foreach (GameObject obj in list_inventory)
            Destroy(obj);
        list_inventory.Clear();
        Destroy(shortWeapon_C);
        Destroy(longWeapon_C);
        Destroy(top_C);
        Destroy(bottoms_C);
        Destroy(shoes_C);
    }

    private void AddInventory(List<Item> items)
    {
        int idx = 1;
        
        for (int i = 1; i < itemSlots.Length + 1; i++)
        {
            if (itemSlots[i].childCount != 0)
                idx++;
            else
                break;
        }

        foreach (var item in items)
        {
            GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemSlot"), itemSlots[idx]);
            obj.GetComponent<SlotItemInfo>().item = item;
            obj.GetComponent<Image>().sprite = item.img;
            list_inventory.Add(obj);
        }
    }

    private GameObject AddEquip(Transform parent, Item item_c)
    {
        if (item_c == null || item_c.name == null || item_c.name == "") return null;
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/ItemSlot"), parent);
        obj.GetComponent<SlotItemInfo>().item = item_c;
        obj.GetComponent<Image>().sprite = item_c.img;
        obj.GetComponent<SlotItemInfo>().isEquip = true;
        return obj;
    }

    public void ShowItemInfo(Item item)
    {
        itemInfoImage.enabled = true;
        itemInfoText.enabled = true;

        Weapon weapon;
        Clothes clothes;
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

    private void Equip(ItemType type)
    {
        Weapon weapon;
        Clothes clothes;

        switch (type)
        {
            case ItemType.shortWeapon:
                if (shortWeapon_C != null)
                    OffEquip(gameDataObject.shortWeapon, gameDataObject.shortWeapon_C, shortWeapon_C);
                SlotItemInfo.instance.transform.SetParent(equiped_shortWeapon);
                gameDataObject.shortWeapon_C = SlotItemInfo.instance.item as Weapon;
                shortWeapon_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                weapon = SlotItemInfo.instance.item as Weapon;
                gameDataObject.Dam += weapon.damage;
                gameDataObject.shortWeapon.Remove(SlotItemInfo.instance.item);


                break;
            case ItemType.longWeapon:
                if (longWeapon_C != null)
                    OffEquip(gameDataObject.longWeapon, gameDataObject.longWeapon_C, longWeapon_C);
                SlotItemInfo.instance.transform.SetParent(equiped_longWeapon);
                gameDataObject.longWeapon_C = SlotItemInfo.instance.item as Weapon;
                longWeapon_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                weapon = SlotItemInfo.instance.item as Weapon;
                gameDataObject.Dam += weapon.damage;
                gameDataObject.longWeapon.Remove(SlotItemInfo.instance.item);


                break;
            case ItemType.shoes:
                if (shoes_C != null)
                    OffEquip(gameDataObject.shoes, gameDataObject.shoes_C, shoes_C);
                SlotItemInfo.instance.transform.SetParent(equiped_shoes);
                gameDataObject.shoes_C = SlotItemInfo.instance.item as Clothes;
                shoes_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                clothes = SlotItemInfo.instance.item as Clothes;
                gameDataObject.Def += clothes.def;
                gameDataObject.shoes.Remove(SlotItemInfo.instance.item);
                break;
            case ItemType.top:
                if (top_C != null)
                    OffEquip(gameDataObject.top, gameDataObject.top_C, top_C);
                SlotItemInfo.instance.transform.SetParent(equiped_top);
                gameDataObject.top_C = SlotItemInfo.instance.item as Clothes;
                top_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                clothes = SlotItemInfo.instance.item as Clothes;
                gameDataObject.Def += clothes.def;
                gameDataObject.top.Remove(SlotItemInfo.instance.item);
                break;
            case ItemType.bottoms:
                if (bottoms_C != null)
                    OffEquip(gameDataObject.bottoms, gameDataObject.bottoms_C, bottoms_C);
                SlotItemInfo.instance.transform.SetParent(equiped_bottoms);
                gameDataObject.bottoms_C = SlotItemInfo.instance.item as Clothes;
                bottoms_C = SlotItemInfo.instance.gameObject;
                SlotItemInfo.instance.isEquip = true;
                gameDataObject.Str += SlotItemInfo.instance.item.str;
                gameDataObject.Agi += SlotItemInfo.instance.item.agi;
                gameDataObject.Con += SlotItemInfo.instance.item.con;
                gameDataObject.Vit += SlotItemInfo.instance.item.vit;
                clothes = SlotItemInfo.instance.item as Clothes;
                gameDataObject.Def += clothes.def;
                gameDataObject.bottoms.Remove(SlotItemInfo.instance.item);
                break;
        }
    }

    private void OffEquip(List<Item> list_item, Item item_c, GameObject object_c)
    {
        int idx = 1;

        for (int i = 1; i < itemSlots.Length + 1; i++)
        {
            if (itemSlots[i].childCount != 0)
                idx++;
            else
                break;
        }

        list_item.Add(item_c);
        object_c.transform.SetParent(itemSlots[idx]);
        SlotItemInfo temp = object_c.GetComponent<SlotItemInfo>();
        temp.isEquip = false;
        gameDataObject.Str -= temp.item.str;
        gameDataObject.Agi -= temp.item.agi;
        gameDataObject.Con -= temp.item.con;
        gameDataObject.Vit -= temp.item.vit;

        if (item_c.itemType == ItemType.shortWeapon || item_c.itemType == ItemType.longWeapon)
        {
            Weapon weapon = temp.item as Weapon;
            gameDataObject.Dam -= weapon.damage;
        }
        else
        {
            Clothes clothes = temp.item as Clothes;
            gameDataObject.Def -= clothes.def;
        }
    }

    public void OnDoubleClickEquip()
    {
        if (SlotItemInfo.instance != null)
        {
            if (!SlotItemInfo.instance.isEquip)
                Equip(SlotItemInfo.instance.item.itemType);
        }
    }

    public void OnDoubleClickEquipOff()
    {
        if (SlotItemInfo.instance.isEquip)
        {
            switch (SlotItemInfo.instance.item.itemType)
            {
                case ItemType.shortWeapon:
                    OffEquip(gameDataObject.shortWeapon, gameDataObject.shortWeapon_C, shortWeapon_C);
                    gameDataObject.shortWeapon_C = null;
                    shortWeapon_C = null;
                    break;
                case ItemType.longWeapon:
                    OffEquip(gameDataObject.longWeapon, gameDataObject.longWeapon_C, longWeapon_C);
                    gameDataObject.longWeapon_C = null;
                    longWeapon_C = null;
                    break;
                case ItemType.top:
                    OffEquip(gameDataObject.top, gameDataObject.top_C, top_C);
                    gameDataObject.top_C = null;
                    top_C = null;
                    break;
                case ItemType.bottoms:
                    OffEquip(gameDataObject.bottoms, gameDataObject.bottoms_C, bottoms_C);
                    gameDataObject.bottoms_C = null;
                    bottoms_C = null;
                    break;
                case ItemType.shoes:
                    OffEquip(gameDataObject.shoes, gameDataObject.shoes_C, shoes_C);
                    gameDataObject.shoes_C = null;
                    shoes_C = null;
                    break;
            }

            if (!list_inventory.Contains(SlotItemInfo.instance.gameObject))
                list_inventory.Add(SlotItemInfo.instance.gameObject);
        }
    }
    #endregion
}