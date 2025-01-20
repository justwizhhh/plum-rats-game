using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class TitleMenu : MonoBehaviour
{
    // A basic title-screen menu, where you can access the options

    public enum TitleSection
    {
        Null,
        StartGame,
        Options,
        Credits,
        Exit
    }
    TitleSection currentTitleSection;

    int menuOption;

    // Positions for the camera to move towards when selecting different menu options
    public Vector2 MainCamPosition;
    public Vector2 OptionsCamPosition;
    public Vector2 CreditsCamPosition;

    [Space(10)]
    public GameObject TitleInstructions;

    TextMeshProUGUI currentCursorPos;

    bool leftClick;
    bool rightClick;

    [Space(10)]

    public GameObject MainMenuUI;
    public GameObject OptionsUI;
    public GameObject CreditsUI;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlayMusic("MenuTheme");
    }

    public void SetScreen(int newSection)
    {
        currentTitleSection = (TitleSection)newSection;
    }

    public void SetMenuOption(int option)
    {
        menuOption = option;
    }

    public void Scroll(bool soundVolume)
    {
        if (soundVolume)
        {
            AudioManager.instance.SoundVolume +=
            Mathf.Sign(Mouse.current.scroll.value.y) > 0 ? 0.1f :
            Mathf.Sign(Mouse.current.scroll.value.y) < 0 ? -0.1f : 0;

            AudioManager.instance.SoundVolume = Mathf.Clamp(AudioManager.instance.SoundVolume, 0, 1);
            AudioManager.instance.ResetSoundVolume();

            OptionsUI.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "Sound Volume - " + (Mathf.RoundToInt(AudioManager.instance.SoundVolume * 100)).ToString() + "%";
        }
        else
        {
            AudioManager.instance.MusicVolume +=
            Mathf.Sign(Mouse.current.scroll.value.y) > 0 ? 0.1f :
            Mathf.Sign(Mouse.current.scroll.value.y) < 0 ? -0.1f : 0;

            AudioManager.instance.MusicVolume = Mathf.Clamp(AudioManager.instance.MusicVolume, 0, 1);
            AudioManager.instance.ResetMusicVolume();

            OptionsUI.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text =
                "Music Volume - " + (Mathf.RoundToInt(AudioManager.instance.MusicVolume * 100)).ToString() + "%";
        }
    }

    // Update is called once per frame
    public void Forward()
    {
        switch (currentTitleSection)
        {
            case TitleSection.Null: // If nothing has been selected on the titlescreen yet
                menuOption += Input.GetKeyDown(KeyCode.DownArrow) ? 1 : Input.GetKeyDown(KeyCode.UpArrow) ? -1 : 0;
                menuOption = Mathf.Clamp(menuOption, 0, 3);

                // Selecting each menu options one-by-one
                switch (menuOption)
                {
                    case 0:
                        currentTitleSection = TitleSection.StartGame;
                        break;
                    case 1:
                        currentTitleSection = TitleSection.Options;
                        MainMenuUI.SetActive(false);
                        OptionsUI.SetActive(true);
                        menuOption = 0;
                        break;
                    case 2:
                        currentTitleSection = TitleSection.Credits;
                        MainMenuUI.SetActive(false);
                        CreditsUI.SetActive(true);
                        break;
                    case 3:
                        Application.Quit();
                        break;
                }

                cam.transform.position = MainCamPosition;
                currentCursorPos = MainMenuUI.transform.GetChild(menuOption).GetComponent<TextMeshProUGUI>();
                break;

            case TitleSection.StartGame:
                // Starts the game
                TitleInstructions.SetActive(true);
                TitleInstructions.GetComponent<Animator>().SetTrigger("Instructions");
                break;

            case TitleSection.Options: // Changing music and sound volumes
                menuOption += Input.GetKeyDown(KeyCode.DownArrow) ? 1 : Input.GetKeyDown(KeyCode.UpArrow) ? -1 : 0;
                menuOption = Mathf.Clamp(menuOption, 0, 1);

                currentTitleSection = TitleSection.Null;
                OptionsUI.SetActive(false);
                MainMenuUI.SetActive(true);

                cam.transform.position = OptionsCamPosition;
                currentCursorPos = OptionsUI.transform.GetChild(menuOption).GetComponent<TextMeshProUGUI>();
                break;

            case TitleSection.Credits:

                // Going back to the main menu
                currentTitleSection = TitleSection.Null;
                CreditsUI.SetActive(false);
                MainMenuUI.SetActive(true);

                cam.transform.position = CreditsCamPosition;
                currentCursorPos = CreditsUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                break;

            case TitleSection.Exit:

                Application.Quit();
                break;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }
}
