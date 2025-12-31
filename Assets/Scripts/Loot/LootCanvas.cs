using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LootCanvas : MonoBehaviour
{
    [SerializeField] Transform lootContainer;
    List<Ability> lootAbilities;

    public void Initialize(List<Ability> lootAbilities)
    {
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

        if(this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }

        EventSystem.current.SetSelectedGameObject(lootContainer.GetChild(0).gameObject);
    }

    public void CloseLootUI()
    {
        this.gameObject.SetActive(false);
    }
}