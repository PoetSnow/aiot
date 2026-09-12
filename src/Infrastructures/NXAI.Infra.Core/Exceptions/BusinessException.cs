namespace NXAI.Infra.Core.Exceptions;

[Serializable]
public class BusinessException(string message) : Exception(message), INXAIException
{
    public int Status { get; set; } = 521;
}
