namespace Console2048Functional;

public record GameState(GameSettings Settings, int Score = 0)
{
	public int[,] Grid { get; init; } = (new int[Settings.GridSize, Settings.GridSize]).AddRandom(Settings.GridSize);

	public GameResult Result =>
		_result ??=
			Score == Settings.TargetScore
				? GameResult.Won
				: Grid.Any(i => i == 0)
					? GameResult.Ongoing
					: GameResult.Lost;
	private GameResult? _result;

	public GameState Move(Direction direction)
	{
		var rotation = (int)direction;
		var currentScore = Score;
		var currentGrid = Grid.Rotate(Settings.GridSize, rotation);

		for (var row = 0; row < Settings.GridSize; row++)
		{
			currentGrid = currentGrid.Shift(Settings.GridSize, row);

			for (var column = 0; column < Settings.GridSize - 1; column++)
			{
				if (currentGrid[row, column] == currentGrid[row, column + 1])
				{
					var combinedValue = currentGrid[row, column + 1] * 2;

					currentGrid[row, column] = combinedValue;
					currentGrid[row, column + 1] = 0;

					currentScore += combinedValue;
				}
			}

			currentGrid = currentGrid.Shift(Settings.GridSize, row);
		}

		currentGrid = currentGrid.Rotate(Settings.GridSize, -1 * rotation);

		if (currentGrid.SequenceEqual(Grid))
		{
			return this;
		}

		var newState = this with
		{
			Score = currentScore,
			Grid = currentGrid
		};

		if (newState.Result != GameResult.Ongoing)
		{
			return newState;
		}

		return newState with
		{
			Grid = currentGrid.AddRandom(Settings.GridSize)
		};
	}

	public void Write()
	{
		Console.Clear();
		Console.WriteLine($"Score: {Score} | Undos: {Settings.MaxUndo}");
		Console.WriteLine();

		var totalSpaces = Settings.TargetScore.ToString().Length;

		for (int row = 0; row < Settings.GridSize; row++)
		{
			for (int column = 0; column < Settings.GridSize; column++)
			{
				Console.Write(" | ");

				if (Grid[row, column] != 0)
				{
					WriteChar(' ', totalSpaces - Grid[row, column].ToString().Length);
					WriteColor(Grid[row, column], Grid[row, column].ToString().Length);
				}
				else
				{
					WriteChar(' ', totalSpaces);
				}
			}

			Console.WriteLine();
			WriteChar('-', (3 + totalSpaces) * Settings.GridSize);
			Console.WriteLine();
		}

		Console.WriteLine();
		Console.Write(">: ");

		static void WriteChar(char toWrite, int times)
		{
			for (var i = 0; i < times; i++)
			{
				Console.Write(toWrite);
			}
		}

		static void WriteColor(int toWrite, int length)
		{
			Console.ForegroundColor = length switch
			{
				1 => ConsoleColor.DarkGray,
				2 => ConsoleColor.DarkRed,
				3 => ConsoleColor.Red,
				4 => ConsoleColor.DarkMagenta,
				_ => ConsoleColor.Magenta,
			};

			Console.Write(toWrite);
			Console.ForegroundColor = ConsoleColor.Black;
		}
	}
}
