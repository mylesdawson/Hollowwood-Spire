


using UnityEngine;

[RequireComponent(typeof(Attackable))]
[RequireComponent(typeof(Healthable))]
public class Plant : MonoBehaviour
{
    Attackable attackable;
    Healthable healthable;
    void Awake()
    {
        attackable = GetComponent<Attackable>();
        healthable = GetComponent<Healthable>();
    }

    void OnEnable()
    {
        attackable.onAttacked.AddListener(OnAttacked);
        healthable.onDie.AddListener(Die);
    }

    void OnDisable()
    {
        attackable.onAttacked.RemoveListener(OnAttacked);
        healthable.onDie.RemoveListener(Die);
    }

    public void OnAttacked(int damage, Vector2 direction, float knockbackStrength, float hitStop = 0f)
    {
        healthable.LoseHealth(damage);
    }

    public void Die()
    {
        Instantiate(Resources.Load("ParticleEffects/GrassDieVFX"), transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}