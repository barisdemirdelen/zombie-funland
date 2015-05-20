using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
    public class MainComponent : MonoBehaviour
    {
       /* private int _investmentCount;
        private decimal _investmentReturn;*/
        private float _lastTime;

        private ScoreComponent _score;


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
            _score = GetComponent<ScoreComponent>();
            //_investmentReturn = 0.2m;

            //LoadGame();
            UpdateTexts();
            CreateFunlandButtons();
        }

        // Update is called once per frame
        private void Update()
        {
            /*var currentTime = Time.time;
            if (_lastTime + 1 > currentTime)
            {
                return;
            }*/
            /*if (_zombie > 0 || _zps>0)
            {
                _money += _mps*_audience;
                _zombie += _zps;
            }

            _audience += _aps;

            _lastTime = currentTime;
            // InvestButton.interactable = _money >= GetInvestmentCost();*/
            UpdateTexts();
            //SaveGame();
        }

      /*  public void OnResourceClicked()
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
        }*/

        private void UpdateTexts()
        {
            MoneyText.text = "$" + _score.Money.ToString("F2");
            //MpsText.text = _mps*_audience + "/sec";
            ZombieText.text = "Zombies: " + _score.Zombie.ToString("F0");
           // ZpsText.text = _zps + "/sec";
            AudienceText.text = "Audience: " + _score.Audience.ToString("F0");
            //ApsText.text = _aps + "/sec";
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

       /* public void BuyZombie()
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
        }*/

        /*private decimal GetResourcePerSecond()
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
        }*/


        private void CreateFunlandButtons()
        {
            FunlandButtonComponent funlandButtonComponent = CreateFunlandButton();
            funlandButtonComponent.MoneyCost = 10;
            funlandButtonComponent.ZombieReward = 1;
        }

        private FunlandButtonComponent CreateFunlandButton()
        {
            GameObject panel = GameObject.Find("/Canvas/Panel/TabPanel/FunlandPanel/CenterPanel");
            GameObject button = (GameObject)Instantiate(Resources.Load("Prefab/ButtonPanel"));
            button.transform.SetParent(panel.transform,false);
            FunlandButtonComponent funlandButtonComponent = button.GetComponent<FunlandButtonComponent>();
            return funlandButtonComponent;
        }

    }
}