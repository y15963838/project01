using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class camfly : MonoBehaviour
{
    public GameObject ObjPoint;     //代理点的各个角度应为0
    public int 手动旋转速度,自动旋转速度,无操作时间;
    public Vector3 vector;
    public float VectorLength;     //摄像机与代理点构成的向量的长度
    private GameObject RecordEmpty; 
    public static bool isReturn;
    public int TimeCounter = 0;    //任何交互操作都应让计时器归零

    public enum CameraState
    {
        手动漫游模式,自动漫游模式,定点,Obrit,Linear
    }
    public enum CameraPos
    {
        East,North,South,West
    }
    public CameraState cameraState;
    public CameraPos cameraPos;

    public static camfly _camfly;
    private void Awake()
    {
        ObjPoint = GameObject.FindGameObjectWithTag("POI");
        VectorLength = TheCamera.GetNumberInt(ObjPoint.name);
       
        cameraPos = CameraPos.South;
        ChangeCameraDir(VectorLength);

        RecordEmpty = new GameObject();
        RecordEmpty.transform.position = transform.position;
        RecordEmpty.transform.rotation = transform.rotation;
       
        _camfly = this;
        cameraState = CameraState.手动漫游模式;
    }

    private void Start()
    {
        手动旋转速度 = 6;
        自动旋转速度 = 10;
        无操作时间 = 15;


        StartCoroutine(CheckState(无操作时间));
    }

    private void Update()
    {
        if(cameraState == CameraState.手动漫游模式)
        {
            view(手动旋转速度,ObjPoint);   //摄像机向量
            vector = transform.position - ObjPoint.transform.position;   //空间两点的Vector3之差即为向量
            MouseScrollWheel(2,50);
            transform.position = ObjPoint.transform.position + vector;
        }
       if(cameraState == CameraState.自动漫游模式)
        {
            AutoRotate(自动旋转速度);
        }
        if (cameraState == CameraState.定点)
        {
            PointView();
        }

    }

    public void view(float speed,GameObject obj)
    {
        if (Input.GetMouseButton(1))
        {
        
            TimeCounter = 0;

            transform.RotateAround(obj.transform.position, Vector3.up, speed * Input.GetAxis("Mouse X"));

            Vector3 a = transform.position;
            Quaternion b = transform.rotation;
            transform.RotateAround(obj.transform.position, transform.right, -speed * Input.GetAxis("Mouse Y"));

        }
        if (Input.anyKey)
        {
            TimeCounter = 0;
        }
    }

    public void AutoRotate(float speed)
    {
        if (isReturn)
        {
            isReturn = false;
             ReturnInital();
        }

        transform.RotateAround(ObjPoint.transform.position,Vector3.up,speed * Time.deltaTime);
    }

    public void MouseScrollWheel(float Min,float Max)                                                  
    {
        float dd;        
        dd = vector.magnitude;                                    
        dd -= 5*Input.GetAxis("Mouse ScrollWheel");
        dd = Mathf.Clamp(dd, Min, Max);
        vector = vector.normalized * dd;
    }

    public void PointView()
    {
         isReturn = true;
         float cameraRotSpeed = 60f;
         float eulerAngles_x = transform.eulerAngles.y;
         float eulerAngles_y = transform.eulerAngles.x;


        if (Input.GetMouseButton(1))
        {

            eulerAngles_x += (Input.GetAxis("Mouse X") * cameraRotSpeed) * Time.deltaTime;

            eulerAngles_y -= (Input.GetAxis("Mouse Y") * cameraRotSpeed) * Time.deltaTime;

            Quaternion quaternion;

            quaternion = Quaternion.Euler(eulerAngles_y, eulerAngles_x,0);

            transform.rotation = quaternion;

        }
    }

    public void ReturnInital()
    {
        if(cameraState != CameraState.手动漫游模式)
        {
            cameraState = CameraState.手动漫游模式;
        }
        TheCamera.theCamera.ToMove(RecordEmpty.transform);
       
     }

    IEnumerator CheckState(int 无操作时间)
    {
        StartCoroutine(shit());
        while (true)
        {
            
            if (cameraState == CameraState.手动漫游模式)
            {
                TimeCounter++;
                
            }
           

            if(TimeCounter >= 无操作时间)
            {
                cameraState = CameraState.自动漫游模式;
                TimeCounter = 0;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator shit()
    {
        while (true)
        {
            if (cameraState == CameraState.自动漫游模式)
            {
                if (Input.anyKey || Input.GetMouseButton(1) || Input.GetMouseButton(0))
                {
                    cameraState = CameraState.手动漫游模式;
                 
                }

            }
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void ChangeCameraDir(float distance)
    {
        switch (cameraPos)
        {
            case CameraPos.East:
                transform.DOMove(new Vector3(ObjPoint.transform.position.x+distance,ObjPoint.transform.position.y, ObjPoint.transform.position.z), 1f);
                transform.DORotate(new Vector3(0, -90, 0), 1f);
                break;
            case CameraPos.North:
                transform.DOMove(new Vector3(ObjPoint.transform.position.x, ObjPoint.transform.position.y, ObjPoint.transform.position.z + distance), 1f);
                transform.DORotate(new Vector3(0, 180, 0), 1f);
                break;
            case CameraPos.South:
                transform.DOMove(new Vector3(ObjPoint.transform.position.x, ObjPoint.transform.position.y, ObjPoint.transform.position.z - distance), 1f);
                transform.DORotate(new Vector3(0, 0, 0), 1f);
                break;
            case CameraPos.West:
                transform.DOMove(new Vector3(ObjPoint.transform.position.x-distance, ObjPoint.transform.position.y, ObjPoint.transform.position.z), 1f);
                transform.DORotate(new Vector3(0, 90, 0), 1f);
                break;
           
        }
    }

}