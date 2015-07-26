using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GnomeServer
{
    public static class TemplateHelper
    {
        /// <summary>
        /// Gets the full path of the directory that is .
        /// </summary>
        public static String GetWebRoot()
        {
            var userDataPath = Configuration.GetUserDataPath();
            var serverWebRootPath = Path.Combine(userDataPath, "wwwroot");
            var indexPath = Path.Combine(serverWebRootPath, "index.html");
            if (File.Exists(indexPath))
            {
                return indexPath;
            }
            return null;
        }

        /// <summary>
        /// Gets the full content of a template.
        /// </summary>
        public static String GetTemplate(String template)
        {
            // Templates seem like something we shouldn't handle internally.
            // Perhaps we should force request handlers to implement their own templating if they so desire, and maintain a more "API" approach within the core.

            String webRootPath = GetWebRoot();
            String specifiedTemplatePath = String.Format("{0}{1}{2}.html", webRootPath, Path.DirectorySeparatorChar, template);

            if (File.Exists(specifiedTemplatePath))
            {
                String templateContents = File.ReadAllText(specifiedTemplatePath);
                return templateContents;
            }

            // All templates must at least have a #PAGEBODY# token.
            // If we can't find the specified template, just return a string that contains only that.
            return "#PAGEBODY#";
        }

        /// <summary>
        /// Retrieves the template with the specified name, and returns the contents of the template after replacing instances of the dictionary keys from <paramref name="tokenReplacements"/> with their coorresponding values.
        /// </summary>
        /// <param name="template">The name of the template to populate.</param>
        /// <param name="tokenReplacements">A dictionary containing key/value pairs for replacement.</param>
        /// <remarks>
        /// The value of <paramref name="template"/> should not include the file extension.
        /// </remarks>
        public static String PopulateTemplate(String template, Dictionary<String, String> tokenReplacements)
        {
            try
            {
                String templateContents = GetTemplate(template);
                foreach (var tokenReplacement in tokenReplacements)
                {
                    templateContents = templateContents.Replace(tokenReplacement.Key, tokenReplacement.Value);
                }
                return templateContents;
            }
            catch (Exception ex)
            {
                IntegratedWebServer.LogMessage(ex.ToString());
                return tokenReplacements["#PAGEBODY#"];
            }
        }

        /// <summary>
        /// Gets a dictionary that contains standard replacement tokens using the specified values.
        /// </summary>
        public static Dictionary<String, String> GetTokenReplacements(String title, List<IRequestHandler> handlers, String body)
        {
            var orderedHandlers = handlers.OrderBy(obj => obj.Priority).ThenBy(obj => obj.Name);
            var handlerLinks = orderedHandlers.Select(obj => String.Format("<li><a href='{0}'>{0}</a></li>", obj.Name)).ToArray();
            String nav = String.Join(Environment.NewLine, handlerLinks);

            return new Dictionary<String, String>
            {
                { "#PAGETITLE#", title },
                { "#NAV#", nav},
                { "#PAGEBODY#", body},
            };
        }
    }
}
