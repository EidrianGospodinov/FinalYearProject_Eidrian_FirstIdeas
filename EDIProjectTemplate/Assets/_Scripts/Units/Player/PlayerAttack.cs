using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Camera cam;

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    [Header("VFX/SFX")]
    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    // State
    private bool attacking = false;
    private bool readyToAttack = true;
    private int attackCount;

    public bool IsAttacking => attacking;
    private int attackSequenceID = 0;

    void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (cam == null) cam = Camera.main;
    }

    public void Attack()
    {
        if(!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;
        
        // If current ID is 0, next is 1. If current ID is 1, next is 0 (toggling).
        int nextAttackID = 1 - attackSequenceID;

        // trigger attack anim based on sequence id
        EventBus<OnAttack>.Trigger(new OnAttack(AttackType.Sword, attackSequenceID));
        attackSequenceID = nextAttackID;
        

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay); // Raycast after a slight delay (swing peak)

        // SFX
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swordSwing);

        // Attack animation cycle logic
        attackCount = (attackCount == 0) ? 1 : 0;
    }

    void ResetAttack()
    {
        attacking = false;
        EventBus<OnAttack>.Trigger(new OnAttack(AttackType.NONE));//todo replace this with attackStart/end event
        readyToAttack = true;
    }

    void AttackRaycast()
    {
       
        // For simple swings, a Raycast/SphereCast is often easier than collision on a fast weapon.
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance, attackLayer))
        { 
            HitTarget(hit.point);

            if (hit.transform.TryGetComponent<Actor>(out Actor T))
            {
                T.TakeDamage(attackDamage);
            }
        } 
    }

    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
}