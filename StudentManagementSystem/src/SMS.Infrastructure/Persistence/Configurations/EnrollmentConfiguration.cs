using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Infrastructure.Persistence.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.ToTable("Enrollments");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.Property(e => e.EnrolledAtUtc).IsRequired();
        builder.Property(e => e.Grade).HasMaxLength(10);

        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        // Prevents the same student enrolling in the same course twice at the DB level too —
        // defense in depth: Student.EnrollIn() checks it in memory, this catches race conditions.
        builder.HasIndex(e => new { e.StudentId, e.CourseId }).IsUnique();
    }
}