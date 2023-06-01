﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writing.Payloads.Responses;
using Writing.Services;
using Action = Writing.Enumerates.Action;

namespace Writing.Controllers; 

[ApiController]
[Route("/api/follow")]
public class RelationshipController : Controller {

    private readonly RelationshipService relationshipService;

    public RelationshipController(RelationshipService relationshipService) {
        this.relationshipService = relationshipService;
    }

    [HttpPost]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult follow([FromHeader] int partnerId, [FromHeader] string action) {
        int ownerId = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<Action> responseObject = relationshipService.follow(partnerId, ownerId, action);

        if (responseObject.Data.Equals(Action.NOTCONSTRAINT)) {
            return BadRequest(responseObject);
        }

        return Ok(responseObject);
    }
}