using CORE;
using CORE.Scripts;
using CORE.Scripts.Game_Rules;

namespace Scenes._50_Minigames.Gamemode
{

    public class PlaneGameSetter : IGameModeSetter
    {



        public (IGameRules, IGenericGameMode) DetermineGamemodeAndGameRulesToUse(int level)
        {

            if (GameManager.Instance.dynamicDifficultyAdjustment.IsLanguageUnitTypeUnlocked(LanguageUnitProperty.word))
            {
                return (new DynamicGameRules(), null);
            }
            //WordproductionLine only supports Words
            else
            {
                return (null, null);
            }
        }

        public IGenericGameMode SetMode(int level)
        {
            return null;
        }

        public IGenericGameMode SetMode(string gamemode)
        {
            return null;
        }

        public IGameRules SetRules(int level)
        {
            return null;
        }

        public IGameRules SetRules(string gamerules)
        {
            return null;
        }

    }
}