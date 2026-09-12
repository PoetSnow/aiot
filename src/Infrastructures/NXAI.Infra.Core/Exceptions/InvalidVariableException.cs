namespace NXAI.Infra.Core.Exceptions;

[Serializable]
public class InvalidVariableException(string message) : Exception(message), INXAIException
{
    public int Status { get; set; } = 520;
}
