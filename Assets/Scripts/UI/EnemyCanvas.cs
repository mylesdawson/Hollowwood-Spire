using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCanvas : MonoBehaviour
{
    [SerializeField] Slider healthSlider;
    [SerializeField] TMP_Text nameText;

    public void Init(string enemyName)
    {
        EventBus.Instance.onEnemyLostHealth.RemoveAllListeners();
        EventBus.Instance.onEnemyLostHealth.AddListener(UpdateHealth);
        healthSlider.value = 1f;
        nameText.text = enemyName;
    }

    public void UpdateHealth(float healthNormalized)
    {
        healthSlider.value = healthNormalized;
    }
}