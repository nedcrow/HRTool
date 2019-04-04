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
        public Dictionary<string,List<GameObject>> restGameobjectDic; //재활용 가능한 오브젝트를 보관하는 리스트.        
        
        void BaseSetting()
        {
            restGameobjectDic = new Dictionary<string, List<GameObject>>();  // Debug.Log("ObjectPoolerBaseSetting");         
        }

        /// <summary>
        /// GameObject Pooling 후 매개변수<System.Action>를 실행.
        /// </summary>
        /// <param name="obj">복제 GameObject</param>
        /// <param name="parent">복제한 GameObject의 위치</param>
        /// <param name="callingCount">복제 요청 수량</param>
        /// <param name="actionAfterPooling">Pooling 후 실행할 Action</param>
        public void ObjectPooling(GameObject obj, Transform parent, int callingCount, Action actionAfterPooling)
        {
            string restListName = parent.name + "s"; 
            int needAddCount=0;

            if (restGameobjectDic.Count == 0 || !restGameobjectDic.ContainsKey(restListName)) // 필요한 List가 없으면 새로 생성.
            {
                restGameobjectDic.Add(restListName,new List<GameObject>());        
            }

            if (callingCount > 0) {
                needAddCount = callingCount - restGameobjectDic[restListName].Count; //필요수량 = 요청 수량 - 예비 수량.
            }
            else 
            {
                RemoveObject(parent, Mathf.Abs(callingCount)); //혹여나 음수일 때를 대비한 Remove.
            }            

            StartCoroutine(AddRestGameobjects(restListName, obj, needAddCount, parent, actionAfterPooling)); // restList에 GameObject생성 코루틴.
        }

        IEnumerator AddRestGameobjects(string key, GameObject obj, int needCount, Transform parent, Action actionAfterPooling)
        {
            int poolingCount = 0;
            if (needCount > 0)
            {
                poolingCount = needCount;
                for (int i = 0; i < needCount; i++)
                {
                    restGameobjectDic[key].Add(Instantiate(obj, transform)); //needCount가 양수인 경우만 생성.
                    yield return null;
                }
            }
            else
            {
                poolingCount = restGameobjectDic[key].Count - Mathf.Abs(needCount);
            } //needCount가 음수면, poolingCount를 재산정. (예비수량 - 필요없는 수량)

            string listName = parent.name + "s";
            PoolObject(restGameobjectDic[listName], parent, poolingCount);
            yield return null;
            actionAfterPooling();
            yield return null;
        }//GameObject를 관리 리스트에 추가

        void PoolObject(List<GameObject> objList, Transform parent, int poolingCount)
        {
            for (int i=0; i< poolingCount; i++)
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

