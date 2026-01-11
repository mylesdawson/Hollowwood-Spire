using System.Collections.Generic;
using UnityEngine;

public class LootCanvas : MonoBehaviour
{
    [SerializeField] public Transform lootContainer;

    public void Initialize(List<Ability> lootAbilities)
    {
        for(var i = 0; i < lootAbilities.Count; i++) {
            var lootItemUI = lootContainer.GetChild(i).GetComponent<LootItemUI>();
            lootItemUI.Initialize(lootAbilities[i], this.transform);
        }
    }

    public void CloseLootUI()
    {
        this.gameObject.SetActive(false);
    }
}