using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoBehaviour
{
    static PortraitManager s_Instance;
    public static PortraitManager Instance => s_Instance;

    [SerializeField] Image portrait;

    void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(this);
            return;
        }

        s_Instance = this;
    }

    public void SetPortrait(Sprite image)
    {
        portrait.sprite = image;
    }
}
