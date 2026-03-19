using FluentValidation.TestHelper;
using NickysManicurePedicure.Models.Entities;
using NickysManicurePedicure.Tests.Infrastructure;
using NickysManicurePedicure.Validation;
using Xunit;

namespace NickysManicurePedicure.Tests.Validation;

public sealed class CreateBookingRequestDtoValidatorTests
{
    [Fact]
    public async Task ValidateAsync_RequiresServiceReference()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();
        var validator = new CreateBookingRequestDtoValidator();
        var request = new NickysManicurePedicure.Dtos.Requests.CreateBookingRequestDto
        {
            FullName = "Nicky Client",
            Email = "nicky@example.com",
            PhoneNumber = "+27 68 251 8739",
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(2)),
            PreferredTime = new TimeOnly(10, 30),
            Message = "I would like a weekday appointment if one is available.",
            SourcePage = "Homepage"
        };

        var result = await validator.TestValidateAsync(request);

        result.ShouldHaveValidationErrorFor(x => x)
            .WithErrorMessage("Either preferred service id or preferred service name is required.");
    }

    [Fact]
    public async Task ValidateAsync_RejectsUnpublishedServiceReference()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        var category = TestDataFactory.CreateCategory();
        dbContext.ServiceCategories.Add(category);
        await dbContext.SaveChangesAsync();

        var unpublishedService = TestDataFactory.CreateService(
            category.Id,
            name: "Draft Service",
            slug: "draft-service",
            status: ContentStatus.Draft);
        dbContext.Services.Add(unpublishedService);
        await dbContext.SaveChangesAsync();

        var validator = new CreateBookingRequestDtoValidator();
        var request = new NickysManicurePedicure.Dtos.Requests.CreateBookingRequestDto
        {
            FullName = "Nicky Client",
            Email = "nicky@example.com",
            PhoneNumber = "+27 68 251 8739",
            PreferredServiceId = unpublishedService.Id,
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(2)),
            PreferredTime = new TimeOnly(10, 30),
            Message = "I would like a weekday appointment if one is available.",
            SourcePage = "Homepage"
        };

        var result = await validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ValidateAsync_AcceptsValidPublishedServiceReference()
    {
        await using var database = new TestSqliteDatabase();
        await using var dbContext = database.CreateDbContext();

        var category = TestDataFactory.CreateCategory();
        dbContext.ServiceCategories.Add(category);
        await dbContext.SaveChangesAsync();

        var service = TestDataFactory.CreateService(category.Id);
        dbContext.Services.Add(service);
        await dbContext.SaveChangesAsync();

        var validator = new CreateBookingRequestDtoValidator();
        var request = new NickysManicurePedicure.Dtos.Requests.CreateBookingRequestDto
        {
            FullName = "Nicky Client",
            Email = "nicky@example.com",
            PhoneNumber = "+27 68 251 8739",
            PreferredServiceId = service.Id,
            PreferredServiceName = service.Name,
            PreferredDate = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(2)),
            PreferredTime = new TimeOnly(10, 30),
            Message = "I would like a weekday appointment if one is available.",
            SourcePage = "Homepage"
        };

        var result = await validator.TestValidateAsync(request);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
