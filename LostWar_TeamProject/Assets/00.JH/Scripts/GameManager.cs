using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ItemSpace;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;// 포즈 여부 판단 변수
    public static GameManager instance;// 싱글턴
    private CanvasGroup group_inventory;// 인벤토리 창
    private CanvasGroup group_status;// 스탯 창
    private GameObject button_inventory;// 인벤토리 창의 버튼
    public GameDataObject gameDataObject;// 데이터 오브젝트
    public Transform Content;//인벤토리 스크롤 안의 콘텐트
    [Header("Equiped Item Panel")]
    public Transform equiped_shortWeapon;//현재 장착 중인 장비들이 있는 Panel
    public Transform equiped_longWeapon;
    public Transform equiped_shoes;
    public Transform equiped_top;
    public Transform equiped_bottoms;
    private GameObject shortWeapon_C;//현재 장착 중인 장비 버튼 오브젝트
    private GameObject longWeapon_C;
    private GameObject shoes_C;
    private GameObject top_C;
    private GameObject bottoms_C;
    public CanvasGroup get_F;
    private List<GameObject> list_inventory;//일괄 destroy를 위한 리스트
    private MoveBehaviour moveBehaviour;// 플레이어 움직임
    [Header("Player Status in Inventory")]// 인벤토리 창의 스탯 표기
    public Text str_I;
    public Text con_I;
    public Text vit_I;
    public Text dd_I;
    [Header("Player Status in Status")]// 스테이터스 창의 스탯 표기
    public Text level_S;
    public Text str_S;
    public Text con_S;
    public Text vit_S;
    public Text dam_S;
    public Text def_S;
    [Header("Enemy Hp")]// 적 체력
    public CanvasGroup canvasGroup_hp;// 판넬
    public Image image_hp;// 체력 이미지
    public Text text_hp;// 적 이름
    [Header("Exp&Level")]
    public Image image_exp;// 경험치 바
    public Text text_level;// 레벨 텍스트
    [Header("Item Image")]// 아이템 이미지들
    public Image axe_image;
    public Image sword_image;
    public Image pistol_image;
    public Image revolver_image;
    public Image shirts_image;
    public Image coat_image;
    public Image jeans_image;
    public Image pants_image;
    public Image boots_image;
    public Image sneakers_image;
    private Image cur_image;// 현재 아이템 이미지
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Cursor.visible = false;// 마우스 커서 안보이게
        Cursor.lockState = CursorLockMode.Locked;// 마우스 커서 잠금
        button_inventory = GameObject.Find("Button-Inventory");// 인벤토리 창 버튼 정의
        group_inventory = GameObject.Find("Panel-Inventory").GetComponent<CanvasGroup>();//인벤토리 판넬 창 정의
        group_status = GameObject.Find("Panel-Status").GetComponent<CanvasGroup>();// 스테이터스 판넬 창 정의
        list_inventory = new List<GameObject>();// destroy 위한 아이템 리스트 할당
        shortWeapon_C = null;//현재 장착 중인 장비 버튼 오브젝트
        longWeapon_C = null;
        shoes_C = null;
        top_C = null;
        bottoms_C = null;
        moveBehaviour = GameObject.FindWithTag("Player").GetComponent<MoveBehaviour>();// 플레이어 움직임 스크립트
        Reset();
}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))// i 버튼 누르면
        {
            if(!isPaused)// 포즈 상태가 아니라면
            Inventory(true); // 인벤토리 창 열기
        }
        if (Input.GetKeyDown(KeyCode.P))// p 버튼 누르면
        {
            if (!isPaused)// 포즈 상태가 아니라면
                Status(true);// 스테이터스 창 열기
        }
        if (Input.GetKeyDown(KeyCode.Escape))// esc 버튼 누르면
        {
            Inventory(false);// 인벤토리 창 닫기
            Status(false);// 스테이터스 창 닫기
            EmptyItemObject();// 아이템 리스트 비우기
        }
    }
    public void Pause(bool stop)// 포즈 함수
    {
        isPaused = stop;// 포즈 상태는 매개 변수에 따름
        Time.timeScale = (stop) ? 0.0f : 1.0f;//포즈 상태면 타임 스케일 0, 아니라면 1
        foreach (var x in GameObject.FindWithTag("Player").GetComponents<GenericBehaviour>())//플레이어에게 있는 모든 스크립트 작동 중지
        {
            x.enabled = !stop;
        }
        Camera.main.GetComponent<ThirdPersonOrbitCamBasic>().enabled = !stop;// 카메라 작동 여부는 매개 변수에 따름
    }
    public void Inventory(bool stop)// 인벤토리 창
    {
        if (stop)
        {
            get_F.alpha = 0f;// 상호작용 창 안보이게
            group_inventory.alpha = 1f;// 인벤토리 창 보이게
            Cursor.lockState = CursorLockMode.None;// 마우스 커서 잠금 해제
            AddInventory(gameDataObject.shortWeapon);// 장비들 인벤토리 창에 추가
            AddInventory(gameDataObject.longWeapon);
            AddInventory(gameDataObject.shoes);
            AddInventory(gameDataObject.top);
            AddInventory(gameDataObject.bottoms);
            shortWeapon_C = AddEquip(equiped_shortWeapon, gameDataObject.shortWeapon_C);//버튼이 들어갈 부모, 게임 데이터 오브젝트의 정보
            longWeapon_C = AddEquip(equiped_longWeapon, gameDataObject.longWeapon_C);
            shoes_C = AddEquip(equiped_shoes, gameDataObject.shoes_C);
            top_C = AddEquip(equiped_top, gameDataObject.top_C);
            bottoms_C = AddEquip(equiped_bottoms, gameDataObject.bottoms_C );
        }
        else
        {
            group_inventory.alpha = 0f;// 인벤토리 창 안보이게
            Cursor.lockState = CursorLockMode.Locked;// 마우스 커서 잠금
        }
        button_inventory.SetActive(!stop);// 인벤토리 창 버튼 활성화 여부는 매개 변수에 따름
        Cursor.visible = stop;// 마우스 커서가 보이느냐는 매개 변수에 따름
        Pause(stop);// 포즈 여부는 매개변수에 따름
        group_inventory.interactable = stop;// 인벤토리 창 상호작용 가능 여부는 매개 변수에 따름
    }
    public void Status(bool stop)// 스테이터스 창
    {
        if (stop)
        {
            isPaused = true;// 포즈 상태
            get_F.alpha = 0f;// 상호작용 창은 안보이게
            group_status.alpha = 1f;// 스테이터스 창은 보이게
            Cursor.lockState = CursorLockMode.None;// 마우스 커서 잠금 해제
            level_S.text = string.Format("LV : {0}", gameDataObject.Level.ToString());// 각 스탯을 데이터에서 가져와 텍스트로 적용
            str_S.text = string.Format("STR : {0}", gameDataObject.Str.ToString());
            con_S.text = string.Format("CON : {0}", gameDataObject.Con.ToString());
            vit_S.text = string.Format("VIT : {0}", gameDataObject.Vit.ToString());
            dam_S.text = string.Format("DAM : {0}", gameDataObject.Dam.ToString());
            def_S.text = string.Format("DEF : {0}", gameDataObject.Def.ToString());
        }
        else
        {
            group_status.alpha = 0f;// 스테이터스 창 안보이게
            Cursor.lockState = CursorLockMode.Locked;// 마우스 커서 잠금
            
        }
        group_status.interactable = stop;// 스테이터스 창 상호작용 가능 여부는 매개 변수에 따름
        group_status.blocksRaycasts = stop;// 스테이터스 창 마우스 클릭 가능 여부는 매개 변수에 따름
        Cursor.visible = stop;// 마우스 커서가 보이느냐는 매개 변수에 따름
        Pause(stop);// 포즈 여부는 매개 변수에 따름
    }

    private void EmptyItemObject()// 아이템 리스트 비우기
    {
        foreach (GameObject o in list_inventory)// 리스트에 들어잇는 오브젝트들 일괄 destroy
        {
            Destroy(o);
        }
        list_inventory.Clear();// 리스트 비우기
        Destroy(shortWeapon_C);// 현재 장착 장비 destroy
        Destroy(longWeapon_C);
        Destroy(top_C);
        Destroy(bottoms_C);
        Destroy(shoes_C);
    }

    private GameObject AddEquip(Transform parent, Item item_c)// 착용 장비 적용
    {
        if (item_c==null|| item_c.name == null || item_c.name == "") return null;
        GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), parent);
        temp.GetComponent<EquipItem>().item = item_c;
        temp.GetComponent<EquipItem>().isEquiped = true;
        temp.GetComponentInChildren<Text>().text = item_c.name;
        return temp;
    }


    private void AddInventory(List<Item> list_item)// 인벤토리 아이템 적용
    {
        for (int i = 0; i < list_item.Count; i++)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>("Inventory_Button_Item"), Content);
            temp.GetComponent<EquipItem>().item = list_item[i];
            temp.GetComponentInChildren<Text>().text = list_item[i].name;
            list_inventory.Add(temp);
        }
    }
    public void Reset()
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
        gameDataObject.Con = 0;
        gameDataObject.Vit = 0;
        gameDataObject.Status_own = 7;
        gameDataObject.Hp = 100;
        gameDataObject.MaxHp = 100;
        gameDataObject.Dam = 5;
        gameDataObject.Weight = 0;
        gameDataObject.Def = 0;

    }
    public void EnemyHp(float health, float startHp, LivingEntity livingEntity)// 적 체력 UI
    {
        StartCoroutine(ShowCanvasGroup(canvasGroup_hp));
        image_hp.fillAmount = health / startHp;
        text_hp.text = livingEntity.gameObject.name;
        if (image_hp.fillAmount > 0.6f)
        {
            image_hp.color = Color.green;
        }
        else if (image_hp.fillAmount > 0.3f)
        {
            image_hp.color = Color.yellow;
        }
        else
        {
            image_hp.color = Color.red;
        }
    }
    IEnumerator ShowCanvasGroup(CanvasGroup canvasGroup)//UI가 보이게 하기 위한 함수(Enemy 체력바를 위한)
    {
        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(3f);
        canvasGroup.alpha = 0f;
    }
    public void ShowInventoryStatus(Item item)// 인벤토리 창의 아이템 스탯
    {
        str_I.text = string.Format("STR : {0}", item.str.ToString());
        con_I.text = string.Format("CON : {0}", item.con.ToString());
        vit_I.text = string.Format("VIT : {0}", item.vit.ToString());
        Weapon weapon;
        Clothes clothes;
        if (item.itemType == ItemType.longWeapon || item.itemType == ItemType.shortWeapon)
        {
            weapon = item as Weapon;
            dd_I.text = string.Format("DAM : {0}", weapon.damage.ToString());
        }
        else
        {
            clothes = item as Clothes;
            dd_I.text = string.Format("DEF : {0}", clothes.def.ToString());
        }
    }
    public void EquipClick()// 장착 버튼
    {
        if (EquipItem.focused != null)
            if (!EquipItem.focused.isEquiped)
            {
                ChangeEquip(EquipItem.focused.item.itemType);
            }
    }
    public void OffClick()// 장비 해제 버튼
    {
        if (EquipItem.focused.isEquiped)
        {
            switch (EquipItem.focused.item.itemType)
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
            if (!list_inventory.Contains(EquipItem.focused.gameObject))
            {
                list_inventory.Add(EquipItem.focused.gameObject);
            }
        }
    }

    private void OffEquip(List<Item> list_item, Item item_c, GameObject object_c)// 장비 해제 시 행동
    {
        list_item.Add(item_c);//리스트에 넣는다
        object_c.transform.SetParent(Content);//버튼을 옮긴다
        EquipItem temp = object_c.GetComponent<EquipItem>();//버튼의 equipitem을 받아온다.
        temp.isEquiped = false;
        gameDataObject.Str -= temp.item.str;
        gameDataObject.Con -= temp.item.con;
        gameDataObject.Vit -= temp.item.vit;
        if (item_c.itemType == ItemType.shortWeapon || item_c.itemType == ItemType.longWeapon)
        {
            Weapon weapon_temp = temp.item as Weapon;
            gameDataObject.Dam -= weapon_temp.damage;
        }
        else
        {
            Clothes clothes_temp = temp.item as Clothes;
            gameDataObject.Def -= clothes_temp.def;
        }
    }

    private void ChangeEquip(ItemType itemType)// 장착 아이템을 바꿀때
    {
        Weapon weapon_temp;
        Clothes clothes_temp;
        //장착 중인게 있을 때와 없을 때를 구분해줘야함.
        switch (itemType)
        {
            case ItemType.shortWeapon:
                if (shortWeapon_C != null)//장착 중인게 있을 때
                {
                    OffEquip(gameDataObject.shortWeapon, gameDataObject.shortWeapon_C, shortWeapon_C);
                }
                EquipItem.focused.transform.SetParent(equiped_shortWeapon);//인벤토리에서 누른 장비의 버튼을 장착칸으로 옮긴다.
                gameDataObject.shortWeapon_C = EquipItem.focused.item as Weapon;//데이터오브젝트
                shortWeapon_C = EquipItem.focused.gameObject;//장비 칸에 들어갈 게임 오브젝트를 변수에 대입
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                weapon_temp = EquipItem.focused.item as Weapon;
                gameDataObject.Dam += weapon_temp.damage;
                gameDataObject.shortWeapon.Remove(EquipItem.focused.item);
                moveBehaviour.ChangeItemObject(EquipItem.focused.item.name);
                moveBehaviour.usingWeapon = MoveBehaviour.UsingWeapon.short_dist;
                break;

            case ItemType.longWeapon:
                if (longWeapon_C != null)
                {
                    OffEquip(gameDataObject.longWeapon, gameDataObject.longWeapon_C, longWeapon_C);
                }
                EquipItem.focused.transform.SetParent(equiped_longWeapon);
                gameDataObject.longWeapon_C = EquipItem.focused.item as Weapon;
                longWeapon_C = EquipItem.focused.gameObject;
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                weapon_temp = EquipItem.focused.item as Weapon;
                gameDataObject.Dam += weapon_temp.damage;
                gameDataObject.longWeapon.Remove(EquipItem.focused.item);
                moveBehaviour.ChangeItemObject(EquipItem.focused.item.name);
                moveBehaviour.usingWeapon = MoveBehaviour.UsingWeapon.long_dist;
                break;

            case ItemType.shoes:
                if (shoes_C != null)
                {
                    OffEquip(gameDataObject.shoes, gameDataObject.shoes_C, shoes_C);
                }
                EquipItem.focused.transform.SetParent(equiped_shoes);
                gameDataObject.shoes_C = EquipItem.focused.item as Clothes;
                shoes_C = EquipItem.focused.gameObject;
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                clothes_temp = EquipItem.focused.item as Clothes;
                gameDataObject.Def += clothes_temp.def;
                gameDataObject.shoes.Remove(EquipItem.focused.item);
                break;

            case ItemType.top:
                if (top_C != null)
                {
                    OffEquip(gameDataObject.top, gameDataObject.top_C, top_C);
                }
                EquipItem.focused.transform.SetParent(equiped_top);
                gameDataObject.top_C = EquipItem.focused.item as Clothes;
                top_C = EquipItem.focused.gameObject;
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                clothes_temp = EquipItem.focused.item as Clothes;
                gameDataObject.Def += clothes_temp.def;
                gameDataObject.top.Remove(EquipItem.focused.item);
                break;

            case ItemType.bottoms:
                if (bottoms_C != null)
                {
                    OffEquip(gameDataObject.bottoms, gameDataObject.bottoms_C, bottoms_C);
                }
                EquipItem.focused.transform.SetParent(equiped_bottoms);
                gameDataObject.bottoms_C = EquipItem.focused.item as Clothes;
                bottoms_C = EquipItem.focused.gameObject;
                EquipItem.focused.isEquiped = true;
                gameDataObject.Str += EquipItem.focused.item.str;
                gameDataObject.Con += EquipItem.focused.item.con;
                gameDataObject.Vit += EquipItem.focused.item.vit;
                clothes_temp = EquipItem.focused.item as Clothes;
                gameDataObject.Def += clothes_temp.def;
                gameDataObject.bottoms.Remove(EquipItem.focused.item);
                break;
        }
        EquipItem.focused.GetComponent<Image>().color = EquipItem.focused.origin_color;
        EquipItem.focused = null;
    }
    public void UpdateLevel(int Level)// UI의 레벨 텍스트를 실시간 반영
    {
        text_level.text = Level.ToString();
    }
    public void UpdateExp(float exp)// UI의 경험치 바를 실시간 반영
    {
        gameDataObject.Exp += exp;
        image_exp.fillAmount = gameDataObject.Exp/gameDataObject.Exp_require;
    }
    public void InventoryImageChange()// 인벤토리 이미지 바꾸기
    {
        if(cur_image!=null)
        cur_image.enabled = false;
        switch (EquipItem.focused.item.name)
        {
            case "Axe":
                axe_image.enabled = true;
                cur_image = axe_image;
                break;
            case "Sword":
                sword_image.enabled = true;
                cur_image = sword_image;
                break;
            case "Pistol":
                pistol_image.enabled = true;
                cur_image = pistol_image;
                break;
            case "Revolver":
                revolver_image.enabled = true;
                cur_image = revolver_image;
                break;
            case "Shirts":
                shirts_image.enabled = true;
                cur_image = shirts_image;
                break;
            case "Coat":
                coat_image.enabled = true;
                cur_image = coat_image;
                break;
            case "Jeans":
                jeans_image.enabled = true;
                cur_image = jeans_image;
                break;
            case "Pants":
                pants_image.enabled = true;
                cur_image = pants_image;
                break;
            case "Boots":
                boots_image.enabled = true;
                cur_image = boots_image;
                break;
            case "Sneakers":
                sneakers_image.enabled = true;
                cur_image = sneakers_image;
                break;
        }
    }
}