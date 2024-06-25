using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;

public class GetLocation : MonoBehaviour
{
    public static GetLocation Instance;
    public Text latitude_text;
    public Text longitude_text;
    public float maxWaitTime = 10.0f;
    public float resendTime = 1.0f;

    public float latitude_init = 0;
    public float longitude_init = 0;
    float waitTime_init = 0;

    public bool receiveLocation = false;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Get_Location());
    }

    // Update is called once per frame
    public IEnumerator Get_Location()
    {
        //만일,GPS사용 허가를 받지 못했다면, 권한 허가 팝업을 띄움
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        //만일 GPS 장치가 켜져 있지 않으면 위치 정보를 수신할 수 없다고 표시
        if (!Input.location.isEnabledByUser)
        {
            latitude_text.text = "GPS Off";
            longitude_text.text = "GPS Off";
            yield break;
        }

        //위치 데이터를 요청 -> 수신 대기
        Input.location.Start();

        //GPS 수신 상태가 초기 상태에서 일정 시간 동안 대기함
        while (Input.location.status == LocationServiceStatus.Initializing && waitTime_init < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime_init++;
        }

        //수신 실패 시 수신이 실패됐다는 것을 출력
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "위치 정보 수신 실패";
            longitude_text.text = "위치 정보 수신 실패";
        }

        //응답 대기 시간을 넘어가도록 수신이 없었다면 시간 초과됐음을 출력
        if (waitTime_init >= maxWaitTime)
        {
            latitude_text.text = "응답 대기 시간 초과";
            longitude_text.text = "응답 대기 시간 초과";
        }

        //수신된 GPS 데이터를 화면에 출력/

        LocationInfo li = Input.location.lastData;
        /*latitude = li.latitude;
       longitude = li.longitude;
       latitude_text.text = "위도 : " + latitude.ToString();
       longitude_text.text = "경도 : " + longitude.ToString();
       */
        //위치 정보 수신 시작 체크
        receiveLocation = true;

        //위치 데이터 수신 시작 이후 resendTime 경과마다 위치 정보를 갱신하고 출력
        while (receiveLocation)
        {
            li = Input.location.lastData;
            latitude_init = li.latitude;
            longitude_init = li.longitude;

            latitude_text.text = "위도 : " + latitude_init.ToString();
            longitude_text.text = "경도 : " + longitude_init.ToString();

            yield return new WaitForSeconds(resendTime);
        }
    }
}
