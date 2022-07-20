﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Utils;
using UnityEditor;
using UnityEngine;

namespace Game.Pool
{
    [CreateAssetMenu(menuName = "Runtime Data/Pool Helper", order = 30)]
    public class RD_PoolHelper : ScriptableObject
    {
        public List<PoolHelperVo> List;

        [SerializeField] private string _projectResourcesPath;

#if UNITY_EDITOR

        private string InfoBoxMessage = string.Empty;

        // [InfoBox("$InfoBoxMessage", InfoMessageType.Warning)] [BoxGroup("Add Pool Key")] [ShowInInspector]

        // EditorGUILayout.

        [SerializeField] private string Key;

        //[ShowInInspector] [BoxGroup("Add Pool Key")]
        [SerializeField] private int Count;

        //[ShowInInspector] [BoxGroup("Add Pool Key")]
        [SerializeField] private GameObject Prefab;


        [Button(nameof(CreatePoolKey))] public bool ButtonCreateField;

        public void CreatePoolKey()
        {
            if (Key == String.Empty)
            {
                ShowMessage("Key Cannot Be Empty");
                return;
            }

            string poolKeyPath = _projectResourcesPath + "/" + "PoolKey.cs";
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(poolKeyPath);

            string data;
            if (obj == null)
            {
                Debug.Log("File didn't existed generating...");
                ShowMessage("There is No PoolKey File");
                return;
            }
            else
            {
                data = LoadFileOnPath(poolKeyPath);
                Debug.Log("Changing on existing file...");
            }

            if (data.Contains(Key))
            {
                ShowMessage("Enum already exists");
                Debug.Log("Enum already exists");
                return;
            }

            string addition = "\r\t\t";
            addition += "//-%Name%";
            addition += "\r\t\t";
            addition += "%Name%,";
            addition += "//-";
            addition += "\r\t\t";
            addition += "ADDPOINT";
            data = data.Replace("//*ADDITION*//", addition);
            data = data.Replace("%Name%", Key);
            data = data.Replace("ADDPOINT", "//*ADDITION*//");
            CodeUtilities.SaveFile(data, poolKeyPath);
            Debug.Log("Added PoolKey");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            ShowMessage("Enum is Added");
            //AddToList();
        }


        public void ShowMessage(string text)
        {
            InfoBoxMessage = text; //"Prefab || Key Empty";
        }


        [Button(nameof(AddToList))] public bool ButtonAddField;

        public void AddToList()
        {
            if (Key == String.Empty || Prefab == null)
            {
                ShowMessage("Prefab || Key Empty");
                return;
            }

            try
            {
                PoolHelperVo vo = new PoolHelperVo();
                PoolKey PK = (PoolKey)Enum.Parse(typeof(PoolKey), Key.ToString());
                vo.Count = Count;
                vo.Key = PK;
                vo.Prefab = Prefab;

                if (List == null)
                    List = new List<PoolHelperVo>();

                List.Add(vo);

                Count = 0;
                Key = string.Empty;
                Prefab = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ShowMessage("Enum is not declared");
            }
        }


        private string LoadFileOnPath(string filePath)
        {
            try
            {
                Debug.Log("Loading File = " + filePath);
                string data = string.Empty;
                string path = filePath;
                StreamReader theReader = new StreamReader(path, Encoding.Default);
                using (theReader)
                {
                    data = theReader.ReadToEnd();
                    theReader.Close();
                }

                return data;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return string.Empty;
            }
        }
#endif
    }
}
