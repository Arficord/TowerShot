using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceenController : MonoBehaviour
{
    private enum AnimationEnu
    {
        APPEARENCE,
        DEPARTURE,
        LEAVE,
    }
    public GameObject camera;
    public GameObject battleTrain;

    public AnimationCurve appearenceCurve;

    public void playTrainAppearence()
    {
        StartCoroutine(startAppearanceAnimation());
    }
    private IEnumerator startAppearanceAnimation()
    {
        for (int i = 0; i<250; i++)
        {
            battleTrain.transform.position = getRideTrainPosition((float)i/250);
            camera.transform.position = getAppearenceCameraPosition((float)i / 250);
            camera.transform.rotation = getAppearenceCameraRotation((float)i/250);
            yield return new WaitForSeconds(0.01f);
        }
        OnAnimationEnded(AnimationEnu.APPEARENCE);
    }
    public void playTrainDeparture()
    {
        StartCoroutine(startDepartureAnimation());
    } 
    private IEnumerator startDepartureAnimation()
    {
        for (int i = 0; i < 250; i++)
        {
            battleTrain.transform.position = getRideTrainPosition((float)i / 250);
            camera.transform.position = getRideCameraPosition((float)i / 250);
            yield return new WaitForSeconds(0.01f);
        }
        OnAnimationEnded(AnimationEnu.DEPARTURE);
    }
    public void playTrainLeave()
    {
        StartCoroutine(startLeaveAnimation());

    }
    private IEnumerator startLeaveAnimation()
    {
        for (int i = 0; i < 50; i++)
        {
            battleTrain.transform.position = Vector3.Lerp(trainShootPosition, trainLeavePosition, (float)i / 50);
            camera.transform.position = getRideCameraPosition((float)i / 250);
            yield return new WaitForSeconds(0.01f);
        }
        OnAnimationEnded(AnimationEnu.LEAVE);
    }
    private void OnAnimationEnded(AnimationEnu anim)
    {
        switch(anim)
        {
            case AnimationEnu.APPEARENCE:
                EventController.eventController.onEndDepartureAnimation();
                break;
            case AnimationEnu.DEPARTURE:
                EventController.eventController.onEndDepartureAnimation();
                break;
            case AnimationEnu.LEAVE:
                EventController.eventController.onEndLeaveAnimation();
                break;
        }
    }


    Vector3 trainStartPosition = new Vector3(0, 0, -170);
    Vector3 trainStopPosition = new Vector3(0, 0, -20);
    Vector3 trainShootPosition = new Vector3(0, 0, -3);
    Vector3 trainLeavePosition = new Vector3(0, 0, 80);

    private Vector3 getRideTrainPosition(float time)
    {

        float zAnswer = Mathf.Lerp(trainStartPosition.z, trainShootPosition.z, appearenceCurve.Evaluate(time));
        return new Vector3(0, 0, zAnswer);
    }

    Vector3 cameraShootPosition = new Vector3(0, 20, 0);
    Vector3 cameraMenuPosition = new Vector3(0, 40, -100);
    Vector3 cameraShootRotation = new Vector3(50, 0, 0);
    Vector3 cameraMenuRotation = new Vector3(90, 0, 0);

    private Vector3 getRideCameraPosition(float time)
    {
        return battleTrain.transform.position + cameraShootPosition;
    }
    private Vector3 getAppearenceCameraPosition(float time)
    {
        float connectTicks = 0.2f;
        float followTicks = 0.4f;

        if (time < connectTicks)
        {
            return cameraMenuPosition;
        }
        else
        {
            if (time >= connectTicks && time < followTicks)
            {
                float t = (time - connectTicks) / (followTicks - connectTicks);
                return Vector3.Lerp(camera.transform.position, getRideCameraPosition(time), t);
            }
            else
            {
                return getRideCameraPosition(time);
            }
        }
    }
    private Quaternion getAppearenceCameraRotation(float time)
    {
        float followTicks = 0.3f;
        float connectTicks = 0.2f;
        if (connectTicks<=time && time <= followTicks)
        {
            return Quaternion.Euler(Vector3.Lerp(cameraMenuRotation, cameraShootRotation, (time - connectTicks) / (followTicks - connectTicks)));
        }
        else
        {
            return camera.transform.rotation;
        }
    }
}

