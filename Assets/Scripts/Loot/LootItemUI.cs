
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootItemUI: MonoBehaviour
{
    Ability ability;

    [SerializeField] TMP_Text lootTitleText;
    [SerializeField] TMP_Text lootDescriptionText;
    [SerializeField] Button lootButton;
    Transform lootUI;

    public void Initialize(Ability ability, Transform lootUI)
    {
        this.ability = ability;
        this.lootUI = lootUI;
        lootTitleText.text = ability.AbilityName;
        lootDescriptionText.text = ability.AbilityDescription;
    }

    void Start()
    {
        lootButton.onClick.AddListener(OnLootButtonClicked);
    }

    public void OnLootButtonClicked()
    {
        Debug.Log($"Looted ability: {ability.AbilityName}");
        EventBus.Instance.onAbilityLooted?.Invoke(ability);
    }
}