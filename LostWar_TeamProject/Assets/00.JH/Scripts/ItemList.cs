using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSpace
{
    public class ItemList : MonoBehaviour//방어구 아이템 리스트
    {
        //아이템 목록, 무기는 장착중인 것 넣어야한다.
        [SerializeField] private List<string> Shoes;
        [SerializeField] private List<string> Top;
        [SerializeField] private List<string> Bottoms;
        public static ItemList instance;
        private void Awake()
        {
            instance = this;
            Shoes = new List<string>();
            Top = new List<string>();
            Bottoms = new List<string>();
            Init_Shoes();
            Init_Top();
            Init_Bottoms();
        }

        private void Init_Bottoms()// 하의 아이템 리스트
        {
            Bottoms.Add("Pants");
            Bottoms.Add("Jeans");
        }

        private void Init_Top()// 상의 아이템 리스트
        {
            Top.Add("Shirts");
            Top.Add("Coat");
        }

        private void Init_Shoes()// 신발 아이템 리스트
        {
            Shoes.Add("Boots");
            Shoes.Add("Sneakers");
        }
        public Item Get(string _name)// 아이템 스탯 무작위 지정
        {
            switch (_name)
            {
                //근거리 무기
                case "Sword":
                    return new Weapon("Sword", Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, ItemType.shortWeapon);
                case "Axe":
                    return new Weapon("Axe", Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, ItemType.shortWeapon);
                //원거리 무기
                case "Pistol":
                    return new Weapon("Pistol", Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, ItemType.longWeapon);
                case "Revolver":
                    return new Weapon("Revolver", Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, ItemType.longWeapon);
                //신발
                case "Boots":
                    return new Clothes("Boots", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes);
                case "Sneakers":
                    return new Clothes("Sneakers", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes);
                //상의
                case "Shirts":
                    return new Clothes("Shirts", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, ItemType.top);
                case "Coat":
                    return new Clothes("Coat", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, ItemType.top);
                //하의
                case "Pants":
                    return new Clothes("Pants", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, ItemType.bottoms);
                case "Jeans":
                    return new Clothes("Jeans", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(10, 13), 3, ItemType.bottoms);
                default:
                    return null;
            }
        }
        public Item[] GetRandom()// 아이템 랜덤 드랍
        {
            Item[] item= new Item[3];// 드랍되는 아이템은 3개
            int randint = Random.Range(0, 5);
            switch (randint)// 첫 번째 아이템 드랍률
            {
                case 0:
                    item[0]=new Weapon("Sword", Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, ItemType.shortWeapon);
                    break;
                case 1:
                    item[0]= new Weapon("Axe", Random.Range(2, 5), Random.Range(0, 3), Random.Range(0, 3), Random.Range(15, 21), 5, ItemType.shortWeapon);
                    break;
                case 2:
                    item[0] = new Weapon("Pistol", Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, ItemType.longWeapon);
                    break;
                case 3:
                    item[0] = new Weapon("Revolver", Random.Range(1, 3), Random.Range(2, 5), Random.Range(0, 3), Random.Range(10, 16), 5, ItemType.longWeapon);
                    break;
                default:
                    item[0] = null;
                    break;
            }
            randint = Random.Range(0, 8);
            switch (randint)// 두 번째 아이템 드랍률
            {
                case 0:
                    item[1] = new Clothes("Boots", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes);
                    break;
                case 1:
                    item[1] = new Clothes("Sneakers", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes);
                    break;
                case 2:
                    item[1] = new Clothes("Shirts", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.top);
                    break;
                case 3:
                    item[1] = new Clothes("Coat", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.top);
                    break;
                case 4:
                    item[1] = new Clothes("Pants", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.bottoms);
                    break;
                case 5:
                    item[1] = new Clothes("Jeans", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.bottoms);
                    break;
                default:
                    item[1] = null;
                    break;
            }
            randint = Random.Range(0, 8);
            switch (randint)// 세 번째 아이템 드랍률
            {
                case 0:
                    item[2] = new Clothes("Boots", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes);
                    break;
                case 1:
                    item[2] = new Clothes("Sneakers", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.shoes);
                    break;
                case 2:
                    item[2] = new Clothes("Shirts", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.top);
                    break;
                case 3:
                    item[2] = new Clothes("Coat", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.top);
                    break;
                case 4:
                    item[2] = new Clothes("Pants", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.bottoms);
                    break;
                case 5:
                    item[2] = new Clothes("Jeans", Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), Random.Range(7, 10), 3, ItemType.bottoms);
                    break;
                default:
                    item[2] = null;
                    break;
            }
            return item;// 아이템 데이터 반환
        }
    }
}