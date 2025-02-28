using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private float legendaryChance = 0.85f; // Chance d'am�lioration l�gendaire
    private PlayerInfos playerInfos;
    private WeaponsBonusUI weaponsBonusUI;

    private void Start()
    {
        playerInfos = PlayerInfos.Instance;
        weaponsBonusUI = WeaponsBonusUI.Instance;
    }

    public void OpenChest()
    {
        List<UpgradeableWeapon> upgradableWeapons = new List<UpgradeableWeapon>();
        List<UpgradeableBonus> upgradableBonuses = new List<UpgradeableBonus>();
        List<UpgradeableWeapon> possibleLegendaries = new List<UpgradeableWeapon>();

        // R�cup�rer toutes les armes et v�rifier leur �tat
        foreach (var weapon in playerInfos.weaponLevels.Keys)
        {
            int currentLevel = playerInfos.weaponLevels[weapon];

            if (currentLevel < weapon.MaxLevel) // Peut �tre am�lior�e normalement
            {
                upgradableWeapons.Add(weapon);
            }
            else if (currentLevel == weapon.MaxLevel && weapon.HasLegendary && playerInfos.bonusLevels.ContainsKey(weapon.requiredBonusForLegendary))
            {
                possibleLegendaries.Add(weapon);
            }
        }

        // R�cup�rer tous les bonus et v�rifier leur �tat
        foreach (var bonus in playerInfos.bonusLevels.Keys)
        {
            int currentLevel = playerInfos.bonusLevels[bonus];

            if (currentLevel < bonus.bonusLevels.Count - 1)
            {
                upgradableBonuses.Add(bonus);
            }
        }

        // V�rifier si tout est max�
        if (upgradableWeapons.Count == 0 && upgradableBonuses.Count == 0 && possibleLegendaries.Count == 0)
        {
            Debug.Log("Tout est d�j� am�lior� au maximum !");
            return;
        }

        // V�rifie si on donne une am�lioration l�gendaire en priorit�
        if (possibleLegendaries.Count > 0 && (upgradableWeapons.Count == 0 && upgradableBonuses.Count == 0 || Random.value < legendaryChance))
        {
            UpgradeToLegendary(possibleLegendaries);
            Debug.Log("Coffre l�gendaire !");
            UpdateUI(); // Met � jour l'UI apr�s am�lioration
            return; // Une seule am�lioration possible -> fin
        }

        // D�terminer combien d�am�liorations donner en fonction du nombre total d�am�liorations possibles
        List<object> allUpgradable = new List<object>();
        allUpgradable.AddRange(upgradableWeapons);
        allUpgradable.AddRange(upgradableBonuses);
        ShuffleList(allUpgradable);

        int maxPossibleUpgrades = allUpgradable.Count;
        int upgradeCount = DetermineUpgradeCount(maxPossibleUpgrades);

        Debug.Log($"Coffre classique - Nombre d'am�liorations : {upgradeCount}");

        // Appliquer les am�liorations
        ApplyUpgrades(upgradeCount, allUpgradable);

        // Mettre � jour l�UI apr�s toutes les am�liorations
        UpdateUI();
    }


    private void UpgradeToLegendary(List<UpgradeableWeapon> possibleLegendaries)
    {
        UpgradeableWeapon legendaryWeapon = possibleLegendaries[Random.Range(0, possibleLegendaries.Count)];
        playerInfos.weaponLevels[legendaryWeapon]++; // Passe en l�gendaire

        Debug.Log($"Arme l�gendaire obtenue : {legendaryWeapon.weaponLevels[^1].abilityName}");
    }

    private int DetermineUpgradeCount(int maxPossibleUpgrades)
    {
        if (maxPossibleUpgrades == 1) return 1;
        if (maxPossibleUpgrades == 2) return Random.value < 0.6f ? 1 : 2;

        float roll = Random.value;
        if (roll < 0.6f) return 1;
        if (roll < 0.9f) return Mathf.Min(3, maxPossibleUpgrades);
        return Mathf.Min(5, maxPossibleUpgrades);
    }

    private void ApplyUpgrades(int upgradeCount, List<object> allUpgradable)
    {
        int upgradesGiven = 0;

        while (upgradesGiven < upgradeCount && allUpgradable.Count > 0)
        {
            object upgradeItem = allUpgradable[Random.Range(0, allUpgradable.Count)];
            bool upgraded = false;

            if (upgradeItem is UpgradeableWeapon weapon)
            {
                playerInfos.UpgradeWeapon(weapon);
                upgradesGiven++;
                upgraded = true;

                //playerInfos.WeaponsBonusUI.UpdateUI(playerInfos.weaponLevels, playerInfos.bonusLevels, playerInfos.abilityLevels);

                // V�rifie si l'arme atteint son niveau max
                if (playerInfos.weaponLevels[weapon] >= weapon.MaxLevel)
                {
                    allUpgradable.Remove(weapon);
                }
            }
            else if (upgradeItem is UpgradeableBonus bonus)
            {
                playerInfos.AddBonus(bonus);
                upgradesGiven++;
                upgraded = true;

                //playerInfos.WeaponsBonusUI.UpdateUI(playerInfos.weaponLevels, playerInfos.bonusLevels, playerInfos.abilityLevels);

                // V�rifie si le bonus atteint son niveau max
                if (playerInfos.bonusLevels[bonus] >= bonus.bonusLevels.Count - 1)
                {
                    allUpgradable.Remove(bonus);
                }
            }

            // Si plus rien � am�liorer, on arr�te
            if (allUpgradable.Count == 0)
            {
                return;
            }
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenChest();
            Destroy(gameObject);
        }
    }

    private void UpdateUI()
    {
        if (weaponsBonusUI != null)
        {
            weaponsBonusUI.UpdateUI(playerInfos.weaponLevels, playerInfos.bonusLevels, playerInfos.abilityLevels);
        }
        else
        {
            Debug.LogWarning("WeaponsBonusUI n'est pas initialis� !");
        }
    }

}