using System.ComponentModel.Design;
using System.Globalization;
using Task = System.Threading.Tasks.Task;
using System.Data;
using DesktopCodeGenerator.UI;

namespace DesktopCodeGenerator
{
    internal class DesktopCodeGenerator
  
    {
      
        private SourceFilesProviderController _ctrl;

       
        public DesktopCodeGenerator(string resultPath)
        {
           
            _ctrl = new SourceFilesProviderController();
            _resultPath = resultPath;
        }

        private string _resultPath { get; }

        public void AddFiles()
        {
            
            try
            {
                               

                _ctrl.LoadLinks();
                using (UI.FrmSelect frm = new UI.FrmSelect())
                {

                    frm.grid.AutoGenerateColumns = true;
                    var dt = new System.Data.DataTable();
                    System.Data.DataView dv;
                    dt.Columns.Add("Checked", typeof(bool));
                    dt.Columns.Add("ID", typeof(string));
                    dt.Columns.Add("TotalFiles", typeof(int));
                    //
                    foreach (var link in _ctrl.Links)
                    {
                        dt.Rows.Add(false, link.VisualName, link.Folders.Sum(x => x.SourceFiles.Count()));
                    }

                    dv = dt.DefaultView;
                    frm.grid.DataSource = dt.DefaultView;
                    for (int i = 0; i <= frm.grid.Columns.Count - 1; i++)
                    {
                        frm.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }


                    frm.txtFilter.TextChanged += (object sender, EventArgs e) =>
                    {
                        dv.RowFilter = $"ID LIKE '%{frm.txtFilter.Text}%'";
                    };

                    if (frm.ShowDialog() != DialogResult.OK)
                        return;

                    dv.RowFilter = "Checked = 'true'";
                    if (dv.Count == 0)
                        return;

                    var checkedItems = (from DataRowView oRow in dv
                                        select _ctrl.Links.First(x => x.VisualName == (string)oRow["ID"])).ToArray();

                    foreach (var link in checkedItems)
                    {

                        Dictionary<string, string> replacements = new Dictionary<string, string>();
                        if (link.Replacements != null)
                        {
                            foreach (var item in link.Replacements)
                            {
                                using (FrmInputValue frmInput = new FrmInputValue())
                                {

                                    frmInput.lblRequestMessage.Text = string.IsNullOrWhiteSpace(item.RequestMessage) ? item.ReplaceField : item.RequestMessage;

                                    if (frmInput.ShowDialog() != DialogResult.OK)
                                    {
                                        System.Windows.Forms.MessageBox.Show("Proceso cancelado por el usuario");
                                        return;
                                    }

                                    replacements.Add(item.ReplaceField, frmInput.txtValue.Text);

                                }

                            }
                        }

                        foreach (var folder in link.Folders)
                        {

                            string[] files = _ctrl.BuildSourceFiles(folder);
                            string[] target = folder.TargetPath.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            
                            foreach (string file in files)
                            {

                                string fileContent = System.IO.File.ReadAllText(file);
                                string resultFileName = System.IO.Path.GetFileName(file);
                                                                                                  
                                foreach (var item in replacements)
                                {
                                    resultFileName = resultFileName.Replace(item.Key, item.Value);
                                    fileContent = fileContent.Replace(item.Key, item.Value);
                                }                              

                                string resultFullFilePath = System.IO.Path.Combine(_resultPath, resultFileName);
                                if (System.IO.File.Exists(resultFullFilePath))
                                    System.IO.File.Delete(resultFullFilePath);

                                System.IO.File.WriteAllText(resultFullFilePath, fileContent);                                 

                            }

                        }

                    }
                    // TODO Copy Files
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
