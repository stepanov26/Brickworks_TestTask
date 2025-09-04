using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brickworks.UI.View.Skills
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField] private Color _purchasedBackgroundColor;

        [SerializeField] private Color _nonPurchasedBackgroundColor;

        [SerializeField] private Image _backgroundImage;

        [SerializeField] private TextMeshProUGUI _titleText;

        [SerializeField] private Button _button;

        private ISkillViewModel _model;
        private Action _onClick;

        public void Init(ISkillViewModel model, Action onClick)
        {
            _model = model;
            _onClick = onClick;

            _titleText.text = _model.Title;

            _model.IsPurchased.ValueChanged += SetBackgroundColor;
            SetBackgroundColor(_model.IsPurchased.Value);
            _button.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            _onClick?.Invoke();
        }

        private void SetBackgroundColor(bool isPurchased)
        {
            _backgroundImage.color = isPurchased ? _purchasedBackgroundColor : _nonPurchasedBackgroundColor;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(Clicked);
            _model.IsPurchased.ValueChanged -= SetBackgroundColor;
        }
    }
}