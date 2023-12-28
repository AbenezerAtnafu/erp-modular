namespace MOLS.Shared;

/// <summary>
/// Represents an error occurs when a required attribute is missing
/// </summary>
public class MissingAttributeException : Exception
{
    public MissingAttributeException(string message) : base(message) { }
}

