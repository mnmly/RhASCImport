using Rhino;
using Rhino.FileIO;
using Rhino.PlugIns;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using Rhino.Geometry;
using Rhino.UI;

namespace MNML
{
    class ASCObject {
        public int ncols;
        public int nrows;
        public int xllcorner;
        public int yllcorner;
        public float cellsize;
        public float NODATA_value;
    }

    ///<summary>
    /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
    /// class. DO NOT create instances of this class yourself. It is the
    /// responsibility of Rhino to create an instance of this class.</para>
    /// <para>To complete plug-in information, please also see all PlugInDescription
    /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
    /// "Show All Files" to see it in the "Solution Explorer" window).</para>
    ///</summary>
   
    public class RhASCImportPlugin : FileImportPlugIn
    {
        ///<summary>Gets the only instance of the RhASCImportPlugin plug-in.</summary>
        public static RhASCImportPlugin Instance { get; private set; }
        
        public RhASCImportPlugin()
        {
            Instance = this;
        }


        protected override FileTypeList AddFileTypes(FileReadOptions options)
        {
            var result = new Rhino.PlugIns.FileTypeList();
            result.AddFileType("Esri ASCII raster format (*.asc)", "asc");            
            return result;
        }

        protected override bool ReadFile(string filename, int index, RhinoDoc doc, FileReadOptions options)
        {
            string line;

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(filename);
            var obj = new ASCObject();
            var points = new List<Point3d>();
            var y = 0;

            while ((line = file.ReadLine()) != null)
            {
                string[] components = Regex.Split(line, @"\s+");
                if (components.Length == 2)
                {
                    var val = components[1];
                    switch (components[0])
                    {
                        case "ncols": obj.ncols = int.Parse(val); break;
                        case "nrows": obj.nrows = int.Parse(val); break;
                        case "xllcorner": obj.xllcorner = int.Parse(val); break;
                        case "yllcorner": obj.yllcorner = int.Parse(val); break;
                        case "cellsize": obj.cellsize = float.Parse(val); break;
                        case "NODATA_value": obj.NODATA_value = float.Parse(val); break;
                    }
                }
                else {
                    var x = 0;
                    foreach(var item in components)
                    {
                        float z = float.Parse(item);
                        points.Add(new Point3d(obj.xllcorner + obj.cellsize * x, obj.yllcorner - obj.cellsize * y, z));
                        x += 1;
                    }
                    y++;
                }
            }
            var importAsPointCloud = true;
            var importAsMesh = true;

            // TODO: Add settings
            //Settings.TryGetBool(ASCImportOptionPage.KEY_POINTCLOUD, out importAsPointCloud);
            //Settings.TryGetBool(ASCImportOptionPage.KEY_MESH, out importAsMesh);

            if (importAsPointCloud)
            {
                var pc = new PointCloud(points);
                doc.Objects.AddPointCloud(pc);
            }

            if (importAsMesh)
            {
                int faceCount = 0;
                Mesh mesh = new Mesh();
                mesh.Vertices.AddVertices(points);

                for (int i = 0; i < obj.nrows * (obj.ncols - 1); i++)
                {
                    if (faceCount < (obj.nrows - 1))
                    {
                        var A = i;
                        var B = i + 1;
                        var C = (i + obj.nrows) + 1;
                        var D = (i + obj.nrows);
                        var face = new MeshFace(A, B, C, D);
                        mesh.Faces.AddFace(face);
                        faceCount += 1;
                    }
                    else
                    {
                        faceCount = 0;
                    }
                }
                mesh.FaceNormals.ComputeFaceNormals();
                mesh.Compact();
                doc.Objects.AddMesh(mesh);
            }
           
            doc.Views.Redraw();
            file.Close();
            return true;
        }

        /// <summary>
        /// Override this function if you want to extend the document properties sections of the options dialog. This function is called whenever the user brings up the Options dialog.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="pages"></param>
        protected override void DocumentPropertiesDialogPages(RhinoDoc doc, List<OptionsDialogPage> pages)
        {
            var settings = base.Settings;
            var page = new ASCImportOptionPage(settings);
            pages.Add(page);
            base.DocumentPropertiesDialogPages(doc, pages);
        }


        protected override void DisplayOptionsDialog(IntPtr parent, string description, string extension)
        {
            //var page = new ASCImportOptionPage(settings);

            var dialog = new Eto.Forms.Dialog();
            var optionPage = new ASCImportOptionPage(base.Settings);
            dialog.Content = optionPage.PageControl as Eto.Forms.Control;
            dialog.ShowModal();
            //base.DisplayOptionsDialog(parent, description, extension);
        }


        // You can override methods here to change the plug-in behavior on
        // loading and shut down, add options pages to the Rhino _Option command
        // and mantain plug-in wide options in a document.
        protected override void OptionsDialogPages(List<OptionsDialogPage> pages)
        {
            var settings = base.Settings;
            var page = new ASCImportOptionPage(settings);
            pages.Add(page);
            base.OptionsDialogPages(pages);
        }
    }

}
