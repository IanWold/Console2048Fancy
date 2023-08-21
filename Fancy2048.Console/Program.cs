using Fancy2048;
using Fancy2048.Console;
using System.Text.Json;

var startingBackgroundColor = Console.BackgroundColor;
var startingForegroundColor = Console.ForegroundColor;

Console.BackgroundColor = ConsoleColor.White;
Console.ForegroundColor = ConsoleColor.Black;
Console.Clear();

WriteIntro();

GameMaster.Execute(GetSettings(), new ConsoleEnvironment());

Console.WriteLine("Press any key to continue.");
Console.ReadKey();

Finalize(startingBackgroundColor, startingForegroundColor);

static GameSettings GetSettings()
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

	return settings;
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

static void Finalize(ConsoleColor background, ConsoleColor foreground)
{
	Console.BackgroundColor = background;
	Console.ForegroundColor = foreground;
	Console.Clear();
}
