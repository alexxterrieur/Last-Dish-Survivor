using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int[] possibleUpgrades = { 1, 3, 5 }; // Nombre d'am�liorations possibles
    [SerializeField] private float[] upgradeChances = { 0.6f, 0.3f, 0.1f }; // Probabilit�s des am�liorations (1, 3, 5)
    [SerializeField] private float legendaryChance = 0.85f; // Chance de passer en l�gendaire si possible

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

        // R�cup�rer les armes poss�d�es et v�rifier leur niveau
        foreach (var weapon in playerInfos.weaponLevels.Keys)
        {
            int currentLevel = playerInfos.weaponLevels[weapon];

            if (currentLevel < weapon.MaxLevel) // Encore am�liorable normalement
            {
                upgradableWeapons.Add(weapon);
            }
            else if (currentLevel == weapon.MaxLevel && weapon.HasLegendary && playerInfos.bonusLevels.ContainsKey(weapon.requiredBonusForLegendary))
            {
                possibleLegendaries.Add(weapon);
            }
        }

        // R�cup�rer les bonus poss�d�s et v�rifier leur niveau
        foreach (var bonus in playerInfos.bonusLevels.Keys)
        {
            int currentLevel = playerInfos.bonusLevels[bonus];

            if (currentLevel < bonus.bonusLevels.Count - 1)
            {
                upgradableBonuses.Add(bonus);
            }
        }

        // Si tout est d�j� max�, forcer une am�lioration l�gendaire si possible
        if (upgradableWeapons.Count == 0 && upgradableBonuses.Count == 0 && possibleLegendaries.Count == 0)
        {
            Debug.Log("Tout est d�j� am�lior� au maximum !");
            return;
        }

        // V�rifie s'il faut forcer une arme l�gendaire
        if (possibleLegendaries.Count > 0 && upgradableWeapons.Count == 0 && upgradableBonuses.Count == 0)
        {
            UpgradeToLegendary(possibleLegendaries);
            return;
        }

        // D�terminer combien d'am�liorations donner
        int upgradeCount = DetermineUpgradeCount();
        Debug.Log(upgradeCount);

        // Appliquer les am�liorations
        ApplyUpgrades(upgradeCount, upgradableWeapons, upgradableBonuses, possibleLegendaries);
    }

    private void UpgradeToLegendary(List<UpgradeableWeapon> possibleLegendaries)
    {
        UpgradeableWeapon legendaryWeapon = possibleLegendaries[Random.Range(0, possibleLegendaries.Count)];
        playerInfos.weaponLevels[legendaryWeapon]++; // Passe en l�gendaire
        Debug.Log($"Arme l�gendaire obtenue : {legendaryWeapon.weaponLevels[^1].abilityName}");
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

            // Tente une am�lioration l�gendaire (85% de chance si possible)
            if (possibleLegendaries.Count > 0 && Random.value < legendaryChance)
            {
                UpgradeToLegendary(possibleLegendaries);
                return; // Si une arme devient l�gendaire, c'est la seule am�lioration
            }

            // Tente d'am�liorer une arme
            if (!upgraded && upgradableWeapons.Count > 0)
            {
                UpgradeableWeapon weapon = upgradableWeapons[Random.Range(0, upgradableWeapons.Count)];
                playerInfos.UpgradeWeapon(weapon);
                upgradesGiven++;
                upgraded = true;

                // V�rifier si l'arme atteint son niveau max
                if (playerInfos.weaponLevels[weapon] >= weapon.MaxLevel)
                {
                    upgradableWeapons.Remove(weapon);

                    // V�rifier si elle peut devenir l�gendaire plus tard
                    if (weapon.HasLegendary && playerInfos.bonusLevels.ContainsKey(weapon.requiredBonusForLegendary))
                    {
                        possibleLegendaries.Add(weapon);
                    }
                }
            }

            // Tente d'am�liorer un bonus
            if (!upgraded && upgradableBonuses.Count > 0)
            {
                UpgradeableBonus bonus = upgradableBonuses[Random.Range(0, upgradableBonuses.Count)];
                playerInfos.AddBonus(bonus); // Ajoute le niveau suivant du bonus
                upgradesGiven++;
                upgraded = true;

                // V�rifie si le bonus atteint son niveau max
                if (playerInfos.bonusLevels[bonus] >= bonus.bonusLevels.Count - 1)
                {
                    upgradableBonuses.Remove(bonus);
                }
            }

            // Si plus rien � am�liorer, on arr�te
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
