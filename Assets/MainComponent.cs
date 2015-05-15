using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class MainComponent : MonoBehaviour
    {

        public Text ResourceText;
        public Button ResourceButton;
        public Text RpsText;
        public Button InvestButton; 

        private double _resourceCount = 0.0;
        private double _resourcePerSecond = 0.0;


        private float _lastTime;

        // Use this for initialization
        void Start()
        {
            ResourceButton.interactable = true;
            _lastTime = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            var currentTime = Time.time;
            if (_lastTime + 1 <= currentTime)
            {
                _resourceCount += _resourcePerSecond;
                _lastTime = currentTime;
                UpdateTexts();
            }

            InvestButton.interactable = _resourceCount >= 10;

        }

        public void OnResourceClicked()
        {
            _resourceCount++;
            UpdateTexts();
        }

        public void OnInvestClicked()
        {
            if (_resourceCount < 10)
            {
                return;
            }
            _resourceCount -= 10;
            _resourcePerSecond += 0.2;
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            ResourceText.text = "Resource: " + _resourceCount;
            RpsText.text = "Resource per second: " + _resourcePerSecond;
        }
    }
}
