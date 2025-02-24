using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsBonusUI : MonoBehaviour
{
    public Transform weaponsParent;
    public Transform bonusesParent;

    private List<GameObject> weaponIcons = new List<GameObject>();
    private List<GameObject> bonusIcons = new List<GameObject>();

    private void Start()
    {
        foreach (Transform child in weaponsParent)
            weaponIcons.Add(child.gameObject);

        foreach (Transform child in bonusesParent)
            bonusIcons.Add(child.gameObject);
    }

    public void UpdateUI(Dictionary<UpgradeableWeapon, int> weaponLevels, Dictionary<UpgradeableBonus, int> bonusLevels)
    {
        UpdateWeaponsUI(weaponLevels);
        UpdateBonusesUI(bonusLevels);
    }

    private void UpdateWeaponsUI(Dictionary<UpgradeableWeapon, int> weaponLevels)
    {
        int index = 0;
        foreach (var weaponEntry in weaponLevels)
        {
            if (index >= weaponIcons.Count) break;

            GameObject weaponIcon = weaponIcons[index];
            weaponIcon.SetActive(true);

            Image weaponImage = weaponIcon.GetComponent<Image>();
            if (weaponImage != null)
                weaponImage.sprite = weaponEntry.Key.weaponLevels[0].icon;

            int level = weaponEntry.Value + 1;
            int maxLevel = weaponEntry.Key.weaponLevels.Count;

            UpdateLevelIndicators(weaponIcon.transform, level, maxLevel);
            index++;
        }
    }


    private void UpdateBonusesUI(Dictionary<UpgradeableBonus, int> bonusLevels)
    {
        int index = 0;
        foreach (var bonusEntry in bonusLevels)
        {
            if (index >= bonusIcons.Count) break;

            GameObject bonusIcon = bonusIcons[index];
            bonusIcon.SetActive(true);

            Image bonusImage = bonusIcon.GetComponent<Image>();
            if (bonusImage != null)
                bonusImage.sprite = bonusEntry.Key.bonusLevels[0].icon;

            int level = bonusEntry.Value + 1;
            int maxLevel = bonusEntry.Key.bonusLevels.Count;

            UpdateLevelIndicators(bonusIcon.transform, level, maxLevel);
            index++;
        }
    }


    private void UpdateLevelIndicators(Transform parent, int level, int maxLevel)
    {
        for (int i = 0; i < 9; i++)
        {
            Transform levelIndicator = parent.Find($"Lvl{i + 1}");
            if (levelIndicator != null)
            {
                bool shouldBeActive = (i < maxLevel); //enable scares according to max level (not all)
                levelIndicator.gameObject.SetActive(shouldBeActive);

                if (shouldBeActive)
                {
                    Image levelImage = levelIndicator.GetComponent<Image>();
                    if (levelImage != null)
                    {
                        levelImage.color = (i < level) ? Color.white : Color.gray;
                    }
                }
            }
        }
    }

}
