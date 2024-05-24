using Fleck;
using Newtonsoft.Json;

public class Controller
{
    public static string CreateRoom(IWebSocketConnection client, string modeInString)
    {
        var mode = GameMode.CommonChess;
        if (modeInString.Trim().ToLower() == "chess20")
        {
            mode = GameMode.Chess20;
        }

        var roomId = Guid.NewGuid().ToString();
        Db.CreateRoom(roomId, client, mode);
        return CreateJsonMessage(MessageType.Create, null, roomId);
    }

    public static string JoinRoom(IWebSocketConnection client, string roomId)
    {
        var gameRoom = Db.JoinRoom(roomId, client);
        if (gameRoom == null)
        {
            return CreateJsonMessage(MessageType.Error, null, roomId);
        }

        return CreateJsonMessage(MessageType.Join, null, roomId);
    }

    public static string Init(string roomId)
    {
        var gameRoom = Db.GetRoomState(roomId);
        var gameRoomDto = new GameRoomDto(gameRoom, FigureColors.WHITE);
        return CreateJsonMessage(MessageType.Init, gameRoomDto, roomId);
    }

    public static (List<Player>, (string firstMessage, string secondMessage)) Start(string roomId)
    {
        var gameRoom = Db.StartGame(roomId);
        var (firstGameRoomData, secondGameRoomData) = CreateGameRoomDtos(gameRoom);
        return CreateOutputForBroadcast(gameRoom, roomId, MessageType.Start, firstGameRoomData, secondGameRoomData);
    }

    public static (Player player, string message)? Leave(string roomId, IWebSocketConnection client)
    {
        var gameRoom = Db.Leave(roomId, client);
        if (gameRoom != null)
        {
            return (gameRoom.Players[0], CreateJsonMessage(MessageType.Leave, null, roomId));
        }

        return null;
    }

    public static (List<Player>, (string firstMessage, string secondMessage)) Restart(string roomId)
    {
        var gameRoom = Db.RestartGame(roomId);
        var (firstGameRoomData, secondGameRoomData) = CreateGameRoomDtos(gameRoom);
        return CreateOutputForBroadcast(gameRoom, roomId, MessageType.Start, firstGameRoomData, secondGameRoomData);
    }

    public static (List<Player>, (string firstMessage, string secondMessage)) GiveUp(string roomId, string playerColor)
    {
        var winner = GameWinner.White;
        if (playerColor.Trim().ToLower() == "white")
        {
            winner = GameWinner.Black;
        }

        var gameRoom = Db.GiveUp(roomId, winner);
        var (firstGameRoomData, secondGameRoomData) = CreateGameRoomDtos(gameRoom);
        return CreateOutputForBroadcast(gameRoom, roomId, MessageType.EndGame, firstGameRoomData, secondGameRoomData);
    }

    public static (List<Player>, (string firstMessage, string secondMessage)) ConfirmDraw(string roomId)
    {
        var gameRoom = Db.ConfirmDraw(roomId);
        var (firstGameRoomData, secondGameRoomData) = CreateGameRoomDtos(gameRoom);
        return CreateOutputForBroadcast(gameRoom, roomId, MessageType.EndGame, firstGameRoomData, secondGameRoomData);
    }

    public static (Player player, string message)? OfferDraw(string roomId, IWebSocketConnection client)
    {
        var players = Db.GetRoomState(roomId).Players;
        foreach (var player in players)
        {
            if (player.Connection != client)
            {
                return (player, CreateJsonMessage(MessageType.ConfirmDraw, null, roomId));
            }
        }

        return null;
    }

    public static (List<Player>, (string firstMessage, string secondMessage)) ChangeFigure(string roomId,
        string figureName)
    {
        var gameRoom = Db.ChangeFigure(roomId, figureName.Trim().ToLower());
        var (firstGameRoomData, secondGameRoomData) = CreateGameRoomDtos(gameRoom);

        if (gameRoom.Winner != null)
        {
            return CreateOutputForBroadcast(gameRoom, roomId, MessageType.EndGame, firstGameRoomData, secondGameRoomData);
        }

        return CreateOutputForBroadcast(gameRoom, roomId, MessageType.Start, firstGameRoomData, secondGameRoomData);
    }

    public static (List<Player>, (string? firstMessage, string? secondMessage)) Move(string roomId, string moveParams)
    {
        var gameRoom = Db.MoveFigure(roomId, moveParams);

        var changingFigureCell = gameRoom.ChangingFigureCell;
        if (changingFigureCell != null)
        {
            if (changingFigureCell.Figure!.Color == FigureColors.WHITE)
            {
                return (gameRoom.Players,
                    (CreateJsonMessage(MessageType.ChangeFigure, null, roomId), null));
            }

            return (gameRoom.Players,
                (null, CreateJsonMessage(MessageType.ChangeFigure, null, roomId)));
        }

        var (firstGameRoomData, secondGameRoomData) = CreateGameRoomDtos(gameRoom);

        if (gameRoom.Winner != null)
        {
            return CreateOutputForBroadcast(gameRoom, roomId, MessageType.EndGame, firstGameRoomData, secondGameRoomData);
        }

        return CreateOutputForBroadcast(gameRoom, roomId, MessageType.NextTurn, firstGameRoomData, secondGameRoomData);
    }

    private static string CreateJsonMessage(MessageType type, GameRoomDto? gameRoomDto, string roomId)
    {
        var message = new MessageToClient(type, gameRoomDto, roomId);
        return JsonConvert.SerializeObject(message);
    }

    private static (GameRoomDto firstGameRoomData, GameRoomDto secondGameRoomData) CreateGameRoomDtos(
        GameRoom gameRoomState)
    {
        var firstGameRoomData = new GameRoomDto(gameRoomState, FigureColors.WHITE);
        var secondGameRoomData = new GameRoomDto(gameRoomState, FigureColors.BLACK);
        return (firstGameRoomData, secondGameRoomData);
    }

    private static (List<Player>, (string firstMessage, string secondMessage)) CreateOutputForBroadcast(
        GameRoom gameRoomState, string roomId, MessageType messageType, GameRoomDto firstGameRoomData,
        GameRoomDto secondGameRoomData)
    {
        return (gameRoomState.Players,
            (CreateJsonMessage(messageType, firstGameRoomData, roomId),
                CreateJsonMessage(messageType, secondGameRoomData, roomId)));
    }
}