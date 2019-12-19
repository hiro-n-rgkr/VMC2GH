using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace VMC2GH
{
    public class VMC2GHInfo : GH_AssemblyInfo
    {
        public override string Name {
            get {
                return "VMC2GH";
            }
        }
        public override Bitmap Icon {
            get {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description {
            get {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id {
            get {
                return new Guid("6c0c1b88-5a3f-498f-b626-73e43a0304f8");
            }
        }

        public override string AuthorName {
            get {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact {
            get {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
