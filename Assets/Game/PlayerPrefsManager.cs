using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour {


    const string MASTER_VOLUME_KEY = "master_volume";
    const string DIFFICULTY_KEY = "difficulty";
    const string LEVEL_KEY = "level_unlocked_";

    const string CARD1_INEDEX_KEY = "card1_index";
    const string CARD1_TYPE_KEY = "card1_type";
    const string CARD2_INEDEX_KEY = "card2_index";
    const string CARD2_TYPE_KEY = "card2_type";
    const string CARD3_INEDEX_KEY = "card3_index";
    const string CARD3_TYPE_KEY = "card3_type";
    const string CARD4_INEDEX_KEY = "card4_index";
    const string CARD4_TYPE_KEY = "card4_type";
    const string CARD5_INEDEX_KEY = "card5_index";
    const string CARD5_TYPE_KEY = "card5_type";
    const string CARD6_INEDEX_KEY = "card6_index";
    const string CARD6_TYPE_KEY = "card6_type";
    const string CARD7_INEDEX_KEY = "card7_index";
    const string CARD7_TYPE_KEY = "card7_type";
    const string CARD8_INEDEX_KEY = "card8_index";
    const string CARD8_TYPE_KEY = "card8_type";
    const string CARD9_INEDEX_KEY = "card9_index";
    const string CARD9_TYPE_KEY = "card9_type";
    const string CARD10_INEDEX_KEY = "card10_index";
    const string CARD10_TYPE_KEY = "card10_type";
    const string CARD11_INEDEX_KEY = "card11_index";
    const string CARD11_TYPE_KEY = "card11_type";
    const string CARD12_INEDEX_KEY = "card12_index";
    const string CARD12_TYPE_KEY = "card12_type";
    const string CARD13_INEDEX_KEY = "card13_index";
    const string CARD13_TYPE_KEY = "card13_type";
    const string CARD14_INEDEX_KEY = "card14_index";
    const string CARD14_TYPE_KEY = "card14_type";
    const string CARD15_INEDEX_KEY = "card15_index";
    const string CARD15_TYPE_KEY = "card15_type";
    const string CARD16_INEDEX_KEY = "card16_index";
    const string CARD16_TYPE_KEY = "card16_type";
    const string CARD17_INEDEX_KEY = "card17_index";
    const string CARD17_TYPE_KEY = "card17_type";
    const string CARD18_INEDEX_KEY = "card18_index";
    const string CARD18_TYPE_KEY = "card18_type";
    const string CARD19_INEDEX_KEY = "card19_index";
    const string CARD19_TYPE_KEY = "card19_type";
    const string CARD20_INEDEX_KEY = "card20_index";
    const string CARD20_TYPE_KEY = "card20_type";


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


    public static void SetDeck(int[] CardIndex, string[] CardType)
    {
        for (int i = 0; i < 20; i++)
        {
            string Key_index = "CARD_INDEX" + (i + 1).ToString() + "_KEY";
            string Key_type = "CARD_TYPE" + (i + 1).ToString() + "_KEY";
            PlayerPrefs.SetInt(Key_index, CardIndex[i]);
            PlayerPrefs.SetString(Key_type, CardType[i]);
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

    public static int[] ReturnDeckIndex()
    {
        int[] CardsIndex = new int[20];
        for (int i = 0; i < 20; i++)
        {

            string Key_index = "CARD_INDEX" + (i + 1).ToString() + "_KEY";
            CardsIndex[i] = PlayerPrefs.GetInt(Key_index);
        }
        return CardsIndex;
    }

    public static string[] ReturnDeckType()
    {
        string[] CardsType = new string[20];
        for (int i = 0; i < 20; i++)
        {
            string Key_type = "CARD_TYPE" + (i + 1).ToString() + "_KEY";
            CardsType[i] = PlayerPrefs.GetString(Key_type);
        }
        return CardsType;
    }



    public static float GetDifficulty()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
    }


}
