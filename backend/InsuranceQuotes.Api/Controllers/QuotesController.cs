using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsuranceQuotes.Api.Services;
using InsuranceQuotes.Api.Models;

namespace InsuranceQuotes.Api.Controllers;

/// <summary>
/// Controller for managing insurance quotes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class QuotesController : ControllerBase
{
    private readonly QuoteService _quoteService;
    private readonly ILogger<QuotesController> _logger;

    /// <summary>
    /// Initializes a new instance of the QuotesController
    /// </summary>
    /// <param name="quoteService">The quote service dependency</param>
    /// <param name="logger">The logger dependency</param>
    public QuotesController(QuoteService quoteService, ILogger<QuotesController> logger)
    {
        _quoteService = quoteService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all available insurance quotes
    /// </summary>
    /// <returns>A collection of insurance quotes</returns>
    /// <response code="200">Returns the list of quotes</response>
    /// <response code="401">If the user is not authenticated</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Quote>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetAll()
    {
        _logger.LogInformation("Getting all quotes for user {User}", User.Identity?.Name);
        var quotes = _quoteService.GetAllQuotes();
        return Ok(quotes);
    }

    /// <summary>
    /// Retrieves a specific insurance quote by ID
    /// </summary>
    /// <param name="id">The ID of the quote to retrieve</param>
    /// <returns>The requested insurance quote</returns>
    /// <response code="200">Returns the requested quote</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <response code="404">If the quote is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Quote), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        _logger.LogInformation("Getting quote {QuoteId} for user {User}", id, User.Identity?.Name);
        var quote = _quoteService.GetQuoteById(id);
        if (quote == null)
        {
            _logger.LogWarning("Quote {QuoteId} not found", id);
            return NotFound();
        }
        return Ok(quote);
    }
}