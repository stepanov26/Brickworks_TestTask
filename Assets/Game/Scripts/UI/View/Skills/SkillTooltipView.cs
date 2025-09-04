using System;
using UnityEngine;
using UnityEngine.UI;

namespace Brickworks.UI.View.Skills
{
    public class SkillTooltipView : MonoBehaviour
    {
        [SerializeField]
        private Button _closeButton;
        
        [SerializeField] private RectTransform _rectTransform;
        
        [SerializeField] private float _tooltipBellowPadding = 76f;
        
        [SerializeField] private float _tooltipAbovePadding = 96f;
        
        [SerializeField] private float _screenBottomPadding = 380f;

        [SerializeField] private RectTransform _tooltipRectTransform;

        [SerializeField] private RectTransform _anchorRectTransform;
        
        [SerializeField]
        private Button _purchaseButton;

        [SerializeField] 
        private Button _forgetButton;

        private string _id;
        private Action<string> _onPurchase;
        private Action<string> _onForget;

        public void Show(string id, RectTransform skillRect, Action<string> onPurchase, Action<string> onForget)
        {
            gameObject.SetActive(true);
            
            _closeButton.onClick.AddListener(Hide);

            _id = id;

            _onPurchase = onPurchase;
            _purchaseButton.gameObject.SetActive(_onPurchase != null);
            _purchaseButton.onClick.AddListener(Purchase);
            
            _onForget = onForget;
            _forgetButton.gameObject.SetActive(_onForget != null);
            _forgetButton.onClick.AddListener(Forget);
            
            var position = CalculateTooltipPosition(skillRect, out var placeBelow);
            _tooltipRectTransform.localPosition = position;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            
            _purchaseButton.onClick.RemoveListener(Purchase);
            _forgetButton.onClick.RemoveListener(Forget);
            _closeButton.onClick.RemoveListener(Hide);
        }
        
        private void Purchase()
        {
            _onPurchase?.Invoke(_id);
            Hide();
        }

        private void Forget()
        {
            _onForget?.Invoke(_id);
            Hide();
        }

        private Vector2 CalculateTooltipPosition(RectTransform skillRect, out bool placeBelow)
        {
            float tooltipHeight = _tooltipRectTransform.rect.height;
            float canvasHeight = _rectTransform.rect.height;

            Vector2 skillPos = _tooltipRectTransform.parent.InverseTransformPoint(skillRect.position);

            float skillBottom = skillPos.y - skillRect.sizeDelta.y / 2;
            float bottomYPos = skillBottom - _tooltipBellowPadding - tooltipHeight / 2;

            placeBelow = bottomYPos - tooltipHeight / 2 > (-canvasHeight / 2f + _screenBottomPadding);

            if (placeBelow)
            {
                return new Vector2(skillPos.x, bottomYPos);
            }

            float skillTop = skillPos.y + skillRect.sizeDelta.y / 2;
            float topYPos = skillTop + _tooltipAbovePadding + tooltipHeight / 2;

            return new Vector2(skillPos.x, topYPos);
        }
    }
}