#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DebugSettingsEditor : EditorWindow
{
    private const int SPACINGS = 10;

    private bool debug;
    private bool saveInEditor;
    private bool loadInEditor;
    private bool skipProlog;
    private bool chooseYourEnemy;
    private bool skipTutorial;
    
    public static bool EnablePlayerDamage
    {
        get => EditorPrefs.GetBool(nameof(EnablePlayerDamage), false);
        set => EditorPrefs.SetBool(nameof(EnablePlayerDamage), value);
    }
    
    public static int PlayerDamage
    {
        get => EditorPrefs.GetInt(nameof(PlayerDamage), 1);
        set => EditorPrefs.SetInt(nameof(PlayerDamage), value);
    }
    
    public static float TimeScale
    {
        get => EditorPrefs.GetFloat(nameof(TimeScale), 1);
        set => EditorPrefs.SetFloat(nameof(TimeScale), value);
    }
    
    [MenuItem("Administrator/Debug")]
    public static void ShowWindow() => GetWindow<DebugSettingsEditor>("DEBUG");

    private void OnGUI()
    {
        EnableDebug();
     
        GUILayout.Space(SPACINGS);
        AddSaveToggle();
        
        GUILayout.Space(SPACINGS);
        AddLoadToggle();

        GUILayout.Space(SPACINGS);
        AddSkipProlog();
        
        GUILayout.Space(SPACINGS);
        AddChooseEnemy();
        
        GUILayout.Space(SPACINGS);
        AddSkipTutorial();

        GUILayout.Space(SPACINGS);
        TimeScaleSlider();
        
        GUILayout.Space(SPACINGS);
        DamageDraw();
    }

    private void EnableDebug()
    {
        debug = GUILayout.Toggle(DebugSettings.Debug, "Debug");
        if (debug != DebugSettings.Debug)
            DebugSettings.Debug = debug;
    }
    
    private void AddSaveToggle()
    {
        saveInEditor = GUILayout.Toggle(DebugSettings.SaveInEditor, "SaveInEditor");
        if (saveInEditor != DebugSettings.SaveInEditor)
            DebugSettings.SaveInEditor = saveInEditor;
    }
    
    private void AddLoadToggle()
    {
        loadInEditor = GUILayout.Toggle(DebugSettings.LoadInEditor, "LoadInEditor");
        if (loadInEditor != DebugSettings.LoadInEditor)
            DebugSettings.LoadInEditor = loadInEditor;
    }
    
    private void AddSkipProlog()
    {
        skipProlog = GUILayout.Toggle(DebugSettings.SkipProlog, "SkipProlog");
        if (skipProlog != DebugSettings.SkipProlog)
            DebugSettings.SkipProlog = skipProlog;
    }
    
    private void AddChooseEnemy()
    {
        chooseYourEnemy = GUILayout.Toggle(DebugSettings.ChooseYourEnemy, "ChooseYourEnemy");
        if (chooseYourEnemy != DebugSettings.ChooseYourEnemy)
            DebugSettings.ChooseYourEnemy = chooseYourEnemy;
    }
    
    private void AddSkipTutorial()
    {
        skipTutorial = GUILayout.Toggle(DebugSettings.SkipTutorial, "SkipTutorial");
        if (skipTutorial != DebugSettings.SkipTutorial)
            DebugSettings.SkipTutorial = skipTutorial;
    }

    private void DamageDraw()
    {
        GUILayout.BeginHorizontal();
        var additionalDamage = GUILayout.Toggle(EnablePlayerDamage, "Player Damage");
        if (additionalDamage != EnablePlayerDamage)
            EnablePlayerDamage = additionalDamage;

        if (!additionalDamage)
        {
            GUILayout.EndHorizontal();
            return;
        }
        
        var dmg = EditorGUILayout.IntField(PlayerDamage);
        if (dmg != PlayerDamage)
            PlayerDamage = dmg;
        GUILayout.EndHorizontal();
    }
    
    private void TimeScaleSlider()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Time Scale: " + TimeScale.ToString("F2"));
        var value = GUILayout.HorizontalSlider(TimeScale, 1, 5);
        if (TimeScale != value)
        {
            TimeScale = value;
            Time.timeScale = value;
        }
        GUILayout.EndHorizontal();
    }
}
#endif