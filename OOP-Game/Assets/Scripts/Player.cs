using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{

    public float speed = 3f;
    protected Animator playerAnimator;
    public float xInput;
    public float zInput;
    public Vector3 direction;
    public float angle = 90f;
    public float attackDamage = 10f;
    [Header("Animation Names")]
    public string runAnimationName;
    public string attack1AnimationName;
    public string attack2AnimationName;
    public string skillAnimationName;


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        InitAnimationNames();
    }

    void Update()
    {
        TickMovement();
        TickRotation();
        BasicAttack();
        Skill();
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
        Debug.Log(attack1AnimationName);
        if (IsAnimationPlaying(attack1AnimationName) || IsAnimationPlaying(attack2AnimationName) || IsAnimationPlaying(skillAnimationName)) return;
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

    protected void Rotate()
    {
        if (IsAnimationPlaying(attack1AnimationName) || IsAnimationPlaying(attack2AnimationName) || IsAnimationPlaying(skillAnimationName)) return;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    protected virtual void BasicAttack()
    {
        if (IsAnimationPlaying(attack2AnimationName)) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsAnimationPlaying(attack1AnimationName))
            {
                playerAnimator.SetTrigger("Attack");
            }
            else
            {
                playerAnimator.SetTrigger("Attack2");
            }
        }
    }

    protected abstract void Skill();


    protected bool IsAnimationPlaying(string animationName)
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    protected abstract void InitAnimationNames();

}
