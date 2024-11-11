using Core.Data;

public class MaterialDataProvider : IStaticDataProvider
{
    public MaterialAsset Asset { get; private set; }

    public MaterialDataProvider(MaterialAsset asset)
    {
        this.Asset = asset;
    }
}