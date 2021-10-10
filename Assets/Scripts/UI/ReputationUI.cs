using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationUI : MonoBehaviour
{
    public int maxReputation = 100;
    //GameManager manager;
    UnityEngine.UI.Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        //manager = FindObjectOfType<GameManager>();
        slider = GetComponentInChildren<UnityEngine.UI.Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = (float)GameManager.Reputation / (float)maxReputation;
    }
}
