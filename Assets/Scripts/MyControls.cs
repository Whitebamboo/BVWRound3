using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyControls : MonoBehaviour
{

    [SerializeField]
    SignalCube thumbCube;
    [SerializeField]
    SignalCube indexCube;
    [SerializeField]
    SignalCube middleCube;
    [SerializeField]
    SignalCube ringCube;
    [SerializeField]
    SignalCube pinkyCube;

    [SerializeField]
    OVRHand input;

    [SerializeField]
    Transform pointer;

    int spinningCubeLayer = 1 << 6;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (!input.IsSystemGestureInProgress)
        {
            //Pinching input for each finger
            if (input.GetFingerIsPinching(OVRHand.HandFinger.Thumb))
                thumbCube.SetHeight(input.GetFingerPinchStrength(OVRHand.HandFinger.Thumb));
            else
                thumbCube.SetHeight(0);
            if (input.GetFingerIsPinching(OVRHand.HandFinger.Index))
                indexCube.SetHeight(input.GetFingerPinchStrength(OVRHand.HandFinger.Index));
            else
                indexCube.SetHeight(0);
            if (input.GetFingerIsPinching(OVRHand.HandFinger.Middle))
                middleCube.SetHeight(input.GetFingerPinchStrength(OVRHand.HandFinger.Middle));
            else
                middleCube.SetHeight(0);
            if (input.GetFingerIsPinching(OVRHand.HandFinger.Ring))
                ringCube.SetHeight(input.GetFingerPinchStrength(OVRHand.HandFinger.Ring));
            else
                ringCube.SetHeight(0);
            if (input.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
                pinkyCube.SetHeight(input.GetFingerPinchStrength(OVRHand.HandFinger.Pinky));
            else
                pinkyCube.SetHeight(0);

            //Raycasting using pointer vector
            if (input.IsPointerPoseValid)
            {
                pointer.rotation = input.PointerPose.rotation;
                pointer.position = input.PointerPose.position;
            }


            RaycastHit hit;
            if( Physics.Raycast(input.PointerPose.position, input.PointerPose.forward, out hit, spinningCubeLayer) )
            {
                SpinningCube S = hit.collider.GetComponent<SpinningCube>();
                if( S )
                {
                    S.Spin();
                }
            }

        }

    }
}
