public static class AppContext
{
    public static AppState State { get; private set; } = AppState.Idle;

    public static void SetState(AppState state)
    {
        State = state;
        Console.WriteLine($"\n[STATE] → {state}\n");
    }
}
