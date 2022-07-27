using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] Transform tr;
    [SerializeField] Animator ani;
    [SerializeField] Rigidbody rb;

    private float h, v;
    private float moveSpeed = 5f;
    private float jumpForce = 5f;

    private readonly int hashMoveV = Animator.StringToHash("SpeedY");
    private readonly int hashMoveH = Animator.StringToHash("SpeedX");

    private bool isJump = false;

    void Start()
    {
        tr = GetComponent<Transform>();
        ani = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnCollisionEnter(Collision col)
    {
        isJump = false;
    }

    private void OnCollisionExit(Collision col)
    {
        isJump = true;
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        Move(moveDir);
        Jump();
        Run();
    }

    private void Run()
    {
        UIManager uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        if (uiManager.isAction == true) return;
        QuestManager questManager = FindObjectOfType<QuestManager>();
        if (questManager.IsStarting == true) return;
        if (SceneLoader.isLoading == true) return;

        if (Input.GetKey(KeyCode.LeftShift))
            moveSpeed = 10f;
        else
            moveSpeed = 5f;
    }

    private void Jump()
    {
        UIManager uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        if (uiManager.isAction == true) return;
        QuestManager questManager = FindObjectOfType<QuestManager>();
        if (questManager.IsStarting == true) return;
        if (SceneLoader.isLoading == true) return;

        if (Input.GetKeyDown(KeyCode.Space) && isJump == false)
        {
            rb.velocity = Vector3.up * jumpForce;
            ani.SetTrigger("Jump");
        }
    }

    private void Move(Vector3 moveDir)
    {
        UIManager uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        if (uiManager.isAction == true) return;
        QuestManager questManager = FindObjectOfType<QuestManager>();
        if (questManager.IsStarting == true) return;
        if (SceneLoader.isLoading == true) return;

        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime);

        if (ani)
        {
            ani.SetFloat(hashMoveV, v);
            ani.SetFloat(hashMoveH, h);
            if (rb)
            {
                Vector3 speed = rb.velocity;
                speed.z = v;
                speed.x = h;
                rb.velocity = speed;
            }
        }
    }
}