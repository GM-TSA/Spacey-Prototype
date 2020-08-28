using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class alienCommunication : MonoBehaviour
{
    //Question - Answers - Point values https://docs.google.com/document/d/1XhxoBv6LwhjHZrLKx-zW-o6IKSEaIfFHQ7DkQehpKvU/edit?usp=sharing//
    public Dictionary<string, Dictionary<string, int>> QuestionsAnswers = new Dictionary<string, Dictionary<string, int>>() {
        {"Olssv ayhclsly, sl hyl aol gvyw-ruvez. Ovd jhu dl olsw fvb.", new Dictionary<string, int>() {
            {"a. zjyld fvb?", -10}, {"b. zhml whzzhzl?", 1}, {"c. myll zabmm?", -1}, {"d. fvby myplukzopw", 10}
        }},
        {"Dolyl hyl fvb myvt?", new Dictionary<string, int>() {
            {"a. Lhyao", 10}, {"b. Fvby tvt", -10}, {"c. Uvdolyl", -1}, {"d. Zvtldolyl lszl", 1}
        }},
        {"Doha iypunz fvb av aopz nhshef", new Dictionary<string, int>() {
            {"a. Dhy", -10}, {"b. Nvsk", -1}, {"c. Ovtl", 10}, {"d. Svza", 1}
        }},
        {"Dolyl hyl fvb xvpux?", new Dictionary<string, int>() {
            {"a. Ovtl", 1}, {"b. Lhyao", 10}, {"c. Fvby tvt'z ovbzl", -10}, {"d. Aol ilualy", -1}
        }},
    };
    //Happiness Level stuff//
    public int currentStatus = 50;
    public Slider statusSlider;

    //Interaction Stuff//
    public bool isToggleable = false;
    public bool isActive = false;
    public GameObject Player;
    private int questionNumber = 0;

    //UI Stuff//
    public Text QuestionText;
    public Text a;
    public Text b;
    public Text c;
    public Text d;

    //Allows player to interact with alien//
    public void Toggle() {
        if(isToggleable && Input.GetKeyDown(KeyCode.Mouse0) && questionNumber < 4) {
            isActive = true;
            Player.GetComponent<PlayerController>().isActive = false;
        }
    }
    public void endCheck() {
        if(questionNumber >= 4) {
            isActive = false;
            isToggleable = false;
            QuestionText.text = "";
            a.text = "";
            b.text = "";
            c.text = "";
            d.text = "";
            Player.GetComponent<PlayerController>().isActive = true;
        }
    }
    public void SliderUpdate() {
        statusSlider.value = currentStatus;
    }

    // Update is called once per frame
    void Update()
    {
        Toggle();
        if (isActive) {
            QuestionText.text = QuestionsAnswers.ElementAt(questionNumber).Key;
            a.text = QuestionsAnswers.ElementAt(questionNumber).Value.ElementAt(0).Key;
            b.text = QuestionsAnswers.ElementAt(questionNumber).Value.ElementAt(1).Key;
            c.text = QuestionsAnswers.ElementAt(questionNumber).Value.ElementAt(2).Key;
            d.text = QuestionsAnswers.ElementAt(questionNumber).Value.ElementAt(3).Key;
            if(Input.GetKeyDown(KeyCode.A)) {
                currentStatus += QuestionsAnswers.ElementAt(questionNumber).Value.ElementAt(0).Value;
                questionNumber ++;
            }
            if(Input.GetKeyDown(KeyCode.B)) {
                currentStatus += QuestionsAnswers.ElementAt(questionNumber).Value.ElementAt(1).Value;
                questionNumber ++;
            }
            if(Input.GetKeyDown(KeyCode.C)) {
                currentStatus += QuestionsAnswers.ElementAt(questionNumber).Value.ElementAt(2).Value;
                questionNumber ++;
            }
            if(Input.GetKeyDown(KeyCode.D)) {
                currentStatus += QuestionsAnswers.ElementAt(questionNumber).Value.ElementAt(3).Value;
                questionNumber ++;
            }
            endCheck();
        }
        SliderUpdate();
    }
    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            isToggleable = true;
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            isToggleable = false;
        }
    }
}
