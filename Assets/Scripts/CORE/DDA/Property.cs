using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public enum languageUnitProperty
{
    wordWithA,
    wordWithB,
    wordWithC,
    wordWithD,
    wordWithE,
    wordWithF,
    wordWithG,
    wordWithH,
    wordWithI,
    wordWithJ,
    wordWithK,
    wordWithL,
    wordWithM,
    wordWithN,
    wordWithO,
    wordWithP,
    wordWithQ,
    wordWithR,
    wordWithS,
    wordWithT,
    wordWithU,
    wordWithV,
    wordWithW,
    wordWithX,
    wordWithY,
    wordWithZ,
    wordWithAE,
    wordWithOE,
    wordWithAA,
    consonant,
    vowel,
    word,
    letter

}
[Serializable]
public class LanguageUnitPropertyInfo
{
    public languageUnitProperty property;
    [NonSerialized]
    public float weight;
    public int levelLock;
}
