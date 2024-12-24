using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts.TurnsBasedSystem
{
    public class PhaseStateHandler
    {
        private Button phaseChangerButton;
        private Image phaseChangerImage;
        private TMP_Text phaseChangerButtonTextField;

        public PhaseStateHandler(Button button, Image image, TMP_Text textField)
        {
            phaseChangerButton = button;
            phaseChangerImage = image;
            phaseChangerButtonTextField = textField;
        }

        public void StartNewPhase(Sprite imageVariant, string phaseText, bool isButtonInteractable)
        {
            phaseChangerButton.interactable = isButtonInteractable;
            SetGamePhasePanelElements(imageVariant, phaseText);
        }

        public void SetGamePhasePanelElements(Sprite phaseSprite, string phaseName)
        {
            phaseChangerImage.sprite = phaseSprite;
            phaseChangerButtonTextField.text = phaseName;
        }
    }
}