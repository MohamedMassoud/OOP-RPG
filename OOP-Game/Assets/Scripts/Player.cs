using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 3f;
    private Animator playerAnimator;
    public float xInput;
    public float zInput;
    public Vector3 direction;
    public float angle = 90f;
    public Vector3 cameraOffset = new Vector3(3f, 3f, -3f);
    public float attackDamage = 10f;
    

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        TickMovement();
        TickRotation();
        BasicAttack();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    protected void TickMovement()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");
        direction = new Vector3(xInput, 0f, zInput).normalized;
        if (direction.magnitude > 0.1)
        {
            playerAnimator.SetBool("isWalking", true);
        }
        else
        {
            playerAnimator.SetBool("isWalking", false);
        }
    }

    protected void TickRotation()
    {
        if (direction.magnitude > 0.1)
        {
            angle = Mathf.Atan2(xInput, zInput) * Mathf.Rad2Deg;
        }
    }

    protected void Move()
    {
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

    protected void Rotate()
    {
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    protected void BasicAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.SetTrigger("Attack");
        }
    }

    protected virtual void Skill()
    {

    }



}
