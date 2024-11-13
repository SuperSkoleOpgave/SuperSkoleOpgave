using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[Serializable]
public enum property
{
    testProperty,
    testProperty1,
    testProperty2,
    testProperty3,
    testProperty4,
    testProperty5,
    testProperty6,
    testProperty7,
    testProperty8,
    testProperty9,
    testProperty10,
    testProperty11,
    testProperty12,
    testProperty13,
    testProperty14,
    testProperty15,
    testProperty16,
    testProperty17,
    testProperty18,
    testProperty19,
    word,
    letter

}
[Serializable]
public class Property
{
    public property property;
    [NonSerialized]
    public float weight;
    public int levelLock;
}
