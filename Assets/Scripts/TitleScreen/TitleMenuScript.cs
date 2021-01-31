using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Utilities.fader;

namespace TitleScreen
{
    public class TitleMenuScript : MonoBehaviour
    {

        public Credits theCredits;

        public CanvasGroup titleGroup;

        public CanvasGroup startGameGroup;

        public TitleState menuState;

        public AudioSource theAudioSource;

        public void Awake()
        {
            menuState = TitleState.ShowingTitle;

            startGameGroup.alpha = 0F;
            titleGroup.alpha = 1f;
            titleGroup.interactable = true;

            startGameGroup.gameObject.SetActive(false);
            theCredits.gameObject.SetActive(false);
        }


        public void Update()
        {
            
            if (menuState != TitleState.ItsFadeTime && CrossPlatformInputManager.GetButtonDown("Cancel"))
            {
                #if UNITY_WEBGL || UNITY_EDITOR
                if (theAudioSource.isPlaying)
                {
                    theAudioSource.Pause();
                }
                else
                {
                    theAudioSource.UnPause();
                }
                #else
                QuitGame();
                #endif
            }
            
            if (menuState == TitleState.ShowingIntro)
            {
                if (Input.anyKeyDown)
                {
                    menuState = TitleState.ItsFadeTime;
                    FindObjectOfType<FaderScript>().ChangeLevel(1);
                }
            }

        }

        public void StartGame()
        {
            titleGroup.interactable = false;
            titleGroup.alpha = 0F;
            menuState = TitleState.ShowingIntro;
            startGameGroup.alpha = 1F;
            startGameGroup.gameObject.SetActive(true);
        }


        public void ShowCredits()
        {
            
            titleGroup.interactable = false;
            titleGroup.alpha = 0F;
            menuState = TitleState.ShowingCredits;
            theCredits.gameObject.SetActive(true);
            theCredits.StartCredits();
        }

        public void CreditsFinished()
        {
            theCredits.gameObject.SetActive(false);
            titleGroup.interactable = true;
            titleGroup.alpha = 1f;
            menuState = TitleState.ShowingTitle;
        }

        public void QuitGame()
        {
            if (menuState != TitleState.ItsFadeTime)
            {
                titleGroup.interactable = false;
                menuState = TitleState.ItsFadeTime;
                GameObject.FindObjectOfType<FaderScript>().FadeAndQuit(FaderScript.FadeType.BLACK, false);
            }
        }

    }

    public enum TitleState
    {
        ShowingTitle,
        ShowingCredits,
        ShowingIntro,
        ItsFadeTime
    }
}