using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private float legendaryChance = 0.85f;
    private GameObject chestPanel;
    private GameObject panelToEnable;
    private List<Image> upgradeImages = new List<Image>();

    private PlayerInfos playerInfos;
    private WeaponsBonusUI weaponsBonusUI;

    private void Start()
    {
        playerInfos = PlayerInfos.Instance;
        weaponsBonusUI = WeaponsBonusUI.Instance;

        chestPanel = GameObject.FindWithTag("ChestPanel");
        panelToEnable = chestPanel.transform.GetChild(0).gameObject;
        FindUpgradeImages();
        HideAllUpgradeImages();
    }

    private void FindUpgradeImages()
    {
        upgradeImages.Clear();
        foreach (Transform child in panelToEnable.transform)
        {
            if (child.TryGetComponent(out Image img))
            {
                upgradeImages.Add(img);
                img.gameObject.SetActive(false);
            }
        }
    }

    public void OpenChest()
    {
        Time.timeScale = 0f;
        panelToEnable.SetActive(true);

        List<object> upgrades = GetUpgrades();
        if (upgrades.Count == 0)
        {
            Debug.Log("Tout est déjà amélioré au maximum");
            panelToEnable.SetActive(false);
            return;
        }

        ShowUpgradeVisuals(upgrades);
        UpdateUI();
        StartCoroutine(CloseChestPanelAfterDelay(3f));
    }

    private void ShowUpgradeVisuals(List<object> upgrades)
    {
        HideAllUpgradeImages();

        for (int i = 0; i < upgrades.Count && i < upgradeImages.Count; i++)
        {
            Image upgradeImage = upgradeImages[i];

            if (upgrades[i] is UpgradeableWeapon weaponUpgrade)
            {
                int currentLevel = playerInfos.weaponLevels[weaponUpgrade];
                upgradeImage.sprite = weaponUpgrade.GetWeaponAtLevel(currentLevel).icon;
            }
            else if (upgrades[i] is UpgradeableBonus bonusUpgrade)
            {
                int currentLevel = playerInfos.bonusLevels[bonusUpgrade];
                upgradeImage.sprite = bonusUpgrade.bonusLevels[currentLevel].icon;
            }

            upgradeImage.gameObject.SetActive(true);
        }
    }

    private void HideAllUpgradeImages()
    {
        foreach (var img in upgradeImages) img.gameObject.SetActive(false);
    }

    private IEnumerator CloseChestPanelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        panelToEnable.SetActive(false);
        HideAllUpgradeImages();
        Time.timeScale = 1;
        Destroy(gameObject);
    }

    private void UpdateUI()
    {
        if (weaponsBonusUI != null)
        {
            weaponsBonusUI.UpdateUI(playerInfos.weaponLevels, playerInfos.bonusLevels, playerInfos.abilityLevels);
        }
    }

    private List<object> GetUpgrades()
    {
        List<object> upgrades = new List<object>();
        List<UpgradeableWeapon> upgradableWeapons = new List<UpgradeableWeapon>();
        List<UpgradeableBonus> upgradableBonuses = new List<UpgradeableBonus>();
        List<UpgradeableWeapon> possibleLegendaries = new List<UpgradeableWeapon>();

        foreach (var weapon in playerInfos.weaponLevels.Keys)
        {
            int currentLevel = playerInfos.weaponLevels[weapon];

            if (currentLevel < weapon.MaxLevel)
            {
                upgradableWeapons.Add(weapon);
            }
            else if (currentLevel == weapon.MaxLevel && weapon.HasLegendary && playerInfos.bonusLevels.ContainsKey(weapon.requiredBonusForLegendary))
            {
                possibleLegendaries.Add(weapon);
            }
        }

        foreach (var bonus in playerInfos.bonusLevels.Keys)
        {
            if (playerInfos.bonusLevels[bonus] < bonus.bonusLevels.Count - 1)
            {
                upgradableBonuses.Add(bonus);
            }
        }

        if (possibleLegendaries.Count > 0 && (upgradableWeapons.Count == 0 && upgradableBonuses.Count == 0 || Random.value < legendaryChance))
        {
            var legendaryWeapon = possibleLegendaries[Random.Range(0, possibleLegendaries.Count)];
            playerInfos.weaponLevels[legendaryWeapon]++;
            upgrades.Add(legendaryWeapon);
            return upgrades;
        }

        List<object> allUpgradable = new List<object>();
        allUpgradable.AddRange(upgradableWeapons);
        allUpgradable.AddRange(upgradableBonuses);
        ShuffleList(allUpgradable);

        int upgradeCount = DetermineUpgradeCount(allUpgradable.Count);
        for (int i = 0; i < upgradeCount; i++)
        {
            object upgrade = allUpgradable[i];
            upgrades.Add(upgrade);

            if (upgrade is UpgradeableWeapon weapon)
                playerInfos.UpgradeWeapon(weapon);
            else if (upgrade is UpgradeableBonus bonus)
                playerInfos.AddBonus(bonus);
        }

        return upgrades;
    }

    private int DetermineUpgradeCount(int maxUpgrades)
    {
        if (maxUpgrades == 1) return 1;
        if (maxUpgrades == 2) return Random.value < 0.6f ? 1 : 2;

        float roll = Random.value;
        return roll < 0.6f ? 1 : roll < 0.9f ? Mathf.Min(3, maxUpgrades) : Mathf.Min(5, maxUpgrades);
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randIndex = Random.Range(0, i + 1);
            (list[i], list[randIndex]) = (list[randIndex], list[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OpenChest();
        }
    }
}
