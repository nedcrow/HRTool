using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HRTool
{
    public class ContentsController : MonoBehaviour
    {

        [HideInInspector] public GameObject selectedContent;
        GameObject content;
        [SerializeField] string[] lastContentNames;

        #region ContentsSpawn
        public void ContentsSpawn(string[] contentNames)
        {
            content = transform.GetChild(0).gameObject;
            lastContentNames = contentNames;
            ContentsGameObjectSpawn(contentNames, ContentSetting);

        }//각 컨텐츠별 이름이 다름.

        public void ContentsSpawn(int count, string contentName = "")
        {
            content = transform.GetChild(0).gameObject;
            lastContentNames = new string[count];
            for (int i = 0; i < count; i++)
            {
                lastContentNames[i] = contentName + "_"+i;
            }
            ContentsGameObjectSpawn(lastContentNames, ContentSetting);

        }//컨텐츠 이름이 같고 순서만 부여

        void ContentsGameObjectSpawn(string[] contentNames, Action afterAction)
        {
            content.SetActive(true);
            int needCount = contentNames.Length - transform.GetChildCount(); //이미 있는 content 수량 차감.
            HRTool.ObjectPooler.instance.ObjectPooling(content, transform, needCount, afterAction);
        }
        #endregion


        /// <summary>
        /// lastContentNames을 기준으로 자식들의 이름 변환. 만약 Text를 가진 자식이면 Text도 변환. AfterAction추가.
        /// </summary>
        void ContentSetting()
        {
            if (transform.GetChildCount() != 0)
            {
                for (int i = 0; i < lastContentNames.Length; i++)
                {
                    #region GameobjectName
                    GameObject content = transform.GetChild(i).gameObject;
                    content.name = lastContentNames[i];
                    #endregion

                    #region ActionAfterSelected
                    GameObject selectBox = HRTool.FindObject.FindGameobject.GetChild(content, "Select");
                    if (selectBox != null)
                    {
                        selectBox.GetComponent<HRTool.SelectBox>().afterAction = ActionAfterSelected;
                    }
                    #endregion

                    #region Text
                    GameObject text = HRTool.FindObject.FindGameobject.GetChild(content, "Text");
                    if (text != null)
                    {
                        text.GetComponent<Text>().text = lastContentNames[i];
                    }
                    #endregion

                    content.transform.localScale = Vector3.one;
                }
            }
            else
            {
                Debug.Log("NoContentsError");
            }
        }

        void ActionAfterSelected(GameObject selectedContent)
        {
            this.selectedContent = selectedContent.transform.parent.gameObject; //selectedContent 등록.
            UnSelectedAllContents();
        }

        /// <summary>
        /// SelectBox가 있는 경우에만 실행됨.
        /// </summary>
        void UnSelectedAllContents()
        {
            for (int i = 0; i < transform.GetChildCount(); i++)
            {
                GameObject content = transform.GetChild(i).gameObject;
                GameObject selectBox = HRTool.FindObject.FindGameobject.GetChild(content, "Select");
                if (selectBox != null)
                {
                    selectBox.GetComponent<HRTool.SelectBox>().SelectOnf_Abs(false);
                }
            }
        }
    }
}

