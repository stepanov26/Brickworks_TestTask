using System;
using System.Linq;
using System.Text;
using Brickworks.Model.Skills;
using Brickworks.UI.View.Skills;
using Brickworks.Utils;
using UnityEngine;

namespace Brickworks.UI.Model.Skills
{
    public class SkillsScreenModel : ISkillsScreenViewModel, IDisposable
    {
        public IReactiveProperty<int> SkillPointsCount => _skillPointsCount;
        public IReactiveProperty<string> PurchasedSkills => _purchasedSkills;
        public SkillViewModel[] Skills { get; }
        public SkillsModel SkillsModel => _skillsModel;

        private readonly ReactiveProperty<int> _skillPointsCount = new();
        private readonly ReactiveProperty<string> _purchasedSkills = new();
        
        private readonly SkillsModel _skillsModel;
        private readonly ResourcesModel _resourcesModel;

        public SkillsScreenModel(SkillsModel skillsModel, ResourcesModel resourcesModel)
        {
            _skillsModel = skillsModel;
            _resourcesModel = resourcesModel;

            _resourcesModel.ResourceAmountChanged += SkillsPointsCountChanged;

            Skills = skillsModel.Skills.Select(skill =>
                    new SkillViewModel(skill.Id,
                        skill.Title,
                        _skillsModel.IsSkillPurchased(skill.Id)))
                .ToArray();

            _skillsModel.PurchasedSkillsChanged += UpdatePurchasedSkills;
            UpdatePurchasedSkills();
        }

        public void PurchaseSkill(string id)
        {
            if (_skillsModel.TryPurchaseSkill(id))
            {
                var skill = Skills.FirstOrDefault(skill => skill.Id == id);

                if (skill == null)
                {
                    Debug.LogError($"Skill {id} not found");
                    return;
                }
                
                skill.Purchase();
                UpdatePurchasedSkills();
            }
        }

        public void ForgetSkill(string id)
        {
            if (_skillsModel.TryForgetSkill(id))
            {
                var skill = Skills.FirstOrDefault(skill => skill.Id == id);

                if (skill == null)
                {
                    Debug.LogError($"Skill {id} not found");
                    return;
                }
                
                skill.Forget();
                UpdatePurchasedSkills();
            }
        }

        public void ForgetAllSkills()
        {
            _skillsModel.ForgetAllSkills();
            UpdatePurchasedSkills();
        }

        public void AddSkillPoint()
        {
            _resourcesModel.AddResource(ResourceType.SkillPoints, 1);
        }

        private void SkillsPointsCountChanged(ResourceType resourceType)
        {
            if (resourceType != ResourceType.SkillPoints)
            {
                return;
            }

            _skillPointsCount.Value = _resourcesModel.GetResourceAmount(ResourceType.SkillPoints);
        }

        private void UpdatePurchasedSkills()
        {
            var purchasedSkills = _skillsModel.Skills
                .Where(skill => _skillsModel.IsSkillPurchased(skill.Id))
                .Select(skill => skill.Title);

            var stringBuilder = new StringBuilder();

            foreach (var skill in purchasedSkills)
            {
                stringBuilder.AppendLine($"\n{skill}");
            }
            
            _purchasedSkills.Value = stringBuilder.ToString();
        }

        public void Dispose()
        {
            _resourcesModel.ResourceAmountChanged -= SkillsPointsCountChanged;
            _skillsModel.PurchasedSkillsChanged -= UpdatePurchasedSkills;
        }
    }
}
