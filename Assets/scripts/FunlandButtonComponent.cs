﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{
    public class FunlandButtonComponent : MonoBehaviour
    {
        public Button PanelButton { get; private set; }
        public Text PanelText { get; private set; }
        public Image ProgressBar { get; private set; }

        public decimal MoneyCost { get; set; }
        public decimal ZombieCost { get; set; }
        public decimal AudienceCost { get; set; }

        public decimal MoneyReward { get; set; }
        public decimal ZombieReward { get; set; }
        public decimal AudienceReward { get; set; }

        private float _duration = 3.0f;

        private ScoreComponent _score;
        private bool _inProgress;
        private float _startTime;

        // Use this for initialization
        private void Start()
        {
            PanelButton = transform.FindChild("PanelButton").gameObject.GetComponent<Button>();
            PanelText = transform.FindChild("DescriptionText").gameObject.GetComponent<Text>();
            ProgressBar = transform.FindChild("ProgressBar").gameObject.GetComponent<Image>();

            _score = GameObject.Find("/Main").GetComponent<ScoreComponent>();

            _inProgress = false;
            PanelButton.interactable = true;
            ProgressBar.fillAmount = 0.0f;
        }

        // Update is called once per frame
        private void Update()
        {
            PanelButton.interactable = IsAvailable();

            if (!_inProgress)
            {
                return;
            }
            if (Time.time - _startTime >= _duration)
            {
                ProgressBar.fillAmount = 0.0f;
                _inProgress = false;
                PanelButton.interactable = true;
                GiveRewards();
            }
            else
            {
                ProgressBar.fillAmount = Math.Min((Time.time - _startTime)/_duration, 1.0f);
            }
        }

        private void GiveRewards()
        {
            _score.Money += MoneyReward;
            _score.Zombie += ZombieReward;
            _score.Audience += AudienceReward;
        }

        public void OnButtonClick()
        {
            if (!IsAvailable())
            {
                return;
            }
            _inProgress = true;
            PanelButton.interactable = false;
            _startTime = Time.time;
            _score.Money -= MoneyCost;
            _score.Zombie -= ZombieCost;
            _score.Audience -= AudienceCost;
        }

        private bool IsAvailable()
        {
            return _score.Money >= MoneyCost && _score.Zombie >= ZombieCost && _score.Audience >= AudienceCost && !_inProgress;
        }

        public float Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
    }
}