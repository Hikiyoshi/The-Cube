using System;
using UnityEngine;

public class WayManager : MonoBehaviour
{
    public static WayManager Instance;
    public static event Func<float, WayLauncher> UpdateWay;

    [Header("References"), Space]
    [SerializeField] private GameObject[] wayPrefabs;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform parent;

    [Header("Way Setting")]
    public float MoveSpeed { get; private set; } = 8f;
    public float MaxSpeed { get; private set; } = 14f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void SpawnWay()
    {
        int index = UnityEngine.Random.Range(0, wayPrefabs.Length);
        GameObject way = Instantiate(wayPrefabs[index], spawnPoint.position, Quaternion.identity, parent);
        WayLauncher launcher = way.GetComponent<WayLauncher>();
        launcher.SetEndPoint(endPoint).SetMoveSpeed(MoveSpeed);
    }

    public void UpdateWayControl(float moveSpeed)
    {
        MoveSpeed = moveSpeed;
        UpdateWay?.Invoke(MoveSpeed);
    }
}
