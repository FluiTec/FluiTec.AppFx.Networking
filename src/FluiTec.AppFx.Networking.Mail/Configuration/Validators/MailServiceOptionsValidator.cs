using FluentValidation;

namespace FluiTec.AppFx.Networking.Mail.Configuration.Validators
{
    /// <summary>Validator for <see cref="MailServiceOptions"/></summary>
    /// <seealso cref="FluentValidation.AbstractValidator{MailServiceOptions}" />
    public class MailServiceOptionsValidator : AbstractValidator<MailServiceOptions>
    {
        /// <summary>Initializes a new instance of the <see cref="MailServiceOptionsValidator"/> class.</summary>
        public MailServiceOptionsValidator()
        {
            RuleFor(options => options.SmtpServer).NotEmpty();
            RuleFor(options => options.SmtpPort).GreaterThan(0);
            RuleFor(options => options.FromMail).NotEmpty();
            RuleFor(options => options.FromName).NotEmpty();
            RuleFor(options => options.Username).NotEmpty()
                .When(options => options.Authenticate);
            RuleFor(options => options.Password).NotEmpty()
                .When(options => options.Authenticate);
        }
    }
}