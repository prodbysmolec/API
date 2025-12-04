# 🤖 KI-Review: API Artikelsystem

## 📊 Entwicklungslevel-Bewertung

### Aktueller Stand: **Fortgeschrittener Junior / Solider Intermediate Developer**

Diese API zeigt, dass du bereits fortgeschrittene Konzepte der modernen .NET-Entwicklung verstehst und anwendest. Du bist definitiv über das Anfängerniveau hinaus und zeigst gutes Verständnis für professionelle Architekturmuster.

---

## ✅ Was du bereits richtig machst

### 1. Clean Architecture / Onion Architecture
Du verwendest bereits eine saubere Schichtenarchitektur:
- **Domain** → Entities, Enums, Errors
- **Application** → Commands, Queries, Services, Interfaces
- **Infrastructure** → Repositories, Database Context, Configurations
- **API** → Controllers, Extensions

Das ist der **Industriestandard** und zeigt professionelles Denken.

### 2. CQRS-Pattern mit MediatR
```csharp
public record GetArtikelQuery(GetAllArtikelRequest request) : IRequest<Result<ListContainerDto<GetArtikelResponse>>>;
public record GetArtikelByIdQuery(int Id) : IRequest<Result<GetArtikelResponse>>;
```
Die Trennung von Commands und Queries ist ausgezeichnet!

### 3. Result Pattern
```csharp
public class Result<TValue>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public BaseError? Error { get; }
    // ...
}
```
Das vermeidet Exceptions und macht Error-Handling explizit – sehr professionell!

### 4. FluentValidation
```csharp
public class CreateArtikelCommandValidator : AbstractValidator<CreateArtikelCommand>
{
    RuleFor(x => x.Name)
        .NotEmpty()
        .WithMessage("Name ist erforderlich.")
        .MaximumLength(100);
}
```
Saubere Validierung getrennt von der Geschäftslogik.

### 5. AutoMapper für DTOs
```csharp
CreateMap<Artikel, GetArtikelResponse>()
    .ForMember(dest => dest.BildBase64, opt => opt.MapFrom(...));
```
Gute Trennung zwischen Domain-Entities und API-Responses.

### 6. Auditable Entities
```csharp
public abstract class AuditableEntity : IAuditable
{
    public string? ErstelltVon { get; set; }
    public DateTime ErstelltAm { get; set; }
    public string? BearbeitetVon { get; set; }
    public DateTime BearbeitetAm { get; set; }
}
```
Audit-Trail ist für professionelle Anwendungen unerlässlich.

### 7. Unit of Work Pattern
```csharp
public interface IUnitOfWork : IDisposable
{
    IArtikelRepository ArtikelRepository { get; }
    Task CommitAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
```
Transaktionssicherheit auf höchstem Niveau.

---

## 🚀 Empfehlungen für ein professionelleres Artikelsystem

### 1. **Domain-Driven Design (DDD) - Value Objects einführen**

**Aktuell:**
```csharp
public decimal Preis { get; set; }
public required string Name { get; set; }
```

**Professioneller:**
```csharp
// Domain/ValueObjects/Money.cs
public sealed class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }
    
    private Money(decimal amount, string currency = "EUR")
    {
        if (amount < 0) throw new DomainException("Preis darf nicht negativ sein");
        Amount = amount;
        Currency = currency;
    }
    
    public static Money Create(decimal amount, string currency = "EUR") 
        => new Money(amount, currency);
    
    public static Money Zero => new Money(0);
    
    public Money Add(Money other) 
        => new Money(Amount + other.Amount, Currency);
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}

// Domain/ValueObjects/ArtikelName.cs
public sealed class ArtikelName : ValueObject
{
    public string Value { get; }
    
    private ArtikelName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Artikelname darf nicht leer sein");
        if (value.Length > 100)
            throw new DomainException("Artikelname darf maximal 100 Zeichen haben");
        Value = value;
    }
    
    public static ArtikelName Create(string value) => new ArtikelName(value);
}
```

**Vorteile:**
- Validierung direkt im Domain-Layer
- Typsicherheit (kein versehentliches Verwechseln von Preis und Menge)
- Wiederverwendbar in der gesamten Anwendung

---

### 2. **Aggregate Roots mit Factory Methods**

**Aktuell:**
```csharp
public class Artikel : AuditableEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    // ...
}
```

**Professioneller:**
```csharp
public sealed class Artikel : AuditableEntity, IAggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public ArtikelId Id { get; private set; }
    public ArtikelName Name { get; private set; }
    public Money Preis { get; private set; }
    public Bestand Bestand { get; private set; }
    public ArtikelStatus Status { get; private set; }
    
    // Private Konstruktor - nur Factory Method erlaubt
    private Artikel() { }
    
    // Factory Method mit Validierung
    public static Result<Artikel> Create(
        ArtikelName name,
        Money preis,
        Bestand bestand,
        ArtikelGruppeId artikelGruppeId)
    {
        var artikel = new Artikel
        {
            Id = ArtikelId.CreateUnique(),
            Name = name,
            Preis = preis,
            Bestand = bestand,
            Status = ArtikelStatus.Verfügbar
        };
        
        artikel.RaiseDomainEvent(new ArtikelErstelltEvent(artikel));
        return Result.Success(artikel);
    }
    
    // Domänen-Methoden mit Business-Logik
    public Result BestandReduzieren(int menge)
    {
        if (menge <= 0)
            return Result.Failure(ArtikelErrors.UngültigeMenge());
        
        if (Bestand.Menge < menge)
            return Result.Failure(ArtikelErrors.NichtGenugBestand());
        
        Bestand = Bestand.Reduzieren(menge);
        
        if (Bestand.IstUnterMindestbestand)
            RaiseDomainEvent(new BestandNiedrigEvent(this));
        
        return Result.Success();
    }
    
    public Result PreisAendern(Money neuerPreis)
    {
        var alterPreis = Preis;
        Preis = neuerPreis;
        RaiseDomainEvent(new PreisGeändertEvent(this, alterPreis, neuerPreis));
        return Result.Success();
    }
    
    private void RaiseDomainEvent(DomainEvent domainEvent) 
        => _domainEvents.Add(domainEvent);
}
```

---

### 3. **Domain Events für lose Kopplung**

```csharp
// Domain/Events/ArtikelErstelltEvent.cs
public sealed record ArtikelErstelltEvent(Artikel Artikel) : DomainEvent;

// Domain/Events/BestandNiedrigEvent.cs
public sealed record BestandNiedrigEvent(Artikel Artikel) : DomainEvent;

// Domain/Events/PreisGeändertEvent.cs
public sealed record PreisGeändertEvent(
    Artikel Artikel, 
    Money AlterPreis, 
    Money NeuerPreis) : DomainEvent;

// Application/EventHandlers/BestandNiedrigEventHandler.cs
public class BestandNiedrigEventHandler : INotificationHandler<BestandNiedrigEvent>
{
    private readonly INotificationService _notificationService;
    
    public async Task Handle(BestandNiedrigEvent notification, CancellationToken ct)
    {
        await _notificationService.SendLowStockAlert(notification.Artikel);
    }
}
```

---

### 4. **Specification Pattern für komplexe Queries**

**Aktuell:**
```csharp
if (!string.IsNullOrEmpty(request.NameContains))
{
    artikel = artikel.Where(a => a.Name.Contains(request.NameContains));
}
if (request.MinPreis.HasValue)
{
    artikel = artikel.Where(a => a.Preis >= request.MinPreis.Value);
}
// ... viele weitere if-Statements
```

**Professioneller:**
```csharp
// Domain/Specifications/ArtikelSpecifications.cs
public static class ArtikelSpecifications
{
    public static Specification<Artikel> MitNamenEnthält(string suchbegriff)
        => new(a => a.Name.Value.Contains(suchbegriff, StringComparison.OrdinalIgnoreCase));
    
    public static Specification<Artikel> MitPreisZwischen(decimal min, decimal max)
        => new(a => a.Preis.Amount >= min && a.Preis.Amount <= max);
    
    public static Specification<Artikel> UnterMindestbestand()
        => new(a => a.Bestand.IstUnterMindestbestand);
    
    public static Specification<Artikel> IstVerfügbar()
        => new(a => a.Status == ArtikelStatus.Verfügbar);
}

// Repository
public async Task<PagedResult<Artikel>> GetAllAsync(Specification<Artikel> spec, PagingParams paging)
{
    return await _context.Artikel
        .Where(spec.ToExpression())
        .ToPaginatedListAsync(paging);
}

// Verwendung im Handler
var spec = ArtikelSpecifications.MitNamenEnthält(request.NameContains)
    .And(ArtikelSpecifications.MitPreisZwischen(request.MinPreis, request.MaxPreis))
    .And(ArtikelSpecifications.IstVerfügbar());

var result = await _repository.GetAllAsync(spec, paging);
```

---

### 5. **Strongly-Typed IDs**

**Aktuell:**
```csharp
public int Id { get; set; }
public int ArtikelGruppeId { get; set; }
```

**Professioneller:**
```csharp
// Domain/ValueObjects/ArtikelId.cs
public readonly struct ArtikelId : IEquatable<ArtikelId>
{
    public int Value { get; }
    
    private ArtikelId(int value) => Value = value;
    
    public static ArtikelId Create(int value) => new ArtikelId(value);
    public static ArtikelId CreateUnique() => new ArtikelId(0); // DB generiert
    
    public bool Equals(ArtikelId other) => Value == other.Value;
    public override bool Equals(object? obj) => obj is ArtikelId other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => $"Artikel-{Value}";
}
```

**Vorteil:** Du kannst nicht versehentlich eine `LieferantId` an eine Methode übergeben, die eine `ArtikelId` erwartet!

---

### 6. **Bessere API-Versionierung**

**Empfehlung:**
```csharp
// Program.cs
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

// Controller
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ArtikelController : ControllerBase
{
}
```

---

### 7. **HATEOAS für RESTful APIs**

```csharp
public class ArtikelResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Preis { get; set; }
    
    public ICollection<Link> Links { get; set; } = new List<Link>();
}

public record Link(string Href, string Rel, string Method);

// Im Handler
var response = new ArtikelResponse
{
    Id = artikel.Id,
    Name = artikel.Name,
    Links = new[]
    {
        new Link($"/api/v1/artikel/{artikel.Id}", "self", "GET"),
        new Link($"/api/v1/artikel/{artikel.Id}", "update", "PUT"),
        new Link($"/api/v1/artikel/{artikel.Id}", "delete", "DELETE"),
        new Link($"/api/v1/artikel/{artikel.Id}/wareneingaenge", "wareneingaenge", "GET"),
        new Link($"/api/v1/artikel/{artikel.Id}/statistik", "statistik", "GET")
    }
};
```

---

### 8. **Outbox Pattern für Eventual Consistency**

Für den Wareneingang/-ausgang mit Bestandsänderungen:

```csharp
// Infrastructure/Outbox/OutboxMessage.cs
public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
}

// Im SaveChanges
public override async Task<int> SaveChangesAsync(CancellationToken ct)
{
    var domainEvents = ChangeTracker
        .Entries<IAggregateRoot>()
        .SelectMany(e => e.Entity.DomainEvents)
        .ToList();
    
    foreach (var domainEvent in domainEvents)
    {
        await OutboxMessages.AddAsync(new OutboxMessage
        {
            Type = domainEvent.GetType().Name,
            Content = JsonSerializer.Serialize(domainEvent),
            OccurredOnUtc = DateTime.UtcNow
        });
    }
    
    return await base.SaveChangesAsync(ct);
}
```

---

### 9. **Caching-Strategie**

```csharp
// Application/Behaviors/CachingBehavior.cs
public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery
{
    private readonly IDistributedCache _cache;
    
    public async Task<TResponse> Handle(TRequest request, ...)
    {
        var cacheKey = request.CacheKey;
        var cached = await _cache.GetStringAsync(cacheKey);
        
        if (cached is not null)
            return JsonSerializer.Deserialize<TResponse>(cached);
        
        var response = await next();
        
        await _cache.SetStringAsync(
            cacheKey, 
            JsonSerializer.Serialize(response),
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
        
        return response;
    }
}

// Query mit Caching
public record GetArtikelQuery : IRequest<Result<ArtikelDto>>, ICacheableQuery
{
    public int ArtikelId { get; init; }
    public string CacheKey => $"artikel_{ArtikelId}";
}
```

---

### 10. **Rate Limiting hinzufügen**

```csharp
// Program.cs (.NET 7+)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", limiterOptions =>
    {
        limiterOptions.PermitLimit = 100;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 10;
    });
});

// Controller
[EnableRateLimiting("fixed")]
public class ArtikelController : ControllerBase
{
}
```

---

## 📁 Empfohlene Ordnerstruktur für DDD

```
src/
├── Domain/
│   ├── Aggregates/
│   │   └── Artikel/
│   │       ├── Artikel.cs              # Aggregate Root
│   │       ├── ArtikelId.cs            # Strongly-Typed ID
│   │       ├── ArtikelStatistik.cs     # Value Object
│   │       └── Bestand.cs              # Value Object
│   ├── Common/
│   │   ├── Entity.cs
│   │   ├── AggregateRoot.cs
│   │   └── ValueObject.cs
│   ├── Events/
│   │   ├── ArtikelErstelltEvent.cs
│   │   └── BestandGeändertEvent.cs
│   ├── Errors/
│   │   └── ArtikelErrors.cs
│   └── Specifications/
│       └── ArtikelSpecifications.cs
│
├── Application/
│   ├── Artikel/
│   │   ├── Commands/
│   │   │   ├── CreateArtikel/
│   │   │   │   ├── CreateArtikelCommand.cs
│   │   │   │   ├── CreateArtikelCommandHandler.cs
│   │   │   │   └── CreateArtikelCommandValidator.cs
│   │   │   └── UpdateArtikel/
│   │   │       └── ...
│   │   └── Queries/
│   │       └── GetArtikel/
│   │           ├── GetArtikelQuery.cs
│   │           └── GetArtikelQueryHandler.cs
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   ├── LoggingBehavior.cs
│   │   │   ├── ValidationBehavior.cs
│   │   │   └── CachingBehavior.cs
│   │   └── Interfaces/
│   │       └── IArtikelRepository.cs
│   └── EventHandlers/
│       └── ArtikelErstelltEventHandler.cs
│
├── Infrastructure/
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── Configurations/
│   │   │   └── ArtikelConfiguration.cs
│   │   └── Repositories/
│   │       └── ArtikelRepository.cs
│   ├── Services/
│   │   └── ...
│   └── Outbox/
│       └── OutboxProcessor.cs
│
└── API/
    ├── Controllers/
    │   └── V1/
    │       └── ArtikelController.cs
    ├── Middleware/
    │   └── ExceptionHandlingMiddleware.cs
    └── Extensions/
        └── ...
```

---

## 🎯 Prioritäten für dein Portfolio

### Hohe Priorität (Sofort umsetzen):
1. ✅ Value Objects für Preis, Menge, etc.
2. ✅ Strongly-Typed IDs
3. ✅ Domain Events
4. ✅ API-Versionierung
5. ✅ Specification Pattern

### Mittlere Priorität (Für Fortgeschrittene):
1. Outbox Pattern
2. HATEOAS
3. Caching-Strategie
4. Rate Limiting

### Niedrige Priorität (Optional, aber beeindruckend):
1. Event Sourcing
2. CQRS mit separaten Read/Write-Datenbanken
3. GraphQL-Endpoint zusätzlich zu REST

---

## 📚 Empfohlene Ressourcen

1. **Bücher:**
   - "Domain-Driven Design" von Eric Evans
   - "Clean Architecture" von Robert C. Martin
   - "Implementing Domain-Driven Design" von Vaughn Vernon

2. **GitHub Repositories:**
   - [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb) - Microsoft Reference Architecture
   - [Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture)

3. **YouTube:**
   - Milan Jovanović (Clean Architecture in .NET)
   - Nick Chapsas (Advanced .NET Concepts)

---

## ⭐ Fazit

Deine API ist bereits **deutlich über dem Durchschnitt**. Du verwendest moderne Patterns und hast ein gutes Verständnis für Softwarearchitektur. 

Mit den oben genannten Verbesserungen kannst du dein Portfolio auf **Senior-Level** heben und bei Bewerbungen einen exzellenten Eindruck hinterlassen.

**Tipp für Vorstellungsgespräche:** Erkläre, WARUM du diese Patterns verwendest, nicht nur WAS sie sind. Zeige, dass du die Trade-offs verstehst!

---

*Generiert am: 2024-12-04*
