namespace Market.Application.Modules.Shifts.CashRegisters.Queries.GetById;

public sealed class GetCashRegisterByIdQueryHandler(IAppDbContext context)
    : IRequestHandler<GetCashRegisterByIdQuery, GetCashRegisterByIdQueryDto>
{
    public async Task<GetCashRegisterByIdQueryDto> Handle(GetCashRegisterByIdQuery request, CancellationToken cancellationToken)
    {
        var cashRegister = await context.CashRegisters
            .Where(x => x.Id == request.Id)
            .Select(x => new GetCashRegisterByIdQueryDto
            {
                Id = x.Id,
                Name = x.Name,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (cashRegister == null)
        {
            throw new MarketNotFoundException($"Kasa (ID={request.Id}) nije pronađena.");
        }

        return cashRegister;
    }
}
