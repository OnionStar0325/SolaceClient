# SolaceClient

�� ������Ʈ�� PubSub+Event Broker ��ǰ�� [solace](https://solace.com/ko/products/event-broker) ������ �޽����� �۽�(Send) �� ť ����¡�� �� �� �ִ�
DOTNET 6 ����� Winform ������Ʈ �Դϴ�.

>**SolaceManagement ������Ʈ�� �ڵ�� solace���� �����ϴ� [Sample Project](https://github.com/SolaceSamples/solace-samples-dotnet)�� �����߽��ϴ�.**
## ���� OS
DOTNET 6 �� �����ϴ� Windows �迭 OS

## ���� ���

### ���� �غ�
�� ������Ʈ�� �����ϱ� ���ؼ� DOTNET 6.0 SDK�� ��ġ�Ǿ�� �մϴ�. 
[DOTNET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

### ���� ����

1.Project Clone
```
git clone https://github.com/OnionStar0325/SolaceClient.git
```

2.Project ����
DOTNET �ַ�� ��ġ���� �Ʒ� ����� ����
```shell
dotnet build -P:Configuration=Release
```

3.���� ���� ��ġ
������ ���������� �Ϸ�Ǿ��ٸ�, �Ʒ� ��ġ�� Winform ���α׷��� �����˴ϴ�. 
```shell
[DOTNET �ַ��]\output\Release\net6.0-windows\publish\SolaceClient.exe
```

## ��� ���

1. Solace ��������(connectioninfo.json)
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
���� ��� ���
> �޽��� SEND�� ${������} �� �������� ������ ����� �� �ֽ��ϴ�.

2. Solace Logging ����(log4net.config)
Solace Client�� log4net ���̺귯���� ����ϰ� �ֽ��ϴ�. 
�α� ���������� [config-examples](https://logging.apache.org/log4net/release/config-examples.html)�� ���� �� �ֽʽÿ�.

3. Solace Client UI
![image](https://github.com/user-attachments/assets/d2a4d7d6-3766-4789-b51c-b72ab650f2f0)

## Revision History

### v0.1
���� ������Ʈ ����