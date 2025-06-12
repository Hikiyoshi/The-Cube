using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private Rigidbody rb;

    [Header("Attributes"), Space]
    [SerializeField] private float speed;
    [SerializeField] private float knockBackForce;

    private float horizontalInput;

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }
    void FixedUpdate()
    {
        Vector3 direction = Vector3.right * horizontalInput * speed;
        rb.linearVelocity = direction;
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.CanEscape)
        {
            GameManager.Instance.Pause();
            return;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            GameManager.Instance.IncreaseScore();
        }

        if (other.tag == "Obstacle")
        {
            WayManager.Instance.UpdateWayControl(knockBackForce);

            GameManager.Instance.GameOver();
            this.enabled = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "CheckPoint")
        {
            WayManager.Instance.SpawnWay();
        }
    }
}
