using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace AISandbox {
    public class UIScript : MonoBehaviour
    {
        Text stateText;
        Pathfollowing follower;

        void Start()
        {
            stateText = GetComponent<Text>();
            //follower = GameObject.Find("TreasureSeeker").GetComponent<Pathfollowing>();
        }

        void Update()
        {
            stateText.text = GameObject.Find("TreasureSeeker").GetComponent<Pathfollowing>().currentState;
        }
    }
}
