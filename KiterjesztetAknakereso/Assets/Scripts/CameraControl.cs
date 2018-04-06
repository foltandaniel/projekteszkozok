using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public static CameraControl singleton;
    public float zoomSpeed = 0.5f;



    private Vector3 startMousePos;
    public float distanceThreshold = 1f; // legalább ennyit kell elmozdulnia a kurzornak, hogy ne számítson kattintásnak
    private float dragDistance; //mennyit mozgott a kurzor?



    Camera cam;
    private float origCamScale;
    private float minZoom;
    private float mapSize;







    void Awake()
    {
        singleton = this;
        cam = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update() {

        //DRAG
        if (Input.GetMouseButtonDown(0)) { //ha nyomjuk a bal egér gombot (vagy touch0)
            dragDistance = 0f;
            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //hol nyomtuk le az egeret?
            startMousePos.z = 0.0f;
        }

        if (Input.GetMouseButton(0)) { //ha nyomva TARTJUK az egeret
            Vector3 nowMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //hol van most a kurzor? (World pozíció)
            nowMousePos.z = 0.0f;
            Vector3 newPos;
            dragDistance += (startMousePos - nowMousePos).magnitude; //mennyit ment eddig a kurzor?
            newPos = transform.position + startMousePos - nowMousePos; //a kamerát mozgatjuk (azon van ez a script)





            var vertExtent = cam.orthographicSize;
            var horzExtent = vertExtent * Screen.width / Screen.height;

            // Calculations assume map is position at the origin
            var minX = horzExtent - (mapSize+5f) / 2.0f;
            var maxX = (mapSize + 5f) / 2.0f - horzExtent;
            var minY = vertExtent - (mapSize + 30f) / 2.0f;
            var maxY = (mapSize + 30f) / 2.0f - vertExtent;

            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            transform.position = newPos;
        }


        if (Input.GetMouseButtonUp(0)) { //felengedtük a gombot

            if (dragDistance < distanceThreshold) { //ha kevesebbet ment a kurzor, mint a threshold.
                Debug.Log("CLICK"); //klikk
            }

        }
    }


    public void Zoom(bool inorout) //true = nagyítás
    {
        if(inorout)
        {
            cam.orthographicSize -= zoomSpeed;
           
        } else
        {
           if(cam.orthographicSize < minZoom) cam.orthographicSize += zoomSpeed;
        }
    }

    public void AlignCamera(int n)
    {
        mapSize = n;
        cam.orthographicSize =n;
       // cam.transform.position = new Vector3(n / 2f, n / 2f, -10);
        origCamScale = n;
        minZoom =n;
    }
}
