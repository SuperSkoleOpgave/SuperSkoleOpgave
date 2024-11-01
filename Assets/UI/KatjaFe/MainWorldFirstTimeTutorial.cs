using CORE;
using Scenes._50_Minigames._58_MiniRacingGame.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWorldFirstTimeTutorial : MonoBehaviour
{
    private KatjaFe katjafe;

    [SerializeField] private AudioClip Hey;
    [SerializeField] private AudioClip Explane;

    private void Start()
    {
        katjafe = GetComponent<KatjaFe>();
        if (GameManager.Instance.PlayerData.TutorialMainWorldFirstTime)
        {

            katjafe.Initialize(false, Explane);
            return;
        }
        katjafe.Initialize(false, Explane);
        Speak();
    }

    private void Speak()
    {
        katjafe.KatjaIntro(() =>
        {
            katjafe.KatjaSpeak(Hey, () =>
            {
                katjafe.KatjaSpeak(Explane, () =>
                {
                    katjafe.KatjaExit();
                    GameManager.Instance.PlayerData.TutorialMainWorldFirstTime = true;
                });
            });
        });
    }
}
