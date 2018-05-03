using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

	Camera cam;
    public static CameraControl singleton;
    



    private Vector3 startMousePos; //ahol lenyomtuk az egeret
    private readonly Vector3 invalidPos = new Vector3(-1f,-1f,-1f); // null érték helyett kell
    public float distanceThreshold = 1f; // legalább ennyit kell elmozdulnia a kurzornak, hogy ne számítson kattintásnak
    private float dragDistance; //mennyit mozgott a kurzor?
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
	float zoomSpeed = 0.03f;
    public CanvasGroup zoomSliderCanvas;
    #endregion



    WaitForSeconds holdDownWait = new WaitForSeconds(0.5f);

    

    void Awake()
    {
        singleton = this;
        cam = GetComponent<Camera>();

#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        zoomSliderCanvas.gameObject.SetActive(true);
        Console.Log("Windows, enabling zoom slider");
#endif
    }

	IEnumerator HoldDownTimer() {
		
		yield return holdDownWait;
		if (dragDistance < distanceThreshold) {
			ignoreMouseUp = true;
			Click (true);
		}
	}
#if UNITY_ANDROID && !UNITY_EDITOR
    //ANDROID UPDATE
    void Update() {
		if (!GameManager.PLAYING)
			return;
    
        if (Input.touchCount == 0) return;




        if (Input.touchCount == 2)
        {
            StopCoroutine (holdDownCoroutine);
            ignoreMouseUp = true;
             startMousePos = invalidPos;

            // A két touch
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // megkeressük mind2 touch előző pozícióját
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // megnézzük az előző frameben, és a mostani frameben a 2 touch távolságát
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // megnézzük mennyit változott ez a táv.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                // ... zoom
                cam.orthographicSize += deltaMagnitudeDiff *zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 3f,mapSize); //ne zoomoljunk túl

                RefreshBounds();
            return;
        }


        //DRAG
        if (Input.touches[0].phase == TouchPhase.Began) { //ha nyomjuk a bal egér gombot (vagy touch0)
           
			holdDownCoroutine = StartCoroutine (HoldDownTimer());
            dragDistance = 0f;
            startMousePos = cam.ScreenToWorldPoint(Input.mousePosition); //hol nyomtuk le az egeret?
            startMousePos.z = 0.0f;
        } else 

        if (Input.touches[0].phase == TouchPhase.Moved) { //ha nyomva TARTJUK az egeret
         
            if (startMousePos == invalidPos) return;

            Vector3 nowMousePos = cam.ScreenToWorldPoint(Input.mousePosition); //hol van most a kurzor? (World pozíció)
            nowMousePos.z = 0.0f;
            Vector3 newPos;
            dragDistance += (startMousePos - nowMousePos).magnitude; //mennyit ment eddig a kurzor?
            newPos = transform.position + startMousePos - nowMousePos; //a kamerát mozgatjuk (azon van ez a script)
            if(dragDistance > distanceThreshold) zoomSliderCanvas.alpha = 0.2f;
			//RefreshBounds ();




           newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
           newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            transform.position = newPos;
        }


        if (Input.touches[0].phase == TouchPhase.Ended) { //felengedtük a gombot
           
            zoomSliderCanvas.alpha = 1f;
    StopCoroutine (holdDownCoroutine);
            if (startMousePos == invalidPos) return;
            startMousePos = invalidPos;
            if (ignoreMouseUp) {
				ignoreMouseUp = false;
				return;
			}
			
			if (dragDistance < distanceThreshold) { //ha kevesebbet ment a kurzor, mint a threshold.
				

					Click (false);
				 

			}
           
        }

    }

#else

    //WINDOWS UPDATE
    void Update()
    {
        if (!GameManager.PLAYING)
            return;

       if(Input.GetMouseButtonUp(0))
            /*ezt ide kell raknom, az IsPointerOver... elé, mert különben ha mozgok, 
             * és az egeret az átlátszó zoomslider felett engedem fel, akkor nem kapcsol vissza, hiszen ignorálva lesz az egész update..
             * */
        {
            zoomSliderCanvas.alpha = 1f;
        }
        if (EventSystem.current.IsPointerOverGameObject()) //-1 , azaz bal egér
        {
            return;
        }




        //DRAG
        if (Input.GetMouseButtonDown(0))
        { //ha nyomjuk a bal egér gombot (vagy touch0)
          
            holdDownCoroutine = StartCoroutine(HoldDownTimer());
            dragDistance = 0f;
            startMousePos = cam.ScreenToWorldPoint(Input.mousePosition); //hol nyomtuk le az egeret?
            startMousePos.z = 0.0f;
        }
        else

    if (Input.GetMouseButton(0))
        { //ha nyomva TARTJUK az egeret
          
            if (startMousePos == invalidPos) return;

            Vector3 nowMousePos = cam.ScreenToWorldPoint(Input.mousePosition); //hol van most a kurzor? (World pozíció)
            nowMousePos.z = 0.0f;
            Vector3 newPos;
            dragDistance += (startMousePos - nowMousePos).magnitude; //mennyit ment eddig a kurzor?
            newPos = transform.position + startMousePos - nowMousePos; //a kamerát mozgatjuk (azon van ez a script)
            if (dragDistance > distanceThreshold) zoomSliderCanvas.alpha = 0.2f;
            //RefreshBounds ();




            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
            transform.position = newPos;
        }


        if (Input.GetMouseButtonUp(0))
        { //felengedtük a gombot

           
           
            if (startMousePos == invalidPos) return;
            startMousePos = invalidPos;
            if (ignoreMouseUp)
            {
                ignoreMouseUp = false;
                return;
            }
            StopCoroutine(holdDownCoroutine);
            if (dragDistance < distanceThreshold)
            { //ha kevesebbet ment a kurzor, mint a threshold.


                Click(false);


            }

        }

    }

    //Windows update


#endif
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


    public void SliderZoom(float value)
    {
        cam.orthographicSize =4 + (1-value) * (mapSize-4);
       
        RefreshBounds();
    }
    public void Zoom(bool inorout) //true = nagyítás
    {
        
        if(inorout)
        {
            if( cam.orthographicSize > 4) cam.orthographicSize -= zoomSpeed;
           
        } else
        {
           if(cam.orthographicSize < mapSize) cam.orthographicSize += zoomSpeed;
        }
		RefreshBounds ();
    }

    public void AlignCamera(int n)
    {
        Console.Log("Align camera: " + n);
        mapSize = n;
        transform.GetChild(0).localScale *= n;
        cam.orthographicSize =n;
		RefreshBounds ();

    }


    public IEnumerator ResetCamera()
    {
        zoomSliderCanvas.interactable = false;
        float time = 7f;
        float speed = 1f;
        Vector3 zero = new Vector3(0f, 0f, -10f);
        while(time > 0)
        {
            time -= Time.deltaTime;

            Vector3 pos = Vector3.Lerp(cam.transform.position, zero, Time.deltaTime * speed);
            cam.transform.position = pos;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, mapSize, Time.deltaTime * speed);

            yield return null;
        }





    }


    public void SetSliderAlpha(float f)
    {
        zoomSliderCanvas.alpha = f;
    }
  
}
