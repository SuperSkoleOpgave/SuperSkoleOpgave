using CORE.Scripts;
using CORE;
using Scenes._50_Minigames._67_WordProductionLine.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGameTutorial : MonoBehaviour
{
    private KatjaFe katjafe;

    [SerializeField] private AudioClip Hey;
    [SerializeField] private AudioClip Explane;

    private bool waitingForInput = false;
    private PlaneGameManager controller;

    private void Start()
    {
        katjafe = GetComponent<KatjaFe>();
        if (GameManager.Instance.PlayerData.TutorialPlaneGame)
        {

            katjafe.Initialize(false, Explane);
            return;
        }
        katjafe.Initialize(true, Explane);
        controller = FindFirstObjectByType<PlaneGameManager>();
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
                    waitingForInput = true;
                });
            });
        });
    }

    private void Update()
    {
        if (waitingForInput)
        {
            if (controller.isTutorialOver)
            {
                waitingForInput = false;
                katjafe.KatjaSpeak(CongratsAudioManager.GetAudioClipFromDanishSet(Random.Range(0, CongratsAudioManager.GetLenghtOfAudioClipDanishList())), () =>
                {
                    katjafe.KatjaExit();
                    GameManager.Instance.PlayerData.TutorialPlaneGame = true;
                });
            }
        }
    }
}
