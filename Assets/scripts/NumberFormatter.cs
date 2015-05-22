using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class NumberFormatter : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }

        public static string Format(decimal number, bool money = false)
        {
            string[] suffixes =
            {
                "K", "M", "B", "T", "Qa", "Qt", "Sx", "Sp" /*, "Oc", "Nn", "Dc", "UDc", "DDc", "TDc", "QaDc",
            "QtDc", "SxDc", "SpDc", "ODc", "NDc", "Vi", "UVi", "DVi", "TVi", "QaVi", "QtVi", "SxVi", "SpVi", "OcVi",
            "NnVi", "Tg", "UTg", "DTg", "TTg", "QaTg", "QtTg", "SxTg", "SpTg", "OcTg", "NnTg", "Qd", "UQd", "DQd", "TQd",
            "QaQd", "QtQd", "SxQd", "SpQd", "OcQd", "NnQd", "Qq", "UQq", "DQq", "TQq", "QaQq", "QtQq", "SxQq", "SpQq",
            "OcQq", "NnQq", "Sg"*/
            };

            for (var i = suffixes.Length - 1; i >= 0; i--)
            {
                if (Math.Abs(number) >= new decimal(Math.Pow(10, 3*i + 3)*0.99999))
                {
                    return i < 4
                        ? (number/new decimal(Math.Pow(10, 3*i + 3))).ToString("F2") + suffixes[i]
                        : (number/new decimal(Math.Pow(10, 3*i + 3))).ToString("F2") + " " + suffixes[i];
                    //spaces out first four suffixes
                }
            }

            return money ? number.ToString("F2") : number.ToString("F0");
        }
    }
}