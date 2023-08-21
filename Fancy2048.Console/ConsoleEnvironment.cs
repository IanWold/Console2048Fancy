using static System.Console;

namespace Fancy2048.Console;

public class ConsoleEnvironment : IGameEnvironment
{
	static readonly Dictionary<ConsoleKey, GameInput> _inputs = new()
	{
		[ConsoleKey.LeftArrow] = GameInput.MoveLeft,
		[ConsoleKey.RightArrow] = GameInput.MoveRight,
		[ConsoleKey.UpArrow] = GameInput.MoveUp,
		[ConsoleKey.DownArrow] = GameInput.MoveDown,
		[ConsoleKey.U] = GameInput.Undo,
		[ConsoleKey.Escape] = GameInput.Forfeit
	};

	static void PrintHelp()
	{
		WriteLine();
		WriteLine("Press the arrow keys on the keyboard to move,");
		WriteLine("Press U to undo,");
		WriteLine("Press Escape to forfeit.");
	}

	static void PrintPoorInput()
	{
		WriteLine();
		WriteLine("That is not an acceptable input.");
		WriteLine("Press H if you need help.");
	}

	static void WriteChar(char toWrite, int times)
	{
		for (var i = 0; i < times; i++)
		{
			Write(toWrite);
		}
	}

	static void WriteColor(int toWrite, int length)
	{
		ForegroundColor = length switch
		{
			1 => ConsoleColor.DarkGray,
			2 => ConsoleColor.DarkRed,
			3 => ConsoleColor.Red,
			4 => ConsoleColor.DarkMagenta,
			_ => ConsoleColor.Magenta,
		};

		Write(toWrite);
		ForegroundColor = ConsoleColor.Black;
	}

	public GameInput GetNextInput()
	{
		ConsoleKey input;

		do
		{
			input = ReadKey().Key;

			if (!_inputs.ContainsKey(input))
			{
				if (input == ConsoleKey.H)
				{
					PrintHelp();
				}
				else
				{
					PrintPoorInput();
				}
			}
		}
		while (!_inputs.ContainsKey(input));

		return _inputs[input];
	}

	public void OnStateChanged(GameState state)
	{
		Clear();
		WriteLine($"Score: {state.Score} | Undos: {state.Settings.MaxUndo}");
		WriteLine();

		var totalSpaces = state.Settings.TargetScore.ToString().Length;

		for (int row = 0; row < state.Settings.GridSize; row++)
		{
			for (int column = 0; column < state.Settings.GridSize; column++)
			{
				Write(" | ");

				if (state.Grid[row, column] != 0)
				{
					WriteChar(' ', totalSpaces - state.Grid[row, column].ToString().Length);
					WriteColor(state.Grid[row, column], state.Grid[row, column].ToString().Length);
				}
				else
				{
					WriteChar(' ', totalSpaces);
				}
			}

			WriteLine();
			WriteChar('-', (3 + totalSpaces) * state.Settings.GridSize);
			WriteLine();
		}

		WriteLine();
		Write(">: ");
	}
}
