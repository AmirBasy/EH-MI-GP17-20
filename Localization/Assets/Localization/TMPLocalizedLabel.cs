using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class TMPLocalizedLabel : MonoBehaviour
{
    public string key;
 
    // Start is called before the first frame update
    void OnEnable()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        if (text!=null)
        {
            text.text = key.Localized();
        }
    }


}
