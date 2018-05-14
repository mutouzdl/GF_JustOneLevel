/// <summary>
/// 游戏入口。
/// </summary>
public partial class GameEntry {
    public static BuiltinDataComponent BuiltinData {
        get;
        private set;
    }

    private static void InitCustomComponents () {
        BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent> ();
    }
}