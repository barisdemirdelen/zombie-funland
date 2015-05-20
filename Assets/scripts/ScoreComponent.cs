using UnityEngine;

namespace Assets.scripts
{
    public class ScoreComponent : MonoBehaviour {
        public decimal Money { get; set; }
        public decimal Zombie { get; set; }
        public decimal Audience { get; set; }

        public decimal TicketPrice { get; set; }

        public decimal Mps { get; set; }
        public decimal Zps { get; set; }
        public decimal Aps { get; set; }

        // Use this for initialization
        void Start () {
            Money = 100m;
            Zombie = 0m;
            Audience = 4m;
            TicketPrice = 3m;
            Mps = 0;
            Zps = 0;
            Aps = 0;
        }
	
        // Update is called once per frame
        void Update () {
	
        }
    }
}
