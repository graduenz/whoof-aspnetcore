using FluentAssertions.Equivalency;
using Whoof.Domain.Common;

namespace Whoof.Tests.Extensions;

public static class AssertionExtensions
{
    public static EquivalencyAssertionOptions<TExpectation> ExcludingBaseFields<TExpectation>(
        this EquivalencyAssertionOptions<TExpectation> options)
        where TExpectation : BaseEntity
    {
        return options.Excluding(m => m.Id)
            .Excluding(m => m.CreatedAt)
            .Excluding(m => m.CreatedBy)
            .Excluding(m => m.ModifiedAt)
            .Excluding(m => m.ModifiedBy);
    }
    
    public static EquivalencyAssertionOptions<TExpectation> ExcludingOwnershipFields<TExpectation>(
        this EquivalencyAssertionOptions<TExpectation> options)
        where TExpectation : BaseOwnedEntity
    {
        return options.Excluding(m => m.OwnerId);
    }
}