using System;
using UnityEngine;

internal static class DataTransfer
{
    public static string GetJsonMessagePlayerName(string playerName)
    {
        MessageData messageData = new MessageData();
        messageData.messageType = MessageType.PlayerName;
        messageData.MessageValue = playerName;

        return JsonUtility.ToJson(messageData);
    }

    public static string GetJsonMessage(string message)
    {
        MessageData messageData = new MessageData();
        messageData.messageType = MessageType.Message;
        messageData.MessageValue = message;

        return JsonUtility.ToJson(messageData);
    }

    public static MessageData GetMessageData(string jsonString)
    {
        return JsonUtility.FromJson<MessageData>(jsonString);
    }
}

[Serializable]
internal sealed class MessageData
{
    public MessageType messageType;
    public string MessageValue;
}

internal enum MessageType
{
    Message,
    PlayerName
}