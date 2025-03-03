using Newtonsoft.Json;

namespace ULTRAVR.Input.Actions
{
    /// <summary>
    ///     Represents a modified action object of Binds.json. If it's an array, use only the zeroth index.
    /// </summary>
    public class ModifiedAction
    {
        [JsonProperty("path")] public string LocalPath { get; set; }
        [JsonIgnore] public string Path => GetPath();
        public string GetPath()
        {
            if (LocalPath.Contains("/"))
            {
                string[] Temp = LocalPath.Split('/');
                return Temp[Temp.Length - 1];
            }
            return LocalPath;
        }
        public ModifiedAction(string LocalPath)
        {
            this.LocalPath = LocalPath;
        }
    }
}
