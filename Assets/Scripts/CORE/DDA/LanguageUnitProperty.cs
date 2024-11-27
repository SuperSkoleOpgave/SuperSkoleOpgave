using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public enum LanguageUnitProperty
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
    letterA,
    letterB,
    letterC,
    letterD,
    letterE,
    letterF,
    letterG,
    letterH,
    letterI,
    letterJ,
    letterK,
    letterL,
    letterM,
    letterN,
    letterO,
    letterP,
    letterQ,
    letterR,
    letterS,
    letterT,
    letterU,
    letterV,
    letterW,
    letterX,
    letterY,
    letterZ,
    letterAE,
    letterOE,
    letterAA,
    consonant,
    vowel,
    word,
    letter,
    softD,
    vowelConfuse,
    silentConsonant,
    doubleConsonant

}
[Serializable]
public class LanguageUnitPropertyInfo
{
    public LanguageUnitProperty property;
    public float weight;
    public int levelLock;
}
