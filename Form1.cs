using System;
using System.IO.Ports;
using System.Windows.Forms;
using System.IO;

namespace ZebraPrinterGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadComPorts();

            // Set default COM port to COM4 if available
            for (int i = 0; i < cboComPort.Items.Count; i++)
            {
                if (cboComPort.Items[i]?.ToString() == "COM4")
                {
                    cboComPort.SelectedIndex = i;
                    break;
                }
            }

            txtQty.Text = "1";

            // Load built-in models first, then any user-added models from file
            comboModels.Items.Add("5INCH");
            comboModels.Items.Add("7INCH");
            LoadUserModelsFromFile();

            comboModels.SelectedIndex = 0;

            // Read the last saved values for the initially selected model
            if (comboModels.SelectedItem != null)
            {
                string selectedModel = comboModels.SelectedItem.ToString();
                ReadFromFileByModel(selectedModel);
            }

            UpdateData();
        }

        // ─────────────────────────────────────────────
        //  COM / Serial helpers
        // ─────────────────────────────────────────────

        private void LoadComPorts()
        {
            cboComPort.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            cboComPort.Items.AddRange(ports);

            if (ports.Length > 0)
                cboComPort.SelectedIndex = 0;
        }

        // ─────────────────────────────────────────────
        //  Print
        // ─────────────────────────────────────────────

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQty.Text, out int qty) || qty <= 0)
            {
                MessageBox.Show("Enter a valid quantity.");
                return;
            }

            if (cboComPort.SelectedItem == null)
            {
                MessageBox.Show("Select a COM port.");
                return;
            }

            string comPort = cboComPort.Text;
            string zpl = txtZPL.Text;

            SerialPort serial = new SerialPort
            {
                PortName = comPort,
                BaudRate = 9600,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None
            };

            try
            {
                serial.Open();
                for (int i = 0; i < qty; i++)
                    serial.Write(zpl);

                MessageBox.Show("Printing completed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (serial.IsOpen)
                    serial.Close();
            }
        }

        // ─────────────────────────────────────────────
        //  Model dropdown
        // ─────────────────────────────────────────────

        private void comboModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboModels.SelectedItem != null)
            {
                string selectedModel = comboModels.SelectedItem.ToString();
                ReadFromFileByModel(selectedModel);
                UpdateData();
            }
        }

        /// <summary>
        /// Opens a dialog so the user can add a new model name, part number, and description.
        /// The new entry is saved to part_info.txt and appended to the dropdown.
        /// </summary>
        private void btnAddModel_Click(object sender, EventArgs e)
        {
            using (Form dialog = new Form())
            {
                dialog.Text = "Add New Model";
                dialog.Size = new System.Drawing.Size(340, 220);
                dialog.FormBorderStyle = FormBorderStyle.FixedDialog;
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.MaximizeBox = false;
                dialog.MinimizeBox = false;

                // Model name
                Label lblModel = new Label { Text = "Model Name:", Left = 10, Top = 15, Width = 100 };
                TextBox txtModel = new TextBox { Left = 120, Top = 12, Width = 180 };

                // Part number
                Label lblPart = new Label { Text = "Part No:", Left = 10, Top = 50, Width = 100 };
                TextBox txtPart = new TextBox { Left = 120, Top = 47, Width = 180 };

                // Description
                Label lblDesc = new Label { Text = "Description:", Left = 10, Top = 85, Width = 100 };
                TextBox txtDesc = new TextBox { Left = 120, Top = 82, Width = 180 };

                // Buttons
                Button btnOk = new Button
                {
                    Text = "Add",
                    Left = 120,
                    Top = 130,
                    Width = 80,
                    DialogResult = DialogResult.OK
                };
                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    Left = 210,
                    Top = 130,
                    Width = 80,
                    DialogResult = DialogResult.Cancel
                };

                dialog.Controls.AddRange(new Control[]
                {
                    lblModel, txtModel,
                    lblPart,  txtPart,
                    lblDesc,  txtDesc,
                    btnOk,    btnCancel
                });
                dialog.AcceptButton = btnOk;
                dialog.CancelButton = btnCancel;

                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                string newModel = txtModel.Text.Trim().ToUpper();
                string newPart = txtPart.Text.Trim();
                string newDesc = txtDesc.Text.Trim();

                // Validation
                if (string.IsNullOrEmpty(newModel))
                {
                    MessageBox.Show("Model name cannot be empty.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboModels.Items.Contains(newModel))
                {
                    MessageBox.Show($"Model \"{newModel}\" already exists.", "Duplicate",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Save to file (unified method) and add to dropdown
                SaveOrUpdateModelInFile(newModel, newPart, newDesc);
                comboModels.Items.Add(newModel);
                comboModels.SelectedItem = newModel;

                MessageBox.Show($"Model \"{newModel}\" added successfully.", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Removes the currently selected model from the dropdown and the saved file.
        /// Built-in models (5INCH / 7INCH) cannot be removed.
        /// </summary>
        private void btnRemoveModel_Click(object sender, EventArgs e)
        {
            if (comboModels.SelectedItem == null) return;

            string model = comboModels.SelectedItem.ToString();

            if (model == "5INCH" || model == "7INCH")
            {
                MessageBox.Show("Built-in models cannot be removed.", "Not Allowed",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Remove model \"{model}\"?", "Confirm Remove",
                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            RemoveModelFromFile(model);
            comboModels.Items.Remove(model);

            if (comboModels.Items.Count > 0)
                comboModels.SelectedIndex = 0;
        }

        /// <summary>
        /// Saves the current PartNo and Description for the selected model to part_info.txt.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboModels.SelectedItem == null) return;

            string model = comboModels.SelectedItem.ToString();
            SaveOrUpdateModelInFile(model, PartNoTxt.Text.Trim(), DescTxt.Text.Trim());

            MessageBox.Show($"Saved \"{model}\" successfully.", "Saved",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ─────────────────────────────────────────────
        //  ZPL / data update
        // ─────────────────────────────────────────────

        private void UpdateDesc_Click(object sender, EventArgs e) => UpdateData();

        private void DescTxt_TextChanged(object sender, EventArgs e) => UpdateData();

        private void UpdateData()
        {
            txtZPL.Text = "^XA\n" +
                          "^BY2,6,50\n" +
                          "^FO20,5^A0N,10,10^BC^FD" + PartNoTxt.Text + "^FS\n" +
                          "^FO60,61^FD" + DescTxt.Text + "^FS\n" +
                          "^FO300,5^A0N,10,10^BC^FD" + PartNoTxt.Text + "^FS\n" +
                          "^FO340,61^FD" + DescTxt.Text + "^FS\n" +
                          "^FO560,5^A0N,10,10^BC^FD" + PartNoTxt.Text + "^FS\n" +
                          "^FO600,61^FD" + DescTxt.Text + "^FS\n" +
                          "^FO820,5^A0N,10,10^BC^FD" + PartNoTxt.Text + "^FS\n" +
                          "^FO860,61^FD" + DescTxt.Text + "^FS\n" +
                          "^XZ";
        }

        // ─────────────────────────────────────────────
        //  File I/O helpers
        // ─────────────────────────────────────────────

        private string GetFilePath()
        {
            return Path.Combine(Application.StartupPath, "part_info.txt");
        }

        /// <summary>
        /// Reads the last saved PartNo and Description for a given model from part_info.txt.
        /// </summary>
        private void ReadFromFileByModel(string model)
        {
            string filePath = GetFilePath();

            if (!File.Exists(filePath))
            {
                ApplyDefaultValues(model);
                return;
            }

            string lastPartNo = "";
            string lastDesc = "";

            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Model: ") && lines[i].Substring(7).Trim() == model)
                {
                    if (i + 1 < lines.Length && lines[i + 1].StartsWith("PartNo: "))
                        lastPartNo = lines[i + 1].Substring(8).Trim();

                    if (i + 2 < lines.Length && lines[i + 2].StartsWith("Description: "))
                        lastDesc = lines[i + 2].Substring(13).Trim();
                }
            }

            if (!string.IsNullOrEmpty(lastPartNo))
                PartNoTxt.Text = lastPartNo;
            else
                ApplyDefaultValues(model);   // fall back to defaults if not in file yet

            if (!string.IsNullOrEmpty(lastDesc))
                DescTxt.Text = lastDesc;
        }

        /// <summary>
        /// Applies hard-coded default PartNo / Description for built-in models.
        /// </summary>
        private void ApplyDefaultValues(string model)
        {
            if (model == "5INCH")
            {
                PartNoTxt.Text = "554571";
                DescTxt.Text = "MOD SBC ANDR 5INCH";
            }
            else if (model == "7INCH")
            {
                PartNoTxt.Text = "554572";
                DescTxt.Text = "MOD SBC ANDR 7INCH";
            }
            else
            {
                PartNoTxt.Text = "";
                DescTxt.Text = "";
            }
        }

        /// <summary>
        /// Saves or updates a model block in part_info.txt.
        /// If the model already exists, its PartNo and Description are updated in-place.
        /// If it does not exist, a new block is appended.
        /// This replaces the old SaveNewModelToFile method.
        /// </summary>
        private void SaveOrUpdateModelInFile(string model, string partNo, string desc)
        {
            string filePath = GetFilePath();

            var lines = File.Exists(filePath)
                ? new System.Collections.Generic.List<string>(File.ReadAllLines(filePath))
                : new System.Collections.Generic.List<string>();

            bool found = false;

            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("Model: ") && lines[i].Substring(7).Trim() == model)
                {
                    // Update PartNo line in-place
                    if (i + 1 < lines.Count && lines[i + 1].StartsWith("PartNo: "))
                        lines[i + 1] = $"PartNo: {partNo}";

                    // Update Description line in-place
                    if (i + 2 < lines.Count && lines[i + 2].StartsWith("Description: "))
                        lines[i + 2] = $"Description: {desc}";

                    found = true;
                    break;
                }
            }

            if (!found)
            {
                // Append new block
                lines.Add($"Model: {model}");
                lines.Add($"PartNo: {partNo}");
                lines.Add($"Description: {desc}");
                lines.Add(""); // blank separator
            }

            File.WriteAllLines(filePath, lines);
        }

        /// <summary>
        /// Removes all blocks for the given model from part_info.txt.
        /// </summary>
        private void RemoveModelFromFile(string model)
        {
            string filePath = GetFilePath();
            if (!File.Exists(filePath)) return;

            string[] lines = File.ReadAllLines(filePath);
            var newLines = new System.Collections.Generic.List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Model: ") && lines[i].Substring(7).Trim() == model)
                {
                    // Skip the Model / PartNo / Description lines and any trailing blank line
                    int skip = i;
                    while (skip < lines.Length &&
                           (lines[skip].StartsWith("Model: ") ||
                            lines[skip].StartsWith("PartNo: ") ||
                            lines[skip].StartsWith("Description: ") ||
                            string.IsNullOrWhiteSpace(lines[skip])))
                    {
                        skip++;
                        if (skip < lines.Length &&
                            lines[skip - 1].StartsWith("Model: ") &&
                            !lines[skip - 1].Substring(7).Trim().Equals(model))
                            break; // stop if we have bumped into the next model block
                    }
                    i = skip - 1; // outer loop will i++ past the skipped block
                }
                else
                {
                    newLines.Add(lines[i]);
                }
            }

            File.WriteAllLines(filePath, newLines);
        }

        /// <summary>
        /// On startup, reads part_info.txt and adds any models not already in the dropdown.
        /// </summary>
        private void LoadUserModelsFromFile()
        {
            string filePath = GetFilePath();
            if (!File.Exists(filePath)) return;

            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (line.StartsWith("Model: "))
                {
                    string model = line.Substring(7).Trim();
                    if (!comboModels.Items.Contains(model))
                        comboModels.Items.Add(model);
                }
            }
        }

        // ─────────────────────────────────────────────
        //  Misc
        // ─────────────────────────────────────────────

        private void label1_Click(object sender, EventArgs e) { }
    }
}