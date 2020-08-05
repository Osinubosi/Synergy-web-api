using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Synergy.Repository.Models.Mapping
{
    public class CustomerAccountMap : IEntityTypeConfiguration<CustomerAccount>
    {
        public void Configure(EntityTypeBuilder<CustomerAccount> builder)
        {
            builder.ToTable("CustomerAccount");

            #region
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Id)
                    .HasColumnName("Id")
                    .IsRequired();

            builder.Property(t => t.CountryId)
                   .HasColumnName("CountryId")
                   .IsRequired();

            builder.Property(t => t.FirstName)
                    .HasColumnName("FirstName")
                    .IsRequired();

            builder.Property(t => t.LastName)
                    .HasColumnName("LastName")
                    .IsRequired();

            builder.Property(t => t.EmailAddress)
                    .HasColumnName("EmailAddress")
                    .IsRequired();

            builder.Property(t => t.DailingCode)
                  .HasColumnName("DailingCode");

            builder.Property(a => a.PhoneNumber)
                .HasColumnName("PhoneNumber")
                .IsRequired();

            builder.Property(a => a.PasswordKey)
                .HasColumnName("PasswordKey")
                .IsRequired();

            builder.Property(a => a.Password)
                .HasColumnName("Password")
                .IsRequired();

            builder.Property(a => a.HowDoyouKnow)
                .HasColumnName("HowDoyouKnow");

            builder.Property(a => a.isEmailVerified)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("isEmailVerified");

            builder.Property(a => a.CreatedDate)
                .HasColumnName("CreatedDate")
                .IsRequired()
                .HasDefaultValue(DateTime.UtcNow);

            builder.HasIndex(t => new
            {
                t.PhoneNumber
            }).HasName("IX_phoneNumbers_UniqueIndex").IsUnique();

            builder.HasIndex(t => new
            {
                t.EmailAddress

            }).HasName("IX_EmailAddress_UniqueIndex").IsUnique();

            #endregion

            #region Relationship

            builder.HasOne(t => t.Country)
                .WithMany(t => t.CustomerAccounts)
                .HasForeignKey(t => t.CountryId);

            #endregion
        }
    }
}
