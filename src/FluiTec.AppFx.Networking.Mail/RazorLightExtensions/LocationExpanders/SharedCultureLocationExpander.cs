﻿using System.Collections.Generic;
using System.Globalization;

namespace FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders
{
    /// <summary>A shared culture location expander.</summary>
    public class SharedCultureLocationExpander : IFileLocationExpander, IResourceExpander
    {
        /// <summary>Enumerates expand in this collection.</summary>
        /// <param name="viewName"> Name of the view. </param>
        /// <returns>An enumerator that allows foreach to be used to process expand in this collection.</returns>
        public IEnumerable<string> Expand(string viewName)
        {
            return new[] {$"Shared/{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}/{viewName}"};
        }

        /// <summary>Enumerates expand in this collection.</summary>
        /// <param name="viewName"> Name of the view. </param>
        /// <returns>An enumerator that allows foreach to be used to process expand in this collection.</returns>
        public IEnumerable<string> ExpandResource(string viewName)
        {
            return new[] {$"Shared.{CultureInfo.CurrentUICulture.TwoLetterISOLanguageName}.{viewName}"};
        }
    }
}