using Writing.Payloads.Responses;
using Action = Writing.Enumerates.Action;

namespace Writing.Services; 

public interface RelationshipService {

    ResponseObject<Action> follow(int partnerId, int ownerId, string action);
    
}