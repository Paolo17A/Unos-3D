using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour
{
    private const string baseURL = "https://unosserver.onrender.com";

    // Add User API
    public IEnumerator AddUser(string gender)
    {
        string url = baseURL + "/usercount/create";

        GenderSelect selectedGender = new GenderSelect(gender);
        string json = JsonUtility.ToJson(selectedGender);

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        string completeURL = request.url + "?" + Encoding.UTF8.GetString(bodyRaw);
        Debug.Log("Complete URL with Headers: " + completeURL);

        yield return request.SendWebRequest();
        //ApiResponse response = JsonUtility.FromJson<ApiResponse>(request.downloadHandler.text);
        Debug.Log("Message:" + request.downloadHandler.text);

        if(request.error != null)
        {
            Debug.Log("no errors found");
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            GameManager.Instance.DisplayErrorPanel(request.error);
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("gender form upload complete");
        }
    }

    // Disaster Choice API
    public IEnumerator MakeDisasterChoice(string scenario, string gender, string choice)
    {
        string url = baseURL + "/disaster/create";

        ScenarioSelect scenarioSelect = new ScenarioSelect(scenario, gender, choice);
        string json = JsonUtility.ToJson(scenarioSelect);

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        string completeURL = request.url + "?" + Encoding.UTF8.GetString(bodyRaw);
        Debug.Log("Complete URL with Headers: " + completeURL);

        yield return request.SendWebRequest();

        Debug.Log("Message:" + request.downloadHandler.text);

        if (request.result != UnityWebRequest.Result.Success)
        {
            GameManager.Instance.DisplayErrorPanel(request.error);
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("scenario form upload complete");
        }
    }
}

[System.Serializable]
public class ApiResponse
{
    public string message;
}

public class GenderSelect
{
    public string gender;

    public GenderSelect(string gender)
    {
        this.gender = gender;
    }
}

public class ScenarioSelect
{
    public string scenario;
    public string gender;
    public string choice;

    public ScenarioSelect(string scenario, string gender, string choice)
    {
        this.scenario = scenario;
        this.gender = gender;
        this.choice = choice;
    }
}
