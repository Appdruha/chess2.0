using Newtonsoft.Json;

public class MessageToClient
{
    [JsonProperty("type")]
    public MessageType Type;
    [JsonProperty("params")]
    public GameRoomDto? Params;
    [JsonProperty("roomId")]
    public string RoomId;

    public MessageToClient(MessageType type, GameRoomDto? messParams, string roomId)
    {
        Type = type;
        Params = messParams;
        RoomId = roomId;
    }
}

public class MessageFromClient
{
    [JsonProperty("type")]
    public MessageType Type;
    [JsonProperty("params")]
    public string Params;
    [JsonProperty("roomId")]
    public string RoomId;
}

public enum MessageType
{
    Create,
    Join,
    Init,
    Start,
    Leave,
    Move,
    NextTurn,
    EndGame,
    ChangeFigure,
    Restart,
    Error,
}