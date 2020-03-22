namespace FluiTec.AppFx.Networking.Mail.Services
{
    /// <summary>Interface for templating service.</summary>
    public interface ITemplatingService
    {
        /// <summary>	Parses the given model. </summary>
        /// <typeparam name="TModel">	Type of the model. </typeparam>
        /// <param name="model">	The model. </param>
        /// <returns>	A string. </returns>
        string Parse<TModel>(TModel model) where TModel : IMailModel;

        /// <summary>	Parses. </summary>
        /// <typeparam name="TModel">	Type of the model. </typeparam>
        /// <param name="viewName">	Name of the view. </param>
        /// <param name="model">   	The model. </param>
        /// <returns>	A string. </returns>
        string Parse<TModel>(string viewName, TModel model) where TModel : IMailModel;
    }
}