using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class Player : Creature
{
    [Header("Player Properties")]
    [SerializeField] private int basicAttackDamage1 = 10;
    [SerializeField] private int basicAttackDamage2 = 20;
    [SerializeField] private float skillCoolDown;
    [HideInInspector] public int currentBasicAttackDamage
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


    protected string runAnimationName;
    protected string attack1AnimationName;
    protected string attack2AnimationName;
    protected string skillAnimationName;

    [Header("Player Sounds", order = 1)]
    [SerializeField] protected AudioClip attack1AirSound;
    [SerializeField] protected AudioClip attack2AirSound;
    [SerializeField] protected AudioClip attack1Sound;
    [SerializeField] protected AudioClip attack2Sound;
    [SerializeField] protected AudioClip skillSound;

    private Slider coolDownSlider; 

    private Animator playerAnimator;
    private float xInput;
    private float zInput;
    private Vector3 direction;
    private float angle = 0;
    private bool canSkill = true;
    private int coolDownPercent = 0;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        coolDownSlider = GameObject.Find("Skill Icon Canvas").GetComponentInChildren<Slider>();
        CreateHealthBar();
    }
    void Start()
    {
        
        InitAnimationNames();
        LoadBaseStats();
        ViewPlayerName();
    }

    protected abstract void LoadBaseStats();
    private void ViewPlayerName()
    {
        healthBarScript.transform.root.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = MainManager.playerName;
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
        if (Input.GetMouseButtonDown(1) && canSkill)
        {
            playerAnimator.SetTrigger("Skill");
            audioSource.PlayOneShot(skillSound);
            canSkill = false;
            coolDownPercent = 100;
            StartCoroutine(SkillCoolDownRoutine());
        }
    }

    private void ActivateSkill()
    {
        canSkill = true;
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
        PlayAttackedSound();
    }

    protected abstract void PlayAttackedSound();


    public override void Die()
    {
        playerAnimator.SetBool("Dead", true);
        this.enabled = false;
        LevelManager.GameOver();
    }

    IEnumerator SkillCoolDownRoutine()
    {
        while(coolDownPercent > 0)
        {
            yield return new WaitForSeconds(skillCoolDown/100);
            coolDownPercent--;
            coolDownSlider.value = coolDownPercent;
        }
        ActivateSkill();
    }
}
