using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
public enum GestureType
{
    Undefined,
    ThumbsUp,
    ThumbsDown,
    MiddleFinger,
    MoveNext
}

[System.Serializable]
public struct Gesture
{
    public GestureType gestureType;
    public List<Vector3> fingerDatas;
    public UnityEvent onRecognized;
}

struct GestureWeight
{
    public GestureType gesture;
    public float weight;
}

public class GestureDectection : MonoBehaviour
{
    [Header("Gesture Recognize Settings")]
    [Range(0.001f, 100f)]
    public float threshold = .1f;
    [Range(1f, 10f)]
    public float recognizingTime = 1.0f;

    [Header("Status")]
    public GestureType matchedGesture = GestureType.Undefined;
    public Gesture currentGesture;

    [Header("Record Gesture")]
    public bool recording = false;
    public List<Gesture> gestures;

    public OVRSkeleton skeleton;
    private List<OVRBone> fingerBones;
    private Gesture previousGesture;

    // Variables related to recognizing gesture in a given time
    private bool recognizing = false;
    private float recognizeTimer = .0f;
    private GestureWeight[] GestureWeights;

    // Start is called before the first frame update
    void Start()
    {
        fingerBones = new List<OVRBone>(skeleton.Bones);
        previousGesture = new Gesture();
        currentGesture = new Gesture();
        InitiateGestureWeightsList();
    }

    // Update is called once per frame
    void Update()
    {
        if(fingerBones.Count == 0)
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
        }

        if(recording && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }

        currentGesture = Recognize();
        bool hasRecognized = !currentGesture.Equals(new Gesture());

        if (hasRecognized && !currentGesture.Equals(previousGesture))
        {
            previousGesture = currentGesture;
            //currentGesture.onRecognized.Invoke();
        }

        if (recording)
            recognizing = true;
        Recognizing();

    }

    public void Save()
    {
        Gesture g = new Gesture();
        g.gestureType = GestureType.Undefined;
        List<Vector3> data = new List<Vector3>();
        foreach (var bone in fingerBones)
        {
            //Finger pos relative to root
            data.Add(skeleton.transform.InverseTransformPoint(bone.Transform.position));
        }

        g.fingerDatas = data;
        gestures.Add(g);
    }

    Gesture Recognize()
    {
        Gesture currentGesture = new Gesture();
        float currentMin = Mathf.Infinity;

        foreach (var gesture in gestures)
        {
            float sumDistance = 0f;
            bool isDiscarded = false;
            for (int i = 0; i < fingerBones.Count; i++)
            {
                Vector3 currentData = skeleton.transform.InverseTransformPoint(fingerBones[i].Transform.position);
                float distance = Vector3.Distance(currentData, gesture.fingerDatas[i]);

                if (distance > threshold)
                {
                    isDiscarded = true;
                    break;
                }

                sumDistance += distance;
            }

            if (!isDiscarded && sumDistance < currentMin)
            {
                currentMin = sumDistance;
                currentGesture = gesture;
            }
        }

        return currentGesture;
    }

    public Gesture GetGesture()
    {
        return currentGesture;
    }

    public void BeginRecognize()
    {
        if(!recognizing)
        {
            recognizing = true;
            recognizeTimer = .0f;
            ClearGestureWeightsList();
        }
        else
        {
            //TODO
        }
    }

    public void StopRecognize()
    {
        recognizing = false;
        matchedGesture = GestureType.Undefined;
        recognizeTimer = .0f;
        ClearGestureWeightsList();
    }

    public void Recognizing()
    {
        if (recognizing)
        {
            Gesture curGesture = Recognize();

            int index = (int)curGesture.gestureType;
            GestureWeights[index].weight += Time.deltaTime;
            recognizeTimer += Time.deltaTime;
        }
        if (recognizeTimer >= recognizingTime)
        {
            matchedGesture = MatchGesture();

            if(matchedGesture != GestureType.Undefined)
                recognizing = false;

            ClearGestureWeightsList();
            recognizeTimer = .0f;
        }

    }

    public GestureType MatchGesture()
    {
        GestureType g = GestureType.Undefined;
        float maxMatch = .0f;
        
        foreach (var i in GestureWeights)
        {
            if (i.weight > maxMatch)
            {
                maxMatch = i.weight;
                g = i.gesture;
            }
        }
        
        return g;
    }

    private void InitiateGestureWeightsList()
    {
        GestureType t_gestureType = GestureType.Undefined;
        GestureWeights = new GestureWeight[Enum.GetNames(typeof(GestureType)).Length];
        foreach (int i in Enum.GetValues(typeof(GestureType)))
        {
            GestureWeight g = new GestureWeight();
            g.gesture = t_gestureType++;
            g.weight = .0f;
            GestureWeights[i] = g;
        }
    }

    private void ClearGestureWeightsList()
    {
        foreach (int i in Enum.GetValues(typeof(GestureType)))
        {
            GestureWeights[i].weight = .0f;
        }
    }
}
