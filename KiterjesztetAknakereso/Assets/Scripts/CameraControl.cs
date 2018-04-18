using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

	Camera cam;
    public static CameraControl singleton;
    



    private Vector3 startMousePos; //ahol lenyomtuk az egeret
    private readonly Vector3 invalidPos = new Vector3(-1f,-1f,-1f); // null érték helyett kell
    public float distanceThreshold = 1f; // legalább ennyit kell elmozdulnia a kurzornak, hogy ne számítson kattintásnak
    private float dragDistance; //mennyit mozgott a kurzor?
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
	public float zoomSpeed = 0.5f;
    public CanvasGroup zoomSliderCanvas;


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

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
       


            //DRAG
            if (Input.GetMouseButtonDown(0)) { //ha nyomjuk a bal egér gombot (vagy touch0)
			
			holdDownCoroutine = StartCoroutine (HoldDownTimer());
            dragDistance = 0f;
            startMousePos = cam.ScreenToWorldPoint(Input.mousePosition); //hol nyomtuk le az egeret?
            startMousePos.z = 0.0f;
        }

        if (Input.GetMouseButton(0)) { //ha nyomva TARTJUK az egeret

            if (startMousePos == invalidPos) return;

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

            if (startMousePos == invalidPos) return;
            
            if (ignoreMouseUp) {
				ignoreMouseUp = false;
				return;
			}
			StopCoroutine (holdDownCoroutine);
			if (dragDistance < distanceThreshold) { //ha kevesebbet ment a kurzor, mint a threshold.
				

					Click (false);
				 

			}
            startMousePos = invalidPos;
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
