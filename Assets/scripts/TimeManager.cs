using UnityEngine;

namespace Assets.scripts
{
    public class TimeManager : MonoBehaviour {

        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }

        public static double GetMilliseconds()
        {
            var epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
            var timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
            return timestamp;
        }
    }
}
