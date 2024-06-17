using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enums;

public class DesktopMenuController : MonoBehaviour
{
    private static DesktopMenuController instance;

    public static DesktopMenuController Instance { get { return instance; } }


    [Header("Lists Of Menu Options")]
    public List<DesktopMenuItem> menuItems = new List<DesktopMenuItem>();
    public List<DesktopMenuItem> main_Items = new List<DesktopMenuItem>();
    public List<DesktopMenuItem> cat_Items = new List<DesktopMenuItem>();
    public List<DesktopMenuItem> closet_Items = new List<DesktopMenuItem>();
    public List<DesktopMenuItem> visit_Items = new List<DesktopMenuItem>();
    public List<DesktopMenuItem> option_Items = new List<DesktopMenuItem>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        
    }

    public void MenuButtonClicked(DesktopMenuItem item)
    {
        switch (item.menuType)
        {
            //Main Button Options
            case MenuOptions_Main.Main:
                switch (item.subMenuType)
                {
                    case MenuOptions_Main.Main:
                        Debug.Log("Main Button Pressed");
                        ToggleListOfButtons(main_Items);
                        break;
                    case MenuOptions_Main.Cats:
                        ToggleListOfButtons(cat_Items);
                        break;
                    case MenuOptions_Main.Closet:
                        ClosetMenuController.Instance.ToggleMenu();
                        ToggleListOfButtons(closet_Items);
                        break;
                    case MenuOptions_Main.Visit:
                        ToggleListOfButtons(visit_Items);
                        break;
                    case MenuOptions_Main.Options:
                        ToggleListOfButtons(option_Items);
                        break;
                }
                break;

            //Cat Menu Button Options
            case MenuOptions_Main.Cats:
                switch (item.menuType_Cats)
                {
                    case MenuOptions_Cats.Manage: 
                        break;
                    case MenuOptions_Cats.Info:
                        break;
                    case MenuOptions_Cats.Adopt:
                        break;
                }
                RetractAllButtons();
                break;

            //Closet Menu Button Options
            case MenuOptions_Main.Closet:
                switch (item.menuType_Closet)
                {
                    default:
                        //REFER TO 'case MenuOptions_Main.Main:' FOR CLOSET MENU OPTION
                        //CLOSET DOES NOT CONTAIN SUB_BUTTONS AT THIS TIME, IF IT CHANGES MOVE
                        //TO THIS AREA OF CODE
                        break;
                }
                RetractAllButtons();
                break;

            //Visit Menu Button Options
            case MenuOptions_Main.Visit:
                switch (item.menuType_Visit)
                {
                    case MenuOptions_Visit.Store:
                        break;
                    case MenuOptions_Visit.Salon:
                        break;
                    case MenuOptions_Visit.Friend:
                        break;
                }
                RetractAllButtons();
                break;

            //Options Menu Button Options
            case MenuOptions_Main.Options:
                switch (item.menuType_Options)
                {
                    case MenuOptions_Options.Settings:
                        Settings.Instance.ToggleSettingsMenu();
                        break;
                    case MenuOptions_Options.Help:
                        break;
                    case MenuOptions_Options.Exit:
                        Settings.Instance.MainMenu();
                        break;
                }
                RetractAllButtons();
                break;

            //Fallback Case, Probably an error!
            default: 
                
                break;
        }
    }

    void RetractAllButtons()
    {
        ToggleListOfButtons(main_Items, false);
        ToggleListOfButtons(cat_Items, false);
        ToggleListOfButtons(closet_Items, false);
        ToggleListOfButtons(visit_Items, false);
        ToggleListOfButtons(option_Items, false);
    }

    void ToggleListOfButtons(List<DesktopMenuItem> menuItems)
    {
        if(menuItems.Count == 0) { return; }

        foreach (DesktopMenuItem menuButton in menuItems)
        {
            menuButton.ToggleButton();
        }
    }
    void ToggleListOfButtons(List<DesktopMenuItem> menuItems, bool mode)
    {
        if (menuItems.Count == 0) { return; }

        foreach (DesktopMenuItem menuButton in menuItems)
        {
            if(mode) menuButton.EnableButton();
            else menuButton.DisableButton();
        }
    }

    public void SubscribeMenuItem(DesktopMenuItem item)
    {
        menuItems.Add(item);

        switch (item.menuType)
        {
            case MenuOptions_Main.Main: if (item.subMenuType != MenuOptions_Main.Main) main_Items.Add(item); break;
            case MenuOptions_Main.Cats: cat_Items.Add(item); break;
            case MenuOptions_Main.Closet: closet_Items.Add(item); break;
            case MenuOptions_Main.Visit: visit_Items.Add(item); break;
            case MenuOptions_Main.Options: option_Items.Add(item); break;
        }

        item.button.onClick.AddListener(delegate { MenuButtonClicked(item); });
    }

    public void UnSubscribeMenuItem(DesktopMenuItem item)
    {
        menuItems.Remove(item);

        switch (item.menuType)
        {
            case MenuOptions_Main.Main: main_Items.Remove(item); break;
            case MenuOptions_Main.Cats: cat_Items.Remove(item); break;
            case MenuOptions_Main.Closet: closet_Items.Remove(item); break;
            case MenuOptions_Main.Visit: visit_Items.Remove(item); break;
            case MenuOptions_Main.Options: option_Items.Remove(item); break;
        }

        item.button?.onClick.RemoveListener(delegate { MenuButtonClicked(item); });
    }
}
