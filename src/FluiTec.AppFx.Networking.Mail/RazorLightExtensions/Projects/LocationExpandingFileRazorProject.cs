using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders;
using Microsoft.Extensions.Logging;
using RazorLight.Razor;

namespace FluiTec.AppFx.Networking.Mail.RazorLightExtensions.Projects
{
    /// <summary>A location expanding razor project using simple files.</summary>
    public sealed class LocationExpandingFileRazorProject : FileSystemRazorProject
    {
        #region Fields

        /// <summary>The expanders.</summary>
        private readonly IEnumerable<IFileLocationExpander> _expanders;

        /// <summary>   The logger. </summary>
        private readonly ILogger<LocationExpandingFileRazorProject> _logger;

        #endregion

        #region Constructors

        /// <summary>
        ///   <para>Constructor.</para>
        /// </summary>
        /// <param name="expanders">The expanders.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="baseDirectory"></param>
        /// <param name="extension"></param>
        /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are
        ///                                         null.</exception>
        public LocationExpandingFileRazorProject(IEnumerable<IFileLocationExpander> expanders, ILogger<LocationExpandingFileRazorProject> logger, string baseDirectory, string extension) 
            : base(baseDirectory)
        {
            Extension = extension ?? throw new ArgumentNullException(nameof(extension));
            _expanders = expanders ?? throw new ArgumentNullException(nameof(expanders));
            _logger = logger; // we accept null here
        }

        #endregion

        #region FileSystemRazorProject

        /// <summary>Gets item asynchronous.</summary>
        /// <param name="templateKey">  The template key. </param>
        /// <returns>The asynchronous result that yields the item asynchronous.</returns>
        public override Task<RazorLightProjectItem> GetItemAsync(string templateKey)
        {
            foreach (var expander in _expanders)
                foreach (var location in expander.Expand(templateKey))
                {
                    var absolutePath = NormalizeKey(location);
                    _logger?.LogInformation($"Trying to find MailTemplate {templateKey} in {absolutePath}.");

                    if (!File.Exists(absolutePath)) continue;
                    _logger?.LogInformation($"Found MailTemplate {templateKey} in {absolutePath}.");
                    return Task.FromResult(
                        (RazorLightProjectItem) new FileSystemRazorProjectItem(location,
                            new FileInfo(absolutePath)));
                }

            // let the base-class try it's best and throw...
            return base.GetItemAsync(templateKey);
        }

        #endregion
    }
}
