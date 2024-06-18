namespace AplicacaoWeb.Models.Enums
{
    [Flags]
    public enum AccessLevel
    {
        View = 1,
        Create = 2,
        Update = 4,
        Delete = 8
    }
}
