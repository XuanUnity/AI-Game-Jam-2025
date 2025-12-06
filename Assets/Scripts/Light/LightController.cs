using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private GameObject Light;
    [SerializeField] private float limitRotationLeft;
    [SerializeField] private float limitRotationRight;

    [SerializeField] private float speedLight;
    [SerializeField] private TimeSlider timeSlider;

    private float rotationZ;
    private bool movingLeft = true;  // true = từ trái sang phải, false = từ phải sang trái
    private bool isTimeReversing = false;
    private bool isPause = false;

    private void OnEnable()
    {
        GameManagerInMap.Instance.InitLight(this);
    }
    public void SetActionLight(bool pause)
    {
        isPause = pause;
        if(!pause)
        {
            isTimeReversing = false;
            timeSlider.ContinueTime();
            timeSlider.TimeReversal(false);
        }
        else
        {
            timeSlider.PauseTime();
        }
    }

    public void StartLight()
    {
        rotationZ = limitRotationLeft;
        Light.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
        SetActionLight(false);
        isTimeReversing = false;
        timeSlider.StartTimer();
    }

    private void Update()
    {
        if (isPause)
        {
            return;
        }

        RunLight();
        TimeReversal();
    }

    public void RunLight()
    {
        if (movingLeft)
        {
            if(isTimeReversing)
            {
                rotationZ += speedLight * Time.deltaTime;
                PlayerController.Instance.Energy -= 1;
                PlayerController.Instance.SetEnergyBar();
                timeSlider.TimeReversal(true);
                if(PlayerController.Instance.Energy < 0)
                {
                    isTimeReversing = false;
                    timeSlider.TimeReversal(false);
                }
            } else
                rotationZ -= speedLight * Time.deltaTime;

            if (rotationZ <= limitRotationRight)
            {
                rotationZ = limitRotationRight;
                movingLeft = false;  // đổi hướng
            }
        }
        else
        {
            if (isTimeReversing)
            {
                rotationZ += speedLight * Time.deltaTime;
                PlayerController.Instance.Energy -= 1;
                PlayerController.Instance.SetEnergyBar();
                timeSlider.TimeReversal(true);
                if (PlayerController.Instance.Energy < 0)
                {
                    isTimeReversing = false;
                    timeSlider.TimeReversal(false);
                }
            }
            else
                rotationZ -= speedLight * Time.deltaTime;

            if (rotationZ >= limitRotationLeft)
            {
                rotationZ = limitRotationLeft;
                movingLeft = true;   // đổi hướng
            }
        }

        Light.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    public void TimeReversal()
    {

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(PlayerController.Instance.Energy >= 0.1f)
            {
                isTimeReversing = true;
            }
        }
        if(Input.GetKeyUp(KeyCode.F))
        {
            isTimeReversing = false;
            timeSlider.TimeReversal(false);
        }
    }
}
