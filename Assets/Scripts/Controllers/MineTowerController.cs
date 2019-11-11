using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTowerController : MonoBehaviour {
    public int rotateAngle = 70;
    public float shootSpeed = 1;
    private float shootProgress = 0;

    private ShellFactory [] shellFactory;
    private Transform towerHead;

    private void Start()
    {
        towerHead = transform.GetChild(0);

        shellFactory = new ShellFactory[towerHead.childCount];
        for(int i=0; i<shellFactory.Length;i++){
            shellFactory[i] = towerHead.GetChild(i).GetComponent<ShellFactory>() ;
        }

        StartCoroutine(DoReload());
    }
    private IEnumerator DoReload()
    {
        while(true){
            if (shootProgress < 1)
            {
                shootProgress += Time.fixedDeltaTime * shootSpeed;
            }
            yield return new WaitForFixedUpdate();
        }
    }
    public void rotateTower(float rotation)
    {
        if(towerHead.transform.eulerAngles.y <= rotateAngle || towerHead.transform.eulerAngles.y >= 360- rotateAngle)
        {
            //Это ужасно и должно фризиться в центре, но пока работает
            if(towerHead.eulerAngles.y<= rotateAngle)
                towerHead.eulerAngles = new Vector3(0, Mathf.Clamp(towerHead.eulerAngles.y + rotation, -rotateAngle, rotateAngle), 0);
            else{
                towerHead.eulerAngles = new Vector3(0, Mathf.Clamp(towerHead.eulerAngles.y + rotation, 360- rotateAngle, 360), 0);
            }
        }
    }

    public bool rotateTower(Quaternion rotation)
    {
        //THE BEST transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -Mathf.Clamp(rotation, -rotateAngle, rotateAngle), transform.localEulerAngles.z); 
        if (rotation.eulerAngles.y<rotateAngle||rotation.eulerAngles.y>360-rotateAngle)
        {
            towerHead.transform.rotation = rotation;
            return true;
        }
        else{
            return false;
        }
    }   //old
    private Vector3 getTubeRotaiting(){
        return towerHead.eulerAngles;
    }
    public void shoot(){
        if (shootProgress >= 1)
        {
            for (int i = 0; i < shellFactory.Length; i++)
            {
                shellFactory[i].createShell(ShellFactory.shellType.sample);//TODO;
            }
            shootProgress = 0;
        }
        else
        {
            // Shoot delay!;
        }
    }
}
