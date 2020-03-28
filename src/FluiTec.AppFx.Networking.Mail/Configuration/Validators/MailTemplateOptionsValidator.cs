using FluentValidation;

namespace FluiTec.AppFx.Networking.Mail.Configuration.Validators
{
    /// <summary>Validator for <see cref="MailTemplateOptions" /></summary>
    /// <seealso cref="FluentValidation.AbstractValidator{MailTemplateOptions}" />
    public class MailTemplateOptionsValidator : AbstractValidator<MailTemplateOptions>
    {
        /// <summary>Initializes a new instance of the <see cref="MailTemplateOptionsValidator" /> class.</summary>
        public MailTemplateOptionsValidator()
        {
            RuleFor(options => options.BaseDirectory).NotEmpty()
                .When(options => options.TemplateSource == MailTemplateOptions.MailTemplateSource.File);
            RuleFor(options => options.DefaultNamespace).NotEmpty()
                .When(options => options.TemplateSource == MailTemplateOptions.MailTemplateSource.Embedded);
            RuleFor(options => options.Extension)
                .NotEmpty()
                .Must(str => str.StartsWith("."))
                .WithMessage(
                    $"{nameof(MailTemplateOptions)}.{nameof(MailTemplateOptions.Extension)} must start with a dot.");
        }
    }
}