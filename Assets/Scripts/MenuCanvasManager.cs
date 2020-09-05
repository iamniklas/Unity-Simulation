using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvasManager : MonoBehaviour
{
    [Header("UI Seed Input Field")]
    [SerializeField] InputField mInputSeed = null;
    [Space(10)]
    [Header("UI Slider")]
    [SerializeField] Slider mSliderPreset = null;
    [SerializeField] Slider mSliderPatrols = null;
    [SerializeField] Slider mSliderKeycards = null;
    [SerializeField] Slider mSliderCameras = null;
    [SerializeField] Slider mSliderBoxes = null;
    [Space(10)]
    [Header("Index der Main Szene")]
    [SerializeField] int mNextSceneIndex = 1;
    [Header("Name des Beschreibungs-Textes")]
    [SerializeField] string mValueDescriptGameObjectName = "ValueDescript";

    //Festgelegte Parameter
    int mPresetValue = 0;
    int mPatrolsValue = 0;
    int mKeycardsValue = 0;
    int mCamerasValue = 0;
    int mBoxesValue = 0;

    //Preset Array
    int[][] mPresetValues = null;

    [Space(10)]
    [Header("Presets der Schwierigkeiten")]
    [SerializeField] int[] mPresetValuesMinimum = null;
    [SerializeField] int[] mPresetValuesEasy = null;
    [SerializeField] int[] mPresetValuesIntermediate = null;
    [SerializeField] int[] mPresetValuesHard = null;
    [SerializeField] int[] mPresetValuesExtreme = null;
    [SerializeField] int[] mPresetValuesMaximum = null;
    [Header("Namen der Schwierigkeiten")]
    [SerializeField] string[] mPresetNames = null;
    [Space(10)]
    [SerializeField] int mTotalDifficulties = 6;

    void Start()
    {
        mPresetValues = new int[mTotalDifficulties][];

        mPresetValues[0] = mPresetValuesMinimum;
        mPresetValues[1] = mPresetValuesEasy;
        mPresetValues[2] = mPresetValuesIntermediate;
        mPresetValues[3] = mPresetValuesHard;
        mPresetValues[4] = mPresetValuesExtreme;
        mPresetValues[5] = mPresetValuesMaximum;

        OnSliderPresetChange();
        OnSliderPatrolsChange();
        OnSliderKeycardsChange();
        OnSliderCamerasChange();
        OnSliderBoxesChange();

        //Cursor freigeben, da er sonst beim Rückkehren von Main gelockt wäre
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //Slider anpassen an Preset-Einstellung
    void UpdateSlidersByPreset(int _difficultyIndex)
    {
        mSliderPatrols.value = mPresetValues[_difficultyIndex][0];
        mSliderKeycards.value = mPresetValues[_difficultyIndex][1];
        mSliderCameras.value = mPresetValues[_difficultyIndex][2];
        mSliderBoxes.value = mPresetValues[_difficultyIndex][3];
        UpdateText(mSliderPreset.transform, mPresetNames[_difficultyIndex]);
    }

    //Reagieren auf Preset Slider Änderung
    public void OnSliderPresetChange()
    {
        mPresetValue = (int) mSliderPreset.value;
        UpdateSlidersByPreset(mPresetValue);
    }

    //Reagieren auf Wachen Slider Änderung
    public void OnSliderPatrolsChange()
    {
        mPatrolsValue = (int) mSliderPatrols.value;
        UpdateText(mSliderPatrols.transform, mPatrolsValue);
    }

    //Reagieren auf Keycard Slider Änderung
    public void OnSliderKeycardsChange()
    {
        mKeycardsValue = (int) mSliderKeycards.value;
        UpdateText(mSliderKeycards.transform, mKeycardsValue);
    }

    //Reagieren auf Kamera Slider Änderung
    public void OnSliderCamerasChange()
    {
        mCamerasValue = (int) mSliderCameras.value;
        UpdateText(mSliderCameras.transform, mCamerasValue);
    }

    //Reagieren auf Boxen Slider Änderung
    public void OnSliderBoxesChange()
    {
        mBoxesValue = (int) mSliderBoxes.value;
        UpdateText(mSliderBoxes.transform, mBoxesValue);
    }

    //Reagieren auf Klick des Start-Buttons
    public void OnButtonStartClicked()
    {
        int tempSeed = 0;
        int.TryParse(mInputSeed.text, out tempSeed);
        
        //Übergeben der Daten an den Singleton
        GameInfo.instance.SetData(mPatrolsValue,
                                  mKeycardsValue, 
                                  mCamerasValue, 
                                  mBoxesValue, 
                                  tempSeed);
        //Main Szene laden
        SceneManager.LoadScene(mNextSceneIndex);
    }

    //Reagieren auf Klick des Beenden-Buttons
    public void OnButtonQuitClicked()
    {
        Application.Quit();
    }

    //Aktualisieren des Textes, der den aktuellen Wert eines Sliders angibt
    void UpdateText(Transform _t, int _value)
    {
        _t.Find(mValueDescriptGameObjectName).GetComponent<Text>().text =
            _value.ToString();
    }

    void UpdateText(Transform _t, string _text)
    {
        _t.Find(mValueDescriptGameObjectName).GetComponent<Text>().text =
            _text;
    }
}