using System;
using Eto.Forms;
using Eto.Drawing;

using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;

namespace MNML
{

    public class ASCImportOptionPageLayout: TableLayout
    {
		public static string KEY_POINTCLOUD = "ASC_IMPORT_PointCloud";
		public static string KEY_MESH = "ASC_IMPORT_MESH";

		public CheckBox pointCloudCheckBox;
		public CheckBox meshCheckBox;
		private PersistentSettings m_Settings;

		public ASCImportOptionPageLayout(PersistentSettings settings): base()
        {
			m_Settings = settings;

			Spacing = new Size(5, 5);

		    pointCloudCheckBox = new CheckBox();
			meshCheckBox = new CheckBox();
			var row1 = new TableRow(
				new TableCell(new Label { Text = "Point Cloud" }, true),
				new TableCell(pointCloudCheckBox, true));
			var row2 = new TableRow(
				new TableCell(new Label { Text = "Mesh" }, true),
				new TableCell(meshCheckBox, true));
			Rows.Add(row1);
			Rows.Add(row2);
		}

        public void OnApply()
        {
			m_Settings.SetBool(KEY_MESH, meshCheckBox.Checked.GetValueOrDefault(false));
			m_Settings.SetBool(KEY_POINTCLOUD, pointCloudCheckBox.Checked.GetValueOrDefault(false));
		}
    };

	public class ASCImportOptionPage : OptionsDialogPage
	{
		PersistentSettings m_settings;

		public ASCImportOptionPage(string title, PersistentSettings settings)
			: base(title)
		{
			m_settings = settings;
		}

        public override object PageControl
        {
			get
			{
				var layout = new ASCImportOptionPageLayout(m_settings);
				return layout;
			}
		}
      
        public override string LocalPageTitle => "ASC Import Options";
        public override bool OnApply()
		{
			try
			{
                var layout = (PageControl as ASCImportOptionPageLayout);
				layout.OnApply();
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}
	}
}
