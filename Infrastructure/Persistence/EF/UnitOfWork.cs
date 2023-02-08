using Application.Common.Interfaces;
using Domain.Entities;

namespace Infrastructure.Persistence.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EFContext _context;
        private readonly IGenericRepository<CreditCard> _creditCardRepository;
        private readonly IGenericRepository<CreditCardProvider> _creditCardProviderRepository;

        public UnitOfWork(EFContext context)
        {
            _context = context;
            _creditCardRepository = new GenericRepository<CreditCard>(context);
            _creditCardProviderRepository = new GenericRepository<CreditCardProvider>(context);
        }

        public IGenericRepository<CreditCard> CreditCardRepository => _creditCardRepository;
        public IGenericRepository<CreditCardProvider> CreditCardProviderRepository => _creditCardProviderRepository;


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
