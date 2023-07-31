using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Google;
using Firebase;
using Firebase.Extensions;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;


public class FirebaseLogin : MonoBehaviour
{
    public TextMeshProUGUI console;
    public GameObject enterUsernamePanel, welcomePanel, selectDifficulty;
    public GameManager gmScript;
    public Player player;



    public string GoogleWebAPI = "957866578538-0pmulhdvedas34mm5qb2m9607nb56tge.apps.googleusercontent.com";
    GoogleSignInConfiguration configuration;

    //Firebase.DependencyStatus dependencyStatus=Firebase.DependencyStatus.UnavailableOther;

    
    Firebase.Auth.FirebaseAuth auth;

    private void Awake() {
        configuration = new GoogleSignInConfiguration {
            WebClientId = GoogleWebAPI, RequestIdToken = true };
        }
    
    private void Start() {
        InitFirebase();
    }

    void InitFirebase() {
        gmScript.auth=Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void GoogleSignInClick() {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }

    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task) {
        if(task.IsFaulted) {
            console.SetText("Fault" + task.Exception);
        }
        else if(task.IsCanceled) {
            console.SetText("Login Canceled");
        }
        else {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            gmScript.auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
                if(task.IsCanceled) {
                    console.SetText("SigninWithCredentialAsync was canceled.");
                    return;
                }
                if(task.IsFaulted) {
                    console.SetText("SignInWithCredentialAsync encountered an error" + task.Exception);
                    return;
                }
                    gmScript.user= gmScript.auth.CurrentUser;
                    console.SetText(console.text + "\n login success!: " + gmScript.user.UserId.ToString());
                    console.SetText(console.text + "\n initialized database");

                    //load player data;
                    gmScript.LoadPlayerData();
                    StartCoroutine(CheckUsernameWait());

                    
                   
                
            });
        }
    }

    IEnumerator CheckUsernameWait() {
        console.SetText(console.text + "\n courotine started");
    yield return new WaitForSeconds(3f);
    if(player.username != "") {
        console.SetText(console.text + "\n username: " + player.username);
        gmScript.UpdateUI();
        selectDifficulty.SetActive(true);
        }
    else {
            gameObject.SetActive(false);
            enterUsernamePanel.SetActive(true);
            console.SetText(console.text + "\n username empty");
        }
    }

    

    
}

    
   