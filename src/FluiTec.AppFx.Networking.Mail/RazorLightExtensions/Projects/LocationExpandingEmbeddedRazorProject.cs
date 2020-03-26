using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.Extensions.Logging;
using RazorLight.Razor;

namespace FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects
{
    /// <summary>A location expanding razor project using embedded resources.</summary>
    /// <seealso cref="RazorLight.Razor.EmbeddedRazorProject" />
    public class LocationExpandingEmbeddedRazorProject : EmbeddedRazorProject
    {
        #region Fields

        /// <summary>The expanders.</summary>
        private readonly IEnumerable<IResourceExpander> _expanders;

        /// <summary>The logger.</summary>
        private readonly ILogger<LocationExpandingFileRazorProject> _logger;

        #endregion

        /// <summary>Initializes a new instance of the <see cref="LocationExpandingEmbeddedRazorProject"/> class.</summary>
        /// <param name="rootType">Type of the root.</param>
        /// <param name="expanders">The expanders.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="rootNamespace">The rootNameSpace.</param>
        public LocationExpandingEmbeddedRazorProject(Type rootType, IEnumerable<IResourceExpander> expanders, ILogger<LocationExpandingFileRazorProject> logger, string rootNamespace) : base(rootType)
        {
            if (string.IsNullOrWhiteSpace(rootNamespace))
                throw new ArgumentException($"{nameof(rootNamespace)} must not be null or empty.");
            RootNamespace = rootNamespace;
            _expanders = expanders ?? throw new ArgumentNullException(nameof(expanders));
            _logger = logger; // we accept null here
        }

        /// <summary>Initializes a new instance of the <see cref="LocationExpandingEmbeddedRazorProject"/> class.</summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="expanders">The expanders.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        public LocationExpandingEmbeddedRazorProject(Assembly assembly, IEnumerable<IResourceExpander> expanders, ILogger<LocationExpandingFileRazorProject> logger, string rootNamespace) : base(assembly, rootNamespace)
        {
            if (string.IsNullOrWhiteSpace(rootNamespace))
                throw new ArgumentException($"{nameof(rootNamespace)} must not be null or empty.");
            RootNamespace = rootNamespace;
            _expanders = expanders ?? throw new ArgumentNullException(nameof(expanders));
            _logger = logger; // we accept null here
        }

        public override Task<RazorLightProjectItem> GetItemAsync(string templateKey)
        {
            foreach (var expander in _expanders)
            foreach (var location in expander.ExpandResource(templateKey))
            {
                _logger?.LogInformation($"Trying to find MailTemplate {templateKey} in {location}.");
                var item = new EmbeddedRazorProjectItem(Assembly, RootNamespace, location);

                if (!item.Exists) continue;
                _logger?.LogInformation($"Found MailTemplate {templateKey} in {location}.");
                return Task.FromResult((RazorLightProjectItem) item);
            }

            // let the base-class try it's best and throw...
            return base.GetItemAsync(templateKey);
        }
    }
}