using System.Collections;
using System.Collections.Generic;
using Analytics;
using CORE;
using Scenes;
using Scenes._50_Minigames.Gamemode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneSetup : MonoBehaviour
{
    [SerializeField]private int sceneID;

    /// <summary>
    /// Adds OnSceneLoaded to be run then a scene is loaded and begins loading the gamemode selector.
    /// </summary>
    public void Load()
    {
        DynamicDifficultyAdjustment DDA = GameManager.Instance.dynamicDifficultyAdjustment;
        //loads the general level selector and prepares the setup method if the player level is low enough 
        if(DDA.GetPlayerLevel() < 7)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SwitchScenes.SwitchToMinigameLoadingScene();
        }
        //loads the specific gamemode selector for the given sceneID
        else 
        {
            switch (sceneID)
            {

                 
                case 0:

                    if (DDA.IsLanguageUnitTypeUnlocked(LanguageUnitProperty.letter))
                    {
                        SwitchScenes.SwitchToTowerLoaderScene();
                    }
                    if (DDA.IsLanguageUnitTypeUnlocked(LanguageUnitProperty.word))
                    {
                        SwitchScenes.SwitchToTowerLoaderScene_Words();
                    }

                    break;
                case 1:
                    SwitchScenes.SwitchToSymbolEaterLoaderScene();
                    break;
                case 2:
                    SwitchScenes.SwitchToLetterGardenLoaderScene();
                    break;
                case 5:
                    if (DDA.IsLanguageUnitTypeUnlocked(LanguageUnitProperty.letter))
                    {
                        SwitchScenes.SwitchToPathOfDangerAllModesSelector();
                    }
                    
                    if(DDA.IsLanguageUnitTypeUnlocked(LanguageUnitProperty.word))
                    {
                        SwitchScenes.SwitchToPathOfDangerAllModesSelector_Words();
                    }
                    break;
                case 6:
                    SwitchScenes.SwitchToProductionLineLoadingScene();
                    break;

                case 7:
                    SwitchScenes.SwitchToPizzaRestaurant();

                    break;
                case 8:
                    SwitchScenes.SwitchToBoxGamePhase1();
                    break;
                default:
                    Debug.LogError("unknown sceneID");
                    break;
            }
        }
        
    }

    /// <summary>
    /// Finds the SetGameModeAndDestroy script and sets its sceneid. Removes itself as a method to be run on sceneload afterwards.
    /// </summary>
    /// <param name="scene">not used</param>
    /// <param name="loadSceneMode">not used</param>
    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        GameObject gamemode = GameObject.FindGameObjectWithTag("GamemodeSelect");
        
        gamemode.GetComponent<SetGameModeAndDestroy>().sceneID = sceneID;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
