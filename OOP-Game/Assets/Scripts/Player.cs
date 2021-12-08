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
    [Header("Animation Names")]
    public string runAnimationName;
    public string attack1AnimationName;
    public string attack2AnimationName;
    public string skillAnimationName;
    private int basicAttackDamage1 = 10;
    private int basicAttackDamage2 = 20;
    public int currentBasicAttackDamage {
        get
        {
            if (IsAnimationPlaying(attack1AnimationName))
            {
                return basicAttackDamage1;
            }else if (IsAnimationPlaying(attack2AnimationName))
            {
                return basicAttackDamage2;
            }
            else
            {
                return 0;
            }
        }
    }


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        InitAnimationNames();
    }

    void Update()
    {
        TickMovement();
        TickRotation();
        StartBasicAttackAnimation();
        StartSkillAnimation();
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
        if (IsAnimationPlaying(attack1AnimationName) || IsAnimationPlaying(attack2AnimationName) || IsAnimationPlaying(skillAnimationName)) return;
        transform.Translate(direction * speed * Time.fixedDeltaTime, Space.World);
    }

    protected void Rotate()
    {
        if (IsAnimationPlaying(attack1AnimationName) || IsAnimationPlaying(attack2AnimationName) || IsAnimationPlaying(skillAnimationName)) return;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    protected virtual void StartBasicAttackAnimation()
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

    protected virtual void StartSkillAnimation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAnimator.SetTrigger("Skill");
        }
    }


    protected bool IsAnimationPlaying(string animationName)
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    protected abstract void InitAnimationNames();



    public abstract string GetPlayerClass();
}
