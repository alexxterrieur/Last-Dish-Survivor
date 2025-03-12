using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardInfo : MonoBehaviour
{
    [SerializeField] private UpgradeableAbility ability;

    [SerializeField] private Image abilityIcon;
    [SerializeField] private TMP_Text abilityName;
    [SerializeField] private TMP_Text abilityDescription;

    private void Awake()
    {
        abilityIcon.sprite = ability.abilityLevels[0].icon;
        abilityName.text = ability.abilityLevels[0].name;
        abilityDescription.text = ability.abilityDescription;
    }
}
