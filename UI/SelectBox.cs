using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
namespace HRTool
{
    public class SelectBox : MonoBehaviour
    {       
        public UnityAction<GameObject> afterAction;
        bool selected = false;
        public void SelectOnf()
        {
            selected = selected == false ? true : false;
            BoxOnf();
        }

        public void SelectOnf_Abs(bool onf)
        {
            selected = onf;
            BoxOnf();
        }

        void BoxOnf()
        {
            Vector4 lineColor = GetComponent<Image>().color;
            if (selected == true)
            {
                if (afterAction != null)
                {
                    afterAction(gameObject);
                }
                GetComponent<Image>().color = new Vector4(lineColor.x, lineColor.y, lineColor.z, 1);
            }
            else
            {
                GetComponent<Image>().color = new Vector4(lineColor.x, lineColor.y, lineColor.z, 0);
            }
        }
    }
}

