using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace HS
{
    /// <summary>
    /// 按钮组件
    /// </summary>
    public class GButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField, Header("缩放系数"), Range(0, 1)]
        private float downScale = 0.85f;
        [SerializeField, Header("按下动画时长")]
        private float downDuration = 0.2f;
        [SerializeField, Header("抬起动画时长")]
        private float upDuration = 0.15f;
        [SerializeField, Header("长按时间")]
        private float longPressDuration = 1f;
        [SerializeField, Header("点击音效")]
        private AudioClip audioClip;
        private Vector3 originScale;    //原始缩放系数，用来记录初始-0.5,1,1这类带负号和不是1的原始值
        private bool isPressed; //是否按下
        private float pressTime;    //按下时长
        private Selectable button;
        private RectTransform rectTransform;
        private Vector3 offset;

        public UnityEvent OnLongPress;  //长按回调
        public UnityEvent OnClickUp;  //抬起回调

        private void Awake()
        {
        }


        private RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }

        private void OnEnable()
        {
            this.originScale = RectTransform.transform.localScale;
            this.button = transform.GetComponent<Selectable>();
            //originalMaterial = new Material(Shader.Find("UI/Default"));//button.targetGraphic?.material;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (button.interactable == false)
            {
                return;
            }

            StopAllCoroutines();
            StartCoroutine(ChangeScaleCoroutine(1, downScale, downDuration));

            isPressed = true;
            pressTime = Time.time;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (OnClickUp != null)
                OnClickUp.Invoke();
            if (button.interactable == false)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(ChangeScaleCoroutine(downScale, 1, upDuration));

            isPressed = false;
        }

        private IEnumerator ChangeScaleCoroutine(float beginScale, float endScale, float duration)
        {
            var isFlip = Mathf.Sign(beginScale) != Mathf.Sign(endScale);
            if (isFlip)
            {
                endScale = -endScale;
            }
            float timer = 0f;
            while (timer < duration)
            {
                RectTransform.localScale = this.originScale * Mathf.Lerp(beginScale, endScale, timer / duration);
                timer += Time.fixedDeltaTime;
                yield return null;
            }
            RectTransform.localScale = this.originScale * endScale;
        }

        private void OnDisable()
        {
            RectTransform.localScale = this.originScale;
        }

        private void Update()
        {
            if (isPressed && Time.time - pressTime >= longPressDuration)
            {
                pressTime = Time.time;
                OnLongPress?.Invoke();
            }
        }

        private void OnDestroy()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.audioClip)
            {
                AudioManager.Inst.PlayEffect(this.audioClip);
            }
            else
            {
                AudioManager.Inst.PlayEffect("Assets/GamePackage/Audios/通用/通用_点击.mp3");
            }
        }

        Transform FindParentWithName()
        {
            Transform currentParent = transform.parent;
            while (currentParent != null)
            {
                if (currentParent.name.IndexOf("Win") >= 0 || currentParent.name.IndexOf("Comp") >= 0 || currentParent.name.IndexOf("Scene") >= 0)
                {
                    return currentParent;
                }
                currentParent = currentParent.parent;
            }
            return null;
        }

        public void EndLongPress()
        {
            isPressed = false;
        }
    }
}