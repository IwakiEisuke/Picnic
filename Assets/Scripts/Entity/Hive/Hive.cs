using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] Health health;

    public Health Health => health;

    private void Start()
    {
        health.Init(transform);
    }
}