using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Gameplay
{
    [Serializable]
    public class DialogueScript : MonoBehaviour
    {
        public CanvasGroup dialogueGroup;

        public GameController gc;

        public Button replyButton1;

        public TextMeshProUGUI replyButton1Text;

        public Button replyButton2;

        public TextMeshProUGUI replyButton2Text;

        public TextMeshProUGUI NPCtext;

        [SerializeField]
        public DialogueStruct[] firstConversation;

        [SerializeField]
        public DialogueStruct[] secondConversation;

        [SerializeField]
        public DialogueStruct[] thirdConversation;

        [SerializeField]
        public DialogueStruct[] lastConversation;

        private DialogueEnum currentConversation;

        private int conversationState;

        public void Awake()
        {
            conversationState = 0;
            currentConversation = DialogueEnum.First;
            dialogueGroup.alpha = 0F;
            dialogueGroup.interactable = false;
        }


        public void StartConversation(DialogueEnum whichOne)
        {
            currentConversation = whichOne;
            conversationState = 0;
            dialogueGroup.alpha = 1F;
            dialogueGroup.interactable = true;
            ShowCurrent();
        }

        private void ShowCurrent()
        {
            DialogueStruct showThis = new DialogueStruct();
            switch (currentConversation)
            {
                case DialogueEnum.First:
                    showThis = firstConversation[conversationState];
                    break;
                case DialogueEnum.Second:
                    showThis = secondConversation[conversationState];
                    break;
                case DialogueEnum.Third:
                    showThis = thirdConversation[conversationState];
                    break;
                case DialogueEnum.Last:
                    showThis = lastConversation[conversationState];
                    break;
            }

            ShowDialogue(showThis);
        }

        private void ShowDialogue(DialogueStruct current)
        {
            NPCtext.text = current.npcWords;

            replyButton1Text.text = current.firstOption;

            replyButton2Text.text = current.secondOption;
        }

        public void AdvanceConversation()
        {
            conversationState++;

            bool done = true;

            switch (currentConversation)
            {
                case DialogueEnum.First:
                    done = (conversationState == firstConversation.Length);
                    break;
                case DialogueEnum.Second:
                    done = (conversationState == secondConversation.Length);
                    break;
                case DialogueEnum.Third:
                    done = (conversationState == thirdConversation.Length);
                    break;
                case DialogueEnum.Last:
                    done = (conversationState == lastConversation.Length);
                    break;
            }

            if (done)
            {
                dialogueGroup.alpha = 0F;
                dialogueGroup.interactable = false;
                gc.ConversationIsDone();
            }
            else
            {
                ShowCurrent();
            }
        }
        
        

    }

    public enum DialogueEnum
    {
        First,
        Second,
        Third,
        Last
    }

    [Serializable]
    public struct DialogueStruct
    {
        [SerializeField]
        public string npcWords;
        [SerializeField]
        public string firstOption;
        [SerializeField]
        public string secondOption;

        public DialogueStruct(string npcWords = "test", string firstOption = "test1", string secondOption = "test2")
        {
            this.npcWords = npcWords;
            this.firstOption = firstOption;
            this.secondOption = secondOption;
        }
    }
    
    
    
}