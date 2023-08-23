namespace Fancy2048;

public record GameState(GameSettings Settings, int Score = 0, bool IsForfeit = false)
{
	public int[,] Grid { get; init; } = (new int[Settings.GridSize, Settings.GridSize]).AddRandom(Settings.GridSize);

	public GameResult Result =>
		_result ??=
			IsForfeit
				? GameResult.Lost
				: Score == Settings.TargetScore
					? GameResult.Won
					: Grid.Any(i => i == 0)
						? GameResult.Ongoing
						: GameResult.Lost;
	private GameResult? _result;

	internal GameState Move(MovementDirection direction)
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

		return newState.Result != GameResult.Ongoing
			? newState
			: newState with
			{
				Grid = currentGrid.AddRandom(Settings.GridSize)
			};
	}
}
