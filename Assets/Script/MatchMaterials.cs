using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;


public class MatchMaterials
{
    [MenuItem("Tools/自动匹配材质")]
    private static void Match()
    {
        Debug.Log("开始匹配...");
        List<Material> AllMaterial = new List<Material>();
        List<string> MaterialName = new List<string>();
        List<GameObject> AllgameObjects = new List<GameObject>();
        Dictionary<string, Material> TarMaterial = new Dictionary<string, Material>();


        Object[] allmaterials = Resources.LoadAll("Prefabs/Materials",typeof(Material));
        Object[] allgameobjects = Resources.LoadAll("Prefabs",typeof(GameObject));

        foreach (var item in allmaterials)
        {
            AllMaterial.Add(item as Material);
            TarMaterial.Add(item.name, item as Material);
            MaterialName.Add(item.name);   
        }
        foreach (var item in allgameobjects)
        {
            AllgameObjects.Add(item as GameObject);
        }

        Debug.Log("材质数量："+AllMaterial.Count);
        Debug.Log("材质名称数量："+MaterialName.Count);
        Debug.Log("游戏物体数量："+AllgameObjects.Count);
        Debug.Log("字典包含数量："+TarMaterial.Count);

        foreach (var item in AllgameObjects)
        {
            List<Material> thisMaterials = new List<Material>();

            foreach (var ite in MaterialName)
            {
                if (CheckString(ite) == item.name )   //当材质名称中包含物体名称时 （如 obj55 包含 obj）
                {
                    thisMaterials.Add(TarMaterial[ite]);
                }
            }

            int length = thisMaterials.Count;

            if (length == 1)
            {
                item.GetComponent<MeshRenderer>().sharedMaterial = thisMaterials[0];
            }
            else
            {
                Material[] made = new Material[length];
              
                for (int i = 0; i < length; i++)
                {
                    made[i] = thisMaterials[i];
                    
                }
                item.GetComponent<MeshRenderer>().sharedMaterials = made;
            }
          
          
            
            
          
        }

        Debug.Log("匹配成功！");
    }

    //字符串中提取下划线前的字符串
    public static string CheckString(string a)
    {
        string[] b = a.Split('_');
        return b[0];
    }
}
