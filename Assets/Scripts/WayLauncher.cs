using UnityEngine;

public class WayLauncher : MonoBehaviour
{
    [Header("References"), Space]
    [SerializeField] private Transform endPoint;

    [Header("Attributes"), Space]
    [SerializeField] private float moveSpeed = 8f;

    public WayLauncher SetEndPoint(Transform endPoint)
    {
        this.endPoint = endPoint;
        return this;
    }
    public WayLauncher SetMoveSpeed(float newSpeed)
    {
        this.moveSpeed = newSpeed;
        return this;
    }

    void Start()
    {
        WayManager.Instance.UpdateWayControl(WayManager.Instance.MoveSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        WaySectionMove();
    }

    private void WaySectionMove()
    {
        transform.position += new Vector3(0, 0, 1) * -moveSpeed * Time.deltaTime;
    }

    private void DestroyWaySection()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Destroy")
        {
            DestroyWaySection();
        }
    }

    void OnEnable()
    {
        WayManager.UpdateWay -= SetMoveSpeed;
        WayManager.UpdateWay += SetMoveSpeed;
    }
    void OnDisable()
    {
        WayManager.UpdateWay -= SetMoveSpeed;
    }
}
