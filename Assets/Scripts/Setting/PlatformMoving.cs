using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlatformMoving : MonoBehaviour
{
    [SerializeField] private GameObject PosA;
    [SerializeField] private GameObject PosB;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float delay = 1f;
    private GameObject Target;
    private bool isMoving = true;
    private Rigidbody2D rb;
    private Vector3 direction;
    private PlayerMovement player;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

    }

    void Start()
    {
        Target = PosA;
        directioCaculate();
    }
    private void FixedUpdate()
    {
        rb.velocity =  direction * moveSpeed;
    }

    void Update()
    {
        GameObject newPlayer = GameObject.FindGameObjectWithTag("Player");
        player = newPlayer.GetComponent<PlayerMovement>();
        if (isMoving)
        {
            PlatformMove();
        }
    }

    private void PlatformMove()
    { 
        // Nếu đạt đến điểm đích, dừng và chuyển hướng
        if (Vector2.Distance(transform.position, Target.transform.position) < 1f)
        {
            isMoving = false;
            StartCoroutine(DelayToFlip());
        }
    }

    IEnumerator DelayToFlip()
    {
        yield return new WaitForSeconds(delay);
        Target = Target == PosA ? PosB : PosA;
        directioCaculate();
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.setIsPlatform(true);
            player.setRiggidbodyPlatform(rb);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.parent = null;
            player.setIsPlatform(false);
        }
    }
    private void directioCaculate()
    {
        direction = (Target.transform.position - transform.position).normalized;
    }

    private void OnDrawGizmos()
    {
        if (PosA != null && PosB != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PosA.transform.position, 0.3f);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(PosB.transform.position, 0.3f);
            Gizmos.color = Color.green;
            Gizmos.DrawLine(PosA.transform.position, PosB.transform.position);
        }
    }
}
