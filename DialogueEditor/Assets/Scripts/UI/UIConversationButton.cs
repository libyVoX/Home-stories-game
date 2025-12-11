using UnityEngine;
using UnityEngine.UI;

namespace DialogueEditor
{
    public class UIConversationButton : MonoBehaviour
    {
        [SerializeField] private RectTransform visualRect;

        public enum eHoverState
        {
            idleOff,
            animatingOn,
            idleOn,
            animatingOff,
        }

        public enum eButtonType
        {
            Option,
            Speech,
            End
        }

        public eButtonType ButtonType { get { return m_buttonType; } }

        [SerializeField] private TMPro.TextMeshProUGUI TextMesh = null;
        [SerializeField] private Image OptionBackgroundImage = null;

        private RectTransform m_rect;

        // Node data
        private eButtonType m_buttonType;
        private ConversationNode m_node;

        // Hover
        private float m_hoverT = 0.0f;
        private eHoverState m_hoverState;
        private bool Hovering { get { return (m_hoverState == eHoverState.animatingOn || m_hoverState == eHoverState.animatingOff); } }
        private Vector3 BigSize { get { return Vector3.one * 1.2f; } }

        // Сохраняем дефолтный шрифт из инспектора
        private TMPro.TMP_FontAsset defaultFont;


        private void Awake()
        {
            m_rect = GetComponent<RectTransform>();
            defaultFont = TextMesh.font;        // <-- главное изменение
        }

        private void Update()
        {
            if (Hovering)
            {
                m_hoverT += Time.deltaTime;
                float normalised = m_hoverT / 0.2f;
                bool done = false;
                if (normalised >= 1)
                {
                    normalised = 1;
                    done = true;
                }

                float ease = EaseOutQuart(normalised);
                Vector3 size = Vector3.one;

                switch (m_hoverState)
                {
                    case eHoverState.animatingOn:
                        size = Vector3.Lerp(Vector3.one, BigSize, ease);
                        break;

                    case eHoverState.animatingOff:
                        size = Vector3.Lerp(BigSize, Vector3.one, ease);
                        break;
                }

                m_rect.localScale = size;

                if (done)
                {
                    m_hoverState =
                        (m_hoverState == eHoverState.animatingOn)
                        ? eHoverState.idleOn
                        : eHoverState.idleOff;
                }
            }
        }


        //--------------------------------------
        // Input
        //--------------------------------------

        public void OnHover(bool hovering)
        {
            if (!ConversationManager.Instance.AllowMouseInteraction) { return; }

            if (hovering)
                ConversationManager.Instance.AlertHover(this);
            else
                ConversationManager.Instance.AlertHover(null);
        }

        public void OnClick()
        {
            if (!ConversationManager.Instance.AllowMouseInteraction) { return; }
            DoClickBehaviour();
        }

        public void OnButtonPressed()
        {
            DoClickBehaviour();
        }


        //--------------------------------------
        // Hover control
        //--------------------------------------

        public void SetHovering(bool selected)
        {
            if (selected && (m_hoverState == eHoverState.animatingOn || m_hoverState == eHoverState.idleOn)) return;
            if (!selected && (m_hoverState == eHoverState.animatingOff || m_hoverState == eHoverState.idleOff)) return;

            m_hoverState = selected ? eHoverState.animatingOn : eHoverState.animatingOff;
            m_hoverT = 0f;
        }


        //--------------------------------------
        // UI Setup
        //--------------------------------------

        public void SetImage(Sprite sprite, bool sliced)
        {
            if (sprite != null)
            {
                OptionBackgroundImage.sprite = sprite;
                OptionBackgroundImage.type = sliced ? Image.Type.Sliced : Image.Type.Simple;
            }
        }

        public void InitButton(OptionNode option)
        {
            // ЕСЛИ у ноды есть шрифт — используем
            if (option.TMPFont != null)
                TextMesh.font = option.TMPFont;

            // ИНАЧЕ оставляем дефолтный (НЕ трогаем!)
        }

        public void SetAlpha(float a)
        {
            Color c_image = OptionBackgroundImage.color;
            c_image.a = a;
            OptionBackgroundImage.color = c_image;
        }


        public void SetupButton(eButtonType buttonType, ConversationNode node,
            TMPro.TMP_FontAsset continueFont = null,
            TMPro.TMP_FontAsset endFont = null)
        {
            m_buttonType = buttonType;
            m_node = node;

            switch (m_buttonType)
            {
                case eButtonType.Option:
                    TextMesh.text = node.Text;
                    if (node.TMPFont != null)
                        TextMesh.font = node.TMPFont;
                    else
                        TextMesh.font = defaultFont;  // <-- сохраняем дефолт
                    break;

                case eButtonType.Speech:
                    TextMesh.text = "Продолжить";
                    TextMesh.font = continueFont != null ? continueFont : defaultFont;
                    break;

                case eButtonType.End:
                    TextMesh.text = "Закончить";
                    TextMesh.font = endFont != null ? endFont : defaultFont;
                    break;
            }
        }


        //--------------------------------------
        // Logic
        //--------------------------------------

        private void DoClickBehaviour()
        {
            switch (m_buttonType)
            {
                case eButtonType.Speech:
                    ConversationManager.Instance.SpeechSelected(m_node as SpeechNode);
                    break;

                case eButtonType.Option:
                    ConversationManager.Instance.OptionSelected(m_node as OptionNode);
                    break;

                case eButtonType.End:
                    ConversationManager.Instance.EndButtonSelected();
                    break;
            }
        }


        //--------------------------------------
        // Util
        //--------------------------------------

        private static float EaseOutQuart(float normalized)
        {
            return (1 - Mathf.Pow(1 - normalized, 4));
        }
    }
}
