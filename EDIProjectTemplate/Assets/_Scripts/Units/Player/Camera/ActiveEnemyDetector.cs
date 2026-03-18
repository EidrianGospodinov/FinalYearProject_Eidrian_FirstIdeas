using _Scripts.Units.Enemy;
using UnityEngine;

public class ActiveEnemyDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float noticeZone = 10f;
    [Tooltip("Angle in Degrees - How wide your 'vision cone' is")] 
    [SerializeField] private float maxNoticeAngle = 60f;

    [Header("UI Indicator (Optional)")]
    [SerializeField] private Transform lockOnCanvas;
    [SerializeField] private float crossHairScale = 0.1f;

    // Other scripts can read this to know who to attack, but they can't change it.
    public Transform CurrentActiveEnemy { get; private set; } 

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
        if (lockOnCanvas) lockOnCanvas.gameObject.SetActive(false);
    }

    void Update()
    {
        // 1. Find the best target
        UpdateActiveEnemy();
        
        // 2. Move the UI over their head
        UpdateUI();
    }

    private void UpdateActiveEnemy()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform bestTarget = null;
        
        if (nearbyTargets.Length > 0)
        {
            for (int i = 0; i < nearbyTargets.Length; i++)
            {
                Vector3 dir = nearbyTargets[i].transform.position - cam.position;
                dir.y = 0;
                
                // Check if the enemy is within our viewing angle
                float angle = Vector3.Angle(cam.forward, dir);

                if (angle < closestAngle)
                {
                    bestTarget = nearbyTargets[i].transform;
                    closestAngle = angle;
                }
            }
        }

        // Update our public property so other scripts know who is active
        CurrentActiveEnemy = bestTarget;
    }

    private void UpdateUI()
    {
        if (!lockOnCanvas) return;

        // If we have a target, show the UI over them
        if (CurrentActiveEnemy != null)
        {
            lockOnCanvas.gameObject.SetActive(true);
            
            // Figure out the height offset for the UI
            float yOffset = 1.6f;
            AiAgent agent = CurrentActiveEnemy.GetComponent<AiAgent>();
            if (agent != null)
            {
                yOffset = agent.agentConfig.Height * CurrentActiveEnemy.localScale.y * 0.75f;
            }

            Vector3 targetPos = CurrentActiveEnemy.position + new Vector3(0, yOffset, 0);
            
            // Move and scale the crosshair
            lockOnCanvas.position = targetPos;
            lockOnCanvas.localScale = Vector3.one * ((cam.position - targetPos).magnitude * crossHairScale);
        }
        else
        {
            // No target, hide the UI
            lockOnCanvas.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (CurrentActiveEnemy != null)
        {
            Gizmos.color = Color.cyan;
        }
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}