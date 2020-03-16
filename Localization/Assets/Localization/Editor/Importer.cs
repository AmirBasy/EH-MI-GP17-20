using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class Importer : MonoBehaviour
{
    [MenuItem("Localization/Import")]
    public static void Import()
    {

        string path = string.Format("{0}/Localization/Localization_Example.csv", Application.dataPath);
        string[] lines = System.IO.File.ReadAllLines(path);

        Debug.LogFormat("Lines:{0}", lines.Length);

        // Determino le lingue da importare
        string[] firstCols = lines[0].Split(',');
        LanguageData[] languages = new LanguageData[firstCols.Length - 3];
        for (int i = 3; i < firstCols.Length; i++)
        {
            //prelevo il codice lingua
            string languageCode = firstCols[i];

            // creo il languageData per la lingua, lo inizializzo e lo salvo
            string languagePath = string.Format("Assets/Localization/{0}.asset", languageCode);
            LanguageData language;
            language = AssetDatabase.LoadAssetAtPath<LanguageData>(languagePath);
            // se non esiste lo creo
            if (language==null)
            {
                language = ScriptableObject.CreateInstance<LanguageData>();
                Debug.LogFormat("Creating language file:{0}", languagePath);
                AssetDatabase.CreateAsset(language, languagePath);
            }

            
            language.languageCode = languageCode;
            language.values = new string[lines.Length - 1];
            language.keys = new string[lines.Length - 1];

            
            
            

            // lo salvo in un array così posso referenziarlo dopo
            languages[i - 3] = language;
        }

        // per tutte le linee
        for (int i = 1; i < lines.Length; i++)
        {
            // prendo le colonne
            string[] cols = lines[i].Split(',');

            // per ogni lingua
            for (int langIndex = 3; langIndex < cols.Length; langIndex++)
            {
                try
                {
                    // salvo il corrispondente di quella chiave
                    languages[langIndex-3].values[i-1] = cols[langIndex];
                    languages[langIndex-3].keys[i-1] = cols[0];
                    EditorUtility.SetDirty(languages[langIndex - 3]);
                }
                
                catch (System.Exception e)
                {
                    Debug.LogErrorFormat("Error importing row {0} col {1}", i, langIndex);
                    Debug.LogException(e);
                }
            }
        }

        AssetDatabase.Refresh();

        CheckConsistency(languages);

        BuildCharacterCollection(languages);
    }

    static void BuildCharacterCollection(LanguageData[] languages)
    {
        var china = languages.Where(a => a.iso == "zh").FirstOrDefault();

        // colleziono tutti i caratteri usati
        List<char> characters = new List<char>();
        foreach (var item in china.values)
        {
            if (string.IsNullOrEmpty(item)) continue;
            foreach (var c in item)
            {
                if (c == 10 || c == 13) continue;
                if (!characters.Contains(c)) characters.Add(c);
            }
        }

        // trasformo in uno 'stringone'
        string alls = string.Join("", characters.Select(a => a.ToString()).ToArray());

        System.IO.File.WriteAllText(Application.dataPath + "/Localization/china_usage.txt", alls);

    }

    static void CheckConsistency(LanguageData[] languages)
    {
        var eng = languages.Where(a => a.iso == "en").FirstOrDefault();

        if (eng == null) Debug.LogErrorFormat("English language data not found");

        // per tutte le chiavi
        for (int i = 0; i < eng.keys.Length; i++)
        {
            string key = eng.keys[i];
            string eng_value = eng.values[i];

            if (string.IsNullOrEmpty(eng_value)) continue;

            int numberOfBraces_eng_open = eng_value.Count(a => a == '{');
            int numberOfBraces_eng_close = eng_value.Count(a => a == '}');

            if (numberOfBraces_eng_open != numberOfBraces_eng_close)
            {
                Debug.LogErrorFormat("Open and Close braces do not match in english for key {0}",key);
            }

            // per tutte le lingue
            foreach (var language in languages)
            {
                string lang_value = language.values[i];


                int numberOfBraces_lang_open = lang_value.Count(a => a == '{');
                int numberOfBraces_lang_close = lang_value.Count(a => a == '}');

                if (numberOfBraces_lang_open != numberOfBraces_lang_close)
                {
                    Debug.LogErrorFormat("Open and Close braces do not match in {1} for key {0}", key, language.name);
                }

                if (numberOfBraces_eng_open!=numberOfBraces_lang_open)
                {
                    Debug.LogErrorFormat("Braces do not match with english in {1} for key {0}", key, language.name);
                }

                for (int index = 0; index < numberOfBraces_lang_open; index++)
                {
                    if (lang_value.IndexOf("{" + index.ToString())==-1)
                        Debug.LogErrorFormat("Format string index {0} not found on key {1} for language {2}", index , key, language.name);
                }
            }
        }
    }

}
