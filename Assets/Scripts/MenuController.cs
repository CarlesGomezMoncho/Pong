using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject mainMenu;
    public GameObject firstMainMenu;
    public GameObject startMenu;
    public GameObject firstStartMenu;
    public GameObject pauseMenu;
    public GameObject firstPauseMenu;
    public GameObject OptionsMenu;
    public GameObject firstOptionsMenu;
    public TextMeshProUGUI player1UpText;
    public TextMeshProUGUI player1DownText;
    public TextMeshProUGUI player2UpText;
    public TextMeshProUGUI player2DownText;
    public GameObject panelRebindKey;
    public GameObject startPanel;

    public EventSystem eventSystem;

    private string buttonToRebind = null;
    private GameObject currentButton = null;

    void Start ()
    {
        player1UpText.text = InputManager.instance.getValue("Player1Up").ToString();
        player1DownText.text = InputManager.instance.getValue("Player1Down").ToString(); 
        player2UpText.text = InputManager.instance.getValue("Player2Up").ToString();
        player2DownText.text = InputManager.instance.getValue("Player2Down").ToString();
    }
	
	void Update ()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //if not demo (not in game menu)
            if (!GameController.instance.getIsDemo())
            {
                //if the menu is shown
                if (isActive())
                {
                    hideMenu();         //hide menu
                }
                else
                {
                    showMenu();         //show menu
                }
            }
        }

        //if rebinding key
        if (buttonToRebind != null)
        {
            eventSystem.SetSelectedGameObject(null);

            if (Input.anyKey)
            {
                foreach(KeyCode key in Enum.GetValues(typeof(KeyCode)))
                {
                    if(Input.GetKeyDown(key) && key != KeyCode.Space && key != KeyCode.Return && key != KeyCode.KeypadEnter)
                    {
                        InputManager.instance.setButtonForKey(buttonToRebind, key);
                        currentButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = key.ToString();
                        buttonToRebind = null;
                        panelRebindKey.SetActive(false);
                        eventSystem.SetSelectedGameObject(currentButton);
                        break;
                    }
                }
            }
        }
    }

    public void showMenu()
    {
        //if main menu
        if (GameController.instance.getIsDemo())
        {
            pausePanel.SetActive(true);
            startMenu.SetActive(false);
            mainMenu.SetActive(true);
            pauseMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            eventSystem.SetSelectedGameObject(firstMainMenu);
        }
        //if pause menu
        else
        {
            pausePanel.SetActive(true);
            startMenu.SetActive(false);
            mainMenu.SetActive(false);
            pauseMenu.SetActive(true);
            OptionsMenu.SetActive(false);
            eventSystem.SetSelectedGameObject(null);        //desactivem i reactivem per que si estava ja seleccionat es queda desmarcat)
            eventSystem.SetSelectedGameObject(firstPauseMenu);
            startPanel.SetActive(false);
            Time.timeScale = 0; //pause movement
        }
    }

    public void hideMenu()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1; //restore movement
    }

    public bool isActive()
    {
        return pausePanel.activeSelf;
    }

    public void backButton()
    {
        //if main menu
        if(GameController.instance.getIsDemo())
        {
            startMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            mainMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(firstMainMenu);
        }
        //if pause menu
        else
        {
            startMenu.SetActive(false);
            OptionsMenu.SetActive(false);
            pauseMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(firstPauseMenu);
        }
    }

    public void quit()
    {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void reBind( string button)
    {
        buttonToRebind = button;
        currentButton = eventSystem.currentSelectedGameObject;
        panelRebindKey.SetActive(true);
        panelRebindKey.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "Press Key for\n"+ buttonToRebind;
    }
}
