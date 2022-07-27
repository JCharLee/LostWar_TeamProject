using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretWall : MonoBehaviour, IInteraction
{
    [SerializeField] Transform secretWall;
    [SerializeField] Transform openPoint;
    [SerializeField] Image interactionImg;
    [SerializeField] Text _text;
    [SerializeField] Image castingBar_back;
    [SerializeField] Image castingBar;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float moveTime;
    [SerializeField] private float current;
    private float castingTime = 5.0f;

    [SerializeField] string prompt;

    private UIManager uiManager;

    public Coroutine moveRoutine;

    void Start()
    {
        secretWall = GetComponent<Transform>();
        openPoint = GameObject.Find("OpenPoint").GetComponent<Transform>();

        uiManager = GameObject.Find("UI").GetComponent<UIManager>();

        moveTime = (openPoint.position - secretWall.position).magnitude / speed;
        prompt = "[F] 조사하기";
    }

    public string interactionPrompt => prompt;

    public bool Action(PlayerInteraction interactor)
    {
        moveRoutine = StartCoroutine(WallOpen());
        return true;
    }

    IEnumerator WallOpen()
    {
        uiManager.castRoutine = StartCoroutine(uiManager.InteractionCasting(castingTime));
        if (uiManager.castRoutine == null) StopCoroutine(moveRoutine);
        yield return new WaitForSeconds(castingTime);
        this.gameObject.layer = 0;

        current = 0f;
        while (current <= moveTime)
        {
            current += Time.deltaTime;
            secretWall.position = Vector3.MoveTowards(secretWall.position, openPoint.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}