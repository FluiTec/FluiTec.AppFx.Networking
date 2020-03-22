using System;
using FluiTec.AppFx.Networking.Mail.Configuration;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>A templating service.</summary>
    public abstract class TemplatingService : ITemplatingService
    {
        #region Properties

        /// <summary>Gets the options.</summary>
        /// <value>The options.</value>
        public MailTemplateOptions Options { get; }

        #endregion

        #region Constructors

        /// <summary>Initializes a new instance of the <see cref="TemplatingService"/> class.</summary>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException">options</exception>
        protected TemplatingService(MailTemplateOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        #endregion

        #region Methods

        /// <summary>Parses the given model.</summary>
        /// <typeparam name="TModel">   Type of the model. </typeparam>
        /// <param name="model">    The model. </param>
        /// <returns>A string.</returns>
        public virtual string Parse<TModel>(TModel model)
        {
            return Parse(GetViewName<TModel>(), model);
        }

        /// <summary>Parses.</summary>
        /// <typeparam name="TModel">   Type of the model. </typeparam>
        /// <param name="viewName"> Name of the view. </param>
        /// <param name="model">    The model. </param>
        /// <returns>A string.</returns>
        public abstract string Parse<TModel>(string viewName, TModel model);

        /// <summary>	Gets view name. </summary>
        /// <typeparam name="TModel">	Type of the model. </typeparam>
        /// <returns>	The view name. </returns>
        protected virtual string GetViewName<TModel>()
        {
            var modelType = typeof(TModel);
            return $"{modelType.Name}{Options.Extension}";
        }

        #endregion
    }
}