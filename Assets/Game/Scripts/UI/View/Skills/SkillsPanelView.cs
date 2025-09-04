using System.Collections.Generic;
using System.Linq;
using Brickworks.Model.Skills;
using Brickworks.UI.Model.Skills;
using UnityEngine;

namespace Brickworks.UI.View.Skills
{
    public class SkillsPanelView : MonoBehaviour
    {
        [SerializeField] 
        private RectTransform _content;

        [SerializeField] 
        private SkillView _skillViewPrefab;

        [SerializeField] 
        private SkillTooltipView _tooltipView;

        [SerializeField] 
        private GameObject _linePrefab;

        [SerializeField] 
        private float _firstRingRadius = 300f;
        
        [SerializeField] 
        private float _stepRadius = 200f;

        private ISkillsScreenViewModel _model;
        private readonly Dictionary<string, SkillView> _spawned = new();
        private SkillViewModel[] _skills;

        public void Init(SkillViewModel[] skills, ISkillsScreenViewModel model)
        {
            _model = model;
            _skills = skills;
            _spawned.Clear();

            SkillItemData baseSkill = null;
            foreach (var skill in model.SkillsModel.Skills)
            {
                if (skill.Id == SkillsModel.BASE_ID)
                {
                    baseSkill = skill;
                    break;
                }
            }
            if (baseSkill == null) return;

            var baseView = SpawnSkill(baseSkill, Vector2.zero);
            _spawned.Add(baseSkill.Id, baseView);

            var firstLevel = new List<SkillItemData>();
            foreach (var skill in model.SkillsModel.Skills)
            {
                if (skill.PreviousSkills == null) continue;
                foreach (var prev in skill.PreviousSkills)
                {
                    if (prev.Id == SkillsModel.BASE_ID)
                    {
                        firstLevel.Add(skill);
                        break;
                    }
                }
            }

            int count = firstLevel.Count;
            for (int i = 0; i < count; i++)
            {
                float angle = i * Mathf.PI * 2f / count;
                Vector2 pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _firstRingRadius;

                var view = SpawnSkill(firstLevel[i], pos);
                _spawned[firstLevel[i].Id] = view;

                DrawConnection(baseView.GetComponent<RectTransform>().anchoredPosition, pos);
                SpawnChildren(firstLevel[i], pos, angle, _stepRadius);
            }
        }

        private void SpawnChildren(SkillItemData parent, Vector2 parentPos, float parentAngle, float radius)
        {
            if (parent.NextSkills == null || parent.NextSkills.Length == 0)
                return;

            int count = parent.NextSkills.Length;
            float sector = Mathf.Deg2Rad * 60;
            float step = sector / Mathf.Max(1, count - 1);

            float startAngle = parentAngle - sector / 2f;

            for (int i = 0; i < count; i++)
            {
                var child = parent.NextSkills[i];
                if (_spawned.ContainsKey(child.Id)) continue;

                float angle = startAngle + step * i;
                Vector2 pos = parentPos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

                var view = SpawnSkill(child, pos);
                _spawned[child.Id] = view;
                
                DrawConnection(parentPos, pos);
                SpawnChildren(child, pos, angle, radius);
            }
        }

        private SkillView SpawnSkill(SkillItemData data, Vector2 pos)
        {
            var view = Instantiate(_skillViewPrefab, _content);
            var rect = view.GetComponent<RectTransform>();
            rect.anchoredPosition = pos;

            view.Init(_skills.First(skill => data.Id == skill.Id), () => ShowTooltip(data.Id, rect));
            return view;
        }

        private void DrawConnection(Vector2 start, Vector2 end)
        {
            if (_linePrefab == null) return;

            var lineObj = Instantiate(_linePrefab, _content);
            var rectTransform = lineObj.GetComponent<RectTransform>();
            rectTransform.SetSiblingIndex(0);

            Vector2 direction = end - start;
            float distance = direction.magnitude;

            rectTransform.sizeDelta = new Vector2(distance, rectTransform.sizeDelta.y);
            rectTransform.anchoredPosition = (start + direction / 2f);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        private void ShowTooltip(string id, RectTransform skillRect)
        {
            var isAbleToPurchase = _model.SkillsModel.IsAbleToPurchase(id);
            var isAbleToForget = _model.SkillsModel.IsAbleToForget(id);

            _tooltipView.Show(id, 
                skillRect, 
                isAbleToPurchase ? _model.PurchaseSkill : null,
                isAbleToForget ? _model.ForgetSkill : null);
        }
    }
}
