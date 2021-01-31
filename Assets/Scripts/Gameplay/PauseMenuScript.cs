using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class PauseMenuScript : MonoBehaviour
    {
        private CanvasGroup pauseMenuGroup;

        public Button unpauseButton;

        public GameController gc;

        public void Awake()
        {
            pauseMenuGroup = GetComponent<CanvasGroup>();
            gc = GameObject.FindObjectOfType<GameController>();
            unpauseButton.gameObject.SetActive(false);
        }


        public void ShowPauseMenu()
        {
            unpauseButton.gameObject.SetActive(true);
            pauseMenuGroup.alpha = 1f;
        }

        public void HidePauseMenu()
        {
            unpauseButton.gameObject.SetActive(false);
            pauseMenuGroup.alpha = 0f;
        }

        public void Unpause()
        {
            gc.Unpause();
        }
    }
}