using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using FluiTec.AppFx.Networking.Mail.Configuration.Validators;
using FluiTec.AppFx.Options.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>A templating service.</summary>
    public abstract class TemplatingService : ITemplatingService
    {
        #region Fields

        private readonly MailTemplateOptions _options;

        #endregion

        #region Properties

        /// <summary>Gets the optionsMonitor.</summary>
        /// <value>The optionsMonitor.</value>
        protected IOptionsMonitor<MailTemplateOptions> OptionsMonitor { get; }

        /// <summary>Gets the options.</summary>
        /// <value>The options.</value>
        public MailTemplateOptions Options => OptionsMonitor != null ? OptionsMonitor.CurrentValue : _options;

        /// <summary>Gets the logger.</summary>
        /// <value>The logger.</value>
        protected ILogger<TemplatingService> Logger { get; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TemplatingService"/> class.</summary>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">options</exception>
        protected TemplatingService(MailTemplateOptions options, ILogger<TemplatingService> logger)
        {
            Logger = logger; // we accept null here
            _options = options ?? throw new ArgumentNullException(nameof(options));
            ValidateOptions(options);
        }

        /// <summary>Initializes a new instance of the <see cref="TemplatingService"/> class.</summary>
        /// <param name="optionsMonitor">The options monitor.</param>
        /// <param name="logger">The logger.</param>
        protected TemplatingService(IOptionsMonitor<MailTemplateOptions> optionsMonitor, ILogger<TemplatingService> logger) : this(optionsMonitor.CurrentValue, logger)
        {
            OptionsMonitor = optionsMonitor;
            OptionsMonitor.OnChange(ValidateOptions);
        }

        #endregion

        #region Methods

        /// <summary>Parses the given model.</summary>
        /// <typeparam name="TModel">   Type of the model. </typeparam>
        /// <param name="model">    The model. </param>
        /// <returns>A string.</returns>
        public virtual string Parse<TModel>(TModel model) where TModel : IMailModel
        {
            Logger?.LogDebug($"Parsing model '{typeof(TModel).Name}'.");
            return Parse(GetViewName<TModel>(), model);
        }

        /// <summary>Parses.</summary>
        /// <typeparam name="TModel">   Type of the model. </typeparam>
        /// <param name="viewName"> Name of the view. </param>
        /// <param name="model">    The model. </param>
        /// <returns>A string.</returns>
        public abstract string Parse<TModel>(string viewName, TModel model) where TModel : IMailModel;

        /// <summary>	Gets view name. </summary>
        /// <typeparam name="TModel">	Type of the model. </typeparam>
        /// <returns>	The view name. </returns>
        protected virtual string GetViewName<TModel>()
        {
            var modelType = typeof(TModel);
            var result = $"{modelType.Name}{Options.Extension}";
            Logger?.LogDebug($"ViewName of '{typeof(TModel).Name}' is '{result}'.");
            return result;
        }

        private static void ValidateOptions(MailTemplateOptions options)
        {
            var validationResult = new MailTemplateOptionsValidator().Validate(options);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult, typeof(MailServiceOptions), "Invalid settings.");
        }

        #endregion
    }
}