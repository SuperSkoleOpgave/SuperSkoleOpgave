using System.Collections.Generic;
using CORE.Scripts;
using Scenes._50_Minigames._56_WordFactory.Scripts.Managers;
using UnityEngine;

namespace Scenes._50_Minigames._56_WordFactory.Scripts.GameModeStrategy
{
    public class SingleGearWordCheckStrategy : IWordCheckStrategy
    {
        public bool ValidateWord(string word, List<Transform> teeth)
        {
            throw new System.NotImplementedException();
        }

        public void Initialize(
            WordValidator wordValidator,
            HashSet<string> createdWords,
            ScoreManager scoreManager,
            INotificationDisplay notification)
        {
            throw new System.NotImplementedException();
        }
    }
}
