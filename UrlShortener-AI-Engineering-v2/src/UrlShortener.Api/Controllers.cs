using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Contracts;
using UrlShortener.Application.Services;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/v1/urls")]
public sealed class UrlsController(
    IShortUrlService service,
    ILogger<UrlsController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ShortUrlResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ShortUrlResponse>> Create(
        CreateShortUrlRequest request,
        CancellationToken ct)
    {
        var response = await service.CreateAsync(
            request,
            $"{Request.Scheme}://{Request.Host}",
            ct);

        logger.LogInformation(
            "Short URL created. Id={UrlId}, Code={ShortCode}",
            response.Id,
            response.ShortCode);

        return CreatedAtAction(
            nameof(GetAnalytics),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:long}/analytics")]
    [ProducesResponseType(typeof(AnalyticsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AnalyticsResponse>> GetAnalytics(
        long id,
        CancellationToken ct)
    {
        var result = await service.GetAnalyticsAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}

public sealed class RedirectController(
    IShortUrlService service,
    ILogger<RedirectController> logger) : ControllerBase
{
    [HttpGet("/r/{code:minlength(3):maxlength(20)}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Redirect(string code, CancellationToken ct)
    {
        var resolved = await service.ResolveAsync(code, ct);

        if (resolved is null)
            return NotFound(new { error = "Short URL not found or expired." });

        // Analytics is deliberately best-effort. It must not break the redirect path.
        try
        {
            await service.RecordClickAsync(
                resolved.Id,
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers.UserAgent.ToString(),
                Request.Headers.Referer.ToString(),
                ct);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Analytics recording failed for URL {UrlId}",
                resolved.Id);
        }

        return Redirect(resolved.OriginalUrl);
    }
}