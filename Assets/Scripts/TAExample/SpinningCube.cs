using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningCube : MonoBehaviour
{

    float spinningTime = 0.0f;

    float spinningRate = 0;
    float spinningAcceleration = 50.0f;
    float maxSpinningRate = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Spin()
    {
        spinningTime = 0.25f;
    }

    public void Update()
    {
        if (spinningTime > 0)
        {
            spinningRate = Mathf.Min(spinningRate + spinningAcceleration * Time.deltaTime, maxSpinningRate);
        }
        else
            spinningRate = Mathf.Max(spinningRate - spinningAcceleration * Time.deltaTime, 0.0f);

        transform.rotation *= Quaternion.Euler(0, spinningRate * Time.deltaTime, 0);

        spinningTime = Mathf.Max(0.0f, spinningTime - Time.deltaTime);
    }
}
