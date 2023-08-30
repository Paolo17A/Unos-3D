using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class APIClient : MonoBehaviour
{
    private const string baseURL = "https://unosserver.onrender.com";

    // Add User API
    public IEnumerator AddUser(string gender)
    {
        string url = baseURL + "/usercount/create";

        WWWForm form = new WWWForm();
        form.AddField("gender", gender);

        using (WWW www = new WWW(url, form))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                ApiResponse response = JsonUtility.FromJson<ApiResponse>(www.text);
                Debug.Log("Add User API Response: " + response.message);
            }
            else
            {
                Debug.LogError("Add User API Error: " + www.error);
                GameManager.Instance.DisplayErrorPanel("Add user API Error: " + www.error);
            }
        }
    }

    // Disaster Choice API
    public IEnumerator MakeDisasterChoice(string scenario, string gender, string choice)
    {
        string url = baseURL + "/disaster/create";

        WWWForm form = new WWWForm();
        form.AddField("scenario", scenario);
        form.AddField("gender", gender);
        form.AddField("choice", choice);

        using (WWW www = new WWW(url, form))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                ApiResponse response = JsonUtility.FromJson<ApiResponse>(www.text);
                Debug.Log("Disaster Choice API Response: " + response.message);
            }
            else
            {
                Debug.LogError("Disaster Choice API Error: " + www.error);
                GameManager.Instance.DisplayErrorPanel("Disaster Choice API Error: " + www.error);
            }
        }
    }
}

[System.Serializable]
public class ApiResponse
{
    public string message;
}
