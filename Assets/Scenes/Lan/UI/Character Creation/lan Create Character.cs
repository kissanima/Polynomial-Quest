using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanCreateCharacter : MonoBehaviour
{

    public GameObject createUsername, selectClassPanel, classInfo, character, customizeCharacter, warriorPanel, magePanel, assassinPanel,
    ui, lanPanel;
    TMP_InputField usernameField;
    public string warriorDescription, warriorStory, mageDescription, mageStory, assassinDescription, assassinStory;
    public Sprite[] belt,boots_l, boots_r, elbow_l, elbow_r, face, hood, legs_l, legs_r, shoulder_l, shoulder_r, torso, wrist_l, wrist_r;
    public Lan lanScript;

    public string playerClass, username;
    public int beltIndex, bootsIndex,elbowIndex, faceIndex, hoodIndex,
    legsIndex, shoulderIndex, torsoIndex, wirstindex, index;

    private void Start() {
        selectClassPanel = transform.GetChild(0).GetChild(3).gameObject;
        createUsername = selectClassPanel.transform.GetChild(1).gameObject;
        classInfo = selectClassPanel.transform.GetChild(2).gameObject;
        usernameField = createUsername.transform.GetChild(1).GetComponent<TMP_InputField>();
        character = transform.GetChild(1).GetChild(0).gameObject;
        customizeCharacter = transform.GetChild(0).GetChild(4).gameObject;

        warriorPanel = selectClassPanel.transform.GetChild(0).GetChild(1).gameObject;
        magePanel = selectClassPanel.transform.GetChild(0).GetChild(2).gameObject;
        assassinPanel = selectClassPanel.transform.GetChild(0).GetChild(3).gameObject;

        lanPanel = ui.transform.GetChild(7).GetChild(3).gameObject;
    }

    
    public void ButtonWarrior() {
        classInfo.SetActive(true);
        classInfo.transform.GetChild(0).GetComponent<Image>().sprite = warriorPanel.transform.GetChild(0).GetComponent<Image>().sprite;
        classInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Warrior");
        classInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
        classInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(warriorDescription);
        classInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(warriorStory);

        playerClass = "Warrior";
    }

    public void ButtonMage() {
        classInfo.SetActive(true);
        classInfo.transform.GetChild(0).GetComponent<Image>().sprite = magePanel.transform.GetChild(0).GetComponent<Image>().sprite;
        classInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Mage");
        classInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(0,148, 255);
        classInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(mageDescription);
        classInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(mageStory);

        playerClass = "Mage";
    }

    public void ButtonAssassin() {
        classInfo.SetActive(true);
        classInfo.transform.GetChild(0).GetComponent<Image>().sprite = assassinPanel.transform.GetChild(0).GetComponent<Image>().sprite;
        classInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Assassin");
        classInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(230, 0, 255);
        classInfo.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText(assassinDescription);
        classInfo.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(assassinStory);

        playerClass = "Assassin";
    }

    public void ButtonNext() {
        if(usernameField.text == "" || playerClass == "") {
            createUsername.transform.GetChild(2).gameObject.SetActive(true);
        }
        else {
            username = usernameField.text;
            selectClassPanel.SetActive(false);
            customizeCharacter.SetActive(true);
        }
    }


    public void SelectBelt(int index) {
        beltIndex = index;
        character.transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().sprite = belt[beltIndex];
    }

    public void SelectBoots(int index) {
        bootsIndex = index;
        character.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = boots_l[bootsIndex];
        character.transform.GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = boots_r[bootsIndex];
    }

    public void SelectElbow(int index) {
        elbowIndex = index;
        character.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = elbow_l[elbowIndex];
        character.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = elbow_r[elbowIndex];
    }

    public void SelectFace(int index) {
        faceIndex = index;
        character.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetComponent<SpriteRenderer>().sprite = face[faceIndex];
    }

    public void SelectHood(int index) {
        hoodIndex = index;
        character.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(2).GetComponent<SpriteRenderer>().sprite = hood[hoodIndex];
    }

    public void SelectLegs(int index) {
        legsIndex = index;
        character.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = legs_l[legsIndex];
        character.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sprite = legs_r[legsIndex];
    }

    public void SelectShoulder(int index) {
        shoulderIndex = index;
        character.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetComponent<SpriteRenderer>().sprite = shoulder_l[shoulderIndex];
        character.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(1).GetComponent<SpriteRenderer>().sprite = shoulder_r[shoulderIndex];
    }

    public void SelectTorso(int index) {
        torsoIndex = index;
        character.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().sprite = torso[torsoIndex]; 
    }

    public void SelectWrist(int index) {
        wirstindex = index;
        character.transform.GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = wrist_l[wirstindex]; 
        character.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = wrist_r[wirstindex]; 
    }

    public void ButtonBack() {
        customizeCharacter.SetActive(false);
        selectClassPanel.SetActive(true);
    }

    public void FinishButton() {
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("playerClass", playerClass);
        PlayerPrefs.SetInt("belt", beltIndex);
        PlayerPrefs.SetInt("boots", bootsIndex);
        PlayerPrefs.SetInt("elbow", elbowIndex);
        PlayerPrefs.SetInt("face", faceIndex);
        PlayerPrefs.SetInt("hood", hoodIndex);
        PlayerPrefs.SetInt("legs", legsIndex);
        PlayerPrefs.SetInt("shoulder", shoulderIndex);
        PlayerPrefs.SetInt("torso", torsoIndex);
        PlayerPrefs.SetInt("wrist", wirstindex);
        
        gameObject.SetActive(false);
        ui.SetActive(true);
        lanPanel.transform.GetChild(1).gameObject.SetActive(false); //disable host button
        lanPanel.transform.GetChild(7).gameObject.SetActive(false); //disable create character button
        lanPanel.transform.GetChild(6).gameObject.SetActive(true); //showw select difficulty

        Debug.Log("Save success " + "/n" + PlayerPrefs.GetString("username"));
    }
}
