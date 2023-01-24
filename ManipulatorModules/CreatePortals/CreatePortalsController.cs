using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Api;
using FortyFingers.DnnMassManipulate.Components._40FingersLib;
using FortyFingers.DnnMassManipulate.ManipulatorModules.CreatePortals;
using Newtonsoft.Json;

// Leave the ApiController in this namespace to avoid the need for a custom routemapper
namespace FortyFingers.DnnMassManipulate.Services
{
    [DnnModuleAuthorize]
    [SupportedModules("40Fingers.DnnMassManipulate")] // can be comma separated list of supported module
    public class CreatePortalsController : DnnApiController
    {
        [HttpPost]
        public HttpResponseMessage Do(CreatePortalsPostModel model)
        {
            string ret = $"Your Input: \"{JsonConvert.SerializeObject(model)}<br/>";

            var templates = PortalController.Instance.GetAvailablePortalTemplates();
            if (templates.All(t => t.Name != model.Template))
            {
                return Request.CreateResponse(HttpStatusCode.OK, $"Template {model.Template} not found.");
            }

            var allPortals = PortalController.Instance.GetPortals().ToList<PortalInfo>();
            var tpl = templates.First(t => t.Name == model.Template);
            var serverPath = this.GetAbsoluteServerPath();
            for (int i = 1; i <= model.NumberOfPortals; i++)
            {
                var name = $"{model.PortalNamePrefix}{i}";
                if (allPortals.Exists(p => p.PortalName == name))
                {
                    ret += $"Skipped (already exists): {name}<br/>";
                    continue;
                }
                var alias = Regex.Replace(model.HttpAlias, "\\[name\\]", name, RegexOptions.IgnoreCase);
                var newId = PortalController.Instance.CreatePortal(
                    name,
                    UserInfo.UserID,
                    "",
                    "",
                    tpl,
                    "",
                    alias,
                    serverPath,
                    Path.Combine(serverPath, name),
                    model.AsChildPortals);

                if (newId != -1)
                {
                    // Create a Portal Settings object for the new Portal
                    var objPortal = PortalController.Instance.GetPortal(newId);
                    var newSettings = new PortalSettings
                    {
                        PortalAlias = new PortalAliasInfo { HTTPAlias = alias },
                        PortalId = newId,
                        DefaultLanguage = objPortal.DefaultLanguage,
                    };

                    // mark default language as published if content localization is enabled
                    var contentLocalizationEnabled = PortalController.GetPortalSettingAsBoolean("ContentLocalizationEnabled", newId, false);
                    if (contentLocalizationEnabled)
                    {
                        var lc = new LocaleController();
                        var locales = lc.GetLocales(newId);
                        foreach (var locale in locales.Keys)
                        {
                            lc.PublishLanguage(newId, locale, true);
                        }
                    }
                }

                if (newId > 0)
                {
                    ret += $"Created (id={newId}): <a href=\"//{alias}\">{name}</a><br/>";
                }


            }

            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }
        private string GetAbsoluteServerPath()
        {
            var httpContext = this.Request.Properties["MS_HttpContext"] as HttpContextWrapper;
            var strServerPath = string.Empty;
            if (httpContext != null)
                strServerPath = httpContext.Request.MapPath(httpContext.Request.ApplicationPath);
            if (!strServerPath.EndsWith("\\"))
            {
                strServerPath += "\\";
            }
            return strServerPath;
        }

    }
}