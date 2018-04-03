using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class TheCamera : MonoBehaviour {

    //UI界面
    public Button ToRotateAround;
    public Button LeavePoint;
    public Image InfoPlane;
    public Button W, S, N, E;

    //需要查找的代理组
    public List<GameObject> CameraDlg_Vector = new List<GameObject>();     //相机定点代理
    public List<GameObject> CameraDlg_Orbit, CameraDlg_Linear = new List<GameObject>();   //相机动作代理
    public List<GameObject> ExplodeDlg_V, ExplodeDlg_U = new List<GameObject>();     //模型代理

    private bool ison = true;
    public static TheCamera theCamera;

    private void Awake()
    {
        theCamera = this;

        gameObject.GetComponent<TestGLDraw>().enabled = false;
       
        foreach(var temp in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        {
            FindObj(temp);  
        }

        
        MouseHit[] Ponits = FindObjectsOfType<MouseHit>();
        foreach (var item in Ponits)
        {
           
            item.gameObject.AddComponent<MeshCollider>();

            item.MoveToObj += new Action<GameObject>(PointMove);
            item.ExplodeDlgV += new Action<GameObject>(EV);
            item.ExplodeDlgU += new Action<GameObject>(EU);
            item.CameraDlgOrbit += new Action<GameObject>(CO);
            item.CameraDlgLinear += new Action<GameObject>(CL);
        }
       
    }

    void Start()
    {
        ToRotateAround.onClick.AddListener(toRotateAround);
        LeavePoint.onClick.AddListener(toLeavePonit);
        W.onClick.AddListener(toWest);
        E.onClick.AddListener(toEast);
        N.onClick.AddListener(toNorth);
        S.onClick.AddListener(toSouth);
    }

    

    //查找模型代理  物体越多耗时越长  
    void FindObj(GameObject obj)
    {
        //模糊查找
        if (obj.name.Contains("POI"))
        {
            obj.tag = "POI";
        }
        if (obj.name.Contains("CameraDlg_Vector"))
        {
            CameraDlg_Vector.Add(obj);
            obj.tag = "DirPoint";
            obj.AddComponent<MouseHit>();
        }
        if (obj.name.Contains("CameraDlg_Orbit"))
        {
            CameraDlg_Orbit.Add(obj);
            obj.tag = "CameraDlg_Orbit";
            obj.AddComponent<MouseHit>();
        }
        if (obj.name.Contains("CameraDlg_Linear"))
        {
            CameraDlg_Linear.Add(obj);
            obj.tag = "CameraDlg_Linear";
            obj.AddComponent<MouseHit>();
        }
        if (obj.name.Contains("ExplodeDlg_V"))
        {
            ExplodeDlg_V.Add(obj);
            obj.tag = "ExplodeDlg_V";
            obj.AddComponent<MouseHit>();
        }
        if (obj.name.Contains("ExplodeDlg_U"))
        {
            ExplodeDlg_U.Add(obj);
            obj.tag = "ExplodeDlg_U";
            obj.AddComponent<MouseHit>();
        }
        if (obj.name.Contains("LGT_SP"))
        {
            obj.AddComponent<Light>();
            obj.GetComponent<Light>().type = LightType.Spot;
        }
        if (obj.name.Contains("LGT_P"))
        {
            obj.AddComponent<Light>();
            obj.GetComponent<Light>().type = LightType.Point;
        }
    }


    #region UI关联方法
    void toRotateAround()
    {
       if(camfly._camfly.cameraState != camfly.CameraState.自动漫游模式)
        {
            camfly._camfly.cameraState = camfly.CameraState.自动漫游模式;
        }
       else
        {
            camfly._camfly.cameraState = camfly.CameraState.手动漫游模式;
        }
    }

    void toLeavePonit()
    {
        camfly._camfly.ReturnInital();
    }

    void toEast()
    {
        camfly._camfly.cameraPos = camfly.CameraPos.East;
        camfly._camfly.ChangeCameraDir(camfly._camfly.VectorLength);
    }
    void toWest()
    {
        camfly._camfly.cameraPos = camfly.CameraPos.West;
        camfly._camfly.ChangeCameraDir(camfly._camfly.VectorLength);
    }
    void toNorth()
    {
        camfly._camfly.cameraPos = camfly.CameraPos.North;
        camfly._camfly.ChangeCameraDir(camfly._camfly.VectorLength);
    }
    void toSouth()
    {
        camfly._camfly.cameraPos = camfly.CameraPos.South;
        camfly._camfly.ChangeCameraDir(camfly._camfly.VectorLength);
    }
    #endregion


    //摄像机定点移动并改变视角
    private void PointMove(GameObject obj)
    {
        camfly._camfly.cameraState = camfly.CameraState.定点;

        ToMove(obj.transform); 
    }  
    public void ToMove(Transform obj)
    {

        float time = CountDistance(transform, obj.transform) / 10;
        transform.DOMove(obj.transform.position, time);

        Vector3 rotate = new Vector3(obj.eulerAngles.x, obj.eulerAngles.y, obj.eulerAngles.z);
        print("X:" + rotate.x + "Y:" + rotate.y + "Z:" + rotate.z);
        transform.DORotate(rotate, time);
    }
   
    //模型弹开与文字展示
    private void EV(GameObject target)
    {
        Vector3 pos = target.transform.position;
        float distance = GetNumberInt(target.name);
        if (ison)
        {
            ison = false;
            target.transform.DOMove(new Vector3(pos.x, pos.y + distance, pos.z), 1f);
            InfoPlane.gameObject.SetActive(true);
            gameObject.GetComponent<TestGLDraw>().enabled = true;
        }
        else
        {
            ison = true;
            target.transform.DOMove(new Vector3(pos.x, pos.y - distance, pos.z), 1f);
            InfoPlane.gameObject.SetActive(false);
            gameObject.GetComponent<TestGLDraw>().enabled = false;
        }
       
        
    }
    private void EU(GameObject target)
    {
        Vector3 pos = target.transform.position;
        float distance = GetNumberInt(target.name);
        if (ison)
        {
            ison = false;
            target.transform.DOMove(new Vector3(pos.x+ distance, pos.y, pos.z), 1f);
            InfoPlane.gameObject.SetActive(true);
        }
        else
        {
            ison = true;
            target.transform.DOMove(new Vector3(pos.x- distance, pos.y, pos.z), 1f);
            InfoPlane.gameObject.SetActive(false);
        }
    }


    //相机环绕移动
    private void CO(GameObject target)
    {
       
        int Obrit = GetNumberInt(target.name);   //获取环绕半径

        CameraToForward(target, Obrit);
        StartCoroutine(toCO(target));
    }
    //相机线性移动   相机到达物体法线方向
    private void CL(GameObject target)
    {
        
        int Obrit = GetNumberInt(target.name);

        CameraToForward(target, Obrit);
        StartCoroutine(toCL(target, Obrit));
    }

    IEnumerator toCO(GameObject target)
    {
        camfly._camfly.cameraState = camfly.CameraState.手动漫游模式;
        yield return new WaitForSeconds(0.1f);
        camfly._camfly.cameraState = camfly.CameraState.Obrit;

        while (true)
        {
            camfly._camfly.view(6, target);
            yield return new WaitForSeconds(0.02f);

            if (camfly._camfly.cameraState != camfly.CameraState.Obrit)
            {
                yield break;
            }
        }
    }
    IEnumerator toCL(GameObject target,float orbit)
    {
        camfly._camfly.cameraState = camfly.CameraState.手动漫游模式;
        yield return new WaitForSeconds(0.1f);
        camfly._camfly.cameraState = camfly.CameraState.Linear;

        while (true)
        {
            Vector3 vector = transform.position - target.transform.position;
            float dd;
            dd = vector.magnitude;
            dd -= 5 * Input.GetAxis("Mouse ScrollWheel");
            dd = Mathf.Clamp(dd, 1, orbit);
            vector = vector.normalized * dd;
            transform.position = target.transform.position + vector;

            yield return new WaitForSeconds(0.02f);

            if (camfly._camfly.cameraState != camfly.CameraState.Linear)
            {
                yield break;
            }
        }
    }


    //返回向量长度
    public float CountDistance(Transform a, Transform b)
    {
        return (Vector3.Distance(a.position, b.position));
    }
    

    //返回向量水平投影线上的观察点坐标(已知半径)  p1为摄像机，p2为观察点
    public Vector3 CountTriangleEndpoint(Transform p1,Transform p2,float length)
    {
        float a1 = p1.position.x, b1 = p1.position.y, c1 = p1.position.z;
        float a2 = p2.position.x, b2 = p2.position.y, c2 = p2.position.z;
        float a3 = a1, c3 = c1, b3 = b2;
        float l = Vector3.Distance(new Vector3(a2, b2, c2), new Vector3(a3, b3, c3));
        //向量线性插值
        Vector3 pos = Vector3.Lerp(new Vector3(a2, b2, c2), new Vector3(a3, b3, c3), length/l);

        return pos;
    }

    //摄像机移动到指定距离，并看向物体法线方向
    public void CameraToForward(GameObject target,float Obrit)
    {
        GameObject a = new GameObject();
        a.transform.position = target.transform.position;
        a.transform.rotation = target.transform.rotation;
        a.transform.Translate(Vector3.forward * Obrit);
        a.transform.Rotate(Vector3.up * 180);
        transform.DOMove(a.transform.position, 1.5f);
        transform.DORotate(new Vector3(a.transform.eulerAngles.x, a.transform.eulerAngles.y, a.transform.eulerAngles.z), 1f);
        Destroy(a);
    }

    //从字符串中提取数字
    public static int GetNumberInt(string str)
    {
        int result = 0;
        if (str != null && str != string.Empty)
        {
            // 正则表达式剔除非数字字符（不包含小数点.）
            str = Regex.Replace(str, @"[^\d.\d]", "");
            // 如果是数字，则转换为decimal类型
            if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
            {
                result = int.Parse(str);
            }
        }
        return result;
    }

}
