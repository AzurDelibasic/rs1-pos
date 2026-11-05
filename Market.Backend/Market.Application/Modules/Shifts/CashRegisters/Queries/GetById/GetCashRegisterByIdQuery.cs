namespace Market.Application.Modules.Shifts.CashRegisters.Queries.GetById;

public sealed class GetCashRegisterByIdQuery : IRequest<GetCashRegisterByIdQueryDto>
{
    public required int Id { get; set; }
}
