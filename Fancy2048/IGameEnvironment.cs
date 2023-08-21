namespace Fancy2048;

public interface IGameEnvironment
{
	GameInput GetNextInput();

	void OnStateChanged(GameState state);
}
