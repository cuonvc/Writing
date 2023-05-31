using Writing.Entities;
using Writing.Repositories;

namespace Writing.Services.Impl; 

public class RefreshTokenImpl : RefreshTokenService {

    private readonly DataContext dataContext;

    public RefreshTokenImpl(DataContext dataContext) {
        this.dataContext = dataContext;
    }
    
    public void initBlank(User user) {
        RefreshToken refreshToken = new RefreshToken {
            Token = "",
            User = user
        };

        dataContext.RefreshTokens.Add(refreshToken);
        dataContext.SaveChanges();
    }

    public RefreshToken renewRefreshToken(int userId) {
        RefreshToken refreshToken = dataContext.RefreshTokens
            .Where(refreshToken => refreshToken.User.Id.Equals(userId))
            .FirstOrDefault();

        if (refreshToken == null) {
            return null;
        }

        refreshToken.Token = "refresh_" + Guid.NewGuid().ToString().Replace("-", "");
        refreshToken.ExpireDate = DateTime.Now.AddDays(7);
        refreshToken.ModifiedDate = DateTime.Now;

        dataContext.SaveChanges();

        return refreshToken;
    }
}