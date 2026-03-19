using Microsoft.AspNetCore.Mvc;
using NickysManicurePedicure.Application.Abstractions;
using NickysManicurePedicure.Common.Exceptions;
using NickysManicurePedicure.Dtos.Common;
using NickysManicurePedicure.Dtos.Requests;
using NickysManicurePedicure.Dtos.Responses;

namespace NickysManicurePedicure.Api.Controllers.Admin;

[Route("api/admin/contact-inquiries")]
[Route("api/contact-inquiries")]
public sealed class AdminContactInquiriesApiController(IContactInquiryAdminService contactInquiryAdminService) : AdminControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ContactInquiryReadResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PagedResponse<ContactInquiryReadResponse>>> GetList(
        [FromQuery] ContactInquiryQueryParameters query,
        CancellationToken cancellationToken)
    {
        return Ok(await contactInquiryAdminService.GetInquiriesAsync(query, cancellationToken));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ContactInquiryReadResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ContactInquiryReadResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var inquiry = await contactInquiryAdminService.GetByIdAsync(id, cancellationToken);
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
        var inquiry = await contactInquiryAdminService.UpdateStatusAsync(id, request, cancellationToken);
        return Ok(inquiry ?? throw new NotFoundException("Contact inquiry", id));
    }
}
