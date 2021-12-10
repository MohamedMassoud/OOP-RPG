using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour, Attackable
{
    [Header("Player Properties")]
    [SerializeField] private float speed = 3f;
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
    [SerializeField] private int health = 100;

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

    [Header("MISC")]
    private AudioSource audioSource;

    private Animator playerAnimator;
    private float xInput;
    private float zInput;
    private Vector3 direction;
    private float angle = 90f;


    private SkinnedMeshRenderer skinnedMeshRenderer;
    


    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        audioSource = GetComponent<AudioSource>();
        InitAnimationNames();
    }

    void Update()
    {
        TickMovement();
        TickRotation();
        StartBasicAttackAnimationAndSound();
        StartSkillAnimationAndSound();
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

    protected virtual void StartBasicAttackAnimationAndSound()
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

    protected virtual void StartSkillAnimationAndSound()
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(PlayHitAnimation());
        if (health <= 0)
        {
            Die();

        }

    }
    public void Die()
    {
        Debug.Log("Player Has Died");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other is BoxCollider)          //Sword Trigger Collider
        {
            if (other.gameObject.CompareTag("Melee_Weapon_Enemy"))
            {
                Debug.Log("Hit By Enemy" + " " + other.gameObject.name);
                Enemy enemy = other.transform.root.gameObject.GetComponent<Enemy>() as Enemy;
                TakeDamage(enemy.currentBasicAttackDamage);
            }
        }
    }

    protected IEnumerator PlayHitAnimation()
    {
        Debug.Log("HEREREERE");
        skinnedMeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        skinnedMeshRenderer.material.color = Color.white;
    }

    public void PlayHitAirSound()
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
    public void PlayHitSound()
    {
        if (IsAnimationPlaying(attack1AnimationName))
        {
            audioSource.PlayOneShot(attack1Sound);
        }else if (IsAnimationPlaying(attack2AnimationName))
        {
            audioSource.PlayOneShot(attack2Sound);
        }
    }
}
