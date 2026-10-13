using Asp.Versioning;
using Asp.Versioning.Builder;
using ECommerce.API.Endpoints.Extensions;
using ECommerce.API.Filters;
using ECommerce.API.Models;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Abstractions;
using ECommerce.UseCases.Commons.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class NotificationEndpoints
{
    public static IEndpointRouteBuilder MapNotificationEndpoints(
        this IEndpointRouteBuilder endpoints,
        ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/v{version:apiVersion}/notifications")
            .WithTags("Notifications")
            .WithApiVersionSet(apiVersionSet)
            .HasApiVersion(new ApiVersion(1, 0))
            .RequireAuthorization()
            .AddEndpointFilter<AuditEndpointFilter>()
            .RequireAuthorization();

        group.MapGet("/", async (
            [AsParameters] NotificationPaginationRequest request,
            INotificationQueryService queryService,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var result = await queryService.GetUserNotificationsPagedAsync(
                userId,
                request.PageNumber,
                request.PageSize,
                ct);

            return result.FromResult(
                httpContext,
                ApiMessages.NotificationsRetrieved);
        })
        .WithSummary("Get current user's notifications")
        .WithDescription("Returns paginated notifications for the authenticated user.")
        .Produces<ApiResponse<PagedResult<NotificationDto>>>(
            StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/unread-count", async (
            INotificationQueryService queryService,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var result = await queryService.GetUnreadCountAsync(userId, ct);

            return result.FromResult(
                httpContext,
                ApiMessages.UnreadNotificationCountRetrieved);
        })
        .WithSummary("Get unread notifications count")
        .Produces<ApiResponse<int>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized);

        group.MapPatch("/{id:int}/read", async (
            int id,
            INotificationCommandService commandService,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var result = await commandService.MarkAsReadAsync(id, userId, ct);

            return result.FromResult(
                httpContext,
                ApiMessages.NotificationMarkedAsRead);
        })
        .WithSummary("Mark notification as read")
        .Produces<ApiResponse<Result>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPatch("/read-all", async (
            INotificationCommandService commandService,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var result = await commandService.MarkAllReadAsync(userId, ct);

            return result.FromResult(
                httpContext,
                ApiMessages.AllNotificationsMarkedAsRead);
        })
        .WithSummary("Mark all notifications as read")
        .Produces<ApiResponse<Result>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized);

        group.MapDelete("/{id:int}", async (
            int id,
            INotificationCommandService commandService,
            HttpContext httpContext,
            CancellationToken ct) =>
        {
            var userIdClaim = httpContext.User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Results.Unauthorized();

            var result = await commandService.DeleteAsync(id, userId, ct);

            return result.FromResult(
                httpContext,
                ApiMessages.NotificationDeleted);
        })
        .WithSummary("Delete notification")
        .Produces<ApiResponse<Result>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound);

        return endpoints;
    }
}

public sealed class NotificationPaginationRequest
{
    [FromQuery]
    public int PageNumber { get; init; } = 1;

    [FromQuery]
    public int PageSize { get; init; } = 10;
}