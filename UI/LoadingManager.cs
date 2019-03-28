using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HRTool
{
    public class LoadingManager : MonoBehaviour
    {
        #region Singleton
        private static LoadingManager _instance;
        public static LoadingManager instance
        {
            get
            {
                if (!_instance)
                {
                    GameObject obj = GameObject.Find("LoadingManager");
                    if (obj == null)
                    {
                        obj = new GameObject("LoadingManager");
                    }
                    if (!obj.GetComponent<LoadingManager>())
                    {
                        obj.AddComponent<LoadingManager>();
                    }
                    return obj.GetComponent<LoadingManager>();
                }
                return _instance;
            }
        }
        #endregion

        public List<GameObject> loadingControllerList = new List<GameObject>();
        LoadingController[] loadingCon = new LoadingController[] { };

        private void Start()
        {
            BaseSetting();
        }

        void BaseSetting()
        {
            int cnt = loadingControllerList.Count;
            if (cnt > 0)
            {
                loadingCon = new LoadingController[cnt];
                for (int i = 0; i < loadingControllerList.Count; i++)
                {
                    if (!loadingControllerList[i].GetComponent<LoadingController>())
                    {
                        loadingControllerList[i].AddComponent<LoadingController>();
                    }
                    loadingCon[i] = loadingControllerList[i].GetComponent<LoadingController>();
                }
            }
        }

        public void OnLoadingPop(int loadingObjNum)
        {
            if (loadingCon!=null && loadingCon[loadingObjNum]!=null)
            {
                loadingControllerList[loadingObjNum].SetActive(true);
                loadingCon[loadingObjNum].GetComponent<LoadingController>().WaitLoading(999, 2);
            }
            else
            {
                Debug.Log("Null_LoadingController");
            }
        }

        public void AddSuccess(int loadingObjNum)
        {
            if (loadingCon[loadingObjNum])
            {
                loadingCon[loadingObjNum].GetComponent<LoadingController>().AddSuccess();
            }
            else
            {
                Debug.Log("Null_LoadingController");
            }
        }

    }
}

