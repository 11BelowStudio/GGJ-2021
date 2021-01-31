using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

namespace TitleScreen
{
    public class Credits : MonoBehaviour
    {
        public TitleMenuScript title;

        public CanvasGroup creditsCanvasGroup;

        public RectTransform creditsText;
        
        public float scrollSpeed = 150f;

        public float creditsStartYPos = -3000;

        public float creditsEndYPos = 1500;

        public bool isDoingCredits = false;


        public void Awake()
        {
            creditsCanvasGroup.alpha = 0F;
        }

        public void StartCredits()
        {
            creditsCanvasGroup.alpha = 1F;
            
            
            creditsText.anchoredPosition = new Vector2(0, creditsStartYPos);
            isDoingCredits = true;
        }

        public void EndCredits()
        {
            isDoingCredits = false;
            creditsCanvasGroup.alpha = 0F;
            creditsText.anchoredPosition =  new Vector2(0, creditsStartYPos);
            title.CreditsFinished();
        }

        public void Update()
        {
            if (isDoingCredits)
            {

                float creditsY = creditsText.anchoredPosition.y;
                creditsY += (scrollSpeed * Time.deltaTime);
                
                creditsText.anchoredPosition =  new Vector2(0, creditsY);

                if (creditsY > creditsEndYPos || Input.anyKeyDown)
                {
                    EndCredits();
                }
                
            }
        }


    }
}