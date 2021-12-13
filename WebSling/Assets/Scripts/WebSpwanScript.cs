using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSpwanScript : MonoBehaviour
{
    
    [Header("Prefabs")]
    [SerializeField]private GameObject player;
    //[SerializeField]private GameObject webPrefab;

    [Header("Web Properties")]
    [SerializeField] private float lineWidth = 0.2f;    
    [SerializeField] private Material WebMaterial;   
    private GameObject web;
    private LineRenderer lr;            
    private Vector3 startPoint;        // has the 2 positions needed for line renderer

    private Vector3 endPoint;
    private const string WEB_TAG = "Web";

    private bool isFirstClick = false;
    private bool isSecondClick = false;

    RaycastHit2D hit2D;
    
    private Vector3 worldPosition;      // postion of mouse in the World
    private float hitData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        worldPosition.z = 0;
    
        if(Input.GetMouseButtonDown(0))
        {  
            if(!isFirstClick && !isSecondClick)         // to start The web
            {   
                // Debug.Log("FirstClick");
                startPoint = Vector2.zero;
                endPoint = Vector2.zero;
                LoadWeb();
                SetBools(true,false);
            }
            else if(isFirstClick)                       // To get the next position
            {
                // Debug.Log("isSecondclick");
                SetBools(false,true);    
            }   
            else if(isSecondClick)                      // To finialize the web
            {
                // Debug.Log("clickConfirm");
                CreateWeb();
                SetBools(false,false);
            }
        }

        if(Input.GetMouseButtonDown(1) && web != null)
        {
            //delete instantiated GameObject
            Destroy(web.gameObject);
            SetBools(false,false);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
             if(transform.childCount>0)
            {
                StartCoroutine(SetEffector());
            }
        }
     
        if(isFirstClick)
        {   
            AimWeb();
        }
        else if(isSecondClick)
        {
            FinializeWeb();
        }     
    }

    IEnumerator SetEffector()
    {
        for(int i= 0;i< transform.childCount; i++)
            transform.GetChild(i).GetComponent<PlatformEffector2D>().surfaceArc = 0;

        yield return new WaitForSeconds(0.3f);

        for(int i= 0;i< transform.childCount; i++)
            transform.GetChild(i).GetComponent<PlatformEffector2D>().surfaceArc = 180;
    }

    private void SetBools(bool first,bool second)
    {
        isFirstClick = first;
        isSecondClick = second;
    }

    private void AimWeb()
    {
    //    Debug.Log("Aim Loaded");   
        hit2D = Physics2D.Raycast(player.transform.position,worldPosition,100f,LayerMask.GetMask("Web","Ground"));
        startPoint = player.transform.position;
        endPoint = hit2D.point;
        lr.SetPosition(0,startPoint);
        lr.SetPosition(1,endPoint); 
       // lr.SetPosition(1,worldPosition);
    }

    private void FinializeWeb()
    {
    //    Debug.Log("Filianize Web");
        hit2D = Physics2D.Raycast(player.transform.position,worldPosition,100f,LayerMask.GetMask("Web","Ground"));
        startPoint = hit2D.point;
        lr.SetPosition(0,startPoint);
        lr.SetPosition(1,endPoint);
     //   lr.SetPosition(1,worldPosition);

    }

    private void CreateWeb()
    {
        web.transform.position = GetWebPosition();
        // change rotation
        float angle = Mathf.Atan2(lr.GetPosition(1).y -lr.GetPosition(0).y,lr.GetPosition(1).x -lr.GetPosition(0).x) * 180 /Mathf.PI;
        if(lr.GetPosition(1).x < lr.GetPosition(0).x)
            angle -= 180;
        web.transform.Rotate(0,0,angle);

        BoxCollider2D col = web.AddComponent<BoxCollider2D>();
        // change collider size
        float length = Mathf.Sqrt(Mathf.Pow(lr.GetPosition(1).x-lr.GetPosition(0).x,2) + Mathf.Pow(lr.GetPosition(1).y-lr.GetPosition(0).y,2));

        Vector2 size = new Vector2(length,lineWidth);
        col.size = size;
        col.usedByEffector = true;

        web.AddComponent<PlatformEffector2D>().useSideFriction = true;

        web.transform.parent = transform;
        web = null;
    }

    private Vector3 GetWebPosition()
    {
        Vector3 webPos = new Vector3();
        webPos = (startPoint + endPoint) / 2; 
        return webPos;
    }

    private void LoadWeb()
    {
        web =  new GameObject("Web");
        web.layer = LayerMask.NameToLayer("Web");
        web.transform.tag = WEB_TAG;
        web.transform.position = Vector2.zero; 
        lr = web.AddComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.positionCount = 2;   
        lr.material = WebMaterial;
    }
}
