﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<CreditCard> CreditCardRepository { get; }
        IGenericRepository<CreditCardProvider> CreditCardProviderRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
