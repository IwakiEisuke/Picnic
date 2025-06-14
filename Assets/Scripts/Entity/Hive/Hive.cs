using UnityEngine;

public class Hive : MonoBehaviour
{
    [SerializeField] Health health;

    public Health Health => health;
}