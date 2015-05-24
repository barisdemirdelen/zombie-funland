using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Remoting;
using Assets.Scripts;
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
        private double _lastTime;

        private ScoreComponent _score;

        private List<FunlandButtonComponent> _funlandButtons;
        private List<ManagementButtonComponent> _managementButtons;

        public Text MoneyText;
        public Text MpsText;
        public Text ZombieText;
        public Text ZpsText;
        public Text ApsText;
        public Text AudienceText;


        // Use this for initialization
        private void Start()
        {
            var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            var timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
            _lastTime = timestamp;
            _score = GetComponent<ScoreComponent>();

            CreateFunlandButtons();
            CreateManagementButtons();

            LoadGame();
            UpdateTexts();
        }

        // Update is called once per frame
        private void Update()
        {
            var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            var timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
            var currentTime = timestamp;
            if (_lastTime + 1000 > currentTime)
            {
                return;
            }
            _lastTime = currentTime;
            _score.Mps = 0;
            _score.Zps = 0;
            _score.Aps = 0;
            foreach (var managementButton in _managementButtons)
            {
                var activated = managementButton.OnSecond();
                if (!activated)
                {
                    continue;
                }
                _score.Mps += managementButton.MoneyReward;
                _score.Zps += managementButton.ZombieReward;
                _score.Aps += managementButton.AudienceReward;
            }

            UpdateTexts();
            SaveGame();
        }


        private void UpdateTexts()
        {
            MoneyText.text = "$" + NumberFormatter.Format(_score.Money, true);
            MpsText.text = _score.Mps + "/sec";
            ZombieText.text = "Zombies: " + NumberFormatter.Format(_score.Zombie);
            ZpsText.text = _score.Zps + "/sec";
            AudienceText.text = "Audience: " + NumberFormatter.Format(_score.Audience);
            ApsText.text = _score.Aps + "/sec";
        }

        private void LoadGame()
        {
            decimal money;
            Decimal.TryParse(PlayerPrefs.GetString("money", "10"), out money);
            _score.Money = money;

            decimal zombie;
            Decimal.TryParse(PlayerPrefs.GetString("zombie", "0.0"), out zombie);
            _score.Zombie = zombie;

            decimal audience;
            Decimal.TryParse(PlayerPrefs.GetString("audience", "0.0"), out audience);
            _score.Audience = audience;

            decimal mps;
            Decimal.TryParse(PlayerPrefs.GetString("mps", "0.0"), out mps);
            _score.Mps = mps;

            decimal zps;
            Decimal.TryParse(PlayerPrefs.GetString("zps", "0.0"), out zps);
            _score.Zps = zps;

            decimal aps;
            Decimal.TryParse(PlayerPrefs.GetString("aps", "0.0"), out aps);
            _score.Aps = aps;

            foreach (var managementButton in _managementButtons)
            {
                var active = PlayerPrefs.GetInt("management" + managementButton.Id, 0);
                if (active > 0)
                {
                    managementButton.Activated = true;
                }
            }

            foreach (var funlandButton in _funlandButtons)
            {
                var active = PlayerPrefs.GetInt("funland" + funlandButton.Id, 0);
                if (active > 0)
                {
                    funlandButton.InProgress = true;
                    var startTime = Double.Parse(PlayerPrefs.GetString("funlandStart" + funlandButton.Id, "0"));
                    funlandButton.StartTime = startTime;
                }
            }
        }

        private void SaveGame()
        {
            PlayerPrefs.SetString("money", _score.Money.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetString("zombie", _score.Zombie.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetString("audience", _score.Audience.ToString(CultureInfo.InvariantCulture));

            PlayerPrefs.SetString("mps", _score.Aps.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetString("zps", _score.Zps.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.SetString("aps", _score.Aps.ToString(CultureInfo.InvariantCulture));

            foreach (var managementButton in _managementButtons)
            {
                if (managementButton.Activated)
                {
                    PlayerPrefs.SetInt("management" + managementButton.Id, 1);
                }
            }

            foreach (var funlandButton in _funlandButtons)
            {
                if (funlandButton.InProgress)
                {
                    PlayerPrefs.SetInt("funland" + funlandButton.Id, 1);
                    PlayerPrefs.SetString("funlandStart" + funlandButton.Id,
                        funlandButton.StartTime.ToString(CultureInfo.InvariantCulture));
                }
            }
        }

        private void CreateFunlandButtons()
        {
            _funlandButtons = new List<FunlandButtonComponent>();
            var funlandJson = Resources.Load("Data/Funland") as TextAsset;
            Debug.Assert(funlandJson != null, "funlandJson != null");
            var json = JSON.Parse(funlandJson.text);
            var i = 0;

            while (json["buttons"][i] != null)
            {
                var jsonButton = json["buttons"][i];
                var funlandButtonComponent = CreateFunlandButton();
                _funlandButtons.Add(funlandButtonComponent);
                funlandButtonComponent.Id = jsonButton["id"].AsInt;
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
                    decimal zombieCost;
                    Decimal.TryParse(jsonButton["zombieCost"].Value, out zombieCost);
                    funlandButtonComponent.ZombieCost = zombieCost;
                }

                if (jsonButton["audienceCost"] != null)
                {
                    decimal audienceCost;
                    Decimal.TryParse(jsonButton["audienceCost"].Value, out audienceCost);
                    funlandButtonComponent.AudienceCost = audienceCost;
                }

                if (jsonButton["moneyReward"] != null)
                {
                    decimal moneyReward;
                    Decimal.TryParse(jsonButton["moneyReward"].Value, out moneyReward);
                    funlandButtonComponent.MoneyReward = moneyReward;
                }

                if (jsonButton["zombieReward"] != null)
                {
                    decimal zombieReward;
                    Decimal.TryParse(jsonButton["zombieReward"].Value, out zombieReward);
                    funlandButtonComponent.ZombieReward = zombieReward;
                }

                if (jsonButton["audienceReward"] != null)
                {
                    decimal audienceReward;
                    Decimal.TryParse(jsonButton["audienceReward"].Value, out audienceReward);
                    funlandButtonComponent.AudienceReward = audienceReward;
                }

                i++;
            }
            var panel = GameObject.Find("/Canvas/Panel/TabPanel/FunlandPanel/CenterPanel/Mask/GridPanel");
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(
                panel.GetComponent<RectTransform>().sizeDelta.x,
                Math.Min((float) Math.Ceiling((i + 2)/2.0)*100 - 400, 400));
            var scrollbar = GameObject.Find("/Canvas/Panel/TabPanel/FunlandPanel/Scrollbar");
            scrollbar.GetComponent<Scrollbar>().value = 1;
        }

        private void CreateManagementButtons()
        {
            _managementButtons = new List<ManagementButtonComponent>();
            var managementJson = Resources.Load("Data/Management") as TextAsset;
            Debug.Assert(managementJson != null, "managementJson != null");
            var json = JSON.Parse(managementJson.text);
            var i = 0;

            while (json["buttons"][i] != null)
            {
                var jsonButton = json["buttons"][i];
                var managementButtonComponent = CreateManagementButton();
                _managementButtons.Add(managementButtonComponent);
                managementButtonComponent.Id = jsonButton["id"].AsInt;
                managementButtonComponent.Name = jsonButton["name"];
                managementButtonComponent.Description = jsonButton["description"];

                if (jsonButton["moneyCost"] != null)
                {
                    decimal moneyCost;
                    Decimal.TryParse(jsonButton["moneyCost"].Value, out moneyCost);
                    managementButtonComponent.MoneyCost = moneyCost;
                }

                if (jsonButton["zombieCost"] != null)
                {
                    decimal zombieCost;
                    Decimal.TryParse(jsonButton["zombieCost"].Value, out zombieCost);
                    managementButtonComponent.ZombieCost = zombieCost;
                }

                if (jsonButton["audienceCost"] != null)
                {
                    decimal audienceCost;
                    Decimal.TryParse(jsonButton["audienceCost"].Value, out audienceCost);
                    managementButtonComponent.AudienceCost = audienceCost;
                }

                if (jsonButton["moneyReward"] != null)
                {
                    decimal moneyReward;
                    Decimal.TryParse(jsonButton["moneyReward"].Value, out moneyReward);
                    managementButtonComponent.MoneyReward = moneyReward;
                }

                if (jsonButton["zombieReward"] != null)
                {
                    decimal zombieReward;
                    Decimal.TryParse(jsonButton["zombieReward"].Value, out zombieReward);
                    managementButtonComponent.ZombieReward = zombieReward;
                }

                if (jsonButton["audienceReward"] != null)
                {
                    decimal audienceReward;
                    Decimal.TryParse(jsonButton["audienceReward"].Value, out audienceReward);
                    managementButtonComponent.AudienceReward = audienceReward;
                }

                i++;
            }
            var panel = GameObject.Find("/Canvas/Panel/TabPanel/ManagementPanel/CenterPanel/Mask/GridPanel");
            panel.GetComponent<RectTransform>().sizeDelta = new Vector2(
                panel.GetComponent<RectTransform>().sizeDelta.x,
                Math.Min((float) Math.Ceiling((i + 2)/2.0)*100 - 400, 400));
            var scrollbar = GameObject.Find("/Canvas/Panel/TabPanel/ManagementPanel/Scrollbar");
            scrollbar.GetComponent<Scrollbar>().value = 1;
        }

        private static FunlandButtonComponent CreateFunlandButton()
        {
            var panel = GameObject.Find("/Canvas/Panel/TabPanel/FunlandPanel/CenterPanel/Mask/GridPanel");
            var buttonPanel = (GameObject) Instantiate(Resources.Load("Prefab/ButtonPanel"));
            buttonPanel.transform.SetParent(panel.transform, false);
            var funlandButtonComponent = buttonPanel.AddComponent<FunlandButtonComponent>();
            var button = buttonPanel.transform.FindChild("PanelButton").gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => funlandButtonComponent.OnButtonClick());
            return funlandButtonComponent;
        }

        private static ManagementButtonComponent CreateManagementButton()
        {
            var panel = GameObject.Find("/Canvas/Panel/TabPanel/ManagementPanel/CenterPanel/Mask/GridPanel");
            var buttonPanel = (GameObject) Instantiate(Resources.Load("Prefab/ButtonPanel"));
            buttonPanel.transform.SetParent(panel.transform, false);
            var managementButtonComponent = buttonPanel.AddComponent<ManagementButtonComponent>();
            var button = buttonPanel.transform.FindChild("PanelButton").gameObject.GetComponent<Button>();
            button.onClick.AddListener(() => managementButtonComponent.OnButtonClick());
            return managementButtonComponent;
        }
    }
}