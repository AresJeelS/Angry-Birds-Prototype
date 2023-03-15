using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    Idle,
    Playing,
    LevelEnd
}
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition _s;

    [Header("Set in Inspector")]
    public Text UitLevel;
    public Text UitShots;
    public Text UitButton;
    public Vector3 CastlePos;
    public GameObject[] Castles;

    [Header("Set Dynamically")]
    public int Level;
    public int LevelMax;
    public int ShotsTaken;
    public GameObject Castle;
    public GameMode Mode = GameMode.Idle;
    public string Showing = "Show Slingshot";

    private void Start()
    {
        _s = this;
        Level = 0;
        LevelMax = Castles.Length;
        StartLevel();

    }
    public void StartLevel()
    {
        if (Castle != null)
        {
            Destroy(Castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        Castle = Instantiate(Castles[Level]);
        Castle.transform.position = CastlePos;
        ShotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.GoalMet = false;
        UpdateGUI();

        Mode = GameMode.Playing;

    }
    private void UpdateGUI()
    {
        UitLevel.text = "Level: " + (Level + 1) + " of " + LevelMax;
        UitShots.text = "Shots Taken: " + ShotsTaken;
    }
    private void Update()
    {
        UpdateGUI();

        if ((Mode == GameMode.Playing) && Goal.GoalMet)
        {
            Mode = GameMode.LevelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    private void NextLevel()
    {
        Level++;
        if (Level == LevelMax)
        {
            Level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = UitButton.text;
        }
        Showing = eView;

        switch (Showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                UitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = _s.Castle;
                UitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                UitButton.text = "Show Slingshot";
                break;

        }

    }

    public static void ShotFired()

    {
        _s.ShotsTaken++;
    }

}
