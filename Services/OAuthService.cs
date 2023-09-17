using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services;

public interface OAuthService {

    Task<ResponseObject<ResponseTokenObject>> validateGoogleToken(String token);
}