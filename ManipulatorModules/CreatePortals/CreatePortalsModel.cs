using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
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
        public IList<ListItem> Templates { get; set; }
    }

    public class CreatePortalsPostModel
    {
        public string Template { get; set; }
        public int NumberOfPortals { get; set; }
        public bool AsChildPortals { get; set; }
        public string PortalNamePrefix { get; set; }
        public string HttpAlias { get; set; }

    }
}