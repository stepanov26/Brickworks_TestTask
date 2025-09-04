using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brickworks.Model.Skills
{
    public class SkillsModel
    {
        public const string BASE_ID = "base_0";
        
        private const string PURCHASED_SKILLS_KEY = nameof(SkillsModel) + "_purchased_skills";
        private const string SKILLS_DATA_PATH = "Skills/";

        private readonly List<string> _purchasedSkills;
        private readonly ResourcesModel _resourcesModel;
        private readonly SkillItemData[] _skills;

        public SkillItemData[] Skills => _skills;

        public event Action PurchasedSkillsChanged;

        public SkillsModel(ResourcesModel resourcesModel)
        {
            _resourcesModel = resourcesModel;
            _purchasedSkills = SaveManager.LoadData(PURCHASED_SKILLS_KEY, new List<string>{ BASE_ID });

            _skills = Resources.LoadAll<SkillItemData>(SKILLS_DATA_PATH);
        }

        public bool TryPurchaseSkill(string id)
        {
            var item = _skills.FirstOrDefault(data => data.Id == id);

            if (item == null)
            {
                Debug.LogError($"Skill {id} not found");
                return false;
            }

            if (_resourcesModel.GetResourceAmount(item.Price.Type) < item.Price.Amount)
            {
                return false;
            }

            PurchaseSkill(item);
            return true;
        }

        public bool TryForgetSkill(string id)
        {
            var item = _skills.FirstOrDefault(data => data.Id == id);

            if (item == null)
            {
                Debug.LogError($"Skill {id} not found");
                return false;
            }

            if (!IsSkillPurchased(item.Id))
            {
                Debug.LogError($"Skill {id} not purchased");
                return false;
            }

            ForgetSkill(item);
            return true;
        }

        public bool IsSkillPurchased(string id)
        {
            return _purchasedSkills.Contains(id);
        }

        public bool IsAbleToPurchase(string id)
        {
            return !IsSkillPurchased(id) && Skills.First(skill => skill.Id == id).PreviousSkills
                .Any(skill => IsSkillPurchased(skill.Id));;
        }

        public bool IsAbleToForget(string id)
        {
            return id != BASE_ID && IsSkillPurchased(id) && Skills.First(skill => skill.Id == id).NextSkills
                .All(skill => !IsSkillPurchased(skill.Id));;
        }

        public void ForgetAllSkills()
        {
            foreach (var skillId in _purchasedSkills)
            {
                var skill = _skills.First(data => data.Id == skillId);
                _resourcesModel.AddResource(skill.Price);
            }
            
            _purchasedSkills.Clear();
            _purchasedSkills.Add(BASE_ID);
            SaveManager.SaveData(PURCHASED_SKILLS_KEY, _purchasedSkills);
        }

        private void PurchaseSkill(SkillItemData data)
        {
            _resourcesModel.UseResource(data.Price);

            _purchasedSkills.Add(data.Id);
            PurchasedSkillsChanged?.Invoke();
            SaveManager.SaveData(PURCHASED_SKILLS_KEY, _purchasedSkills);
        }

        private void ForgetSkill(SkillItemData data)
        {
            _resourcesModel.AddResource(data.Price);

            _purchasedSkills.Remove(data.Id);
            PurchasedSkillsChanged?.Invoke();
            SaveManager.SaveData(PURCHASED_SKILLS_KEY, _purchasedSkills);
        }
    }
}