using UnityEngine;
using System.Collections;
using TetrisSupport;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    public GameMain Instance;

    Transform uiRoot;

    // Use this for initialization
    void Awake()
    {
        GameObject.DontDestroyOnLoad(this.gameObject);
        Instance = this;
        uiRoot = GameObject.Find("Canvas").transform;
        PlayerManager.Instance.Init(uiRoot);
    }

    public void OnClickConnect()
    {
        GameObject ipGo = GameObject.Find("IpAdress");
        NetManager.Instance.Connect(ipGo.GetComponent<InputField>().text, this.OnConnectSuccess);
    }


    // Update is called once per frame
    void Update()
    {
        NetManager.Instance.Tick();
        PlayerManager.Instance.Tick();
    }

    void OnConnectSuccess()
    {
        NetManager.Instance.SendMsg(new CSetName() { Name = uiRoot.Find("Login/Name").GetComponent<InputField>().text });
        ActiveUiSon("Login", false);
        ActiveUiSon("Host", true);
        ActiveUiSon("Other", true);
    }

    void OnApplicationQuit()
    {
        NetManager.Instance.Close();
    }

    void ActiveUiSon(string sonName, bool isActive)
    {
        uiRoot.Find(sonName).gameObject.SetActive(isActive);
    }
}
