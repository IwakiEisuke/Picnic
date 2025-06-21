using System.Collections.Generic;
using UnityEngine;

public class EntityManagerInitializer : MonoBehaviour
{
    [SerializeField] EntityManager entityManager;
    [SerializeField] List<EntityBase> allies;
    [SerializeField] List<EntityBase> enemies;
    [SerializeField] List<EntityBase> objects;

    private void Awake()
    {
        entityManager.Init(allies, enemies, objects);
    }
}