using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telosatis.Domain.Token;
using Telosatis.Infrastructure.Database.Extensions;

namespace Telosatis.Infrastructure.Database.Configurations;

internal sealed class TokenConfiguration : IEntityTypeConfiguration<Token>
{
    public void Configure(EntityTypeBuilder<Token> builder)
    {
        builder.ToTable("Tokens");
        
        builder.HasGuidIdKey();

        builder.OwnsOneValueObject(x => x.Name, x =>
        {
            x.Property(n => n.Value)
                .IsRequired()
                .HasColumnName(nameof(Token.Name))
                .HasMaxLength(TokenName.MaxLength);

            x.HasIndex(n => n.Value).IsUnique();
        });

        builder.OwnsOneValueObject(x => x.SecretHash, x =>
        {
            x.Property(s => s.Value)
                .IsRequired()
                .HasColumnName(nameof(Token.SecretHash))
                .HasMaxLength(TokenSecretHash.HashLength);

            x.HasIndex(s => s.Value).IsUnique();
        });

        builder.OwnsOneValueObject(x => x.Role, x =>
        {
            x.Property(r => r.Value)
                .IsRequired()
                .HasColumnName(nameof(Token.Role));
        });
        
        builder.OwnsOneValueObject(x => x.CreatedAt, x =>
        {
            x.Property(c => c.Value)
                .IsRequired()
                .HasColumnName(nameof(Token.CreatedAt));
        });

        builder.OwnsOneValueObject(x => x.ExpiresAt, x =>
        {
            x.Property(e => e.Value)
                .IsRequired(false)
                .HasColumnName(nameof(Token.ExpiresAt));
        });
    }
}