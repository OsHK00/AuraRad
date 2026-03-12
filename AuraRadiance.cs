using System.Diagnostics;
using System.Reflection;
using Modding;
using UnityEngine;
using GlobalEnums;
using System.Collections.Generic;   
using System.IO;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class AuraRadiance : Mod, ITogglableMod
{
    public static readonly List<Sprite> Sprites = new List<Sprite>();
    public static AuraRadiance instance;

    public static Texture2D Halo0Texture;
    public static Texture2D Halo1Texture;
    public static Texture2D Halo2Texture; 

    public AuraRadiance() : base("Aura Radiance") { }

    public override void Initialize()
    {
        instance = this;
        Log("Initalizing.");

        Assembly asm = Assembly.GetExecutingAssembly();
        foreach (string name in asm.GetManifestResourceNames())
        {
            if (!name.EndsWith(".png")) continue;

            if (name.EndsWith("halo0.png") || name.EndsWith("halo1.png") ||
                name.EndsWith("halo2.png")) continue;
            using Stream stream = asm.GetManifestResourceStream(name);
            if (stream == null) continue;
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(data, markNonReadable: true);
            Sprites.Add(Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f)));
            Log("Sprite cargado: " + name);
        }


        Halo0Texture = LoadPng("halo0.png");
        Halo1Texture = LoadPng("halo1.png");
        Halo2Texture = LoadPng("halo2.png");

        ModHooks.AfterSavegameLoadHook += AfterSaveGameLoad;
        ModHooks.NewGameHook           += AddComponent;
        ModHooks.LanguageGetHook       += LangGet;
        ModHooks.AfterTakeDamageHook   += CreateRespawn;


        GameObject sceneController = new GameObject("AuraSceneController");
        GameObject.DontDestroyOnLoad(sceneController);
        sceneController.AddComponent<AuraSceneController>();
    }

    public static Texture2D LoadPng(string path)
    {

        try
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] names = asm.GetManifestResourceNames();
            string target = null;
            foreach (var n in names)
            {
                if (n.EndsWith(path) || n.EndsWith("." + path))
                { target = n; break; }
            }
            if (target == null)
            {
                instance.Log("LoadPng: no se encontró recurso para '" + path + "'. Disponibles: " + string.Join(", ", names));
                return null;
            }
            using Stream stream = asm.GetManifestResourceStream(target);
            if (stream == null) return null;
            MemoryStream memoryStream = new MemoryStream((int)stream.Length);
            stream.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();
            Texture2D texture2D = new Texture2D(1, 1);
            texture2D.LoadImage(bytes, true);
            return texture2D;
        }
        catch (Exception e)
        {
            instance.Log("Error cargando textura " + path + ": " + e.Message);
            return null;
        }
    }

    public override string GetVersion()
    {
        return FileVersionInfo
            .GetVersionInfo(Assembly.GetAssembly(typeof(AuraRadiance)).Location)
            .FileVersion;
    }

    private static string LangGet(string key, string sheettitle, string orig)
    {
        if (key != null)
        {
            switch (key)
            {
                case "ABSOLUTE_RADIANCE_SUPER":   return "LIGHT PURIFIER VOID";
                case "ABSOLUTE_RADIANCE_MAIN":    return "AURA RADIANCE";
                case "GG_S_RADIANCE":             return "God of Unmatched Aura";
                case "GODSEEKER_RADIANCE_STATUE": return "I LOST MY AURA...";
                default: return Language.Language.GetInternal(key, sheettitle);
            }
        }
        return orig;
    }

    private static void AfterSaveGameLoad(SaveGameData data) => AddComponent();

    private static void AddComponent()
    {
        GameManager.instance.gameObject.AddComponent<AbsFinder>();
    }

    private static int CreateRespawn(int hazardType, int damage)
    {
        GameObject absRad = GameObject.Find("Absolute Radiance");
        if ((HazardType)hazardType == HazardType.ACID && absRad != null)
        {
            if (absRad.transform.GetPositionY() > 150f
                && GameObject.Find("Knight").transform.GetPositionY() > 60f)
            {
                GameObject.Find("Radiant Plat Small (11)")
                    .LocateMyFSM("radiant_plat").SendEvent("APPEAR");
            }
            else if (absRad.LocateMyFSM("Attack Choices")
                     .FsmVariables.GetFsmInt("Arena").Value == 2)
            {
                GameObject.Find("Hazard Plat/Radiant Plat Wide (4)")
                    .LocateMyFSM("radiant_plat").SendEvent("APPEAR");
            }
        }
        return damage;
    }

    public void Unload()
    {
        ModHooks.AfterSavegameLoadHook -= AfterSaveGameLoad;
        ModHooks.NewGameHook           -= AddComponent;
        ModHooks.LanguageGetHook       -= LangGet;
        ModHooks.AfterTakeDamageHook   -= CreateRespawn;

        GameManager gm = GameManager.instance;
        AbsFinder af = (gm != null) ? gm.gameObject.GetComponent<AbsFinder>() : null;
        if (af != null) UnityEngine.Object.Destroy(af);
    }
}

internal class AuraSceneController : MonoBehaviour
{
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GG_Radiance")
        {
            // Aplicar zoom inmediatamente
            if (GameCameras.instance != null && GameCameras.instance.tk2dCam != null)
            {
                GameCameras.instance.tk2dCam.ZoomFactor = 0.85f;
            }

            // Aplicar colores iniciales inmediatamente
            StartCoroutine(ApplyInitialColors());


            StartCoroutine(EnsureAbsFinderExists());
        }
    }

    /// <summary>

    /// </summary>
    private IEnumerator EnsureAbsFinderExists()
    {
        yield return null; 

        try
        {
            var gm = GameManager.instance;
            if (gm == null) yield break;


            var existing = gm.gameObject.GetComponent<AbsFinder>();
            if (existing == null)
            {
                gm.gameObject.AddComponent<AbsFinder>();
                Modding.Logger.Log("[AuraSceneController] AbsFinder añadido en re-entrada a GG_Radiance.");
            }
        }
        catch (Exception e)
        {
            Modding.Logger.Log("[AuraSceneController] Error en EnsureAbsFinderExists: " + e.Message);
        }
    }

    private IEnumerator ApplyInitialColors()
    {
        yield return null; // esperar un frame

        try
        {
            var arena = GameObject.Find("GG_Arena_Prefab");
            if (arena == null) yield break;
            var bg = arena.transform.Find("BG")?.gameObject;
            if (bg == null) yield break;

            SpriteRenderer sky = bg.transform.Find("sky_colour")?.GetComponent<SpriteRenderer>();
            List<SpriteRenderer> pillars = new List<SpriteRenderer>();
            List<SpriteRenderer> hazes   = new List<SpriteRenderer>();
            List<SpriteRenderer> clouds  = new List<SpriteRenderer>();
            List<SpriteRenderer> rays    = new List<SpriteRenderer>();

            foreach (Transform child in bg.transform)
            {
                var nm = child.name.ToLower();
                if (nm.Contains("haze") || nm.Contains("fog"))
                {
                    var s = child.GetComponent<SpriteRenderer>();
                    if (s != null) hazes.Add(s);
                }
                else if (child.name.Contains("GG_scenery_0004_17"))
                {
                    var s = child.GetComponent<SpriteRenderer>();
                    if (s != null) clouds.Add(s);
                }
                else if (nm.Contains("pillar") || nm.Contains("column"))
                {
                    var s = child.GetComponent<SpriteRenderer>();
                    if (s != null) pillars.Add(s);
                    foreach (Transform pChild in child)
                    { var cs = pChild.GetComponent<SpriteRenderer>(); if (cs != null) pillars.Add(cs); }
                }
                else if (nm.Contains("ray"))
                {
                    foreach (Transform rChild in child)
                    { var cs = rChild.GetComponent<SpriteRenderer>(); if (cs != null) rays.Add(cs); }
                    var s = child.GetComponent<SpriteRenderer>();
                    if (s != null) rays.Add(s);
                }
            }

            Color skyColor    = new Color(0.30f, 0.50f, 0.90f, 1f);
            Color pillarColor = new Color(0.20f, 0.40f, 0.80f, 1f);
            Color hazeColor   = new Color(0.40f, 0.60f, 0.90f, 1f);
            Color cloudColor  = new Color(0.40f, 0.55f, 0.80f, 1f);
            Color rayColor    = new Color(0.50f, 0.70f, 1.00f, 0.40f);

            if (sky != null) sky.color = skyColor;
            foreach (var s in pillars) if (s != null) s.color = pillarColor;
            foreach (var s in hazes)   if (s != null) s.color = new Color(hazeColor.r, hazeColor.g, hazeColor.b, s.color.a);
            foreach (var s in clouds)  if (s != null) s.color = new Color(cloudColor.r, cloudColor.g, cloudColor.b, s.color.a);
            foreach (var s in rays)    if (s != null) s.color = rayColor;
        }
        catch (Exception e)
        {
            Modding.Logger.Log("[AuraSceneController] Error aplicando colores iniciales: " + e.Message);
        }
    }
}