using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsBonusUI : MonoBehaviour
{
    public Transform weaponsParent;
    public Transform bonusesParent;
    public Transform abilitiesContainer;
    public Transform cooldownsParent;

    private List<GameObject> weaponIcons = new List<GameObject>();
    private List<GameObject> bonusIcons = new List<GameObject>();
    private List<GameObject> abilitiesIcons = new List<GameObject>();
    private List<Image> abilityCooldownIcons = new List<Image>();


    public static WeaponsBonusUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        foreach (Transform child in weaponsParent)
            weaponIcons.Add(child.gameObject);

        foreach (Transform child in bonusesParent)
            bonusIcons.Add(child.gameObject);

        foreach (Transform child in abilitiesContainer)
            abilitiesIcons.Add(child.gameObject);

        foreach (Transform child in cooldownsParent)
            abilityCooldownIcons.Add(child.GetComponent<Image>());

    }

    public void UpdateUI(Dictionary<UpgradeableWeapon, int> weaponLevels, Dictionary<UpgradeableBonus, int> bonusLevels, Dictionary<UpgradeableAbility, int> abilityLevels)
    {
        UpdateWeaponsUI(weaponLevels);
        UpdateBonusesUI(bonusLevels);
        UpdateAbitiliesUI(abilityLevels);
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
            var visible = weaponImage.color;
            visible.a = 255;

            if (weaponImage != null)
            {
                weaponImage.sprite = weaponEntry.Key.weaponLevels[0].icon;
                weaponImage.color = visible;
            }

            int level = weaponEntry.Value + 1;
            int maxLevel = weaponEntry.Key.weaponLevels.Count - 1;

            if (level >= weaponEntry.Key.weaponLevels.Count)
            {
                DisableAllLevelIndicators(weaponIcon.transform);
            }
            else
            {
                UpdateLevelIndicators(weaponIcon.transform, level, maxLevel);
            }
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
            var visible = bonusImage.color;
            visible.a = 255;

            if (bonusImage != null)
            {
                bonusImage.sprite = bonusEntry.Key.bonusLevels[0].icon;
                bonusImage.color = visible;
            }

            int level = bonusEntry.Value + 1;
            int maxLevel = bonusEntry.Key.bonusLevels.Count;

            UpdateLevelIndicators(bonusIcon.transform, level, maxLevel);
            index++;
        }
    }

    private void UpdateAbitiliesUI(Dictionary<UpgradeableAbility, int> abitiliesLevels)
    {
        int index = 0;
        foreach (var abitilityEntry in abitiliesLevels)
        {
            if (index >= abilitiesIcons.Count) break;

            GameObject abilityIcon = abilitiesIcons[index];
            abilityIcon.SetActive(true);

            Image abilityImage = abilityIcon.GetComponent<Image>();
            if (abilityImage != null)
                abilityImage.sprite = abitilityEntry.Key.abilityLevels[0].icon;

            index++;
        }
    }

    public void UpdateCooldownUI(List<Ability> abilities, Dictionary<Ability, float> cooldownTimers)
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            Ability ability = abilities[i];

            if (cooldownTimers.ContainsKey(ability))
            {
                float cooldown = cooldownTimers[ability];
                float cooldownProgress = cooldown / ability.cooldown;

                abilityCooldownIcons[i].fillAmount = cooldownProgress;
            }
        }
    }

    /////////////////////////////////////////////////
    //public void StartVisualCooldown(Ability ability, float duration, Color color)
    //{
    //    int index = FindAbilityIndex(ability);
    //    if (index == -1) return;

    //    StartCoroutine(VisualCooldownCoroutine(index, duration, color));
    //}

    private int FindAbilityIndex(Ability ability)
    {
        for (int i = 0; i < abilitiesIcons.Count; i++)
        {
            if (abilitiesIcons[i].GetComponent<Image>().sprite == ability.icon)
            {
                return i;
            }
        }
        return -1; // Ability non trouvée
    }

    //private IEnumerator VisualCooldownCoroutine(int index, float duration, Color color)
    //{
    //    float timeElapsed = 0f;
    //    Image cooldownIcon = abilityCooldownIcons[index];
    //    cooldownIcon.color = color;

    //    while (timeElapsed < duration)
    //    {
    //        cooldownIcon.fillAmount = 1 - (timeElapsed / duration);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //    cooldownIcon.fillAmount = 0;
    //}
    /////////////////////////////////////////////////

    public void StartAbilityCooldownVisual(Ability ability, float duration, Color color)
    {
        int index = FindAbilityIndex(ability);
        if (index >= 0 && index < abilityCooldownIcons.Count)
        {
            StartCoroutine(AbilityCooldownCoroutine(index, duration, color));
        }
    }

    private IEnumerator AbilityCooldownCoroutine(int index, float duration, Color color)
    {
        float elapsedTime = 0f;
        abilityCooldownIcons[index].color = color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            abilityCooldownIcons[index].fillAmount = 1 - (elapsedTime / duration);
            yield return null;
        }

        abilityCooldownIcons[index].fillAmount = 0;
    }

    /////////////////////////////////////////////////

    private void UpdateLevelIndicators(Transform parent, int level, int maxLevel)
    {
        for (int i = 0; i < 9; i++)
        {
            Transform levelIndicator = parent.Find($"Lvl{i + 1}");
            if (levelIndicator != null)
            {
                bool shouldBeActive = (i < maxLevel);
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

    private void DisableAllLevelIndicators(Transform parent)
    {
        for (int i = 0; i < 9; i++)
        {
            Transform levelIndicator = parent.Find($"Lvl{i + 1}");
            if (levelIndicator != null)
            {
                levelIndicator.gameObject.SetActive(false);
            }
        }
    }
}