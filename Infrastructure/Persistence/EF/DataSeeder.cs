using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EF
{
    public class DataSeeder
    {
        private readonly IGenericRepository<CreditCardProvider> _creditCardProviderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DataSeeder(EFContext context)
        {
            _creditCardProviderRepository = new GenericRepository<CreditCardProvider>(context);
            _unitOfWork = new UnitOfWork(context);
        }

        public async Task EnsureSeeded()
        {
            var anyCreditCardProviders = await _creditCardProviderRepository.GetAll().AnyAsync();

            if (!anyCreditCardProviders)
            {
                var lastModified = DateTime.UtcNow;
                _creditCardProviderRepository.Add(new CreditCardProvider() { Name = "VISA", CardNumberRegEx = "^4[0-9]{12}(?:[0-9]{3})?$", LastModified = lastModified });
                _creditCardProviderRepository.Add(new CreditCardProvider() { Name = "Mastercard", CardNumberRegEx = "^5[1-5][0-9]{14}|^(222[1-9]|22[3-9]\\d|2[3-6]\\d{2}|27[0-1]\\d|2720)[0-9]{12}$", LastModified = lastModified });
                _creditCardProviderRepository.Add(new CreditCardProvider() { Name = "Amex", CardNumberRegEx = "^3[47][0-9]{13}$", LastModified = lastModified });
                _creditCardProviderRepository.Add(new CreditCardProvider() { Name = "Discover", CardNumberRegEx = "^6(?:011|5[0-9]{2})[0-9]{12}$", LastModified = lastModified });

                await _unitOfWork.SaveChangesAsync(default);
            }
        }
    }
}
