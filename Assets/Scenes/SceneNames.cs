namespace Scenes
{
    /// <summary>
    /// Static class containing the names of all scenes used in the game.
    /// </summary>
    public static class SceneNames
    {
        // System and initialization scenes
        public const string Boot = "00-Bootstrapper";
        public const string Splash = "01-SplashScene";

        // Login and startup scenes
        public const string Login = "02-LoginScene";
        public const string Start = "03-StartScene";
        public const string Tutorial = "04-TutorialScene";  //

        // Ending and miscellaneous
        public const string Credits = "09-EndingCredits";

        // Player related scenes
        public const string Player = "10-PlayerScene";
        public const string House = "11-PlayerHouse";
        public const string PlayerUIScene = "13-PlayerUIScene";
        public const string Profile = "14-ProfileScene"; //


        // Main gameplay and utilities
        public const string Main = "20-MainWorld";
        public const string Pause = "21-PauseMenu";  //
        public const string Options = "22-OptionScene";  //
        public const string CarShowCaseRoom ="23-CarMechanicsScene";  //
        public const string HighScores = "24-HighScoreScene";
        public const string Help = "25-HelpScene";  //
        public const string Story = "26-Cutscene";  //
        public const string Settings = "28-SettingsScene"; //
        public const string MainTwo = "20-MainWorldTwo";

        // NPC and interaction scenes
        public const string NPCInteractions = "30-NPCInteractionScene"; // might be split out

        public const string Shop = "40-ShopScene";
        public const string Wardrope = "41-WardropeScene";

        // Minigames 50-70
        public const string Gamemode = "50-GameModeSelector";
        public const string Box1 = "502-Box";
        public const string Box2 = "5021-Box2";
        public const string LetterLoading = "51-LetterGarden";
        public const string Letter = "52-LetterGarden";
        public const string EaterLoading = "53-SymbolEater";
        public const string Eater = "54-SymbolEater";
        public const string FactoryLoading = "55-WordFactoryLoadingScene";
        public const string Factory = "56-WordFactory";
        public const string RacerLoading = "57-RacingGame";
        public const string Racer = "58-RacingGame";
        public const string BankFrontLoading = "59-BankFrontLoading";
        public const string BankBack = "60-BankBack";
        public const string Bank = "61-BankFront";
        public const string BreakinLoading = "62-BreakInGame";
        
        public const string Breakin = "63-BreakInGame";
        public const string TowerLoading = "64-MonsterTower";
        public const string TowerLoading_Words = "64-MonsterTower_Words";
        public const string Tower = "65-MonsterTower";
        public const string WordLineLoad = "66-WordLineLoadScene";
        public const string WordLine = "67-WordProductionLineMinigame";
        public const string PathOfDangerAllModesSelector = "68-PathOfDangerAllModesSelector";
        public const string PathOfDangerAllModesSelector_Words = "68-PathOfDangerAllModesSelector_Words";
        public const string PathOfDanger = "69-PathOfDanger";
        public const string PathOfDangerLoseScreen = "PathOfDangerLoseScreen";
        public const string MinigameLoading = "LevelSelect";
        public const string PizzaRestaurant = "511-PizzaRestaurant";
        public const string FishingGame = "513-FishingGame";
        public const string BoxBreaker = "502-Box";
        public const string BoxAssembly = "5021 - Box 2";
        public const string FlyingGame = "512-PlaneGame";
        public const string PlaneGame = "512-PlaneGame";

        //Arcade 70
        public const string Arcade = "70-ArcadeScene";
        public const string ArcadeBalloon = "71-BalloonPopper";
        public const string ArcadeAsteroid = "72-AsteroidShooting";
        public const string ArcadeAsteroidLoseScreen = "AsteroidLoseScreen";
        public const string ArcadeCatClock = "73-CatClockMinigame";

        // Multiplayer features
        public const string MultiplayerLobby = "80-MultiplayerLobby";
        public const string Matchmaking = "81-MatchmakingScene";
        public const string LeaderBoard = "88-LeaderBoard";
        public const string MultiPlayerHighScores = "89-MultiPlayerHighScoreScene";
        
    }
}
