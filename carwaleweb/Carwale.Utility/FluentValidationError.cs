using AEPLCore.Logging;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Utility
{
    public static class FluentValidationError
    {
		private static Logger Logger = LoggerFactory.GetLogger();
		public static async void LogValidationErrorsAsync(IList<ValidationFailure> validationErrors, string headerMessage = "")
        {
            await Task.Run(() => LogValidationErrors(validationErrors, headerMessage)).ConfigureAwait(false);
        }

        public static void LogValidationErrors(IList<ValidationFailure> validationErrors,string headerMessage="")
        {
            if (validationErrors != null)
            {
                StringBuilder sb = new StringBuilder(string.IsNullOrWhiteSpace(headerMessage) ? "ErrorMessage :" : (headerMessage+":"));
                foreach (var error in validationErrors)
                {
                    sb.Append(error.PropertyName + "==>" + error.ErrorMessage).Append("|");
                }
                Logger.LogInfo(sb.ToString());
            }
        }
    }
}
