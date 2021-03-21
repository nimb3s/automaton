using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nimb3s.Automaton.Core.Repositories.Sql;
using Nimb3s.Automaton.Pocos;
using Nimb3s.Automaton.Pocos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Api.Controllers
{
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IAutomatonDatabaseContext _dbContext;

        public RequestController(IAutomatonDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets status of a request using the http request id.
        /// </summary>
        /// /// <remarks>
        /// Sample request:
        ///
        ///  GET api/automaton/requests/httpRequestId
        ///
        /// </remarks>
        /// <returns>returns a <see cref="HttpRequestStatusModel"/></returns>
        /// <response code="200">Returns the http status model if http request id is found</response>
        /// <response code="400">If the request is not found</response>   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("api/automaton/requests/{httpRequestId}")]
        public async Task<ActionResult> GetHttpRequestStatusAndResponse(Guid httpRequestId)
        {
            var request = await _dbContext.HttpRequestStatusRepository.GetByHttpRequestIdAsync(httpRequestId);
            var requestStatusNum = request.HttpRequestStatusTypeId;

            return Ok(new HttpRequestStatusModel
            {
                Id = request.Id,
                HttpRequestStatus = (HttpRequestStatusType)requestStatusNum,
                Url = request.Url,
                StatusCode = request.StatusCode,
                Body = request.Body
            });
        }
    }
}
