//поле - двумерный массив размера N на M
//портал - объект, пренадлежит полю
//стены - объекты, прендалежат полю, по краям поля сделать стены
//герой - объект, НЕ пренадлежит полю, рисуется поверх полю

//1) генерация подземелья
//2) генерация комнат
//3) генерация переходов
//4) передвижение героя по полю
//5) вход героя в портал и переход к шагу 1

using System.Xml;
using ConsoleAppRogueLike;

Random random = new Random();

char[,] dungeon;
int[,] roomValues;

int rowsInRoom, colsInRoom;
int roomsQuantity;
bool enoughSpace, allRoomsConnected;
int roomStartPositionRow;
int roomStartPositionCol;

int StartIHero, StartJHero;

int iHero = 1, jHero = 1;

bool heroInAdventure;

while (true)
{
    #region Генерим подземелье

    dungeon = new char[Constants.DungeonHeight, Constants.DungeonWidth];

    for (int i = 0; i < Constants.DungeonHeight; i++)
    {
        for (int j = 0; j < Constants.DungeonWidth; j++)
        {
            dungeon[i, j] = Cell.OutOfBounds;
        }
    }

    #endregion

    #region Генерим комнаты

    roomsQuantity = 0;
    roomValues = new int[Constants.RoomMaxQuantity, 6];

    while (roomsQuantity < Constants.RoomMaxQuantity)
    {
        rowsInRoom = random.Next(Constants.MinRowsInRoom, Constants.MaxRowsInRoom + 1);
        colsInRoom = random.Next(Constants.MinColsInRoom, Constants.MaxColsInRoom + 1);

        roomStartPositionRow = random.Next(1, Constants.DungeonHeight - rowsInRoom);
        roomStartPositionCol = random.Next(1, Constants.DungeonWidth - colsInRoom);
        
        #region Возможность установки комнаты

        enoughSpace = true;

        for (int i = roomStartPositionRow; i < roomStartPositionRow + rowsInRoom && enoughSpace; i++)
        {
            for (int j = roomStartPositionCol; j < roomStartPositionCol + colsInRoom; j++)
            {
                if (dungeon[i, j] != Cell.OutOfBounds
                    || dungeon[i + 1, j] != Cell.OutOfBounds
                    || dungeon[i - 1, j] != Cell.OutOfBounds
                    || dungeon[i, j + 1] != Cell.OutOfBounds
                    || dungeon[i, j - 1] != Cell.OutOfBounds)
                {
                    enoughSpace = false;
                    break;
                }
            }
        }

        #endregion

        if (enoughSpace)
        {
            roomValues[roomsQuantity, 0] = roomsQuantity + 1;
            roomValues[roomsQuantity, 1] = roomStartPositionRow;
            roomValues[roomsQuantity, 2] = roomStartPositionCol;
            roomValues[roomsQuantity, 3] = rowsInRoom;
            roomValues[roomsQuantity, 4] = colsInRoom;
            roomValues[roomsQuantity, 5] = 1;
            
            for (int i = roomStartPositionRow; i < roomStartPositionRow + rowsInRoom; i++)
            {
                for (int j = roomStartPositionCol; j < roomStartPositionCol + colsInRoom; j++)
                {
                    dungeon[i, j] = Cell.FloorEmpty;
                }
            }

            for (int i = roomStartPositionRow; i < roomStartPositionRow + rowsInRoom; i++)
            {
                dungeon[i, roomStartPositionCol] = Cell.Bound;
                dungeon[i, colsInRoom - 1 + roomStartPositionCol] = Cell.Bound;
            }

            for (int j = roomStartPositionCol; j < roomStartPositionCol + colsInRoom; j++)
            {
                dungeon[roomStartPositionRow, j] = Cell.Bound;
                dungeon[rowsInRoom - 1 + roomStartPositionRow, j] = Cell.Bound;
            }

            roomsQuantity++;
        }
    }

    #endregion

    #region Генерация переходов

    allRoomsConnected = true;

    while (!allRoomsConnected)
    {
        allRoomsConnected = true;
        for (int i = 0; i < Constants.RoomMaxQuantity; i++)
        {
            if (roomValues[i, 5] != 1)
            {
                allRoomsConnected = false;
                break;
            }
        }
    }

    #endregion

    #region Посмотреть чё там нагенерилось

    for (int i = 0; i < Constants.DungeonHeight; i++)
    {
        for (int j = 0; j < Constants.DungeonWidth; j++)
        {
            if (i == iHero && j == jHero)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(Constants.HeroSkin);
            }
            else
            {
                switch (dungeon[i, j])
                {
                    case Cell.FloorEmpty:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case Cell.OutOfBounds:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case Cell.Portal:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case Cell.Bound:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                }

                Console.Write(dungeon[i, j]);
            }
        }

        Console.WriteLine();
    }
    
    #endregion

    for (int i = 0; i < Constants.RoomMaxQuantity; i++)
    {
        for (int j = 0; j < 6; j++)
        {
            Console.Write(roomValues[i, j] + " ");
        }

        Console.WriteLine();
    }

    Console.Write(roomValues[0, 0] + " ");
    Console.Write(roomValues[0, 1] + " ");
    Console.Write(roomValues[0, 2] + " ");
    Console.Write(roomValues[0, 3] + " ");
    Console.Write(roomValues[0, 4] + " ");

    Console.ReadKey();
}