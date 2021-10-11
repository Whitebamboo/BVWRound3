using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneUI : MonoBehaviour
{
    
    public static EndSceneUI instance;

    private static GameObject ScoreTxt;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        ScoreTxt = transform.Find("Score").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void ShowEndSceneUI()
    {
        instance.gameObject.SetActive(true);
        ScoreTxt.GetComponent<TMPro.TextMeshProUGUI>().text = GameManager.Reputation.ToString();
        
        //+ " / 100";
    }
}
