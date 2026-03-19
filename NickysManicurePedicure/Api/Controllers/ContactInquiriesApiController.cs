using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers;

[ApiController]
[Route("api/contact-inquiries")]
[Produces("application/json")]
public sealed class ContactInquiriesApiController(IContactInquiryApiService contactInquiryApiService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ContactInquiryCreateResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ContactInquiryCreateResponse>> Create(
        [FromBody] CreateContactInquiryDto request,
        CancellationToken cancellationToken)
    {
        var response = await contactInquiryApiService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.InquiryId }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ContactInquiryReadResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ContactInquiryReadResponse>>> GetList(
        [FromQuery] ContactInquiryQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await contactInquiryApiService.GetInquiriesAsync(query, cancellationToken));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContactInquiryReadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactInquiryReadResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var inquiry = await contactInquiryApiService.GetByIdAsync(id, cancellationToken);
        return Ok(inquiry ?? throw new NotFoundException("Contact inquiry", id));
    }

    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(ContactInquiryReadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactInquiryReadResponse>> UpdateStatus(
        int id,
        [FromBody] UpdateContactInquiryStatusDto request,
        CancellationToken cancellationToken)
    {
        var inquiry = await contactInquiryApiService.UpdateStatusAsync(id, request, cancellationToken);
        return Ok(inquiry ?? throw new NotFoundException("Contact inquiry", id));
    }
}
