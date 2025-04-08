using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Sibers.Core.Entities;
using Sibers.Core.Interfaces;

namespace Sibers.Services.Services;

public class UserService
{
    private readonly IUnitOfWork _uow;
    public UserService(IUnitOfWork uow)
    {
        _uow = uow;
    }


}
