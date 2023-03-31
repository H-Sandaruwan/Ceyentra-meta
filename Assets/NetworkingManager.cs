using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using Algorand;
using Algorand.Unity;
using UnityEngine.SceneManagement;

public class NetworkingManager : MonoBehaviour
{
    public static NetworkingManager instance;
    public static LogIn logIn;

    public string baseURL = @"https://unitybackend120230310135845.azurewebsites.net/";

    //Reg 
    public InputField userNameInputField;
    public InputField emailInputFeild;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;

    //Login
    public InputField loginEmailInputField;
    public InputField loginPasswordInputField;

    //Algorand account
    public Account account;
    public MicroAlgos balance;

    // Start is called before the first frame update 
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRegButton()
    {
        Register tempReg = new Register()
        {
            //userName = userNameInputField.text,
            Email = emailInputFeild.text,
            Password = passwordInputField.text,
            password_confirmation = confirmPasswordInputField.text
        };

        Debug.Log(tempReg.ToString());
        StartCoroutine(Register(tempReg));
    }

    public void OnLogInButton()
    {
        StartCoroutine(LogIn());

    }


    public IEnumerator Register(Register register)
    {
        /*string values = {
          "Email": "abcdef@gmail.com",
          "Password": "Abce@123",
          "password_confirmation": "Abce@123"
        }'
        Debug.Log(values.ToString());
        Debug.Log(JsonUtility.ToJson(values,true));
        {
            "Email": "jkl@mal.com",
            "Password": "Asd@12",
            "password_confirmation": "Asd@12"
        }*/

        var uwr = new UnityWebRequest("https://unitybackend120230310135845.azurewebsites.net/api/Account/Register", "POST");
        string jsonData = JsonUtility.ToJson(register, true);
        Debug.Log(jsonData);

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        uwr.SetRequestHeader("Content-Type", "application/json");

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error :" + uwr.error);
        }
        else
        {
            account = Account.GenerateAccount();
            Debug.Log(
                "Account Address :" + account.Address.ToString() + "\n" +
                "Account Balance :" + balance.Amount.ToString());
            Debug.Log(uwr.downloadHandler.text);
            SceneManager.LoadScene("SampleScene");
        }
    }

    public IEnumerator LogIn()
    {
        WWWForm form = new WWWForm();

        form.AddField("grant_type", "password");  
        form.AddField("username", loginEmailInputField.text);
        form.AddField("password", loginPasswordInputField.text);

        UnityWebRequest uwr = UnityWebRequest.Post("https://unitybackend120230310135845.azurewebsites.net/token", form);

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error :" + uwr.error);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);

            logIn = JsonUtility.FromJson<LogIn>(uwr.downloadHandler.text);

            StartCoroutine(SaveData());
        }
    }

    public IEnumerator SaveData()
    {
        WWWForm form = new WWWForm();

        form.AddField("email", loginEmailInputField.text);
        form.AddField("UserData", "{score:15}");

        form.headers.Add("Authorization", "Bearer " + logIn.access_token);

        UnityWebRequest uwr = UnityWebRequest.Post("https://unitybackend120230310135845.azurewebsites.net/api/UserProfile", form);

        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error :" + uwr.error);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);

            logIn = JsonUtility.FromJson<LogIn>(uwr.downloadHandler.text);
        }

    }
}
