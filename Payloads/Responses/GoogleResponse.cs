﻿namespace Writing.Payloads.Responses;

public class GoogleResponse {
    public string sub {  get; set; }
    public string name { get; set; }
    public string given_name { get; set; }
    public string family_name { get; set; }
    public string picture { get; set; }
    public string email { get; set; }
    public bool email_verified { get; set; }
    public string locale { get; set; }
}
