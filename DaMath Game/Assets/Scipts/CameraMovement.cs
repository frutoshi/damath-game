using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private Touch initialTouch = new Touch();
    public Camera cam;

    private float rotY = 0f;
    private Vector3 originalRotation;

    public float rotationSpeed = 0.3f;
    public float direction = -1;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = cam.transform.eulerAngles;
        rotY = originalRotation.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase == TouchPhase.Began)
            {
                initialTouch = touch;

            }
            else if(touch.phase == TouchPhase.Moved)
            {
                //swiping
                float deltaY = initialTouch.position.y - touch.position.x;

                rotY += deltaY * Time.deltaTime * rotationSpeed * direction;

                //Limits the camera movement on y axis
                rotY = Mathf.Clamp(rotY, -20f, 20f);

                cam.transform.eulerAngles = new Vector3(55f, rotY, 0f);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                initialTouch = new Touch();
            }
        }
    }
}
