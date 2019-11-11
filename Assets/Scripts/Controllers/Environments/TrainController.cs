using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainController : MonoBehaviour, IDestroyable
{
    public GameObject wagon;
    public GameObject connector;
    public static int count=0;
    void Start()
    {
        count++;
        spawnWagons();
    }
    private void Update()
    {
        transform.localPosition += new Vector3(0, 0, 100 * Time.deltaTime);
    }
    private void spawnWagons()
    {

        int wagonsAmount = Random.Range(3, 8);
        for (int i=0; i < wagonsAmount; i++)
        {
            Transform tempConnector = Instantiate(connector).transform;
            tempConnector.SetParent(transform);
            tempConnector.localPosition = new Vector3(0, 0 + i * 1.9f, 0);
            Transform tempWagon = Instantiate(wagon).transform;
            tempWagon.SetParent(transform);
            tempWagon.localPosition = new Vector3(0, 0 + (i + 1) * 1.9f, 0);
        }
    }
    public void kill()
    {
        Debug.Log("NOW COUNT IS " + count);
        count--;
        Destroy(gameObject);
    }
    //Vector3.Lerp(transform.position, destination, 0.5f * Time.deltaTime);s
}
