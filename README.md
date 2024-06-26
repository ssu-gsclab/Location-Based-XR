**Location-based XR**

Unity Version: 2022.3.17f1

Packages
- Meta XR All-in-One SDK ver. 65.0.0
- Meta XR Interaction SDK ver. 65.0.0

Scenes
- SampleScene: Google Map 연동 확인
- TestScene: 테스트용
- Demo: 기능 구현된 scene

Scripts
- LocationtoPosition.cs
위도, 경도에 따라 북쪽을 기준으로 사진 배치

- GPSManager.cs
스마트폰으로부터 GPS 받아오기(아직 구현 X)

- PriximityChecker.cs
사용자가 광화문으로부터 특정 범위 내에 위치하는지 확인

- DirectionChangeDetector.cs
사용자의 머리가 크게 회전하면 재시작 
