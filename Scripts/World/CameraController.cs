using UnityEngine;

public class CameraController : MonoBehaviour 
{
    private bool doMovement = true;
    public float panSpeed = 30f;
    public float panBorderThiccness = 10f;

    public int yawsSpeed;
    public float scrollSpeed = 5f;
    public float minY = 10f;
    public float maxY = 50f;
    public float minZ = 10f;
    public float maxZ = 50f;
    public float minX = 10f;
    public float maxX = 50f;

    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     doMovement = !doMovement;
        // }

        if(!doMovement)
        {
            return;
        }

        if(Input.GetKey("w") || Input.GetMouseButton(0) && Input.mousePosition.y <= panBorderThiccness - Input.mousePosition.y)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if(Input.GetKey("s") || Input.GetMouseButton(0) && Input.mousePosition.y >= Screen.height - panBorderThiccness)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if(Input.GetKey("d") || Input.GetMouseButton(0) && Input.mousePosition.x >= Screen.width - panBorderThiccness)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if(Input.GetKey("a") || Input.GetMouseButton(0) && Input.mousePosition.x <= panBorderThiccness)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        Vector3 pos = transform.position;

        pos.y -= scroll * yawsSpeed * scrollSpeed * Time.deltaTime;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }

    // private Vector2 startTouchPosition;
    // private Vector2 currentPosition;
    // private Vector2 endTouchPosition;
    // private bool stopTouch = false;

    // public float swipeRange;
    // public float tapRange;

    // public void Swipe()
    // {
    //     if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //     {
    //         startTouchPosition = Input.GetTouch(0).position;
    //     }

    //     if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
    //     {
    //         currentPosition = Input.GetTouch(0).position;
    //         Vector2 Distance = currentPosition - startTouchPosition;

    //         if(!stopTouch)
    //         {
    //             if(Input.GetKey("a") || Distance.x < -swipeRange)
    //             {   //  Left
    //                 Debug.Log("Left");
    //                 transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
    //                 stopTouch = true;
    //             }
    //             else if(Input.GetKey("d") || Distance.x > swipeRange)
    //             {   //  Right
    //                 Debug.Log("Right");
    //                 transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
    //                 stopTouch = true;
    //             }
    //             else if(Input.GetKey("w") || Distance.y > swipeRange)
    //             {   //  Up
    //                 Debug.Log("Up");
    //                 transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
    //                 stopTouch = true;
    //             }
    //             else if(Input.GetKey("s") || Distance.y < -swipeRange)
    //             {   //  Down
    //                 Debug.Log("Down");
    //                 transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
    //                 stopTouch = true;
    //             }
    //         }
    //     }

    //     if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
    //     {
    //         stopTouch = false;

    //         endTouchPosition = Input.GetTouch(0).position;

    //         Vector2 Distance = endTouchPosition - startTouchPosition;

    //         if(Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange)
    //         {
    //             // tap
    //             Debug.Log("tap");
    //         }
    //     }
    // }
}