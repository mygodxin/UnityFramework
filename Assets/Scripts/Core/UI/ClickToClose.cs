
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using System.Reflection;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UnityFramework
{
    public class ClickToClose : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        bool isPointerInside = false;
        public void OnPointerEnter(PointerEventData eventData)
        {
            isPointerInside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isPointerInside = false;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isPointerInside)
                {

                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}