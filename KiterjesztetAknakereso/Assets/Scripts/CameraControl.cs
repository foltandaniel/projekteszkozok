using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	Camera cam;
    public static CameraControl singleton;
    



    private Vector3 startMousePos; //ahol lenyomtuk az egeret
    public float distanceThreshold = 1f; // legalább ennyit kell elmozdulnia a kurzornak, hogy ne számítson kattintásnak
    private float dragDistance; //mennyit mozgott a kurzor?
	private float touchTime; //mikor nyomtuk le?
	private float touchTimeThreshold =0.5f;
	private bool ignoreMouseUp;
	private Coroutine holdDownCoroutine;

	#region camera bounds
	float minX;
	float maxX;
	float minY;
	float maxY;
	private float mapSize;
	#endregion
   
	#region zoom
	private float origCamScale;
    private float minZoom;
	public float zoomSpeed = 0.5f;
    #endregion







    void Awake()
    {
        singleton = this;
        cam = GetComponent<Camera>();
    }

	IEnumerator HoldDownTimer() {
		
		yield return new WaitForSeconds (touchTimeThreshold);
		if (dragDistance < distanceThreshold) {
			ignoreMouseUp = true;
			Click (true);
		}
	}
    // Update is called once per frame
    void Update() {
		if (!GameManager.PLAYING)
			return;
        //DRAG
        if (Input.GetMouseButtonDown(0)) { //ha nyomjuk a bal egér gombot (vagy touch0)
			
			holdDownCoroutine = StartCoroutine (HoldDownTimer());
            dragDistance = 0f;
            startMousePos = cam.ScreenToWorldPoint(Input.mousePosition); //hol nyomtuk le az egeret?
            startMousePos.z = 0.0f;
        }

        if (Input.GetMouseButton(0)) { //ha nyomva TARTJUK az egeret



            Vector3 nowMousePos = cam.ScreenToWorldPoint(Input.mousePosition); //hol van most a kurzor? (World pozíció)
            nowMousePos.z = 0.0f;
            Vector3 newPos;
            dragDistance += (startMousePos - nowMousePos).magnitude; //mennyit ment eddig a kurzor?
            newPos = transform.position + startMousePos - nowMousePos; //a kamerát mozgatjuk (azon van ez a script)

			//RefreshBounds ();




           newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
           newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            transform.position = newPos;
        }


        if (Input.GetMouseButtonUp(0)) { //felengedtük a gombot
			if(ignoreMouseUp) {
				ignoreMouseUp = false;
				return;
			}
			StopCoroutine (holdDownCoroutine);
			if (dragDistance < distanceThreshold) { //ha kevesebbet ment a kurzor, mint a threshold.
				float deltaTime = Time.timeSinceLevelLoad - touchTime;

					Click (false);
				 

			}
        }
    }

	private void Click(bool _long)
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition),cam.transform.forward);

        if(hit)
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
				if (_long) {
					hit.collider.gameObject.GetComponent<Field> ().FlagMe ();
				} else {
					hit.collider.gameObject.GetComponent<Field> ().ClickedMe (false);
				}
            }
        }


    }
	private void RefreshBounds(){

		var vertExtent = cam.orthographicSize;
		var horzExtent = vertExtent * Screen.width / Screen.height;

		// Calculations assume map is position at the origin
		minX = horzExtent - (mapSize+5f) / 2.0f;
		maxX = (mapSize + 5f) / 2.0f - horzExtent;
		minY = vertExtent - (mapSize + 30f) / 2.0f;
		maxY = (mapSize + 30f) / 2.0f - vertExtent;
	}



    public void Zoom(bool inorout) //true = nagyítás
    {
        
        if(inorout)
        {
            if( cam.orthographicSize > 4) cam.orthographicSize -= zoomSpeed;
           
        } else
        {
           if(cam.orthographicSize < minZoom) cam.orthographicSize += zoomSpeed;
        }
		RefreshBounds ();
    }

    public void AlignCamera(int n)
    {
		
        mapSize = n;
        cam.orthographicSize =n;
       // cam.transform.position = new Vector3(n / 2f, n / 2f, -10);
        origCamScale = n;
        minZoom =n;
		RefreshBounds ();
    }
}
