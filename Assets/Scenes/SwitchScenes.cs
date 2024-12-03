using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class SwitchScenes : MonoBehaviour
    {
        public static void SwitchToLogin() => SceneManager.LoadScene(SceneNames.Login);
        public static void SwitchToMainWorld()
        {
            bool inEditor = false;
#if UNITY_EDITOR
            inEditor = false;
#endif
            if(inEditor) SceneLoader.Instance.LoadScene(SceneNames.MainTwo);
            else SceneLoader.Instance.LoadScene(SceneNames.Main);
        }

        public static void SwitchToMainWorld2() => SceneLoader.Instance.LoadScene(SceneNames.MainTwo);
        public static void SwitchToPlayerHouseScene() => SceneLoader.Instance.LoadScene(SceneNames.House);
        public static void SwitchToWordFactoryLoadingScene() => SceneLoader.Instance.LoadScene(SceneNames.FactoryLoading);
        public static void SwitchToArcadeAsteroidScene() => SceneManager.LoadScene(SceneNames.ArcadeAsteroid);
        public static void SwitchToArcadeAsteroidLoseScene() => SceneManager.LoadScene(SceneNames.ArcadeAsteroidLoseScreen);
        public static void SwitchToArcadeCatClock() => SceneManager.LoadScene(SceneNames.ArcadeCatClock);
        public static void SwitchToRacingScene() => SceneLoader.Instance.LoadScene(SceneNames.House);
        public static void SwitchToPathOfDangerAllModesSelector() => SceneManager.LoadScene(SceneNames.PathOfDangerAllModesSelector);
        public static void SwitchToBoxGamePhase1() => SceneManager.LoadScene(SceneNames.Box1);
        public static void SwitchToBoxGamePhase2() => SceneManager.LoadScene(SceneNames.Box2);
        public static void SwitchToPathOfDangerAllModesSelector_Words() => SceneManager.LoadScene(SceneNames.PathOfDangerAllModesSelector_Words);
        public static void SwitchToPathOfDanger() => SceneManager.LoadScene(SceneNames.PathOfDanger);
        public static void SwitchToPathOfDangerLoseScene() => SceneManager.LoadScene(SceneNames.PathOfDangerLoseScreen);
        public static void SwitchToSymbolEaterScene() => SceneLoader.Instance.LoadScene(SceneNames.Eater);    
        public static void SwitchToWordFactory() => SceneLoader.Instance.LoadScene(SceneNames.Factory);
        public static void SwitchToSymbolEaterLoaderScene() => SceneLoader.Instance.LoadScene(SceneNames.EaterLoading);
        public static void SwitchToTowerScene() => SceneLoader.Instance.LoadScene(SceneNames.Tower);
        public static void SwitchToTowerLoaderScene() => SceneLoader.Instance.LoadScene(SceneNames.TowerLoading);
        public static void SwitchToTowerLoaderScene_Words() => SceneLoader.Instance.LoadScene(SceneNames.TowerLoading_Words);
        public static void SwitchToRacerLoaderScene() => SceneLoader.Instance.LoadScene(SceneNames.RacerLoading);
        public static void SwitchToRacerScene() => SceneLoader.Instance.LoadScene(SceneNames.Racer);
        public static void SwitchToArcadeScene() => SceneLoader.Instance.LoadScene(SceneNames.Arcade);
        public static void SwitchToArcadeBalloonScene() => SceneLoader.Instance.LoadScene(SceneNames.ArcadeBalloon);
        public static void SwitchToLetterGardenLoaderScene() => SceneLoader.Instance.LoadScene(SceneNames.LetterLoading);
        public static void SwitchToLetterGardenScene() => SceneLoader.Instance.LoadScene(SceneNames.Letter);
        public static void SwitchToBankFrontScene() => SceneLoader.Instance.LoadScene(SceneNames.Bank);
        public static void SwitchToBankBackScene() => SceneLoader.Instance.LoadScene(SceneNames.BankBack);
        public static void SwitchToMinigameLoadingScene() => SceneLoader.Instance.LoadScene(SceneNames.MinigameLoading);
        public static void SwitchToMechnicView() => SceneLoader.Instance.LoadScene(SceneNames.CarShowCaseRoom);
        public static void SwitchToHighscore() => SceneLoader.Instance.LoadScene(SceneNames.HighScores);
        public static void SwitchToLeaderBoard() => SceneLoader.Instance.LoadScene(SceneNames.LeaderBoard);
        public static void SwitchToAllTimeHighscore() => SceneLoader.Instance.LoadScene(SceneNames.MultiPlayerHighScores);
        public static void SwitchToBankFrontLoadingScene() => SceneLoader.Instance.LoadScene(SceneNames.BankFrontLoading);
        public static void SwitchToMultiplayerLobbyScene() => SceneLoader.Instance.LoadScene(SceneNames.MultiplayerLobby);
        public static void SwitchToProductionLine() => SceneLoader.Instance.LoadScene(SceneNames.WordLine);
        public static void SwitchToProductionLineLoadingScene() => SceneLoader.Instance.LoadScene(SceneNames.WordLineLoad);
        public static void SwitchToPizzaRestaurant() => SceneLoader.Instance.LoadScene(SceneNames.PizzaRestaurant);
        public static void SwitchToBoxBreak() => SceneLoader.Instance.LoadScene(SceneNames.BoxBreaker);
        public static void SwitchToBoxAssembly() => SceneLoader.Instance.LoadScene(SceneNames.BoxAssembly);
        public static void SwitchToPlaneGame() => SceneLoader.Instance.LoadScene(SceneNames.FlyingGame);



    }
}
