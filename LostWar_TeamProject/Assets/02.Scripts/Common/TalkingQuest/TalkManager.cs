using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;
    Dictionary<int, string> nameData;
    public string[] nameArr;
    public Sprite[] portraitArr;

    private UIManager uiManager;

    public Dictionary<int, string[]> TalkData => talkData;

    public static TalkManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);

        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        nameData = new Dictionary<int, string>();

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        GenerateData();
    }

    void GenerateData()
    {
        // 일반 대화 데이터
        talkData.Add(2000, new string[] { "1:도와주신 은혜는 평생 잊지 않을게요.:1" });
        talkData.Add(3000, new string[] { "2:이 곳을 책임지고 지키는 것이 저의 의무입니다.:2" });

        // 퀘스트 대화 데이터
        talkData.Add(0 + 0, new string[] { "0:으으.. 얼마나 기절해 있던거지?:0",
                                            "0:갑작스러운 폭격이라니 이게 대체...:0",
                                            "0:우리 가족들은 괜찮은건가!? 찾으러 나가봐야겠군.:0",
                                            "0:밖은 위험해 보이니 일단 장비가 좀 필요하겠군. 안방에 비밀방을 만들어서 과거에 사용했던 장비들이 있었지.:0",
                                            "0:일단 그것들을 먼저 찾아볼까.:0" });
        talkData.Add(11 + 0, new string[] { "0:좋아. 많이 낡긴 했지만 사용하는데는 문제 없겠군.:0",
                                            "0:일단 밖으로 나가자.:0" });
        talkData.Add(21 + 0, new string[] { "0:이 안까지 적들이 들어와있군.:0",
                                            "0:하나 하나 처리하면서 나가야겠어.:0" });
        talkData.Add(31 + 0, new string[] { "0:이제 남은 적들은 없는것 같군. 얼른 빠져 나가자.:0" });

        talkData.Add(41 + 0, new string[] { "0:사람이 공격받고 있잖아!? 구해줘야겠어.:0" });
        talkData.Add(50 + 2000, new string[] { "1:도..도와주세요!!:1" });
        talkData.Add(51 + 2000, new string[] { "1:도와주셔서 감사해요. 정말 큰일날 뻔했어요.:1",
                                               "0:어쩌다가 이런 곳에 계시는겁니까?:0",
                                               "1:근방에 피난소가 있다고 해서 그 쪽으로 향하는 길이었어요.:1",
                                               "1:그러다가 갑작스럽게 공격을 받게 됐지 뭐에요.:1",
                                               "1:저기 혹시.. 또 공격을 받을까봐 무서워서 그러는데 피난소까지 저를 좀 데려다 주실 수 있을까요?:1",
                                               "0:저는 가족들을 찾아야합니다.:0",
                                               "1:피난소에 가면 여러 사람들이 많이 모여있어요.:1",
                                               "1:가족들에 대한 정보도 알 수 있을거에요.:1",
                                               "0:흠, 알겠습니다. 그럼 피난소까지 같이 가시죠.:0" });
        talkData.Add(61 + 2000, new string[] { "1:같이 와주셔서 너무 감사해요.:1" });
        talkData.Add(62 + 2000, new string[] { "1:그러고보니 가족을 찾는다고 하셨죠?:1",
                                               "1:저도 잘은 모르겠지만, 이 캠프를 관리중이신 단장님이라면 알고 계실지도 몰라요.:1",
                                               "1:마침 저 쪽에 계시니 한 번 직접 물어보시는게 좋을거 같아요.:1" });
        talkData.Add(70 + 2000, new string[] { "1:어서 단장님을 만나봐요.:1",
                                               "1:단장님은 캠프 중앙 모닥불에 계셔요.:1" });
        talkData.Add(70 + 3000, new string[] { "2:제가 이곳의 단장인데, 무슨 일이십니까?:2",
                                               "0:가족을 찾고 있습니다. 알고 계시는 정보가 있습니까?:0",
                                               "2:여럿 민간인들이 근방 연구소에 포로로 잡혀가는 것을 본 적이 있습니다.:2",
                                               "0:연구소가 어디 있습니까?:0",
                                               "2:위치를 알려드리는건 어렵지 않지만 그 전에 한 가지만 일을 좀 도와주시겠습니까?:2",
                                               "0:무슨 일입니까?:0",
                                               "2:엘리사에게 들은 바로는 전투 실력이 굉장히 뛰어나다고 들었습니다.:2",
                                               "2:주변을 계속 서성이는 적들이 있는데 그들을 좀 처리해 주실 수 있습니까?:2",
                                               "2:그들만 처리해 주신다면 연구소 위치를 알려드리겠습니다.:2" });
        talkData.Add(80 + 3000, new string[] { "2:적들은 캠프 근방에 돌아다니고 있습니다.:2",
                                               "2:처리해주시면 연구소 위치를 알려드리겠습니다.:2" });
        talkData.Add(81 + 3000, new string[] { "2:고맙습니다. 들은대로 실력이 정말 출중하시군요.:2",
                                               "2:연구소 위치는 여기 캠프를 기준으로 북서쪽으로 향하시면 나올겁니다.:2",
                                               "2:연구소에 침입하면 많은 위험이 도사리고 있을겁니다.:2",
                                               "2:부디 가족들을 찾고 무사히 돌아오길 바라겠습니다.:2" });

        talkData.Add(91 + 0, new string[] { "0:이 곳이 연구소로군.:0",
                                            "0:음? 왠 수첩이 떨어져 있는데?:0",
                                            "0:'연구소 시설 구조' 라.. 이건 좀 도움이 되겠군.:0",
                                            "0:'연구소장실에 들어가기 위해서는 2개의 열쇠가 필요하다.':0",
                                            "0:'첫 번째 열쇠는 좌측 방에, 두 번째 열쇠는 우측 방에,':0",
                                            "0:'첫 번째 열쇠를 먼저 얻어야 그 열쇠로 우측 방 문을 열 수 있다.':0",
                                            "0:'두 개의 열쇠를 모두 모으면 중앙의 연구소장실에 들어갈 수 있다.':0",
                                            "0:일단 첫 번째 열쇠를 먼저 얻어야겠군.:0" });
        talkData.Add(101 + 0, new string[] { "0:열쇠는 저 문 건너편에 있는거 같은데 지금은 지나가기 힘들어 보이는군.:0",
                                             "0:돌아서 가는 수 밖에 없나.:0" });
        talkData.Add(111 + 0, new string[] { "0:이걸로 두 번째 열쇠까지 얻었군.:0",
                                             "0:(쿠구구구궁):0",
                                             "0:방금 밖에 큰 소리가 들린거 같은데 무슨 일이지?:0" });
        talkData.Add(121 + 0, new string[] { "0:길은 열렸는데 적들이 잔뜩이군. 해치우자.:0" });
        talkData.Add(131 + 0, new string[] { "0:좋아. 이제 연구소장실로 가보자.:0" });
        talkData.Add(141 + 0, new string[] { "3:연구소에 침입한게 너구나? 네 놈은 뭐냐?:3",
                                             "0:많은 민간인들이 포로로 이 곳에 잡혀왔다는 이야기를 들었다.:0",
                                             "0:그 중에는 내 가족들이 있을 수도 있어. 그들은 어딨지?:0",
                                             "3:아~ 포로들? 포로들이라면 이미 다른 곳으로 옮겨졌지~ 큭큭..:3",
                                             "3:꽤나 좋은 실험체들이었는데, 안타깝게 됐어. 큭큭..:3",
                                             "0:뭐라고?:0",
                                             "3:나는 특수 군인들보다 더 강력한 개체를 만들어내기 위해 여러 가지 실험중이거든~ 큭큭.:3",
                                             "3:일반인들은 몸이 못 버텨내는지.. 약물을 투여해도 그냥 객사해버리더라고~:3",
                                             "0:사람을 대상으로 실험이라니, 미쳤군.:0",
                                             "3:내 실험 정신이 위대한거라고 말해줄래? 큭큭.. 근데 너.. 특수 군인이구나?:3",
                                             "3:그래.. 특수 군인이라면 내 실험이 성공할지도 모르겠어. 큭큭.. 너.. 내 실험체가 되라.:3",
                                             "0:허튼 소리 하지 말고 포로들이 어디로 갔는지 말해!:0",
                                             "3:내 실험이 성공하면~ 아주 강력한 힘을 손에 넣을 수 있는거야! 하하하하하.:3",
                                             "0:쳇, 말이 안통하는군.:0" });
        talkData.Add(151 + 0, new string[] { "0:하.. 이 곳에는 포로들이 없는건가..:0",
                                             "0:일단 피난소로 돌아가야겠군. 정보를 더 모아야겠어.:0" });

        // 이름 데이터
        nameData.Add(0 + 0, nameArr[0]);
        nameData.Add(0 + 3, nameArr[3]);
        nameData.Add(2000 + 0, nameArr[0]);
        nameData.Add(2000 + 1, nameArr[1]);
        nameData.Add(3000 + 0, nameArr[0]);
        nameData.Add(3000 + 2, nameArr[2]);

        // 초상화 데이터
        portraitData.Add(0 + 0, portraitArr[0]);
        portraitData.Add(0 + 3, portraitArr[3]);
        portraitData.Add(2000 + 0, portraitArr[0]);
        portraitData.Add(2000 + 1, portraitArr[1]);
        portraitData.Add(3000 + 0, portraitArr[0]);
        portraitData.Add(3000 + 2, portraitArr[2]);
    }

    public string GetName(int id, int nameIdx)
    {
        return nameData[id + nameIdx];
    }

    public string GetTalk(int id, int idx)
    {
        if (!talkData.ContainsKey(id))
        {
            if (!talkData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id % 100, idx);
            }
            else
                return GetTalk(id - id % 10, idx);
        }

        if (idx == talkData[id].Length)
            return null;
        else
            return talkData[id][idx];
    }

    public Sprite GetPortrait(int id, int portraitIdx)
    {
        return portraitData[id + portraitIdx];
    }
}
