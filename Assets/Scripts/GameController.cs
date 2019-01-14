using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public Text player1Text;
    public Text player2Text;

    public GameObject startPanel;

    public GameObject pelota;
    public PlayerController player1;
    public PlayerController player2;

    public GameObject pauseMenuController;

    private int player1Points;
    private int player2Points;

    private BallController ballController;
    private MenuController menuController;
    private bool isDemo = true;

    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
            //...set this one to be it...
            instance = this;
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);
    }

    void Start ()
    {
        player1Points = 0;
        player2Points = 0;
        player1Text.text = player1Points.ToString();
        player2Text.text = player2Points.ToString();

        ballController = pelota.GetComponent<BallController>();
        menuController = pauseMenuController.GetComponent<MenuController>();

        //start demo
        ballController.startMovement();
        player1.isIA = true;
        player2.isIA = true;

        //show menu
        menuController.showMenu();
    }
	
	void Update ()
    {

        if (!menuController.isActive() && !ballController.isMoving())
        {
            if (!startPanel.activeSelf)
                startPanel.SetActive(true);
        }

        if (Input.GetButtonDown("Jump") && !menuController.isActive())
        {
            if (ballController.startMovement())
            {
                startPanel.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    public void setPoint(int player)
    {
        if (player == 1)
        {
            player1Points++;
            player1Text.text = player1Points.ToString();
        }
        else
        {
            player2Points++;
            player2Text.text = player2Points.ToString();
        }

        StartCoroutine(pelota.GetComponent<BallController>().setInitPosition(3));

    }

    public void setStartPoint()
    {
        startPanel.SetActive(true);
    }

    //options: 1 = player vs ia 2 = player vs player 3 = ia vs ia
    public void beginGame(int option)
    {
        switch(option)
        {
            case 1:
                player1.isIA = false;
                player2.isIA = true;
                break;
            case 2:
                player1.isIA = false;
                player2.isIA = false;
                break;
            case 3:
            default:
                player1.isIA = true;
                player2.isIA = true;
                break;
        }

        player1Points = 0;
        player2Points = 0;
        player1Text.text = "0";
        player2Text.text = "0";

        isDemo = false;
        StartCoroutine(pelota.GetComponent<BallController>().setInitPosition(0));
    }

    public bool getIsDemo()
    {
        return isDemo;
    }

}
