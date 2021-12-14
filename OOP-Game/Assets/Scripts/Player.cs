using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : Creature
{
    [Header("Player Properties")]
    [SerializeField] private int basicAttackDamage1 = 10;
    [SerializeField] private int basicAttackDamage2 = 20;
    public int currentBasicAttackDamage
    {
        get
        {
            if (IsAnimationPlaying(attack1AnimationName))
            {
                return basicAttackDamage1;
            }
            else if (IsAnimationPlaying(attack2AnimationName))
            {
                return basicAttackDamage2;
            }
            else
            {
                return 0;
            }
        }
    }


    [Header("Animation Names")]
    [SerializeField] protected string runAnimationName;
    [SerializeField] protected string attack1AnimationName;
    [SerializeField] protected string attack2AnimationName;
    [SerializeField] protected string skillAnimationName;

    [Header("Sounds")]

    [SerializeField] protected AudioClip attack1AirSound;
    [SerializeField] protected AudioClip attack2AirSound;
    [SerializeField] public AudioClip attack1Sound;
    [SerializeField] public AudioClip attack2Sound;
    [SerializeField] protected AudioClip skillSound;


    private Animator playerAnimator;
    private float xInput;
    private float zInput;
    private Vector3 direction;
    private float angle = 90f;
    private AudioSource audioSource;

    private SkinnedMeshRenderer skinnedMeshRenderer;


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        InitAnimationNames();
        CreateHealthBar();
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
            audioSource.PlayOneShot(skillSound);
        }
    }


    protected bool IsAnimationPlaying(string animationName)
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    protected abstract void InitAnimationNames();

    public abstract string GetPlayerClass();


    private void OnTriggerEnter(Collider other)
    {
        if (other is BoxCollider)          //Sword Trigger Collider
        {
            if (other.gameObject.CompareTag("Melee_Weapon_Enemy"))
            {
                Enemy enemy = other.transform.root.gameObject.GetComponent<Enemy>() as Enemy;
                TakeDamage(enemy.currentBasicAttackDamage);
            }
        }
    }

    protected IEnumerator PlayHitAnimation()
    {
        skinnedMeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        skinnedMeshRenderer.material.color = Color.white;
    }

    protected override void PlayHitAirSound()
    {
        if (IsAnimationPlaying(attack1AnimationName))
        {
            audioSource.PlayOneShot(attack1AirSound);
        }
        else if (IsAnimationPlaying(attack2AnimationName))
        {
            audioSource.PlayOneShot(attack2AirSound);
        }
    }
    public override void PlayHitSound()
    {
        if (IsAnimationPlaying(attack1AnimationName))
        {
            audioSource.PlayOneShot(attack1Sound);
        }else if (IsAnimationPlaying(attack2AnimationName))
        {
            audioSource.PlayOneShot(attack2Sound);
        }
    }

    protected override void PlayOnHitAnimation()
    {
        StartCoroutine(PlayHitAnimation());
    }

    public override void Die()
    {
        playerAnimator.SetBool("Dead", true);
        this.enabled = false;
    }
}
