using Microsoft.AspNetCore.Mvc;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Services;

namespace Writing.Controllers;

[ApiController]
public class OAuthController : Controller {

    private readonly OAuthService oauthService;

    public OAuthController(OAuthService oauthService) {
        this.oauthService = oauthService;
    }


    [HttpPost]
    [Route("/api/oauth")]
    public async Task<IActionResult> oAuthValidToken(String provider, String token) {
        ResponseObject<ResponseTokenObject> responseObject = await oauthService.validateGoogleToken(token);
        if (responseObject == null) {
            return Unauthorized(responseObject);
        }

        return Ok(responseObject);
    }
}
