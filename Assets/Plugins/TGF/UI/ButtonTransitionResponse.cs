using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TGF.UI
{
    public class ButtonTransitionResponse : MonoBehaviour
    {
        [SerializeField] private TGButton m_Button;
        [SerializeField] private TargetType m_TargetType;

        [SerializeField, ShowIf("@m_TargetType == TargetType.Image")]
        private bool m_SpriteSwap;

        [SerializeField, ShowIf("@m_SpriteSwap && m_TargetType == TargetType.Image")]
        private SpriteState m_SpriteState;

        [SerializeField] private bool m_ColorTint;

        [SerializeField, ShowIf("m_ColorTint")]
        private ColorBlock m_ColorBlock;

        private Image _image;
        private TextMeshProUGUI _textMeshProUGUI;

        private void Start()
        {
            if (m_TargetType is TargetType.Image)
            {
                _image = GetComponent<Image>();
            }

            if (m_TargetType is TargetType.Text)
            {
                _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            }

            m_Button.OnStateTransition += OnStateTransition;
        }

        private void OnStateTransition(TGButton.SelectionState selectionState)
        {
            Color color;
            Sprite sprite;

            if (m_TargetType is TargetType.Image && _image is null) return;
            if (m_TargetType is TargetType.Text && _textMeshProUGUI is null) return;

            switch (selectionState)
            {
                case TGButton.SelectionState.Normal:
                    color = m_ColorBlock.normalColor;
                    sprite = null;
                    break;
                case TGButton.SelectionState.Disabled:
                    color = m_ColorBlock.disabledColor;
                    sprite = m_SpriteState.disabledSprite;
                    break;
                case TGButton.SelectionState.Highlighted:
                    color = m_ColorBlock.highlightedColor;
                    sprite = m_SpriteState.highlightedSprite;
                    break;
                case TGButton.SelectionState.Pressed:
                    color = m_ColorBlock.pressedColor;
                    sprite = m_SpriteState.pressedSprite;
                    break;
                case TGButton.SelectionState.Selected:
                    color = m_ColorBlock.selectedColor;
                    sprite = m_SpriteState.selectedSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectionState), selectionState, null);
            }

            if (m_SpriteSwap)
            {
                _image.overrideSprite = sprite;
            }

            if (m_ColorTint)
            {
                if (m_TargetType is TargetType.Image)
                {
                    _image.color = color;
                }

                else if (m_TargetType is TargetType.Text)
                {
                    _textMeshProUGUI.color = color;
                }
            }
        }

        private enum TargetType
        {
            Image,
            Text
        }
    }
}