
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootItemUI: MonoBehaviour
{
    Ability ability;

    [SerializeField] TMP_Text lootTitleText;
    [SerializeField] TMP_Text lootDescriptionText;
    [SerializeField] Button lootButton;

    public void Initialize(Ability ability)
    {
        this.ability = ability;

        lootTitleText.text = ability.AbilityName;
        lootDescriptionText.text = ability.AbilityDescription;
    }

    void Start()
    {
        lootButton.onClick.AddListener(OnLootButtonClicked);
    }

    public void OnLootButtonClicked()
    {
        // Handle the logic when the loot button is clicked
        Debug.Log($"Looted ability: {ability.AbilityName}");
        // You can add code here to add the ability to the player's inventory or character
        Destroy(this.gameObject); // Remove the loot item UI after looting
    }
}