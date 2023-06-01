using Microsoft.EntityFrameworkCore;
using Writing.Entities;
using Writing.Payloads.Responses;
using Writing.Repositories;
using Action = Writing.Enumerates.Action;

namespace Writing.Services.Impl; 

public class RelationshipServiceImpl : RelationshipService {

    private readonly DataContext dataContext;
    private readonly ResponseObject<Action> responseObject;

    public RelationshipServiceImpl(DataContext dataContext, ResponseObject<Action> responseObject) {
        this.dataContext = dataContext;
        this.responseObject = responseObject;
    }
    
    public ResponseObject<Action> follow(int partnerId, int ownerId, string action) {

        if (partnerId.Equals(ownerId)) {
            return responseObject.responseError(StatusCodes.Status400BadRequest, 
                "Can't follow yourself", Action.NOTCONSTRAINT);
        }
        
        Relationship relationshipExisted = dataContext.Relationships
            .FromSql($"SELECT * FROM Relationships_tbl WHERE FollowerId = {ownerId} AND FollowingId = {partnerId}")
            .FirstOrDefault();

        User partner = dataContext.Users.Where(user => user.Id.Equals(partnerId)).FirstOrDefault();
        if (partner == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound, 
                "User not found with id: " + partnerId, Action.NOTCONSTRAINT);
        }

        User owner = dataContext.Users.Where(user => user.Id.Equals(ownerId)).FirstOrDefault();

        Relationship relationship = new Relationship();
        
        if (action.Equals(Action.FOLLOW.ToString()) && relationshipExisted == null) {
            relationship.Follower = owner;
            relationship.Following = partner;
            
            dataContext.Relationships.Add(relationship);
            dataContext.SaveChanges();
            return responseObject.responseSuccess("Followed", Action.FOLLOW);

        } else if (action.Equals(Action.UNFOLLOW.ToString()) && relationshipExisted != null) {
            relationship = dataContext.Relationships
                .FromSql($"SELECT * FROM Relationships_tbl WHERE FollowerId = {owner.Id} AND FollowingId = {partner.Id}")
                .FirstOrDefault();
            
            dataContext.Relationships.Remove(relationship);
            dataContext.SaveChanges();
            return responseObject.responseSuccess("Unfollowed", Action.UNFOLLOW);
            
        }
        
        return responseObject.responseError(StatusCodes.Status400BadRequest, 
            "Action is invalid", Action.NOTCONSTRAINT);
    }
}