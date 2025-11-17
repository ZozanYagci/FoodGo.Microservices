using FoodGo.CatalogService.Domain.Entities;
using FoodGo.CatalogService.Domain.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodGo.CatalogService.Infrastructure.Context
{
    public class CatalogDbContext : DbContext
    {

        private readonly IMediator _mediator;
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

            var publishTasks = domainEvents
                .Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken))
                .ToArray();

            await Task.WhenAll(publishTasks);
        }

        public DbSet<Restaurant> Restaurants => Set<Restaurant>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<ProductPrice> ProductPrices => Set<ProductPrice>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        }

        // önce db'ye yaz, sonra domain event'leri publish et.
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            if (_mediator != null)
            {
                await DispatchDomainEventsAsync(cancellationToken);
            }
            return result;
        }
    }
}