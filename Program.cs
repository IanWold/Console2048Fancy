using Console2048Functional;
using System.Text.Json;

var startingBackgroundColor = Console.BackgroundColor;
var startingForegroundColor = Console.ForegroundColor;

Console.BackgroundColor = ConsoleColor.White;
Console.ForegroundColor = ConsoleColor.Black;
Console.Clear();

WriteIntro();

ExecuteGameLoop(Initialize());

Console.WriteLine("Press any key to continue.");
Console.ReadKey();

Finalize(startingBackgroundColor, startingForegroundColor);

static GameState Initialize()
{
	GameSettings? settings;

	using (var reader = new StreamReader("Settings.json"))
	{
		settings = JsonSerializer.Deserialize<GameSettings>(reader.ReadToEnd());
	}

	if (settings is null)
	{
		throw new Exception("Unable to deserialize Settings.json.");
	}

	return new(settings);
}

static void WriteIntro()
{
	Console.WriteLine("#################################");
	Console.WriteLine("           Console2048           ");
	Console.WriteLine("#################################");
	Console.WriteLine();
	Console.WriteLine("Use the arrow keys to move around");
	Console.WriteLine("        Type 'H' for help        ");
	Console.WriteLine();
	Console.WriteLine(" Combine numbers to get to 2048! ");
	Console.WriteLine();
	Console.WriteLine("#################################");
	Console.WriteLine();
	Console.Write("Press any key to start...");
	Console.ReadKey();
	Console.Clear();
}

static void ExecuteGameLoop(GameState currentState)
{
	GameState? previousState = null;

	while (true)
	{
		if (previousState != currentState)
		{
			if (currentState.Result == GameResult.Won)
			{
				Console.Clear();
				Console.WriteLine("You won!");
				Console.WriteLine($"Your score was {currentState.Score}");

				break;
			}
			else if (currentState.Result == GameResult.Lost)
			{
				Console.Clear();
				Console.WriteLine("You lost.");
				Console.WriteLine($"Your score was {currentState.Score}");

				break;
			}

			currentState.Write();
		}

		previousState = currentState;

		switch (Console.ReadKey().Key)
		{
			case ConsoleKey.LeftArrow:
				currentState = currentState.Move(Direction.Left);
				break;

			case ConsoleKey.RightArrow:
				currentState = currentState.Move(Direction.Right);
				break;

			case ConsoleKey.DownArrow:
				currentState = currentState.Move(Direction.Down);
				break;

			case ConsoleKey.UpArrow:
				currentState = currentState.Move(Direction.Up);
				break;

			case ConsoleKey.U:
				Console.WriteLine();

				if (currentState.Settings.MaxUndo != 0)
				{
					if (previousState is GameState undoState)
					{
						currentState = undoState with
						{
							Settings = currentState.Settings with
							{
								MaxUndo = currentState.Settings.MaxUndo - 1
							}
						};
						previousState = null;
					}
					else
					{
						Console.WriteLine("You may only undo once at a time.");
					}
				}
				else
				{
					Console.WriteLine("You may not undo anymore.");
				}
				break;

			case ConsoleKey.H:
				Console.WriteLine();
				Console.WriteLine("Console2048 Copyright 2014 Ian Wold");
				Console.WriteLine("Licensed under the MIT Open-Source License");
				Console.WriteLine();
				Console.WriteLine("Use the arrow keys on the keyboard to move");
				break;

			default:
				Console.WriteLine();
				Console.WriteLine("That is not an acceptable input.");
				Console.WriteLine("Use the arrow keys to move around.");
				break;
		}
	}
}

static void Finalize(ConsoleColor background, ConsoleColor foreground)
{
	Console.BackgroundColor = background;
	Console.ForegroundColor = foreground;
	Console.Clear();
}
