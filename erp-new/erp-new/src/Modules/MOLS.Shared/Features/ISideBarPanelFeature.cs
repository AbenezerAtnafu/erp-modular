using MOLS.Shared.Features;

namespace MOLS.Shared.Features;

public interface ISideBarPanelFeature
{
    string Id { get; }
    string Url { get; }
    string Title { get; }
    string FAIcon { get; }
    ICollection<ISubMenuItem> SubMenu { get; }
}

