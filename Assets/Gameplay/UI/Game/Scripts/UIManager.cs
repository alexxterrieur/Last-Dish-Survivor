using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    private int enemiesKilled;
    [SerializeField] TMP_Text enemiesKilledCount;
    [SerializeField] TMP_Text[] abilitiesInput;
    [SerializeField] TMP_Text chrono;
    private float elapsedTime = 0f;

    [SerializeField] private InputActionAsset inputActions;

    private void Start()
    {
        enemiesKilledCount.text = " : 0";
        LifeManager.OnDeath += UpdateEnemiesKilled;

        BindAbilityInputs();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        chrono.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


    private void UpdateEnemiesKilled()
    {
        enemiesKilled++;
        enemiesKilledCount.text = " : " + enemiesKilled.ToString();
    }

    private void BindAbilityInputs()
    {
        InputAction ability1Action = inputActions.FindAction("Ability 1");
        InputAction ability2Action = inputActions.FindAction("Ability 2");
        InputAction ability3Action = inputActions.FindAction("Ability 3");
        InputAction ability4Action = inputActions.FindAction("Ability 4");

        if (ability1Action != null)
        {
            string ability1Input = ability1Action.bindings[0].ToDisplayString();
            abilitiesInput[0].text += ability1Input;
        }

        if (ability2Action != null)
        {
            string ability2Input = ability2Action.bindings[0].ToDisplayString();
            abilitiesInput[1].text += ability2Input;
        }

        if (ability3Action != null)
        {
            string ability3Input = ability3Action.bindings[0].ToDisplayString();
            abilitiesInput[2].text += ability3Input;
        }

        if (ability4Action != null)
        {
            string ability4Input = ability4Action.bindings[0].ToDisplayString();
            abilitiesInput[3].text += ability4Input;
        }
    }
}
