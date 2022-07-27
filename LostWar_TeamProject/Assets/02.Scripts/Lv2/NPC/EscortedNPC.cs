using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortedNPC : MonoBehaviour
{
    public enum State
    {
        IDLE, WALK, RUN, SCARED, DIE
    }
    public State state = State.IDLE;

    [SerializeField] private Transform tr;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider col;

    public bool isDie = false;
    public bool isReach = false;

    private void Awake()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }

    IEnumerator CheckState()
    {
        yield return new WaitForSeconds(2.0f);

        while (!isDie)
        {
            if (state == State.DIE) yield break;
        }
    }

    IEnumerator Action()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.3f);
            switch (state)
            {
                case State.IDLE:
                    anim.SetBool("IsMove", false);
                    break;
                case State.WALK:
                    anim.SetBool("IsMove", true);
                    break;
                case State.RUN:
                    break;
                case State.SCARED:
                    break;
                case State.DIE:
                    break;
            }
        }
    }
}