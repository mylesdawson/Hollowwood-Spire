

using System.Collections.Generic;
using UnityEngine;

public enum LootRarity
{
    none,
    standard,
    rare,
    legendary
}

public class LootManager : MonoBehaviour
{
    public List<Ability> abilities;
    public List<AbilityStatMutation> abilityStatMutations;
    public List<Ability> generatedLoot;

    void Awake()
    {
        var attackPrefab = (GameObject)Resources.Load("Attack");
        abilities = new()
        {
            // Overrides
            new RegularDash(),
            new OmniDash(),
            new RegularAttack(attackPrefab.transform),

            // Mutations
            new PlantSpawnerMove(),
            new ResetDashMutation(),
            new InvincibleDashMutation(),
            new KnockupDownDashMutation(),
        };
    }

    public void SpawnLoot(GameObject enemy)
    {
        // Spawn loot at enemy position
        var loot = Resources.Load<GameObject>("Loot");
        var obj = Instantiate(loot, enemy.transform.position, Quaternion.identity);
        // move loot to center of screen
        LeanTween.move(obj, Vector3.zero, .5f).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
        {
            Debug.Log("LootManager: Loot moved to center of screen.");
            this.generatedLoot = GenerateLoot();
            obj.GetComponent<Loot>().Initialize();
        });
    }

    List<Ability> GenerateLoot()
    {
        // For now only generate mutations
        var mutationAbilities = abilities.FindAll(a => a.AbilitySubtype == AbilitySubtype.Mutation);
        
        var player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        var attackAbilities = player.attackManager.GetAbilities();
        var dashAbilities = player.dashManager.GetAbilities();
        var moveAbilities = player.moveManager.GetAbilities();
        var jumpAbilities = player.jumpManager.GetAbilities();
        var playerAbilities = new List<Ability>();
        playerAbilities.AddRange(attackAbilities);
        playerAbilities.AddRange(dashAbilities);
        playerAbilities.AddRange(moveAbilities);
        playerAbilities.AddRange(jumpAbilities);

        // All abilities player does not have yet
        var filteredAbilities = mutationAbilities.FindAll(a => !playerAbilities.Exists(pa => pa.AbilityName == a.AbilityName));

        // Pick 3 random abilities from filteredAbilities
        var filteredArr = filteredAbilities.ToArray();
        Reshuffle(filteredArr);

        // Pick first 3 abilities
        Ability ability1 = filteredArr[0];
        Ability ability2 = filteredArr[1];
        Ability ability3 = filteredArr[2];
        Debug.Log($"LootManager: Generated loot abilities: {ability1.AbilityName}, {ability2.AbilityName}, {ability3.AbilityName}");
        return new List<Ability>{ ability1, ability2, ability3 };
    }

    void Reshuffle<T>(T[] arr)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < arr.Length; t++ )
        {
            var tmp = arr[t];
            int r = Random.Range(t, arr.Length);
            arr[t] = arr[r];
            arr[r] = tmp;
        }
    }
}