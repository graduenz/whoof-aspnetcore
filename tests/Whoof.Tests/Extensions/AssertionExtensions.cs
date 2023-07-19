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
            .Excluding(m => m.ModifiedAt);
    }
}