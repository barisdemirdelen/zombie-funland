﻿using UnityEngine;

namespace Assets.scripts
{
    public class ScoreComponent : MonoBehaviour
    {
        public decimal Money { get; set; }
        public decimal Zombie { get; set; }
        public decimal Audience { get; set; }

        public decimal Mps { get; set; }
        public decimal Zps { get; set; }
        public decimal Aps { get; set; }

        // Use this for initialization
        private void Start()
        {
            
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}