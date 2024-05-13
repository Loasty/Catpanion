using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{ 
    public enum DialogueSpeed { NORMAL, SLOW, VERYSLOW, FAST, VERYFAST };

    public enum CatType { NONE, WHITE, TABBY, CALICO, BLACK };
   
    public enum Emotes { NONE, HEART, SIGH, ANNOYED  };

    public enum EmoteEffect { NONE, WIGGLE, RESIZE, GO_UP, GO_DOWN };

    public enum Locations { NONE, FIELD, BARN };

    public enum Actions { SIT = 30, WALK = 60, GREET = 75, SWIPE = 80, AFFECTION = 85, ATTENTION = 90, MEOW = 95, PLAY = 100 }



    public enum MenuOptions_Main { Main, Cats, Closet, Visit, Options }
    public enum MenuOptions_Cats { Manage, Info, Adopt }
    public enum MenuOptions_Closet { }
    public enum MenuOptions_Visit { Store, Salon, Friend }
    public enum MenuOptions_Options { Settings, Help, Exit }
}
