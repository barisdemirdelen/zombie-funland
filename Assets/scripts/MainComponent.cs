using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
    public class MainComponent : MonoBehaviour
    {
        private int _investmentCount;
        private decimal _investmentReturn;
        private float _lastTime;

        private decimal _money;
        private decimal _zombie;
        private decimal _audience;

        private decimal _ticketPrice;

        private decimal _mps;
        private decimal _zps;
        private decimal _aps;

        //public Button InvestButton;
        // public Button ResourceButton;
        public Text MoneyText;
        public Text MpsText;
        public Text ZombieText;
        public Text ZpsText;
        public Text ApsText;
        public Text AudienceText;
        // Use this for initialization
        private void Start()
        {
            PlayerPrefs.DeleteAll();
            // ResourceButton.interactable = true;
            _lastTime = Time.time;
            _investmentReturn = 0.2m;
            _money = 100m;
            _zombie = 0m;
            _audience = 4m;
            _ticketPrice = 3m;
            _mps = 0;
            _zps = 0;
            _aps = 0;
            //LoadGame();
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
            if (_zombie > 0 || _zps>0)
            {
                _money += _mps*_audience;
                _zombie += _zps;
            }

            _audience += _aps;

            _lastTime = currentTime;
            // InvestButton.interactable = _money >= GetInvestmentCost();
            UpdateTexts();
            //SaveGame();
        }

        public void OnResourceClicked()
        {
            _money++;
            UpdateTexts();
        }

        public void OnInvestClicked()
        {
            if (_money < GetInvestmentCost())
            {
                return;
            }
            _money -= GetInvestmentCost();
            _investmentCount++;
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            MoneyText.text = "$" + _money.ToString("F2");
            MpsText.text = _mps*_audience + "/sec";
            ZombieText.text = "Zombies: " + _zombie.ToString("F0");
            ZpsText.text = _zps + "/sec";
            AudienceText.text = "Audience: " + _audience.ToString("F0");
            ApsText.text = _aps + "/sec";
            // InvestButton.GetComponentInChildren<Text>().text = "Invest cost: " + GetInvestmentCost();
        }

        /*private void LoadGame()
        {
            Decimal.TryParse(PlayerPrefs.GetString("money", "0.0"), out _money);
            //Decimal.TryParse(PlayerPrefs.GetString("investmentReturn", "0.2m"), out _investmentReturn);
            _investmentCount = PlayerPrefs.GetInt("investmentCount", 0);
        }

        private void SaveGame()
        {
            PlayerPrefs.SetString("money", _money.ToString(CultureInfo.InvariantCulture));
            //PlayerPrefs.SetString("investmentReturn", _investmentReturn.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetInt("investmentCount", _investmentCount);
        }*/

        public void BuyZombie()
        {
            if (_money < 10m)
            {
                return;
            }

            _money -= 10m;
            _zombie += 1;
            UpdateTexts();
        }

        public void StartSession()
        {
            _zps -= 1m;
            _mps += _ticketPrice;
            UpdateTexts();
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