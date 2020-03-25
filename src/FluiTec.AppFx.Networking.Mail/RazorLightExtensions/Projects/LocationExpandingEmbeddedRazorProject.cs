using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IEnumerable<ILocationExpander> _expanders;

        /// <summary>The logger.</summary>
        private readonly ILogger<LocationExpandingFileRazorProject> _logger;

        #endregion

        /// <summary>Initializes a new instance of the <see cref="LocationExpandingEmbeddedRazorProject"/> class.</summary>
        /// <param name="rootType">Type of the root.</param>
        /// <param name="expanders">The expanders.</param>
        /// <param name="logger">The logger.</param>
        public LocationExpandingEmbeddedRazorProject(Type rootType, IEnumerable<ILocationExpander> expanders, ILogger<LocationExpandingFileRazorProject> logger) : base(rootType)
        {
            _expanders = expanders ?? throw new ArgumentNullException(nameof(expanders));
            _logger = logger; // we accept null here
        }

        /// <summary>Initializes a new instance of the <see cref="LocationExpandingEmbeddedRazorProject"/> class.</summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="expanders">The expanders.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="rootNamespace">The root namespace.</param>
        public LocationExpandingEmbeddedRazorProject(Assembly assembly, IEnumerable<ILocationExpander> expanders, ILogger<LocationExpandingFileRazorProject> logger, string rootNamespace = "") : base(assembly, rootNamespace)
        {
            _expanders = expanders ?? throw new ArgumentNullException(nameof(expanders));
            _logger = logger; // we accept null here
        }

        public override Task<RazorLightProjectItem> GetItemAsync(string templateKey)
        {
            foreach (var expander in _expanders)
            foreach (var location in expander.Expand(templateKey))
            {
                _logger?.LogInformation($"Trying to find MailTemplate {templateKey} in {templateKey}.");

                    _logger?.LogInformation($"Found MailTemplate {templateKey} in {templateKey}.");
                    //return Task.FromResult(
                    //    (RazorLightProjectItem)new FileSystemRazorProjectItem(location,
                    //        new FileInfo(absolutePath)));
                    //var ns = Assembly.FullName + ".MailViews";
                    //return Task.FromResult((RazorLightProjectItem)new EmbeddedRazorProjectItem(Assembly, ns, templateKey));
            }

            var nxs = "MailViewsEmbedded";
            var root = RootNamespace;
            return Task.FromResult((RazorLightProjectItem)new EmbeddedRazorProjectItem(Assembly, nxs, templateKey));

            // let the base-class try it's best and throw...
            return base.GetItemAsync(nxs + "." + templateKey);
        }
    }
}