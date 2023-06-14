using Writing.Payloads;

namespace Writing.Services; 

public interface SendEmailService {

    Task<string> sendEmail(EmailTo emailTo);
}