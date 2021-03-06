﻿using System.Collections.Generic;

namespace FluiTec.AppFx.Networking.Mail.RazorLightExtensions.LocationExpanders
{
    /// <summary>Interface for resource expander.</summary>
    public interface IResourceExpander
    {
        /// <summary>Enumerates expand in this collection.</summary>
        /// <param name="viewName"> Name of the view. </param>
        /// <returns>An enumerator that allows foreach to be used to process expand in this collection.</returns>
        IEnumerable<string> ExpandResource(string viewName);
    }
}