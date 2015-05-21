using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace Assets.scripts
{
    public class MainComponent : MonoBehaviour
    {
        /* private int _investmentCount;
        private decimal _investmentReturn;*/
        //private float _lastTime;

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
            //_lastTime = Time.time;
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
            MoneyText.text = "$" + NumberFormatter.FormatMoney(_score.Money);
            //MpsText.text = _mps*_audience + "/sec";
            ZombieText.text = "Zombies: " + NumberFormatter.Format(_score.Zombie);
            // ZpsText.text = _zps + "/sec";
            AudienceText.text = "Audience: " + NumberFormatter.Format(_score.Audience);
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


        private static void CreateFunlandButtons()
        {
            var funlandJson = Resources.Load("Data/Funland") as TextAsset;
            Debug.Assert(funlandJson != null, "funlandJson != null");
            var json = JSON.Parse(funlandJson.text);
            var i = 0;

            while (json["buttons"][i] != null)
            {
                var jsonButton = json["buttons"][i];
                var funlandButtonComponent = CreateFunlandButton();
                funlandButtonComponent.Name = jsonButton["name"];
                funlandButtonComponent.Description = jsonButton["description"];

                if (jsonButton["duration"] != null)
                {
                    funlandButtonComponent.Duration = jsonButton["duration"].AsFloat;
                }

                if (jsonButton["moneyCost"] != null)
                {
                    decimal moneyCost;
                    Decimal.TryParse(jsonButton["moneyCost"].Value, out moneyCost);
                    funlandButtonComponent.MoneyCost = moneyCost;
                }

                if (jsonButton["zombieCost"] != null)
                {
                    funlandButtonComponent.ZombieCost = jsonButton["zombieCost"].AsInt;
                }

                if (jsonButton["audienceCost"] != null)
                {
                    funlandButtonComponent.AudienceCost = jsonButton["audienceCost"].AsInt;
                }

                if (jsonButton["moneyReward"] != null)
                {
                    decimal moneyReward;
                    Decimal.TryParse(jsonButton["moneyReward"].Value, out moneyReward);
                    funlandButtonComponent.MoneyReward = moneyReward;
                }

                if (jsonButton["zombieReward"] != null)
                {
                    funlandButtonComponent.ZombieReward = jsonButton["zombieReward"].AsInt;
                }

                if (jsonButton["audienceReward"] != null)
                {
                    funlandButtonComponent.AudienceReward = jsonButton["audienceReward"].AsInt;
                }

                i++;
            }
            var panel = GameObject.Find("/Canvas/Panel/TabPanel/FunlandPanel/CenterPanel/Mask/GridPanel");
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panel.GetComponent<RectTransform>().sizeDelta.x, Math.Min((float)Math.Ceiling((i+2)/2.0)*100 - 400,400));
            var scrollbar = GameObject.Find("/Canvas/Panel/TabPanel/FunlandPanel/Scrollbar");
            scrollbar.GetComponent<Scrollbar>().value = 1;
            print(i);
        }

        private static FunlandButtonComponent CreateFunlandButton()
        {
            var panel = GameObject.Find("/Canvas/Panel/TabPanel/FunlandPanel/CenterPanel/Mask/GridPanel");
            var button = (GameObject) Instantiate(Resources.Load("Prefab/ButtonPanel"));
            button.transform.SetParent(panel.transform, false);
            var funlandButtonComponent = button.GetComponent<FunlandButtonComponent>();
            return funlandButtonComponent;
        }
    }
}