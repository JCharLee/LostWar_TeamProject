using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour, IInteraction
{
    private float castTime = 3f;

    [SerializeField] string prompt;

    private UIManager uiManager;
    private GameObject questGiver;

    public string interactionPrompt => prompt;

    void Start()
    {
        uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        questGiver = GameObject.FindGameObjectWithTag("Player").transform.GetChild(3).gameObject;

        prompt = "[F] 상자 열기";
    }

    public bool Action(PlayerInteraction interactor)
    {
        StartCoroutine(ItemboxOpen());
        return true;
    }

    IEnumerator ItemboxOpen()
    {
        StartCoroutine(uiManager.InteractionCasting(castTime));
        yield return new WaitForSeconds(castTime);
        Debug.Log("Item box open!");
        this.gameObject.layer = 0;
    }
}
