using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(WorldManager))]
public class WorldManagerEditor : Editor
{
    private WorldManager tar;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        tar = target as WorldManager;
        if (tar.ShowMapInfomation)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ChunkSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PerlinSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PerlinStrength"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("type"));
            if (GUILayout.Button("Create"))
            {
                tar.CreateAWolrd();
            }
            if (GUILayout.Button("Delete"))
            {
                tar.DeleteWorld();
            }
        }
        if (tar.ObjectCreateMode)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Obj"));
        }


        serializedObject.ApplyModifiedProperties();

        GUILayout.Space(20);
        if (GUILayout.Button("保存世界"))
        {
            tar.Save();
        }
        if (GUILayout.Button("加载世界"))
        {
            tar.Load();
        }
        GUILayout.Space(20);
        if (GUILayout.Button("撤回"))
        {
            tar.Back();
        }
        if (GUILayout.Button("打开文件夹"))
        {
            Application.OpenURL("file:///" + Application.persistentDataPath);
        }
    }
    private void OnSceneGUI()//当所选组件被选中，且点击左键
    {
       
        if (tar.ObjectCreateMode == false)
        {
            Draw(1, tar.paintType, false);
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.T)
                Draw(1, tar.paintType, true);
            ///replace
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.X)
                Replace(false);
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.V)
                Replace(true);
            //delete
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.C)
                Draw(-1, BlockType.air, true);
            //back
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Z)
                UseOnce(tar.Back);
        }
        else
        {
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.T)
                UseOnce(CreateGameObj);
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                UseOnce(CreateGameObj);
        }

    }
    public int a;

    public void CreateGameObj()
    {
        if (tar.Obj != null)
        {
            Event currentEvent = Event.current;

            GameObject Obj = Instantiate(tar.Obj,GameObject.FindGameObjectWithTag("Object").transform);

            Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Obj.transform.position = placePos(hit.point, 1) + new Vector3(0.5f,0,0.5f);
            }

        }

    }

    private void Replace(bool random)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,100,1<<3))
        {
            Vector3Int PlacePos = placePos(hit.point, -1);
            if (a >= 2)
                a = 0;
            if (a == 0)
            {
                tar.Strength = (tar.Size + 1) / 2;
                Step step = new Step();
                for (int i = -tar.Strength + 1; i < tar.Strength; i++)
                    for (int j = -tar.Strength + 1; j < tar.Height; j++)
                        for (int k = -tar.Strength + 1; k < tar.Strength; k++)
                        {
                            if (random == false)
                            {
                                SaveBackInfo(PlacePos.x + i, PlacePos.y + j * -1, PlacePos.z + k, ref step);
                                bool a = tar.ReplaceABlock(PlacePos.x + i, PlacePos.y + j, PlacePos.z + k, tar.paintType);
                                if (a == false)
                                {
                                    continue;
                                }
                            }
                            else if (Random.Range(0, 100) < tar.RandomSize)
                            {
                                SaveBackInfo(PlacePos.x + i, PlacePos.y + j * -1, PlacePos.z + k, ref step);
                                if (!tar.ReplaceABlock(PlacePos.x + i, PlacePos.y + j, PlacePos.z + k, tar.paintType))
                                {
                                    continue;
                                }
                            }
                        }
                tar.backInfo.stepLists.Add(step);
            }
            a++;
        }

    }

    private void Draw(int backwards, BlockType type, bool NoClickMouseButton)
    {

        Event currentEvent = Event.current;
        if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 || NoClickMouseButton == true)
        {

            Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit,100,1<<3))
            {
                Vector3Int PlacePos = placePos(hit.point, backwards);
                if (a >= 2)
                    a = 0;
                if (a == 0)
                {
                    tar.Strength = (tar.Size + 1) / 2;
                    Step step = new Step();
                    for (int i = -tar.Strength + 1; i < tar.Strength; i++)
                        for (int j = 0; j < tar.Height; j++)
                            for (int k = -tar.Strength + 1; k < tar.Strength; k++)
                            {
                                
                                SaveBackInfo(PlacePos.x + i, PlacePos.y + j * backwards, PlacePos.z + k, ref step);
                                tar.CreateANewBlock(PlacePos.x + i, PlacePos.y + j * backwards, PlacePos.z + k, type);

                            }
                    tar.backInfo.stepLists.Add(step);
                }
                a++;
            }
        }
    }
    private void SaveBackInfo(int x, int y, int z, ref Step step)
    {
        step.stepList.Add(new LittleStep(x, y, z, tar.GetType(x, y, z)));
    }
    private Vector3Int placePos(Vector3 point, int backwards)
    {
        Vector3 Dir = new Vector3();
        Dir = backwards * ((SceneView.lastActiveSceneView.camera.transform.position - point).normalized) / 4;
        Vector3Int PlacePos = new Vector3Int((int)(Dir.x + point.x), (int)(Dir.y + point.y), (int)(Dir.z + point.z));
        return PlacePos;
    }
    public int aa;
    private void UseOnce(System.Action action)
    {
        if (aa >= 2)
            aa = 0;
        if (aa == 0)
            action();
        aa++;
    }

}
