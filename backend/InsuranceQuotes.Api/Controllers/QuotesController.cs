using Microsoft.AspNetCore.Mvc;
using InsuranceQuotes.Api.Services;
using InsuranceQuotes.Api.Models;

namespace InsuranceQuotes.Api.Controllers;

/// <summary>
/// Controller for managing insurance quotes
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class QuotesController : ControllerBase
{
    private readonly QuoteService _quoteService;

    /// <summary>
    /// Initializes a new instance of the QuotesController
    /// </summary>
    /// <param name="quoteService">The quote service dependency</param>
    public QuotesController(QuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    /// <summary>
    /// Retrieves all available insurance quotes
    /// </summary>
    /// <returns>A collection of insurance quotes</returns>
    /// <response code="200">Returns the list of quotes</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Quote>), StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var quotes = _quoteService.GetAllQuotes();
        return Ok(quotes);
    }

    /// <summary>
    /// Retrieves a specific insurance quote by ID
    /// </summary>
    /// <param name="id">The ID of the quote to retrieve</param>
    /// <returns>The requested insurance quote</returns>
    /// <response code="200">Returns the requested quote</response>
    /// <response code="404">If the quote is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Quote), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        var quote = _quoteService.GetQuoteById(id);
        if (quote == null)
        {
            return NotFound();
        }
        return Ok(quote);
    }
}