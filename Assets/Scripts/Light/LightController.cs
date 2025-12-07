using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private GameObject lightObject;

    // Giới hạn vị trí X (thay cho góc quay)
    [SerializeField] private float limitLeftX = -5f;
    [SerializeField] private float limitRightX = 5f;

    [SerializeField] private float dynamicSpeed;
    [SerializeField] private TimeSlider timeSlider;

    private float durateTime;
    private float currentPosX;

    private bool isRunning = false;
    private bool isTimeReversing = false;
    private bool isPause = false;

    private void OnEnable()
    {
        GameManagerInMap.Instance.InitLight(this);
    }

    public void StartLight(float time)
    {
        // Bắt đầu từ phía LEFT
        currentPosX = limitLeftX;
        SetLightPosition(currentPosX);

        durateTime = time;

        float distance = Mathf.Abs(limitRightX - limitLeftX);
        dynamicSpeed = distance / durateTime; // đơn vị / giây

        isRunning = true;
        isPause = false;
        isTimeReversing = false;

        timeSlider.StartTimer(time);
    }

    private void Update()
    {
        if (isPause || !isRunning) return;

        HandleTimeReversalInput();
        RunLight();
    }

    private void RunLight()
    {
        float moveStep = dynamicSpeed * Time.deltaTime;

        if (isTimeReversing)
        {
            // Di chuyển ngược: RIGHT → LEFT
            currentPosX -= moveStep * 2;
            DrainEnergy();

            if (currentPosX <= limitLeftX)
            {
                currentPosX = limitLeftX;
            }
        }
        else
        {
            // Di chuyển xuôi: LEFT → RIGHT
            currentPosX += moveStep;

            if (currentPosX >= limitRightX)
            {
                currentPosX = limitRightX;
                StopLight();
            }
        }

        SetLightPosition(currentPosX);
    }

    private void SetLightPosition(float x)
    {
        Vector3 pos = lightObject.transform.position;
        pos.x = x;
        lightObject.transform.position = pos;
    }

    private void StopLight()
    {
        isRunning = false;
        OnEnd();
    }

    private void OnEnd()
    {
        GameManagerInMap.Instance.LoseGame();
    }

    private void HandleTimeReversalInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (PlayerController.Instance.Energy > 0)
            {
                EffectTimeManager.Instance.OnUseSkill();
                PlayerController.Instance.SetSkill(true);
                isTimeReversing = true;
                timeSlider.TimeReversal(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            PlayerController.Instance.SetSkill(false);
            isTimeReversing = false;
            timeSlider.TimeReversal(false);
        }
    }

    private void DrainEnergy()
    {
        PlayerController.Instance.Energy -= Time.deltaTime * 10f;
        PlayerController.Instance.SetEnergyBar();

        if (PlayerController.Instance.Energy <= 0)
        {
            isTimeReversing = false;
            timeSlider.TimeReversal(false);
        }
    }

    public void SetActionLight(bool pause)
    {
        isPause = pause;

        if (pause)
        {
            timeSlider.PauseTime();
        }
        else
        {
            timeSlider.ContinueTime();
        }
    }
}
