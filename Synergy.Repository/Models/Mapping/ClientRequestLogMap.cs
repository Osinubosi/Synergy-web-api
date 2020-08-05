using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Synergy.Repository.Models.Mapping
{
    public class ClientRequestLogMap : IEntityTypeConfiguration<ClientRequestLog>
    {
        public void Configure(EntityTypeBuilder<ClientRequestLog> builder)
        {
            builder.ToTable("RequestLog");

            #region
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                    .HasColumnName("Id")
                    .IsRequired();

            builder.Property(t => t.ClientId)
                    .HasColumnName("ClientId")
                    .IsRequired();

            builder.Property(t => t.RequestMethod)
                    .HasColumnName("RequestMethod")
                    .IsRequired();

            builder.Property(t => t.Url)
                    .HasColumnName("Url")
                    .IsRequired();

            builder.Property(t => t.Payload)
                  .HasColumnName("Payload");

            builder.Property(a => a.RequestUniqueRefernceId)
                .HasColumnName("UniqueReferenceId")
                .IsRequired();

            builder.Property(a => a.RequestReferencId)
                .HasColumnName("RequestrefernceId");

            builder.Property(a => a.DateLogged)
                .HasColumnName("DateLogged")
                .IsRequired()
                .HasDefaultValue(DateTime.UtcNow);

            #endregion


        }
    }
}
