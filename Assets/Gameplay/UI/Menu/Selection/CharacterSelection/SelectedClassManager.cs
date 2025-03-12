using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedClassManager : MonoBehaviour
{
    [SerializeField] private CharacterClass selectedCharacterClass;

    //infos
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private TMP_Text characterDescription;
    [SerializeField] private Image characterSprite;
    [SerializeField] private Image startWeapon;

    //Stats
    [SerializeField] private TMP_Text maxHealth;
    [SerializeField] private TMP_Text speed;
    [SerializeField] private TMP_Text damageBonus;


    private void Start()
    {
        UpdateInfos();
        PlayerPrefs.SetInt("SelectedCharacter", 0);
    }

    public void SelectClass(CharacterClass characterClass)
    {
        selectedCharacterClass = characterClass;
        UpdateInfos();
    }

    private void UpdateInfos()
    {
        characterName.text = selectedCharacterClass.className;
        characterDescription.text = selectedCharacterClass.classDescription;
        characterSprite.sprite = selectedCharacterClass.playerSprite;
        startWeapon.sprite = selectedCharacterClass.startWeapon[0].weaponLevels[0].icon;

        maxHealth.text = "Max Health: " + selectedCharacterClass.maxHealth;
        speed.text = "Speed: " + selectedCharacterClass.speed;
        damageBonus.text = "Damage Bonus: + " + selectedCharacterClass.damageBonus;
    }

    public void SaveSelectedCharacter(int selectedClassInt)
    {
        PlayerPrefs.SetInt("SelectedCharacter", selectedClassInt);
    }
}
