# SolaceClient

이 프로젝트는 PubSub+Event Broker 제품인 [solace](https://solace.com/ko/products/event-broker) 서버로 메시지를 송신(Send) 및 큐 브라우징을 할 수 있는
DOTNET 6 기반의 Winform 프로젝트 입니다.

>**SolaceManagement 프로젝트의 코드는 solace에서 제공하는 [Sample Project](https://github.com/SolaceSamples/solace-samples-dotnet)를 참고했습니다.**
## 지원 OS
DOTNET 6 를 지원하는 Windows 계열 OS

## 빌드 방법

### 사전 준비
이 프로젝트를 빌드하기 위해선 DOTNET 6.0 SDK가 설치되어야 합니다. 
[DOTNET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### 빌드 절차

1.Project Clone
```
git clone https://github.com/OnionStar0325/SolaceClient.git
```

2.Project 배포
DOTNET 솔루션 위치에서 아래 명령을 수행
```shell
dotnet build -P:Configuration=Release
```

3.실행 파일 위치
배포가 정상적으로 완료되었다면, 아래 위치에 Winform 프로그램이 생성됩니다. 
```shell
[DOTNET 솔루션]\output\Release\net6.0-windows\publish\SolaceClient.exe
```

## 사용 방법

1. Solace 접속정보(connectioninfo.json)
```JSON
{
  "HostName": "SOLACE SERVER HOSTNAME",
  "VPNName": "SOLACE VPN NAME",
  "QueueName": "SOLACE QUEUE NAME",
  "UserName": "SOLACE USERNAME",
  "Password": "SOLACE PASSWORD",
  "SEMPHostName": "SOLACE URL FOR SEMP v2",
  "ReplyQueueName": "REPLY QUEUE FOR SEND (Only used for ListenReply)",
  "TimeKeyVariable": "Variable Name for SendTimeKey. Solace Client will replace the variable to TimeKey(yyyyMMddHHmmssfffffff)",
  "ExcludePatterns": [
    "REGEXP pattern for exclude message when browsing queues"
  ]
}
```
변수 사용 방법
> 메시지 SEND시 ${변수명} 의 형식으로 변수를 사용할 수 있습니다.

2. Solace Logging 정보(log4net.config)
Solace Client는 log4net 라이브러리를 사용하고 있습니다. 
로그 설정정보는 [config-examples](https://logging.apache.org/log4net/release/config-examples.html)를 참고 해 주십시오.

3. Solace Client UI
![image](https://github.com/user-attachments/assets/d2a4d7d6-3766-4789-b51c-b72ab650f2f0)

## Revision History

### v0.1
최초 프로젝트 생성