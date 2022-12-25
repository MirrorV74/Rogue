//поле - двумерный массив размера N на M
//портал - объект, пренадлежит полю
//стены - объекты, прендалежат полю, по краям поля сделать стены
//герой - объект, НЕ пренадлежит полю, рисуется поверх полю

//1) генерация поля и стен на нём
//2) генерация портала в поле (перандомировать поле если мы оказались заперты)
//3) передвижение героя по полю
//4) вход героя в портал и переход к шагу 1

using System.Xml;
using ConsoleAppRogueLike;

Random random = new Random();

char[,] dungeon;

int rowsInRoom, colsInRoom;
int roomStartPositionRow, roomStartPositionCol;
int roomsQuantity;
bool enoughSpace;
int n;
int m;

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

    while (roomsQuantity < Constants.RoomMaxQuantity)
    {
        rowsInRoom = random.Next(Constants.MinRowsInRoom, Constants.MaxRowsInRoom + 1);
        colsInRoom = random.Next(Constants.MinColsInRoom, Constants.MaxColsInRoom + 1);

        roomStartPositionRow = random.Next(0, Constants.DungeonHeight + 1 - rowsInRoom);
        roomStartPositionCol = random.Next(0, Constants.DungeonWidth + 1 - colsInRoom);

        n = roomStartPositionRow;
        m = roomStartPositionCol;

        #region Возможность установки комнаты

        enoughSpace = true;

        for (int i = n; i < n + rowsInRoom && enoughSpace; i++)
        {
            for (int j = m; j < m + colsInRoom; j++)
            {
                 if (dungeon[i, j] != Cell.OutOfBounds  //|
                //     dungeon[i+1, j] != Cell.OutOfBounds |
                //     dungeon[i-1, j] != Cell.OutOfBounds | 
                //     dungeon[i, j+1] != Cell.OutOfBounds |
                //     dungeon[i, j-1] != Cell.OutOfBounds
                    )
                {
                    enoughSpace = false;
                    break;
                }
            }
        }

        #endregion

        if (enoughSpace)
        {
            n = roomStartPositionRow;
            m = roomStartPositionCol;
            for (int i = n; i < n + rowsInRoom; i++)
            {
                for (int j = m; j < m + colsInRoom; j++)
                {
                    dungeon[i, j] = Cell.FloorEmpty;
                }
            }

            for (int i = n; i < n + rowsInRoom; i++)
            {
                dungeon[i, m] = Cell.Bound;
                dungeon[i, colsInRoom - 1 + m] = Cell.Bound;
            }

            for (int j = m; j < m + colsInRoom; j++)
            {
                dungeon[n, j] = Cell.Bound;
                dungeon[rowsInRoom - 1 + n, j] = Cell.Bound;
            }

            roomsQuantity++;
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
                    case var value when value == Cell.FloorEmpty:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;

                    case var value when value == Cell.Portal:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        break;
                    case var value when value == Cell.Bound:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                }

                Console.Write(dungeon[i, j]);
            }
        }

        Console.WriteLine();
    }
    
    // for (int i = 0; i < Constants.DungeonHeight; i++)
    // {
    //     for (int j = 0; j < Constants.DungeonWidth; j++)
    //     {
    //         Console.Write(field[i, j]+" ");
    //     }
    //
    //     Console.WriteLine();
    // }

    #endregion


    Console.ReadKey();
}