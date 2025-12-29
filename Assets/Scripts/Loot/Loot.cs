using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Loot : MonoBehaviour
{
    BoxCollider2D boxCollider;
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Initialize()
    {
        boxCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Loot: Collected by player.");
            EventBus.Instance.onLootTouched?.Invoke(this.gameObject);
            // TODO: play some fancy fx
            Destroy(gameObject);
        }
    }

}