namespace Core.Saves
{
    public interface ISaveLoad
    {
        public void Save(string key, string value);
        public string Load(string key);
    }
}