using System;
using System.Collections;
using UnityEngine;

namespace HRTool
{
    public class LoadingController : MonoBehaviour
    {
        [SerializeField] private int successConditions=0;

        /// <summary>
        /// if null property action, must add this controller at popup object. it's for Inactivate Gameobject.  
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="action">for close popup</param>
        public void WaitLoading(float limit, int conditionCount=0, Action action = null)
        {
            if (conditionCount > 0)
            {
                successConditions = 0;                
            }
            StartCoroutine(WaitLoading_(limit, conditionCount, action));
        }

        public void AddSuccess() {
            successConditions++; Debug.Log("successConditions : "+successConditions);
        }

        IEnumerator WaitLoading_(float limit, int conditionCount, Action action=null)
        {
            float time = 0; Debug.Log("start loading");
            bool loadingDone=false;
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                time += 0.5f;
                #region timeCheck
                if (time >= limit)
                {
                    loadingDone = true;
                }
                #endregion
                
                #region ConditionCheck
                if (conditionCount > 0 && successConditions == conditionCount)
                {
                        loadingDone = true;
                }
                #endregion
                
                if (loadingDone == true)
                {
                    successConditions = 0;
                    Debug.Log("done loading");

                    if (action != null)
                    {
                        action();
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }                    
                    break;
                }
            }
        }
    }
}
