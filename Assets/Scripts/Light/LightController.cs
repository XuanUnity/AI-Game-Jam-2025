using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private GameObject lightObject;
    [SerializeField] private float limitStart = 70f;     // góc bắt đầu
    [SerializeField] private float limitEnd = -70f;      // góc kết thúc

    [SerializeField] private float speedLight = 60f;
    [SerializeField] private TimeSlider timeSlider;

    private float rotationZ;
    private bool isRunning = false;
    private bool isTimeReversing = false;
    private bool isPause = false;

    private void OnEnable()
    {
        GameManagerInMap.Instance.InitLight(this);
    }

    public void StartLight()
    {
        rotationZ = limitStart;
        lightObject.transform.rotation = Quaternion.Euler(0, 0, rotationZ);

        isRunning = true;
        isPause = false;
        isTimeReversing = false;

        timeSlider.StartTimer();
    }

    private void Update()
    {
        if (isPause || !isRunning) return;

        HandleTimeReversalInput();
        RunLight();
    }

    private void RunLight()
    {
        float rotateSpeed = speedLight * Time.deltaTime;

        if (isTimeReversing)
        {
            // Quay ngược lại (về 70)
            rotationZ += rotateSpeed;
            DrainEnergy();

            if (rotationZ >= limitStart)
            {
                rotationZ = limitStart;
            }
        }
        else
        {
            // Quay xuôi (từ 70 -> -70)
            rotationZ -= rotateSpeed;

            if (rotationZ <= limitEnd)
            {
                rotationZ = limitEnd;
                StopLight();
            }
        }

        lightObject.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    private void StopLight()
    {
        isRunning = false;
        OnEnd();
    }

    // Hàm rỗng – bạn dùng sau này nếu muốn xử lý thêm
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
                isTimeReversing = true;
                timeSlider.TimeReversal(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
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
