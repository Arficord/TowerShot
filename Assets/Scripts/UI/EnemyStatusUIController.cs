using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EnemyStatusUIController : MonoBehaviour
{
    public enum HEALTHBAR_ANIMATION
    {
        NONE,
        SHAKE
    }
    public GameObject damageTextPrefab;


    private RectTransform healtCurrentBar;
    private float healtBarFullLenght;

    private IEnumerator shakingHealthBar;


    private void Start()
    {
        healtCurrentBar = transform.GetChild(0).GetChild(1).GetComponent<RectTransform>(); //HealthCurrent;
        healtBarFullLenght = healtCurrentBar.sizeDelta.x;
    }
    public void resizeHealth(float percent, HEALTHBAR_ANIMATION animationType){
        healtCurrentBar.sizeDelta = new Vector2(healtBarFullLenght * percent, healtCurrentBar.sizeDelta.y);
        
        switch(animationType)
        {
            case HEALTHBAR_ANIMATION.NONE:
                break;
            case HEALTHBAR_ANIMATION.SHAKE:
                shakeHealthBar:
                break;
        }
    }
    public void showDamage(float damage){
        if(GameData.options.damageText == false)
        {
            return;
        }
        GameObject textObject = Instantiate(damageTextPrefab, transform);
        textObject.GetComponent<Text>().text = damage.ToString();

        StartCoroutine(textAnimation(textObject));
    }
    private IEnumerator shakeHealthBarAnimation(){
        Transform shaked =  transform.GetChild(0);
        shaked.localPosition = Vector3.zero;

        float amplitude = 0.2f;
        for (int i = 0; i<10; i++)
        {
            if(i%2==0)
            {
                shaked.localPosition = new Vector3(amplitude, 0, 0);
            }
            else
            {
                shaked.localPosition = new Vector3( -amplitude, 0, 0);
            }
            amplitude *= 0.7f;
            yield return new WaitForSeconds(0.01f);
        }
        shaked.localPosition = Vector3.zero;
    }
    private IEnumerator textAnimation(GameObject textObject){
        for(int i=0;i<100;i++){
            yield return new WaitForSeconds(0.01f);
            textObject.transform.localPosition += new Vector3(0, 0.01f, 0);

            Color textColor = textObject.GetComponent<Text>().color;
            textColor.a -= 0.01f;
            textObject.GetComponent<Text>().color= textColor;
        }
        Destroy(textObject);
    }
    private void shakeHealthBar(){
        if(shakingHealthBar != null)
        {
            StopCoroutine(shakingHealthBar);
        }
        shakingHealthBar = shakeHealthBarAnimation();
        StartCoroutine(shakingHealthBar);
    }

}
