using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using FortyFingers.DnnMassManipulate.Components;

namespace FortyFingers.DnnMassManipulate.ManipulatorModules.CreatePortals
{
    public class CreatePortalsModel
    {
        public CreatePortalsModel()
        {
        }
        public ContextHelper Context { get; set; }
        public IList<PortalController.PortalTemplateInfo> Templates { get; set; }
    }

    public class CreatePortalsPostModel
    {
        public string SampleInput { get; set; }
    }
}