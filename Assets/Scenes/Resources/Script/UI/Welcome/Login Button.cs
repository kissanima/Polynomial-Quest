using System.Collections;
using UnityEngine;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.Services.Core;
using TMPro;

public class LoginButton : MonoBehaviour
{
    public TextMeshProUGUI console;
    public GameObject enterUsername, welcomePanel, googleLoginButton;
    public GameManager gmScript;
    public Player player;
    public async void ButtonPressed() {
        await SignInAnonymouslyAsync();
    }
    async Task SignInAnonymouslyAsync() {
    try
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        gmScript.LoadPlayerData();

        StartCoroutine(CheckUsernameWait());
    }
    catch (AuthenticationException ex)
    {
        // Compare error code to AuthenticationErrorCodes
        // Notify the player with the proper error message
        Debug.LogException(ex);
    }
    catch (RequestFailedException ex)
    {
        // Compare error code to CommonErrorCodes
        // Notify the player with the proper error message
        Debug.LogException(ex);
     }
}
    IEnumerator CheckUsernameWait() {
    yield return new WaitForSeconds(1f);
    if(player.username != "") {
        welcomePanel.SetActive(false);
        }
        else {
            gameObject.SetActive(false);
            googleLoginButton.SetActive(false);
            enterUsername.SetActive(true);
        }
        console.SetText("Sign in anonymously succeeded! ");
        
        // Shows how to get the playerID
        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}"); 
        console.SetText(console.text + $"PlayerID: {AuthenticationService.Instance.PlayerId}");
}
}
