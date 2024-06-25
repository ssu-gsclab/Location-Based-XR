using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public RawImage mapImage;

    [Header("Map Information")]
    //public string strBaseURL = "";
    public int zoom = 10;
    public int mapWidth;
    public int mapHeight;
    public int mapScale = 1;
    public string strAPIKey = "";

    //private GameObject LocationManager;
    private double latitude;
    private double longtitude;

    private void Start()
    {
        mapImage = GetComponent<RawImage>();
        StartCoroutine(UpdateMap());
    }

    IEnumerator UpdateMap()
    {
        while (true)
        {
            // GetLocation �ν��Ͻ��κ��� ������ �浵�� ������
            latitude = GetLocation.Instance.latitude_init;
            longtitude = GetLocation.Instance.longitude_init;

            // ������ �浵�� ��ȿ�� ��쿡�� LoadMap �ڷ�ƾ ȣ��
            if (latitude != 0 && longtitude != 0)
            {
                StartCoroutine(LoadMap());
            }

            // ���� ������Ʈ ���� ���� (���⼭�� 10��)
            yield return new WaitForSeconds(10f);
        }
    }

    IEnumerator LoadMap()
    {
        latitude = GetLocation.Instance.latitude_init;
        longtitude = GetLocation.Instance.longitude_init;

        string url = "https://maps.googleapis.com/maps/api/staticmap?" + "center=" + latitude + "," + longtitude +"&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + mapScale + "&maptype=roadmap" + "&markers=color:blue%7Clabel:S%7C37." + latitude + "," + longtitude + "&key=" + strAPIKey;
        Debug.Log("URL = " + url);

        url = UnityWebRequest.UnEscapeURL(url);
        UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
        req.SetRequestHeader("Authorization", strAPIKey );

        yield return req.SendWebRequest();

        mapImage.texture = DownloadHandlerTexture.GetContent(req);

    }
}
