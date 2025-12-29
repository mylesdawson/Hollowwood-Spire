using System.Collections.Generic;
using UnityEngine;

public class LootUI : MonoBehaviour
{
    Transform lootContainer;
    List<Ability> lootAbilities;

    void Awake()
    {
        lootContainer = transform.Find("LootContainer");
        if(!lootContainer)
        {
            lootContainer = this.transform.GetChild(0);
        }
    }

    public void Initialize(List<Ability> lootAbilities)
    {

        if(this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }
        this.lootAbilities = lootAbilities;

        // Clear existing loot UI
        foreach(Transform child in lootContainer)
        {
            Destroy(child.gameObject);
        }

        // Create UI elements for each loot ability
        foreach(var ability in lootAbilities)
        {
            var lootItemPrefab = (GameObject)Resources.Load("LootItemUI");
            var lootItemObj = Instantiate(lootItemPrefab, lootContainer);
            var lootItemUI = lootItemObj.GetComponent<LootItemUI>();
            lootItemUI.Initialize(ability, this.transform);
        }
    }

    public void CloseLootUI()
    {
        this.gameObject.SetActive(false);
    }
}