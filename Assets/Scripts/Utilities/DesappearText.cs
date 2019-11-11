using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesappearText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(desappear());
    }

    private IEnumerator desappear()
    {
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.localPosition += new Vector3(0, 0.01f, 0);

            Color textColor = gameObject.GetComponent<Text>().color;
            textColor.a -= 0.01f;
            gameObject.GetComponent<Text>().color = textColor;
        }
        Destroy(gameObject);
    }

    void OnDisable()
    {
        Destroy(gameObject);
    }
}
