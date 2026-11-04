namespace Market.Application.Modules.Catalog.Products.Commands.Delete;

public class DeleteProductCommandHandler(IAppDbContext context, IAppCurrentUser appCurrentUser)
      : IRequestHandler<DeleteProductCommand, Unit>
{
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        // Defense in depth: the endpoint is already protected by the "Admin" policy.
        if (!appCurrentUser.IsAdmin)
            throw new MarketForbiddenException("Samo administrator može brisati proizvode.");

        var product = await context.Products
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
            throw new MarketNotFoundException("Proizvod nije pronađen.");

        context.Products.Remove(product);
        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
