using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellFactory : MonoBehaviour {
    public enum shellType{
        sample
    }

    public GameObject[] shells;
    private MineTowerController tower;

    private void Start()
    {
        tower = GetComponent<MineTowerController>();
    }
    public ShellController createShell(shellType type){
        GameObject result = Instantiate(shells[(int)type]);

        correctTransform(result);
        configureShell(result.GetComponent<ShellController>());

        return result.GetComponent<ShellController>();
    }
    private void correctTransform(GameObject obj){
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
    }
    private void configureShell(ShellController shell){
        shell.stats = GameData.shellBaseStats;
    }
    private void Update()
    {
        if (false && Input.GetKeyDown(KeyCode.Space))
        {
            //createShell(shellType.sample);
        }
    }
}
