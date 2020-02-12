using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace VMC2GH {
    public class VMC2GHComponent:GH_Component {

        Point3d Neck, Head, L_Eye, R_Eye, Jaw, Hips, Spine, Chest, Upper_Chest, L_Sholder, R_Sholder, L_Upper_Arm, R_Upper_Arm,
            L_Lower_Arm, R_Lower_Arm, L_Hand, R_Hand, L_Upper_Leg, R_Upper_Leg, L_Lower_Leg, R_Lower_Leg, L_Foot, R_Foot,
            L_Mid_Prox, R_Mid_Prox, L_Mid_Interm, R_Mid_Interm, L_Lit_Distal, R_Lit_Distal, LastBone,
            L_Toe, R_Toe, L_Mid_Distal, R_Mid_Distal, L_Lit_Prox, R_Lit_Prox, L_Lit_Interm, R_Lit_Interm,
            BonePoint;
        double x, y, z;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public VMC2GHComponent()
          : base("VMC2GH", "VMC2GH",
              "Description",
              "VMC2GH", "VMC2GH") {
        }

        public override void ClearData() {
            base.ClearData();
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager) {
            pManager.AddTextParameter("VMCDataList", "VMCDataList", "Input VMC data", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager) {
            pManager.AddPointParameter("Neck", "Neck", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("Head", "Head", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftEye", "L_Eye", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightEye", "R_Eye", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("Jaw", "Jaw", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("Hips", "Hips", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("Spine", "Spine", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("Chest", "Chest", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("UpperChest", "Up_Chest", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftSholder", "L_Sholder", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightSholder", "R_Sholder", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftUpperArm", "L_Up_Arm", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightUpperArm", "R_Up_Arm", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftLowerArm", "L_Low_Arm", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightLowerArm", "R_Low_Arm", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftHand", "L_Hand", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightHand", "R_Hand", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftUpperLeg", "L_Up_Leg", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightUpperLeg", "R_Up_Leg", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftLowerLeg", "L_Low_Leg", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightLowerLeg", "R_Low_Leg", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftFoot", "L_Foot", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightFoot", "R_Foot", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftToe", "L_Toe", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightToe", "R_Toe", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftMiddleProximal", "L_Mid_Prox", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightMiddleProximal", "R_Mid_Prox", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftMiddleIntermediate", "L_Mid_Interm", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightMiddleIntermediate", "R_Mid_Interm", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftMiddleDistal", "L_Mid_Dist", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightMiddleDistal", "R_Mid_Dist", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftLittleProximal", "L_Lit_Prox", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightLittleProximal", "R_Lit_Prox", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftLittleIntermediate", "L_Lit_Interm", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightLittleIntermediate", "R_Lit_Interm", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LeftLittleDistal", "L_Lit_Dist", "output point3d", GH_ParamAccess.item);
            pManager.AddPointParameter("RightLittleDistal", "R_Lit_Dist", "output point3d", GH_ParamAccess.item);
            //
            pManager.AddPointParameter("LastBone", "LastBone", "output point3d", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess DA) {

            List<string> VMCData = new List<string>();

            if (!DA.GetDataList(0, VMCData))
                return;
            if (VMCData[0] != "/VMC/Ext/Bone/Pos")
                return;
            // Y-up を Z-up に修正 
            x = 1000 * double.Parse(VMCData[2]);
            y = 1000 * double.Parse(VMCData[4]);
            z = 1000 * double.Parse(VMCData[3]);
            BonePoint = new Point3d(x, y, z);
            
            switch (VMCData[1]) {
                case "Neck": Neck = BonePoint; break;
                case "Head": Head = BonePoint; break;
                // Eye
                case "LeftEye": L_Eye = BonePoint; break;
                case "RightEye": R_Eye = BonePoint; break;
                //
                case "Jaw": Jaw = BonePoint; break;
                case "Hips": Hips = BonePoint; break;
                case "Spine": Spine = BonePoint; break;
                case "Chest": Chest = BonePoint; break;
                case "UpperChest": Upper_Chest = BonePoint; break;
                // Sholder
                case "LeftSholder": L_Sholder = BonePoint; break;
                case "RightSholder": R_Sholder = BonePoint; break;
                // UpperArm
                case "LeftUpperArm": L_Upper_Arm = BonePoint; break;
                case "RightUpperArm": R_Upper_Arm = BonePoint; break;
                // LowerArm
                case "LeftLowerArm": L_Lower_Arm = BonePoint; break;
                case "RightLowerArm": R_Lower_Arm = BonePoint; break;
                // Hand
                case "LeftHand": L_Hand = BonePoint; break;
                case "RightHand": R_Hand = BonePoint; break;
                // UpperLeg
                case "LeftUpperLeg": L_Upper_Leg = BonePoint; break;
                case "RightUpperLeg": R_Upper_Leg = BonePoint; break;
                // LowerLeg
                case "LeftLowerLeg": L_Lower_Leg = BonePoint; break;
                case "RightLowerLeg": R_Lower_Leg = BonePoint; break;
                // Foot
                case "LeftFoot": L_Foot = BonePoint; break;
                case "RightFoot": R_Foot = BonePoint; break;
                // Toe
                case "LeftToe": L_Toe = BonePoint; break;
                case "RightToe": R_Toe = BonePoint; break;
                // Middle Proximal
                case "LeftMiddleProximal": L_Mid_Prox = BonePoint; break;
                case "RightMiddleProximal": R_Mid_Prox = BonePoint; break;
                // Middle Intermediate
                case "LeftMiddleIntermediate": L_Mid_Interm = BonePoint; break;
                case "RightMiddleIntermediate": R_Mid_Interm = BonePoint; break;
                // Middle Distal
                case "LeftMiddleDistal": L_Mid_Distal = BonePoint; break;
                case "RightMiddleDistal": R_Mid_Distal = BonePoint; break;
                // Littel Proximal
                case "LeftLittelProximal": L_Lit_Prox = BonePoint; break;
                case "RightLittelProximal": R_Lit_Prox = BonePoint; break;
                // Littel Intermediate
                case "LeftLittelIntermediate": L_Lit_Interm = BonePoint; break;
                case "RightLittelIntermediate": R_Lit_Interm = BonePoint; break;
                // Littel Distal
                case "LeftLittleDistal": L_Lit_Distal = BonePoint; break;
                case "RightLittleDistal": R_Lit_Distal = BonePoint; break;
                // LastBone
                case "LastBone": LastBone = BonePoint; break;
                default: break;
            }

            DA.SetData("Neck", Neck);
            DA.SetData("Head", Head);
            //
            DA.SetData("LeftEye", L_Eye);
            DA.SetData("RightEye", R_Eye);
            //
            DA.SetData("Jaw", Jaw);
            DA.SetData("Hips", Hips);
            DA.SetData("Spine", Spine);
            //
            DA.SetData("Chest", Chest);
            DA.SetData("UpperChest", Upper_Chest);
            //
            DA.SetData("LeftSholder", L_Sholder);
            DA.SetData("RightSholder", R_Sholder);
            //
            DA.SetData("LeftUpperArm", L_Upper_Arm);
            DA.SetData("RightUpperArm", R_Upper_Arm);
            //
            DA.SetData("LeftLowerArm", L_Lower_Arm);
            DA.SetData("RightLowerArm", R_Lower_Arm);
            //
            DA.SetData("LeftHand", L_Hand);
            DA.SetData("RightHand", R_Hand);
            //
            DA.SetData("LeftUpperLeg", L_Upper_Leg);
            DA.SetData("RightUpperLeg", R_Upper_Leg);
            //
            DA.SetData("LeftLowerLeg", L_Lower_Leg);
            DA.SetData("RightLowerLeg", R_Lower_Leg);
            //
            DA.SetData("LeftFoot", L_Foot);
            DA.SetData("RightFoot", R_Foot);
            //
            DA.SetData("LeftToe", L_Toe);
            DA.SetData("RightToe", R_Toe);
            //
            DA.SetData("LeftMiddleProximal", L_Mid_Prox);
            DA.SetData("RightMiddleProximal", R_Mid_Prox);
            //
            DA.SetData("LeftMiddleIntermediate", L_Mid_Interm);
            DA.SetData("RightMiddleIntermediate", R_Mid_Interm);
            //
            DA.SetData("LeftMiddleDistal", L_Mid_Distal);
            DA.SetData("RightMiddleDistal", R_Mid_Distal);
            //
            DA.SetData("LeftLittleProximal", L_Lit_Prox);
            DA.SetData("RightLittleProximal", R_Lit_Prox);
            //
            DA.SetData("LeftLittleIntermediate", L_Lit_Interm);
            DA.SetData("RightLittleIntermediate", R_Lit_Interm);
            //
            DA.SetData("LeftLittleDistal", L_Lit_Distal);
            DA.SetData("RightLittleDistal", R_Lit_Distal);
            //
            DA.SetData("LastBone", LastBone);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon {
            get {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid {
            get { return new Guid("32fa5426-d910-4dc5-b1e4-b44a327f4865"); }
        }
    }
}
