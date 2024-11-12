using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public enum property
{
    word,
    letter,
    vowel,
    consonant,
    testProperty
}
[Serializable]
public class Property
{
    public property property;
    [NonSerialized]
    public float weight;
    public int levelLock;
}
