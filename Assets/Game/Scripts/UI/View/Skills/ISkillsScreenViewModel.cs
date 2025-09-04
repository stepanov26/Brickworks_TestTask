using Brickworks.Model.Skills;
using Brickworks.UI.Model.Skills;
using Brickworks.Utils;

namespace Brickworks.UI.View.Skills
{
    public interface ISkillsScreenViewModel
    {
        IReactiveProperty<int> SkillPointsCount { get; }
        IReactiveProperty<string> PurchasedSkills { get; }
        SkillViewModel[] Skills { get; }
        SkillsModel SkillsModel { get; }

        void PurchaseSkill(string id);
        void ForgetSkill(string id);
        void ForgetAllSkills();
        void AddSkillPoint();
    }
}