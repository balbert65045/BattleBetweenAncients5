using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour {


    const string MASTER_VOLUME_KEY = "master_volume";
    const string DIFFICULTY_KEY = "difficulty";
    const string LEVEL_KEY = "level_unlocked_";

    const string CARD1_KEY = "card1";
    const string CARD2_KEY = "card2";
    const string CARD3_KEY = "card3";
    const string CARD4_KEY = "card4";
    const string CARD5_KEY = "card5";
    const string CARD6_KEY = "card6";
    const string CARD7_KEY = "card7";
    const string CARD8_KEY = "card8";
    const string CARD9_KEY = "card9";
    const string CARD10_KEY = "card10";
    const string CARD11_KEY = "card11";
    const string CARD12_KEY = "card12";
    const string CARD13_KEY = "card13";
    const string CARD14_KEY = "card14";
    const string CARD15_KEY = "card15";
    const string CARD16_KEY = "card16";
    const string CARD17_KEY = "card17";
    const string CARD18_KEY = "card18";
    const string CARD19_KEY = "card19";
    const string CARD20_KEY = "card20";


    public static void SetMasterVolume(float volume)
    {
        if (volume >= 0.0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        }

        else
        {
            Debug.LogError("Master volume out of range");
        }
    }


    public static void SetDeck(int[] CardIndex)
    {
        for (int i = 0; i < 20; i++)
        {
            string Key = "CARD" + (i + 1).ToString() + "_KEY";
            Debug.Log(Key);
            PlayerPrefs.SetInt(Key, CardIndex[i]);
        }
    }


    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static void UnlockLevel(int level)
    {
        if (level <= SceneManager.sceneCountInBuildSettings - 1)
        {
            PlayerPrefs.SetInt(LEVEL_KEY + level.ToString(), 1); // Use 1 for True
        }
        else
        {
            Debug.LogError("Trying to unlock level not in build order");
        }
       
    }

    public static bool IsLevelUnlocked(int level)
    {
        int Unlocked = PlayerPrefs.GetInt(LEVEL_KEY + level.ToString());
        if (Unlocked == 1)
        {
            return true;
        }
        else
        {
            Debug.LogError("Trying to query level not in build order");
            return false;
        }
    }

    public static int[] ReturnDeck()
    {
        int[] CardsIndex = new int[20];
        for (int i = 0; i < 20; i++)
        {
            string Key = "CARD" + (i + 1).ToString() + "_KEY";
            Debug.Log(Key);
            CardsIndex[i] = PlayerPrefs.GetInt(Key);
        }
        return CardsIndex;
    }


    public static float GetDifficulty()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
    }


}
