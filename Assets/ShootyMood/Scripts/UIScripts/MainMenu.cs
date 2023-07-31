using Assets.ShootyMood.Scripts.Managers;
using ShootyMood.Scripts.ShootyGameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ShootyMood.Scripts.UIScripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI audioButtonText;
        [Inject] private SignalBus signalBus;

        private void Start()
        {
            SetAudioButtontext();
        }

        public void PlayButtonClick()
        {
            signalBus.Fire(new PlayButtonClickEvent());
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        public void AudioButtonClick()
        {
            AudioOptionManager.Instance.AUDIO_ON = !AudioOptionManager.Instance.AUDIO_ON;
            signalBus.Fire(new AudioOptionChanged());
            SetAudioButtontext();
        }

        private void SetAudioButtontext()
        {
            audioButtonText.text = AudioOptionManager.Instance.AUDIO_ON ? "Audio : ON" : "Audio : OFF";
        }
    }
}