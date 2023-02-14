using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CreditCardProviders.Commands
{
    public record DeleteCreditCardProviderCommand(int Id) : IRequest<int>;

    public class DeleteCreditCardProviderCommandHandler : IRequestHandler<DeleteCreditCardProviderCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<CreditCardProvider> _creditCardProviderRepository;

        public DeleteCreditCardProviderCommandHandler(
            IUnitOfWork unitOfWork,
            IGenericRepository<CreditCardProvider> _creditCardProviderRepository)
        {
            _unitOfWork = unitOfWork;
            this._creditCardProviderRepository = _creditCardProviderRepository;
        }

        public async Task<int> Handle(DeleteCreditCardProviderCommand request, CancellationToken cancellationToken)
        {
            var provider = new CreditCardProvider { Id = request.Id };

            _creditCardProviderRepository.Delete(provider);

            return await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
