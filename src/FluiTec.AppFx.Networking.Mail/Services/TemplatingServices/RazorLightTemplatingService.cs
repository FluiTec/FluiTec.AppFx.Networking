using System;
using FluiTec.AppFx.Networking.Mail.Configuration;
using RazorLight;

namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>	A razor templating mail service. </summary>
    public class RazorLightTemplatingService : TemplatingService
    {
        /// <summary>   The engine. </summary>
        protected readonly IRazorLightEngine Engine;

        /// <summary>Specialized constructor for use only by derived class.</summary>
        /// <param name="engine">The engine.</param>
        /// <param name="options">The options to use.</param>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        public RazorLightTemplatingService(IRazorLightEngine engine, MailTemplateOptions options) : base(options)
        {
            Engine = engine ?? throw new ArgumentNullException(nameof(engine));
        }

        /// <summary>Specialized constructor for use only by derived class.</summary>
        /// <param name="options">The options to use.</param>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
        public RazorLightTemplatingService(MailTemplateOptions options) : base(options)
        {
            if (string.IsNullOrWhiteSpace(options.BaseDirectory))
                throw new ArgumentNullException(nameof(options.BaseDirectory));

            Engine = new RazorLightEngineBuilder().UseFileSystemProject(options.BaseDirectory, options.Extension).Build();
        }

        /// <summary>	Parses. </summary>
        /// <typeparam name="TModel">	Type of the model. </typeparam>
        /// <param name="viewName">	Name of the view. </param>
        /// <param name="model">   	The model. </param>
        /// <returns>	A string. </returns>
        public override string Parse<TModel>(string viewName, TModel model)
        {
            return Engine.CompileRenderAsync(viewName, model).Result;
        }
    }
}