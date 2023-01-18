using System.Net;
using System.Net.Http;
using System.Web.Http;
using DotNetNuke.Web.Api;
using FortyFingers.DnnMassManipulate.ManipulatorModules.CreatePortals;

// Leave the ApiController in this namespace to avoid the need for a custom routemapper
namespace FortyFingers.MmCreatePortals.ManipulatorModules.CreatePortals
{
    [DnnModuleAuthorize]
    [SupportedModules("40Fingers.DnnMassManipulate")] // can be comma separated list of supported module
    public class CreatePortalsController : DnnApiController
    {
        [HttpPost]
        public HttpResponseMessage Do(CreatePortalsPostModel model)
        {
            string ret = $"Your Input: \"{model.SampleInput}\"";
            return Request.CreateResponse(HttpStatusCode.OK, ret);
        }

    }
}