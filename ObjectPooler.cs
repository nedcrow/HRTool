using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace HRTool
{
    public class ObjectPooler : MonoBehaviour
    {
        #region Singleton
        private static ObjectPooler _instance;
        public static ObjectPooler instance
        {
            get
            {
                if (!_instance)
                {
                    GameObject obj = GameObject.Find("ObjectPooler");
                    if (obj == null) { obj = new GameObject("ObjectPooler"); }
                    if (!obj.GetComponent<ObjectPooler>()) { obj.AddComponent<ObjectPooler>().BaseSetting(); }
                    return obj.GetComponent<ObjectPooler>();
                }
                return _instance;
            }
        }
        #endregion

        [SerializeField]
        public Dictionary<string,List<GameObject>> restGameobjectDic;        
        
        void BaseSetting()
        {
            restGameobjectDic = new Dictionary<string, List<GameObject>>();  // Debug.Log("ObjectPoolerBaseSetting");         
        }

        public void ObjectPooling(GameObject obj, Transform parent, int callingCount, Action actionAfterPooling)
        {
            string listName = parent.name + "s"; 
            int needAddCount=0;
            if (restGameobjectDic.Count == 0 || !restGameobjectDic.ContainsKey(listName)) // 필요한 List가 없으면 새로 생성.
            {
                restGameobjectDic.Add(listName,new List<GameObject>());        
            }
            if (callingCount > 0) {
                needAddCount = Mathf.Abs(callingCount - restGameobjectDic[listName].Count); //필요한 GameObject수량 재산정.
            }
            else
            {
                RemoveObject(parent, Mathf.Abs(callingCount));
            }
            //Debug.Log(listName + " callingCount : " + callingCount);
            StartCoroutine(AddRestGameobjects(listName, obj, needAddCount, parent, actionAfterPooling));             
        }

        IEnumerator AddRestGameobjects(string key, GameObject obj, int count, Transform parent, Action actionAfterPooling)
        {
            for (int i = 0; i < count; i++)
            {                
                restGameobjectDic[key].Add(Instantiate(obj, transform));
                yield return null;
            }
            string listName = parent.name + "s";
            PoolObject(restGameobjectDic[listName], parent);
            yield return null;
            actionAfterPooling();
            yield return null;
        }//GameObject를 관리 리스트에 추가

        void PoolObject(List<GameObject> objList, Transform parent)
        {
            int limit = objList.Count;
            for (int i=0; i< limit; i++)
            {
                GameObject obj = objList[0];
                obj.SetActive(true);
                obj.transform.SetParent(parent);
                objList.RemoveAt(0);
            }
        }//실질적인 Pooling

        public void RemoveObject(Transform parent, int count) {
            string listName = parent.name + "s";
            for (int i=0; i<count; i++)
            {
                GameObject obj = parent.GetChild(0).gameObject;
                obj.SetActive(true);
                restGameobjectDic[listName].Add(obj);
                obj.transform.SetParent(transform);
                obj.SetActive(false);
            }
        }//반환 받은 GameObject를 보관


    }
}

