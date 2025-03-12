using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardInfo : MonoBehaviour
{
    [SerializeField] private CharacterClass characterClass;

    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Image characterSprite;
    [SerializeField] private Image startWeapon;

    private void Awake()
    {
        characterName.text = characterClass.className;
        characterSprite.sprite = characterClass.playerSprite;
        startWeapon.sprite = characterClass.startWeapon[0].weaponLevels[0].icon;
    }
}
