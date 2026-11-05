using Market.Domain.Entities.Shifts;

namespace Market.Application.Modules.Shifts.CashRegisters.Commands.Create;

public sealed class CreateCashRegisterCommandHandler(IAppDbContext context)
    : IRequestHandler<CreateCashRegisterCommand, int>
{
    public async Task<int> Handle(CreateCashRegisterCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        bool exists = await context.CashRegisters
            .AnyAsync(x => x.Name.ToLower() == name.ToLower(), cancellationToken);

        if (exists)
            throw new MarketConflictException($"Kasa sa nazivom {name} već postoji.");

        var cashRegister = new CashRegisterEntity
        {
            Name = name,
            IsActive = request.IsActive
        };

        context.CashRegisters.Add(cashRegister);
        await context.SaveChangesAsync(cancellationToken);

        return cashRegister.Id;
    }
}
