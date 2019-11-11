using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIButtonCode
{
    Pause,
    Contine,
    Home,
}
public class EventController : MonoBehaviour
{
    public static EventController eventController;
    public CutSceenController CutSceenController;
    public EnemyFactory enemyFactory;
    public GameObject pauseMenu;
    public GameObject battleUI;
    public ShopController shop;
    public GameObject mainMenu;
    public GameObject DefeateMenu;

    public Text waveText;
    public MineTowerController tower;
    public GameObject TrainFactory;

    public float sensitivity = 4;
    private Vector3 dragPosition;
    private int firstTouchID=-1;
    private bool blockTuretControlFlag = true;

    private void Awake()
    {
        eventController = this;
    }
    private void Start()
    {
        Input.simulateMouseWithTouches = false;
    }
    void Update()
    {
        if (blockTuretControlFlag==false)
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    StartGame();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    showPauseMenu();

                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    dragPosition = Input.mousePosition;
                }

            }
            if (Input.anyKey)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    tower.shoot();
                }
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    float yRotation = (Input.mousePosition.x - dragPosition.x)*Time.deltaTime*sensitivity;
                    tower.rotateTower(yRotation);
                    dragPosition = Input.mousePosition;
                }
            }

            if (Input.touchCount > 0)
            {
                if (Input.touchCount == 1)
                {
                    if (firstTouchID != Input.touches[0].fingerId)
                    {
                        dragPosition = Input.touches[0].position;
                        firstTouchID = Input.touches[0].fingerId;
                    }

                }
                if (dragPosition == null)
                {
                    dragPosition = Input.touches[0].position;
                }
                for (int i = 0; i < Input.touchCount; i++)
                {
                    if (Input.touches[i].fingerId == firstTouchID)
                    {
                        Touch firstTouch = Input.touches[i];
                        float yRotation = (firstTouch.position.x - dragPosition.x)*Time.deltaTime* sensitivity;
                        tower.rotateTower(yRotation);
                        dragPosition = firstTouch.position;
                        break;
                    }
                }
                tower.shoot();
            }
            else
            {
                firstTouchID = -1;
            }
        }
        else{
            firstTouchID = -1;
        }
    }

    #region Buttons
        #region MineMenu
        public void pressPlay_MM()
        {
            hideMainMenu();
            TrainFactory.SetActive(false);
            CutSceenController.playTrainAppearence();
        }
        #endregion
        #region InPlay
        public void pressPauseMenu()
        {
            showPauseMenu();
        }
        public void pressContine()
        {
            hidePauseMenu();
        }
        public void pressHome()
        {
            hidePauseMenu();
            hideBattleUI();
            GameData.reset();
            EnemyController.reset();
            Application.LoadLevel(Application.loadedLevel);
        }
    #endregion
        #region Shop
        public void pressCloseShop()
        {
            hideShop();
            CutSceenController.playTrainDeparture();
        }
        #endregion
    #endregion
    #region UIFunctions
    private void showShop()
    {
        shop.gameObject.SetActive(true);
    }
    private void hideShop()
    {
        shop.gameObject.SetActive(false);
    }
    private void showPauseMenu()
    {
        pauseMenu.SetActive(true);

        blockTuretControlFlag = false;
        Time.timeScale = 0;
    }
    private void hidePauseMenu()
    {
        pauseMenu.SetActive(false);

        blockTuretControlFlag = true;
        Time.timeScale = 1;
    }
    private void showBattleUI()
    {
        battleUI.SetActive(true);
    }
    private void hideBattleUI()
    {
        battleUI.SetActive(false);
    }
    private void showMainMenu()
    {
        mainMenu.SetActive(true);
    }
    private void hideMainMenu()
    {
        mainMenu.SetActive(false);
    }
    private void showDefeateMenu()
    {
        DefeateMenu.SetActive(true);
    }
    private void hideDefeateMenu()
    {
        DefeateMenu.SetActive(true);
    }
    #endregion
    #region cutSceen
    public void onEndDepartureAnimation()
    {
        blockTuretControlFlag = false;
        StartGame();
    }
    public void onEndLeaveAnimation()
    {
        doMiddleStage();
    }
    #endregion
    public void StartGame()
    {
        showBattleUI();
        enemyFactory.startSpawnWaves();
    }
    public void doMiddleStage()
    {
        shop.refresh();
        showShop();
    }
    public void onEndedStage()
    {
        blockTuretControlFlag = true;
        hideBattleUI();
        GameData.money += 100;
        CutSceenController.playTrainLeave();
    }
    public void onDefeate()
    {
        blockTuretControlFlag = true;
        hideBattleUI();
        showDefeateMenu();
    }
    public void updateWaveText(int curWave, int lastWave){
        waveText.text = curWave + "/" + lastWave;
    }
}