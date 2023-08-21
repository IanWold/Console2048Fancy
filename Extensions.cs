namespace Console2048Functional;

public static class Extensions
{
	static readonly Random _random = new();

	static int[,] GetClone(this int[,] grid, int gridSize)
	{
		var newGrid = new int[gridSize, gridSize];
		Array.Copy(grid, newGrid, grid.Length);
		return newGrid;
	}

	static int[,] Rotate(this int[,] grid, int gridSize, bool clockwise)
	{
		var newGrid = new int[gridSize, gridSize];

		for (var row = 0; row < gridSize; row++)
		{
			for (var column = 0; column < gridSize; column++)
			{
				if (clockwise)
				{
					newGrid[column, gridSize - 1 - row] = grid[row, column];
				}
				else
				{
					newGrid[gridSize - 1 - column, row] = grid[row, column];
				}
			}
		}

		return newGrid;
	}

	static int[,] Flip(this int[,] grid, int gridSize)
	{
		var newGrid = new int[gridSize, gridSize];

		for (var i = 0; i < gridSize * gridSize; i++)
		{
			var row = i / gridSize;
			var col = i % gridSize;
			newGrid[gridSize - 1 - row, gridSize - 1 - col] = grid[row, col];
		}

		return newGrid;
	}

	public static int[,] Rotate(this int[,] grid, int gridSize, int timesToRotate) =>
		timesToRotate == 0
		? grid.GetClone(gridSize)
		: Math.Abs(timesToRotate) == 2
			? grid.Flip(gridSize)
			: grid.Rotate(gridSize, timesToRotate > 0);

	public static int[,] Shift(this int[,] grid, int gridSize, int row)
	{
		var newGrid = grid.GetClone(gridSize);

		for (var i = 0; i < gridSize; i++)
		{
			for (var column = i; column > 0; column--)
			{
				if (newGrid[row, column - 1] == 0)
				{
					newGrid[row, column - 1] = newGrid[row, column];
					newGrid[row, column] = 0;

					if (newGrid[row, column - 1] != 0)
					{
						column++;
					}
				}
			}
		}

		return newGrid;
	}

	public static int[,] AddRandom(this int[,] grid, int gridSize)
	{
		var newGrid = grid.GetClone(gridSize);
		int row, column;

		do
		{
			row = _random.Next(gridSize);
			column = _random.Next(gridSize);
		}
		while (newGrid[row, column] != 0);

		newGrid[row, column] =
			_random.Next(1, 101) <= 75
				? 2
				: 4;

		return newGrid;
	}

	public static bool Any(this int[,] grid, Predicate<int> predicate)
	{
		foreach (var i in grid)
		{
			if (predicate(i))
			{
				return true;
			}
		}

		return false;
	}

	public static bool SequenceEqual(this int[,] first, int[,] second) =>
		first.Rank == second.Rank
		&& Enumerable.Range(0, first.Rank).All(dimension => first.GetLength(dimension) == second.GetLength(dimension))
		&& first.Cast<int>().SequenceEqual(second.Cast<int>());
}
