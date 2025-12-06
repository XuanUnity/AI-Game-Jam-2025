using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [SerializeField] private GameObject Light;
    [SerializeField] private float limitRotationLeft;
    [SerializeField] private float limitRotationRight;

    [SerializeField] private float speedLight;

    private float rotationZ;
    private bool movingLeft = true;  // true = từ trái sang phải, false = từ phải sang trái
    private bool isTimeReversing = false;

    private void Start()
    {
        rotationZ = limitRotationLeft;
        Light.transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    private void Update()
    {
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
            isTimeReversing = true;
        }
        if(Input.GetKeyUp(KeyCode.F))
        {
            isTimeReversing = false;
        }
    }
}
