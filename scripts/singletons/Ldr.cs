using Godot;

namespace NFA.Godot
{
    public enum UI_Resource { TitleCard }

    public static class Ldr
    {
        public static GodotTscn[] Levels = new GodotTscn[]
        {
            new GodotTscn(@"uid://2uuiiakyapd5")
        };

        public static GodotTscn[] UI = new GodotTscn[]
        {
            new GodotTscn(@"uid://rahmg6waphiw")
        };

        public static bool LoadLevel(Node _parent, string _UID, out PackedScene lvl_tscn)
        {
            int uidHash = Str.Hash(_UID);
            lvl_tscn = null;

            GodotTscn currLvl;
            for(int i = 0, len = Levels.Length; i < len; ++i)
            {
                currLvl = Levels[i];
                if (currLvl.id == uidHash)
                {
                    lvl_tscn = ResourceLoader.Load<PackedScene>(currLvl.UID);
                    _parent.AddChild(lvl_tscn.Instantiate());
                    return true;
                }
            }
            return false;
        }

        public static bool LoadUI(Node _parent, string _UID, out PackedScene ui_tscn)
        {
            int uidHash = Str.Hash(_UID);
            ui_tscn = null;

            GodotTscn uiElement;
            for (int i = 0, len = UI.Length; i < len; ++i)
            {
                uiElement = UI[i];
                if (uiElement.id == uidHash)
                {
                    ui_tscn = ResourceLoader.Load<PackedScene>(uiElement.UID);
                    _parent.AddChild(ui_tscn.Instantiate());
                    return true;
                }
            }
            return false;
        }
    }

    public class GodotTscn
    {
        public int id;
        public string UID;

        public GodotTscn(string _UID)
        {
            UID = _UID;
            id = Str.Hash(UID);
        }
    }
}
