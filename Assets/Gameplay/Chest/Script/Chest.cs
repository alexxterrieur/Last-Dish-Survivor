using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int[] possibleUpgrades = { 1, 3, 5 }; // Nombre d'améliorations possibles
    [SerializeField] private float[] upgradeChances = { 0.6f, 0.3f, 0.1f }; // Probabilités des améliorations (1, 3, 5)
    [SerializeField] private float legendaryChance = 0.85f; // Chance de passer en légendaire si possible

    private PlayerInfos playerInfos;

    private void Start()
    {
        playerInfos = PlayerInfos.Instance;
    }

    public void OpenChest()
    {
        List<UpgradeableWeapon> upgradableWeapons = new List<UpgradeableWeapon>();
        List<UpgradeableBonus> upgradableBonuses = new List<UpgradeableBonus>();
        List<UpgradeableWeapon> possibleLegendaries = new List<UpgradeableWeapon>();

        // Récupérer les armes possédées et vérifier leur niveau
        foreach (var weapon in playerInfos.weaponLevels.Keys)
        {
            int currentLevel = playerInfos.weaponLevels[weapon];

            if (currentLevel < weapon.MaxLevel) // Encore améliorable normalement
            {
                upgradableWeapons.Add(weapon);
            }
            else if (currentLevel == weapon.MaxLevel && weapon.HasLegendary && playerInfos.bonusLevels.ContainsKey(weapon.requiredBonusForLegendary))
            {
                possibleLegendaries.Add(weapon);
            }
        }

        // Récupérer les bonus possédés et vérifier leur niveau
        foreach (var bonus in playerInfos.bonusLevels.Keys)
        {
            int currentLevel = playerInfos.bonusLevels[bonus];

            if (currentLevel < bonus.bonusLevels.Count - 1)
            {
                upgradableBonuses.Add(bonus);
            }
        }

        // Si tout est déjà maxé, forcer une amélioration légendaire si possible
        if (upgradableWeapons.Count == 0 && upgradableBonuses.Count == 0 && possibleLegendaries.Count == 0)
        {
            Debug.Log("Tout est déjà amélioré au maximum !");
            return;
        }

        // Vérifie s'il faut forcer une arme légendaire
        if (possibleLegendaries.Count > 0 && upgradableWeapons.Count == 0 && upgradableBonuses.Count == 0)
        {
            UpgradeToLegendary(possibleLegendaries);
            return;
        }

        // Déterminer combien d'améliorations donner
        int upgradeCount = DetermineUpgradeCount();
        Debug.Log(upgradeCount);

        // Appliquer les améliorations
        ApplyUpgrades(upgradeCount, upgradableWeapons, upgradableBonuses, possibleLegendaries);
    }

    private void UpgradeToLegendary(List<UpgradeableWeapon> possibleLegendaries)
    {
        UpgradeableWeapon legendaryWeapon = possibleLegendaries[Random.Range(0, possibleLegendaries.Count)];
        playerInfos.weaponLevels[legendaryWeapon]++; // Passe en légendaire
        Debug.Log($"Arme légendaire obtenue : {legendaryWeapon.weaponLevels[^1].abilityName}");
    }

    private int DetermineUpgradeCount()
    {
        float roll = Random.value;
        float cumulativeProbability = 0f;

        for (int i = 0; i < possibleUpgrades.Length; i++)
        {
            cumulativeProbability += upgradeChances[i];
            if (roll < cumulativeProbability)
            {
                return possibleUpgrades[i];
            }
        }
        return 1;
    }

    private void ApplyUpgrades(int upgradeCount, List<UpgradeableWeapon> upgradableWeapons, List<UpgradeableBonus> upgradableBonuses, List<UpgradeableWeapon> possibleLegendaries)
    {
        int upgradesGiven = 0;

        while (upgradesGiven < upgradeCount)
        {
            bool upgraded = false;

            // Tente une amélioration légendaire (85% de chance si possible)
            if (possibleLegendaries.Count > 0 && Random.value < legendaryChance)
            {
                UpgradeToLegendary(possibleLegendaries);
                return; // Si une arme devient légendaire, c'est la seule amélioration
            }

            // Tente d'améliorer une arme
            if (!upgraded && upgradableWeapons.Count > 0)
            {
                UpgradeableWeapon weapon = upgradableWeapons[Random.Range(0, upgradableWeapons.Count)];
                playerInfos.UpgradeWeapon(weapon);
                upgradesGiven++;
                upgraded = true;

                // Vérifier si l'arme atteint son niveau max
                if (playerInfos.weaponLevels[weapon] >= weapon.MaxLevel)
                {
                    upgradableWeapons.Remove(weapon);

                    // Vérifier si elle peut devenir légendaire plus tard
                    if (weapon.HasLegendary && playerInfos.bonusLevels.ContainsKey(weapon.requiredBonusForLegendary))
                    {
                        possibleLegendaries.Add(weapon);
                    }
                }
            }

            // Tente d'améliorer un bonus
            if (!upgraded && upgradableBonuses.Count > 0)
            {
                UpgradeableBonus bonus = upgradableBonuses[Random.Range(0, upgradableBonuses.Count)];
                playerInfos.AddBonus(bonus); // Ajoute le niveau suivant du bonus
                upgradesGiven++;
                upgraded = true;

                // Vérifie si le bonus atteint son niveau max
                if (playerInfos.bonusLevels[bonus] >= bonus.bonusLevels.Count - 1)
                {
                    upgradableBonuses.Remove(bonus);
                }
            }

            // Si plus rien à améliorer, on arrête
            if (upgradableWeapons.Count == 0 && upgradableBonuses.Count == 0)
            {
                return;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            OpenChest();

            Destroy(gameObject);
        }
    }
}
