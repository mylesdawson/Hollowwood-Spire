

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerCanvas: MonoBehaviour
{
    [SerializeField] Slider healthbarSlider;

    void Start()
    {
        EventBus.Instance.onPlayerLostHealth.AddListener(HealthLost);
    }


    void OnDestroy()
    {
        EventBus.Instance.onPlayerLostHealth.RemoveListener(HealthLost);
    }

    void HealthLost(float dmg, float remainingHealth)
    {
        var player = GameObject.Find("Player(Clone)");
        var health = player.GetComponent<Healthable>();
        var percent = health.health / health.maxHealth;

        LeanTween.value(healthbarSlider.gameObject, healthbarSlider.value, percent, 0.5f)
            .setOnUpdate((float val) => healthbarSlider.value = val)
            .setEase(LeanTweenType.easeInOutQuad);
    }



}