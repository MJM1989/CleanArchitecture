using System.Data;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IGetDbConnection
    {
        IDbConnection Get();
    }
}