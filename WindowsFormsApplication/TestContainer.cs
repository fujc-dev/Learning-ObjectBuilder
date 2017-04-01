using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication
{
    public class TestContainer : Container
    {
        AmbientProperties props;

        protected override object GetService(Type service)
        {
            if (service == typeof(AmbientProperties))
            {
                if (props == null)
                {
                    props = new AmbientProperties();
                    props.Font = new Font("Arial", 16);
                }
                return props;
            }
            return base.GetService(service);
        }
    }
}
