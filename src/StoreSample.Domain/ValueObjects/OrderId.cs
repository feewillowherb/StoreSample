using Vogen;

namespace StoreSample.ValueObjects;

[ValueObject<Guid>(conversions: Conversions.SystemTextJson | Conversions.EfCoreValueConverter)]
public readonly partial struct OrderId;