using System.Net;
using System.Net.Mail;
using MailKit;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using Writing.Configurations;
using Writing.Payloads;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Writing.Services.Impl; 

public class SendEmailServiceImpl : SendEmailService {
    
    public async Task<string> sendEmail(EmailTo emailTo) {
        var email = new MimeMessage();
        email.Sender = MailboxAddress.Parse("cuong.test.development@gmail.com");
        email.To.Add(MailboxAddress.Parse(emailTo.To));
        email.Subject = emailTo.Subject;

        var builder = new BodyBuilder();
        builder.HtmlBody = emailTo.Content;

        email.Body = builder.ToMessageBody();
        
        string passwordEncoded = "YWttYW5veGd6Z2htaGZycA=="; //google account password encoded
        string passwordDecoded = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(passwordEncoded));  //decode

        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("cuong.test.development@gmail.com", passwordDecoded);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);

        return "Đăng ký tài khoản thành công. Kiểm tra email để kích hoạt tài khoản";
    }
}