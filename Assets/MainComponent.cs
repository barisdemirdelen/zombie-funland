using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class MainComponent : MonoBehaviour
    {
        private int _investmentCount;
        private decimal _investmentReturn;
        private float _lastTime;
        private decimal _resourceCount;
        public Button InvestButton;
        public Button ResourceButton;
        public Text ResourceText;
        public Text RpsText;
        // Use this for initialization
        private void Start()
        {
            ResourceButton.interactable = true;
            _lastTime = Time.time;
            _investmentReturn = 0.2m;
            LoadGame();
            UpdateTexts();
        }

        // Update is called once per frame
        private void Update()
        {
            var currentTime = Time.time;
            if (_lastTime + 1 > currentTime)
            {
                return;
            }
            _resourceCount += GetResourcePerSecond();
            _lastTime = currentTime;
            InvestButton.interactable = _resourceCount >= GetInvestmentCost();
            UpdateTexts();
            SaveGame();
        }

        public void OnResourceClicked()
        {
            _resourceCount++;
            UpdateTexts();
        }

        public void OnInvestClicked()
        {
            if (_resourceCount < GetInvestmentCost())
            {
                return;
            }
            _resourceCount -= GetInvestmentCost();
            _investmentCount++;
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            ResourceText.text = "Resource: " + _resourceCount;
            RpsText.text = "Resource per second: " + GetResourcePerSecond();
            InvestButton.GetComponentInChildren<Text>().text = "Invest cost: " + GetInvestmentCost();
        }

        private void LoadGame()
        {
            Decimal.TryParse(PlayerPrefs.GetString("resourceCount", "0.0"), out _resourceCount);
            //Decimal.TryParse(PlayerPrefs.GetString("investmentReturn", "0.2m"), out _investmentReturn);
            _investmentCount = PlayerPrefs.GetInt("investmentCount", 0);
        }

        private void SaveGame()
        {
            PlayerPrefs.SetString("resourceCount", _resourceCount.ToString(CultureInfo.InvariantCulture));
            //PlayerPrefs.SetString("investmentReturn", _investmentReturn.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetInt("investmentCount", _investmentCount);
        }

        private decimal GetResourcePerSecond()
        {
            return _investmentCount*_investmentReturn;
        }

        private decimal GetInvestmentCost()
        {
            var baseCost = 10m;
            for (var i = 0; i < _investmentCount; i++)
            {
                baseCost += baseCost*0.27m;
            }
            return baseCost;
        }
    }
}