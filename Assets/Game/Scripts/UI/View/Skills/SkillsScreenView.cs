using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Brickworks.UI.View.Skills
{
    public class SkillsScreenView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _skillPointsCountLabel = null;
        
        [SerializeField]
        private Button _addSkillPointsButton = null;
        
        [SerializeField]
        private Button _forgetAllSkillsButton = null;
        
        [SerializeField]
        private TextMeshProUGUI _purchasedSkillsLabel = null;
        
        [SerializeField]
        private SkillsPanelView _skillsPanel = null;
        
        private ISkillsScreenViewModel _model;
        
        public void Init(ISkillsScreenViewModel model)
        {
            _model = model;

            _model.SkillPointsCount.ValueChanged += SetSkillPointsCount;
            _model.PurchasedSkills.ValueChanged += SetPurchasedSkills;
            _addSkillPointsButton.onClick.AddListener(_model.AddSkillPoint);
            _forgetAllSkillsButton.onClick.AddListener(_model.ForgetAllSkills);

            _skillsPanel.Init(_model.Skills, _model);

            SetPurchasedSkills(_model.PurchasedSkills.Value);
        }

        private void SetSkillPointsCount(int value)
        {
            _skillPointsCountLabel.text = value.ToString();
        }

        private void SetPurchasedSkills(string value)
        {
            _purchasedSkillsLabel.text = $"Purchased Skills: \n{value}";
        }

        private void OnDestroy()
        {
            _model.SkillPointsCount.ValueChanged -= SetSkillPointsCount;
            _model.PurchasedSkills.ValueChanged -= SetPurchasedSkills;
            _addSkillPointsButton.onClick.RemoveListener(_model.AddSkillPoint);
            _forgetAllSkillsButton.onClick.RemoveListener(_model.ForgetAllSkills);
        }
    }
}