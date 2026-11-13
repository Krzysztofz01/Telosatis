using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Telosatis.Common;

namespace Telosatis.Infrastructure.Database.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static KeyBuilder HasGuidIdKey<TEntity>(this EntityTypeBuilder<TEntity> @this)
        where TEntity : Entity
    {
        var keyBuilder = @this.HasKey(x => x.Id);
        @this.Property(x => x.Id).ValueGeneratedNever();

        return keyBuilder;
    }

    public static OwnedNavigationBuilder<TEntity, TValueObject> OwnsOneValueObject<TEntity, TValueObject>(this EntityTypeBuilder<TEntity> @this,
        Expression<Func<TEntity, TValueObject?>> valueObjectExpression)
        where TEntity : Entity
        where TValueObject : ValueObject<TValueObject>
    {
        var ownedNavigationBuilder = @this.OwnsOne(valueObjectExpression);
        @this.Navigation(valueObjectExpression).IsRequired();

        return ownedNavigationBuilder;
    }

    public static EntityTypeBuilder<TEntity> OwnsOneValueObject<TEntity, TValueObject>(this EntityTypeBuilder<TEntity> @this,
        Expression<Func<TEntity, TValueObject?>> valueObjectExpression,
        Action<OwnedNavigationBuilder<TEntity, TValueObject>> buildAction)
        where TEntity : Entity
        where TValueObject : ValueObject<TValueObject>
    {
        @this.OwnsOne(valueObjectExpression, buildAction);
        @this.Navigation(valueObjectExpression).IsRequired();

        return @this;
    }
}