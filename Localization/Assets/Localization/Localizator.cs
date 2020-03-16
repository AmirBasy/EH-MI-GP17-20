using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class Localizator
{
    static LocalizationCollection collections;

    static LanguageData currentLanguage;
    public static CultureInfo currentCultureInfo;
    static Dictionary<string, string> datas;

    static Localizator()
    {
        collections = Resources.Load<LocalizationCollection>("LocalizationCollection");
        Debug.LogFormat("Loading localization data: {0}", collections);
        SetLanguage();

    }

    static void SetLanguage()
    {
#if UNITY_EDITOR
        // ho forzato un linguaggio?
        if (collections.forcedLanguage!=null)
        {
            currentLanguage = collections.forcedLanguage;
        }
        else
#endif
        {
            string customLanguage = PlayerPrefs.GetString("LANGUAGE", null);
            if (!string.IsNullOrEmpty(customLanguage))
            {
                //....
                return;
            }

            // prendere il linguaggio di sistema
            CultureInfo ci = CultureInfo.InstalledUICulture;
            Debug.LogFormat("* 2-letter ISO Name: {0}", ci.TwoLetterISOLanguageName);

            // cerco la lingua
            currentLanguage = null;
            foreach (var lang in collections.languages)
            {
                if (lang.iso == ci.TwoLetterISOLanguageName) currentLanguage = lang;
            }

            // se non la trovo vado in fallback
            if (currentLanguage == null) currentLanguage = collections.fallback;

        }

        // preparo tutti i dati che mi servono
        datas = new Dictionary<string, string>();
        for (int i = 0; i < currentLanguage.keys.Length; i++)
        {
            datas.Add(currentLanguage.keys[i], currentLanguage.values[i]);
        }
        currentCultureInfo = new CultureInfo(currentLanguage.languageCode);
    }

    public static string Localized(this string key,params object[] values)
    {
        try
        {
            return string.Format(currentCultureInfo, key.Localized(), values);
        }
        catch (System.Exception e)
        {
#if UNITY_EDITOR || DEBUG
            Debug.LogErrorFormat("Error parsing format for key {0} in language {1}, see next error", key, currentLanguage.name);
            Debug.LogException(e);
            return string.Format("ERROR_{0}", key);
#else
            return "";
#endif
        }
    }

    public static string Localized(this string key)
    {
        return Localize(key);
    }

    public static string Localize(string key)
    {
        if (!datas.ContainsKey(key))
        {
#if UNITY_EDITOR || DEBUG
            Debug.LogErrorFormat("Key {0} not found on language data {1}", key, currentLanguage.name);
            return string.Format("UNDEF_{0}", key);
#else
            return "";
#endif
        }

        return datas[key];
    }

}



