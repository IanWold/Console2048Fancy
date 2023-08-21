namespace Fancy2048;

public static class GameMaster
{
	public static void Execute(GameSettings settings, IGameEnvironment environment)
	{
		GameState? previousState = null;
		var currentState = new GameState(settings);

		while (true)
		{
			if (previousState != currentState)
			{
				environment.OnStateChanged(currentState);

				if (currentState.Result != GameResult.Ongoing)
				{
					break;
				}
			}

			previousState = currentState;
			currentState = environment.GetNextInput() switch
			{
				GameInput.Forfeit =>
					currentState with
					{
						IsForfeit = true
					},
				GameInput.Undo =>
					currentState.Settings.MaxUndo != 0 && previousState is GameState undoState
						? undoState with
						{
							Settings = currentState.Settings with
							{
								MaxUndo = currentState.Settings.MaxUndo - 1
							}
						}
						: currentState,
				GameInput input =>
					currentState.Move((MovementDirection)input)
			};
		}
	}
}
