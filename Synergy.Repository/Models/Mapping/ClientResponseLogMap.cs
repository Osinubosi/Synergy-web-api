using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Synergy.Repository.Models.Mapping
{
    public class ClientResponseLogMap : IEntityTypeConfiguration<ClientResponseLog>
    {
        public void Configure(EntityTypeBuilder<ClientResponseLog> builder)
        {
            builder.ToTable("ResponseLog");

            #region
            builder.HasKey(t => t.ResponseId);

            builder.Property(t => t.ResponseId)
                    .HasColumnName("Id")
                    .IsRequired();

            builder.Property(t => t.Payload)
                  .HasColumnName("Payload")
                  .IsRequired();

            builder.Property(a => a.RequestUniqueRefernceId)
                .HasColumnName("UniqueReferenceId")
                .IsRequired();


            builder.Property(a => a.HttpStatusCode)
                .HasColumnName("StatusCode");

            builder.Property(a => a.SynergyStatusCode)
               .HasColumnName("SynergyStatusCode");

            builder.Property(a => a.DateLogged)
                .HasColumnName("DateLogged")
                .IsRequired()
                .HasDefaultValue(DateTime.UtcNow);

            #endregion
        }
    }
}
