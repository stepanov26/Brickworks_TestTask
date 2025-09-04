using System;
using Brickworks.UI.View.Skills;
using Brickworks.Utils;

namespace Brickworks.UI.Model.Skills
{
    public class SkillViewModel : ISkillViewModel
    {
        public string Id { get; }
        public string Title { get; }
        public IReactiveProperty<bool> IsPurchased => _isPurchased;

        private readonly ReactiveProperty<bool> _isPurchased;

        public SkillViewModel(string id, string title, bool isPurchased)
        {
            Id = id;
            Title = title;
            _isPurchased = new ReactiveProperty<bool>(isPurchased);
        }
        
                
        public void Purchase()
        {
            _isPurchased.Value = true;
        }

        public void Forget()
        {
            _isPurchased.Value = false;
        }
    }
}