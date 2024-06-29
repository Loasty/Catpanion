using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{ 
    
    //NOVEL RELATED
    public enum DialogueSpeed { NORMAL, SLOW, VERYSLOW, FAST, VERYFAST };
    public enum Locations { NONE, FIELD, BARN, ROAD};
    public enum Emotes { NONE, HEART, SIGH, ANNOYED };
    public enum EmoteEffect { NONE, WIGGLE, RESIZE, GO_UP, GO_DOWN };


    //CAT RELATED
    public enum CatType { NONE, WHITE, TABBY, CALICO, BLACK };
    public enum MeowType { Normal, Long, Loud, Quiet, Soft, Raspy };
    public enum CatPersonality { Average, Anxious, Confident, Energetic, Goofy, Gentle, Grumpy, Lazy, Timid, Wizard};
    //Average - Normal behaviours, no special traits
    //Anxious - More likely to be attention seeking and affectionate -- but requires more care at higher affection levels
    //Confident - Likes to walk around more often, almost like strutting behaviour
    //Energetic - Likes to play more often and sleep less
    //Goofy - Likes to emote and meow more often, maybe gets zoomie behaviour often?
    //Gentle - More likely to be Affectionate rather than Attention Seeking
    //Grumpy - More likely to be Attention Seeking than Affectionate
    //Lazy - Will sleep more than any other action
    //Timid - Will hide & less affectionate at lower affection levels, but more present and affectionate as it rises
    //Wizard - Cautious but smart -- Affection makes it speak humanly more

    //Average, 
    public enum CatMood { Normal };
    public enum CatGender { Unknown, Boy, Girl };
    public enum Actions { SIT = 30, WALK = 60, GREET = 75, SWIPE = 80, AFFECTION = 85, ATTENTION = 90, MEOW = 95, PLAY = 100 };
    public enum AdoptionDifficulty { Easy, Medium, Hard};


    //MENU RELATED
    public enum MenuOptions_Main { Main, Cats, Closet, Visit, Options };
    public enum MenuOptions_Cats { Manage, Info, Adopt };
    public enum MenuOptions_Closet { };
    public enum MenuOptions_Visit { Store, Salon, Friend };
    public enum MenuOptions_Options { Settings, Help, Exit };


    //DESKTOP GAMEPLAY RELATED
    public enum PlayAreaBorders { TOP, BOTTOM, LEFT, RIGHT };
}
