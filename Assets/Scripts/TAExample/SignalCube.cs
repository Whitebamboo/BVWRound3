using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalCube : MonoBehaviour
{

    Vector3 basePosition;

    float height = 0.0f;

    float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        basePosition = transform.position;
    }

    public void SetHeight( float newHeight )
    {
        height = newHeight;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, basePosition + Vector3.up * height * 0.5f, speed * Time.deltaTime);
    }
}
