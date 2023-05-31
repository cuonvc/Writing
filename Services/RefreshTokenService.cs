using Writing.Entities;

namespace Writing.Services; 

public interface RefreshTokenService {
    void initBlank(User user);

    RefreshToken renewRefreshToken(int userId);
}