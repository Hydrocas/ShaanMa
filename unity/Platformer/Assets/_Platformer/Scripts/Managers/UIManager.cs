///-----------------------------------------------------------------
/// Author : Maximilien Galea
/// Date : 21/01/2020 11:37
///-----------------------------------------------------------------

using Com.Isartdigital.Platformer.Data;
using Com.Isartdigital.Platformer.UI;
using Com.IsartDigital.Common.Managers;
using Com.IsartDigital.Common.Objects;
using System;
using System.Collections;
using UnityEngine;

namespace Com.Isartdigital.Platformer.Managers
{

    public class UIManager : GameplayObject {

        [SerializeField] private Canvas[] canvasList;

        [Header("Managers")]
        [SerializeField] private UserManager userManager;
        [SerializeField] private GameManager gameManager;

        [Header("Screens")]
        [SerializeField] private MainPanel mainPanel;
        [SerializeField] private Titlecard titlecard;
        [SerializeField] private LevelSelection levelSelection;
        [SerializeField] private OptionScreen optionScreen;
        [SerializeField] private PauseScreen pauseScreen;
        [SerializeField] private Hud hud;
        [SerializeField] private WinScreen winScreen;
        [SerializeField] private LooseScreen looseScreen;
        [SerializeField] private LeaderBoard leaderBoard;
        [SerializeField] private LoginScreen loginScreen;
        [SerializeField] private Credits credits;
        [SerializeField] private TutoIllu tutoIllustration;

        [Header("transition")]
        [SerializeField] private ScreenObject gameTransition;

        [Header("Other")]
        [SerializeField] private float cameraMoveDuration;
        [SerializeField] private AnimationCurve cameraMoveCurve;
        [SerializeField] private string bgCanvasTag = "bgCanvas";

        private ScreensDisplayer screensDisplayer;
        [SerializeField] private Transform uiCameraTransform;
        private bool isLogin;

        [SerializeField] private GameObject uiBackground;

        private void Start() {
            Init();
        }

        public override void Init() 
        {
            uiCameraTransform = Camera.main.transform;
            screensDisplayer = new ScreensDisplayer(canvasList);

            titlecard.OnContinue += Titlecard_OnContinue;
            screensDisplayer.ForceDisplay(titlecard);
            uiCameraTransform.position = new Vector3(titlecard.transform.position.x, titlecard.transform.position.y, uiCameraTransform.position.z);
        }

        private void Titlecard_OnContinue(ScreenObject sender)
        {
            titlecard.OnContinue -= Titlecard_OnContinue;

            screensDisplayer.Display(loginScreen);
            loginScreen.InputField.onEndEdit.AddListener(LoginScreen_OnEndEdit);
        }

        // =================================================================
        //                          *** login ***
        // =================================================================

        private void LoginScreen_OnEndEdit(string text) {
            if (text == "" || isLogin) return;

            isLogin = true;
            RemoveEvents();
            userManager.Login(text, RemoveLogin);
        }

        private void RemoveLogin()
        {
            screensDisplayer.Remove(loginScreen, MainPanelTransition);
        }

        private void MainPanelTransition()
        {
            SwitchOnUIBackground(titlecard, mainPanel, MainPanelEvent);
        }

        private void SwitchOnUIBackground(ScreenObject lastScreen, ScreenObject nextScreen, Action callback)
        {
            screensDisplayer.ForceDisplay(nextScreen);
            nextScreen.Interectable = false;

            StartCoroutine(MoveCam(lastScreen, nextScreen, callback));
        }

        private IEnumerator MoveCam(ScreenObject lastScreen, ScreenObject nextScreen, Action callback)
        {
            Vector3 targetPos = nextScreen.transform.position;
            targetPos.z = uiCameraTransform.position.z;

            Vector3 camStartPos = uiCameraTransform.position;

            float elapsedTime = 0;
            float coef;

            while (elapsedTime < cameraMoveDuration)
            {
                elapsedTime += Time.deltaTime;
                coef = cameraMoveCurve.Evaluate(elapsedTime / cameraMoveDuration);

                uiCameraTransform.position = Vector3.LerpUnclamped(camStartPos, targetPos, coef);

                yield return null;
            }

            screensDisplayer.ForceRemove(lastScreen);
            nextScreen.Interectable = true;
            callback();
        }

        // =================================================================
        //                          *** mainPanel ***
        // =================================================================

        private void MainPanelEvent() {
            RemoveEvents();

            mainPanel.PlayButton.OnClick += LevelButton_OnClick;
            mainPanel.LevelSelectionButton.onClick.AddListener(TitleCard_OnClickLevelSelection);

            mainPanel.OptionButton.onClick.AddListener(TitleCard_OnClickOption);
            mainPanel.CreditsButton.onClick.AddListener(TitleCard_OnClickCredits);
        }

        private void TitleCard_OnClickCredits() {
            screensDisplayer.Display(credits);
        }

        private void TitleCard_OnClickOption() {
            screensDisplayer.Display(optionScreen);
        }

        private void LevelButton_OnClick(LevelButton sender) 
        {
            RemoveEvents();
            screensDisplayer.DisplayTransition(gameTransition, () =>
            {
                screensDisplayer.ForceRemoveAll();
                screensDisplayer.ForceDisplay(hud);

                if (sender.LevelIndex == 1)
                    screensDisplayer.ForceDisplay(tutoIllustration);
                
                gameManager.LoadLevel(sender.LevelIndex, EndLevelTransition);
            });
        }

        private void EndLevelTransition()
        {
            uiBackground.SetActive(false);
            screensDisplayer.RemoveTransition(DisplayHud);
        }

        // =================================================================
        //                     *** LevelSelection ***
        // =================================================================

        private void TitleCard_OnClickLevelSelection()
        {
            levelSelection.SetData(userManager.User);
            SwitchOnUIBackground(mainPanel, levelSelection, LevelSelectionEvent);
        }

        private void LevelSelectionEvent()
        {
            RemoveEvents();
            levelSelection.BackButton.onClick.AddListener(levelSelection_Back);

            levelSelection.Level1Button.OnClick += LevelButton_OnClick;
            levelSelection.Level2Button.OnClick += LevelButton_OnClick;
        }

        private void levelSelection_Back()
        {
            SwitchOnUIBackground(levelSelection, mainPanel, MainPanelEvent);
        }

        // =================================================================
        //                          *** Hud ***
        // =================================================================

        private void DisplayHud() {
            RemoveEvents();

            hud.PauseButton.onClick.AddListener(Hud_OnClickPause);
            hud.WinButton.onClick.AddListener(gameManager.Win);
            hud.LooseButton.onClick.AddListener(gameManager.Loose);

            gameManager.OnTimeChangedPercent += hud.OnTimeChanged;
            
            AddHudEvents();
        }

        private void AddHudEvents()
        {
            gameManager.OnWin += GameManager_OnWin;
            gameManager.OnLoose += GameManager_OnLoose;
        }

        // =================================================================
        //                          *** Pause ***
        // =================================================================

        private void Hud_OnClickPause() 
        {
            screensDisplayer.Display(pauseScreen);
            pauseScreen.QuitButton.onClick.AddListener(Quit);
        }

        // =================================================================
        //                          *** Loose ***
        // =================================================================

        private void GameManager_OnLoose(GameManager sender) {
            DisplayLoose();
        }

        private void DisplayLoose() {
            screensDisplayer.Display(looseScreen);

            looseScreen.QuitButton.onClick.AddListener(Quit);
            looseScreen.RetryButton.onClick.AddListener(Retry);
        }

        private void Retry() {
            screensDisplayer.DisplayTransition(gameTransition, RetryTransitionStart);
            gameManager.Restart(RetryTransitionStart);
        }

        private void RetryTransitionStart()
        {
            screensDisplayer.ForceRemoveAt(1);
            gameManager.Restart(RetryTransitionEnd);
        }

        private void RetryTransitionEnd()
        {
            screensDisplayer.RemoveTransition();
        }

        private void Quit() {
            RemoveEvents();
            screensDisplayer.DisplayTransition(gameTransition, QuitTransitionStart);
        }

        private void QuitTransitionStart()
        {
            screensDisplayer.ForceRemoveAll();
            gameManager.Quit(QuitTransitionEnd);
        }

        private void QuitTransitionEnd()
        {
            uiBackground.SetActive(true);
            uiCameraTransform = Camera.main.transform;

            uiCameraTransform.position = new Vector3(mainPanel.transform.position.x, mainPanel.transform.position.y, uiCameraTransform.position.z);
            screensDisplayer.ForceDisplay(mainPanel);
            
            screensDisplayer.RemoveTransition(MainPanelEvent);
        }

        // =================================================================
        //                          *** Win ***
        // =================================================================

        private void GameManager_OnWin(GameManager sender) {
            DisplayWin();
        }

        private void DisplayWin() {
            screensDisplayer.Display(winScreen);

            winScreen.RetryButton.onClick.AddListener(Retry);
            winScreen.NextButton.onClick.AddListener(Win_OnClickNext);
        }

        private void Win_OnClickNext() {
            RemoveEvents();
            screensDisplayer.DisplayTransition(gameTransition, WinTransitionStart);
        }

        private void WinTransitionStart()
        {
            screensDisplayer.ForceRemoveAll();
            gameManager.Quit(WinTransitionEnd);
        }

        private void WinTransitionEnd()
        {
            uiBackground.SetActive(true);
            uiCameraTransform = Camera.main.transform;

            uiCameraTransform.position = new Vector3(levelSelection.transform.position.x, levelSelection.transform.position.y, uiCameraTransform.position.z);
            levelSelection.SetData(userManager.User);
            screensDisplayer.ForceDisplay(levelSelection);
            LevelSelectionEvent();

            gameManager.GetUsers(DisplayLeaderBoard);
        }

        // =================================================================
        //                       *** LeaderBoard ***
        // =================================================================


        private void DisplayLeaderBoard(User[] users) {
            leaderBoard.SetList(users);
            screensDisplayer.ForceDisplay(leaderBoard);
            screensDisplayer.RemoveTransition();
        }

        public override void RemoveEvents() 
        {
            loginScreen.InputField.onEndEdit.RemoveListener(LoginScreen_OnEndEdit);

            if(mainPanel)
                mainPanel.PlayButton.OnClick -= LevelButton_OnClick;

            if(levelSelection)
            {
                levelSelection.Level1Button.OnClick -= LevelButton_OnClick;
                levelSelection.Level2Button.OnClick -= LevelButton_OnClick;
            }
            
            if(titlecard)
                titlecard.OnContinue -= Titlecard_OnContinue;

            gameManager.OnWin -= GameManager_OnWin;
            gameManager.OnLoose -= GameManager_OnLoose;
        }
    }
}