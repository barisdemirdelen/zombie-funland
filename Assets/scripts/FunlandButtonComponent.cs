using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
    public class FunlandButtonComponent : MonoBehaviour
    {
        private Button _panelButton;
        private Text _panelText;
        private Image _progressBar;

        private bool _inProgress;

        private float _duration;
        private float _startTime;


        // Use this for initialization
        private void Start()
        {
            _panelButton = transform.FindChild("PanelButton").gameObject.GetComponent<Button>();
            _panelText = transform.FindChild("DescriptionText").gameObject.GetComponent<Text>();
            _progressBar = transform.FindChild("ProgressBar").gameObject.GetComponent<Image>();

            _inProgress = false;
            _panelButton.interactable = true;
            _progressBar.fillAmount = 0.0f;
            _duration = 3.0f;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_inProgress)
            {
                return;
            }
            if (Time.time - _startTime >= _duration)
            {
                _progressBar.fillAmount = 0.0f;
                _inProgress = false;
                _panelButton.interactable = true;
                GiveRewards();
            }
            else
            {
                _progressBar.fillAmount = Math.Min((Time.time - _startTime)/_duration, 1.0f);
            }
        }

        private void GiveRewards()
        {
        }

        public void OnButtonClick()
        {
            _inProgress = true;
            _panelButton.interactable = false;
            _startTime = Time.time;
        }
    }
}