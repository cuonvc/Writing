using Writing.Entities;
using Writing.Enumerates;

namespace Writing.Payloads.Responses
{
    public class ResponseData<T>
    {
        public ActionStatus Status { get; set; }
        public T Data { get; set; }
        public string message { get; set; }
    }
}
