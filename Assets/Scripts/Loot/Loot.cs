using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Loot : MonoBehaviour
{
    BoxCollider2D boxCollider;
    List<Ability> loot;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Initialize(List<Ability> lootAbilities)
    {
        loot = lootAbilities;
        boxCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Loot: Collected by player.");
            FindFirstObjectByType<LootUI>().Initialize(loot);
            // TODO: play some fancy fx
            Destroy(gameObject);
        }
    }

}