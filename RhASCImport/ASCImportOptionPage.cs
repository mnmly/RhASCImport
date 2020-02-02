using System;
using Eto.Forms;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;

namespace MNML
{
	public class ASCImportOptionPage : OptionsDialogPage
	{
		PersistentSettings m_settings;

		public static string KEY_POINTCLOUD = "ASC_IMPORT_PointCloud";
		public static string KEY_MESH = "ASC_IMPORT_MESH";


		public ASCImportOptionPage(Rhino.PersistentSettings settings)
			: base("ASC")
		{
			m_settings = settings;

			if (!m_settings.TryGetChild(KEY_POINTCLOUD, out m_settings))
            {
				m_settings.AddChild(KEY_POINTCLOUD);
				m_settings.SetBool(KEY_POINTCLOUD, true);

			}
			if (!m_settings.TryGetChild(KEY_MESH, out m_settings))
			{
				m_settings.AddChild(KEY_MESH);
				m_settings.SetBool(KEY_MESH, true);

			}
		}

        public override object PageControl
        {
			get
			{
				var layout = new TableLayout();
				var pointCloudCheckBox = new CheckBox();
				var meshCheckBox = new CheckBox();
				var row1 = new TableRow(
                    new TableCell(new Label { Text = "Point Cloud" }, true),
                    new TableCell(pointCloudCheckBox, true));
				var row2 = new TableRow(
                    new TableCell(new Label { Text = "Mesh" }, true),
		            new TableCell(meshCheckBox, true));
				layout.Rows.Add(row1);
				layout.Rows.Add(row2);

				return layout;
			}
		}
      
        public override string LocalPageTitle => "ASC Import Options";
        public override bool OnApply()
		{
			try
			{
                var layout = (PageControl as TableLayout);
				var checkbox1 = (layout.Rows[0].Cells[1] as TableCell).Control
                     as CheckBox;
				var checkbox2 = (layout.Rows[1].Cells[1] as TableCell).Control
					 as CheckBox;
				m_settings.SetBool(KEY_MESH, checkbox1.Checked.GetValueOrDefault(false));
				m_settings.SetBool(KEY_POINTCLOUD, checkbox2.Checked.GetValueOrDefault(false));

				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}
	}
}
