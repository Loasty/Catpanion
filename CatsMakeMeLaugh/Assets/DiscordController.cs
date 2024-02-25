using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordController : MonoBehaviour
{
    /////////////
    /// Instance
    /// 
    private static DiscordController instance;

    public static DiscordController Instance { get { return instance; } }

    public long applicationID = 1211121275590676512;
    public Discord.Discord discord;

    [Space]
    public string details = "Launching Catpanions";
    public string state = "Loading";
    [Space]
    public string largeImage = "game_logo";
    public string largeText = "Discord Tutorial";
    [Space]
    public long timeLaunched;

    private void Awake()
    {
        if(instance == null) instance = this;

        discord = new Discord.Discord(applicationID, (UInt64)Discord.CreateFlags.NoRequireDiscord);

        timeLaunched = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    private void OnDestroy()
    {
       discord?.Dispose();
    }

    void Update()
    {
        // Destroy the GameObject if Discord isn't running
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        details = "Playing with their Catpanions";
        state = "Active Catpanions: " + GameData.Instance.savedCats.cats.Count;
        UpdateStatus();
    }

    void UpdateStatus()
    {
        // Update Status every frame
        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = details,
                State = state,
                Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
                Timestamps =
                {
                    Start = timeLaunched
                }
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            // If updating the status fails, Destroy the GameObject
            Destroy(gameObject);
        }
    }
}
