using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;
using Label = System.Windows.Forms.Label;

namespace XrmCommander
{
    public class XrmCommanderControl : MultipleConnectionsPluginControlBase
    {
        private List<Connection> connections;
        private TextBox commandTextBox;
        private Button executeButton;
        private Button helpButton;
        private ListBox connectionsListBox;
        private Button addConnectionButton;
        private Button removeConnectionButton;
        private Button setActiveButton;
        private Label activeConnectionLabel;
        private TextBox outputTextBox;
        private TabControl outputTabControl;
        private DataGridView outputDataGridView;
        private CommandParser commandParser;
        private CommandExecutor commandExecutor;
        private string activeConnectionName;

        public XrmCommanderControl()
        {
            connections = new List<Connection>();
            commandParser = new CommandParser();
        }

        // Override the OnLoad event to initialize the plugin UI
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeUI();
            commandExecutor = new CommandExecutor(commandParser, outputTextBox, outputTabControl, outputDataGridView);
        }

        // Initialize the plugin's UI components
        private void InitializeUI()
        {
            int firstColumnWidth = (int)(ClientSize.Width * 0.75); // First column takes 75% of the width
            int secondColumnWidth = ClientSize.Width - firstColumnWidth; // Second column takes the remaining 25%

            // Set up the command text box (multi-line)
            commandTextBox = new TextBox
            {
                Location = new System.Drawing.Point(20, 20),
                Multiline = false,
                ScrollBars = ScrollBars.Vertical,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Height = 100,
                Width = firstColumnWidth - 40 // Leave some padding
            };

            commandTextBox.Text = "Enter 'help' to see a list of commands";
            commandTextBox.Enter += CommandTextBox_Enter;
            commandTextBox.Leave += CommandTextBox_Leave;
            commandTextBox.KeyDown += CommandTextBox_KeyDown;

            Controls.Add(commandTextBox);

            executeButton = new Button
            {
                Text = "Execute",
                Anchor = AnchorStyles.Top | AnchorStyles.Right, 
                Size = new System.Drawing.Size(100, 30)
            };

            executeButton.Location = new System.Drawing.Point(
                commandTextBox.Right - executeButton.Width,
                commandTextBox.Bottom + 10
            );
            executeButton.Click += ExecuteButton_Click;
            Controls.Add(executeButton);

            helpButton = new Button
            {
                Text = "Help",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Size = new System.Drawing.Size(100, 30)
            };

            helpButton.Location = new System.Drawing.Point(
                executeButton.Right - executeButton.Width - helpButton.Width - 10,
                commandTextBox.Bottom + 10
            );
            helpButton.Click += HelpButton_Click;
            Controls.Add(helpButton);

            // Set up the output tab control
            outputTabControl = new TabControl
            {
                Location = new System.Drawing.Point(20, 170),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Height = ClientSize.Height - 220,
                Width = firstColumnWidth - 40 // Leave some padding
            };

            // Add the text output tab
            var textOutputTab = new TabPage("Text Output");
            outputTextBox = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14)
            };
            textOutputTab.Controls.Add(outputTextBox);
            outputTabControl.TabPages.Add(textOutputTab);

            // Add the table output tab
            var tableOutputTab = new TabPage("Table Output");
            outputDataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            outputDataGridView.CellDoubleClick += OutputDataGridView_CellDoubleClick;
            tableOutputTab.Controls.Add(outputDataGridView);
            outputTabControl.TabPages.Add(tableOutputTab);

            Controls.Add(outputTabControl);

            // Set up the active connection label
            activeConnectionLabel = new Label
            {
                Location = new System.Drawing.Point(firstColumnWidth + 20, 20),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Text = "Active Connection: None",
                AutoSize = true
            };
            Controls.Add(activeConnectionLabel);

            // Set up the connections list box
            connectionsListBox = new ListBox
            {
                Location = new System.Drawing.Point(firstColumnWidth + 20, 50),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Height = 100,
                Width = secondColumnWidth - 40 // Leave some padding
            };
            Controls.Add(connectionsListBox);

            // Set up the add connection button
            addConnectionButton = new Button
            {
                Location = new System.Drawing.Point(firstColumnWidth + 20, 160),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Text = "Add Connection",
                Width = secondColumnWidth - 40 // Leave some padding
            };
            addConnectionButton.Click += AddConnectionButton_Click;
            Controls.Add(addConnectionButton);

            // Set up the remove connection button
            removeConnectionButton = new Button
            {
                Location = new System.Drawing.Point(firstColumnWidth + 20, 185),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Text = "Remove Connection",
                Width = secondColumnWidth - 40 // Leave some padding
            };
            removeConnectionButton.Click += RemoveConnectionButton_Click;
            Controls.Add(removeConnectionButton);

            // Set up the set active connection button
            setActiveButton = new Button
            {
                Location = new System.Drawing.Point(firstColumnWidth + 20, 210),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Text = "Set Active",
                Width = secondColumnWidth - 40 // Leave some padding
            };
            setActiveButton.Click += SetActiveButton_Click;
            Controls.Add(setActiveButton);

            // Populate the connections list box with existing connections
            RefreshConnectionsList();
            activeConnectionName = ConnectionDetail.ConnectionName;
            UpdateActiveConnectionLabel();
        }

        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                executeButton.PerformClick();   
            }
        }

        private void RefreshConnectionsList()
        {
            connectionsListBox.Items.Clear();
            connections.Clear();

            connections.Add(new Connection() { ConnectionDetails = ConnectionDetail, IsActive = true });
            foreach (var acd in AdditionalConnectionDetails)
                connections.Add(new Connection() { ConnectionDetails = acd, IsActive = false });

            foreach (var connection in connections)
            {
                connectionsListBox.Items.Add(connection.ConnectionDetails.ConnectionName);
            }
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            commandExecutor.ShowHelp();
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            string command = commandTextBox.Text;

            var parsedCommand = commandParser.Parse(command);
            if (parsedCommand == null)
            {
                WriteOutput($"Invalid command.");
                return;
            }

            commandExecutor.ExecuteParsedCommand(connections, parsedCommand);
            commandTextBox.Text = "";
        }

        private void CommandTextBox_Enter(object sender, EventArgs e)
        {
            if (commandTextBox.Text == "Enter 'help' to see a list of commands")
            {
                commandTextBox.Text = ""; // Clear the placeholder text
                commandTextBox.ForeColor = SystemColors.WindowText; // Reset text color
            }
        }

        private void CommandTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(commandTextBox.Text))
            {
                commandTextBox.Text = "Enter 'help' to see a list of commands"; // Restore placeholder text
                commandTextBox.ForeColor = SystemColors.GrayText; // Set placeholder text color
            }
        }

        private void OutputDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure a valid row is clicked
            {
                var row = outputDataGridView.Rows[e.RowIndex];
                string recordId = row.Cells[0].Value?.ToString();
                if (!string.IsNullOrEmpty(recordId))
                {
                    string[] tags = outputDataGridView.Tag?.ToString().Split('|');
                    switch (tags[0])
                    {
                        case "record":
                            commandExecutor.NavigateToTable(tags[2], tags[1], recordId);
                            break;
                        case "tablemetadata":
                            commandExecutor.NavigateToTableFieldsMetadata(tags[2], tags[1]);
                            break;
                    }
                }
            }
        }

        private void AddConnectionButton_Click(object sender, EventArgs e)
        {
            AddAdditionalOrganization();
        }

        // Handle the remove connection button click event
        private void RemoveConnectionButton_Click(object sender, EventArgs e)
        {
            if (connectionsListBox.SelectedItem != null)
            {
                // Get the selected connection name
                string selectedConnectionName = connectionsListBox.SelectedItem.ToString();

                // Find the corresponding ConnectionDetail
                var connectionToRemove = AdditionalConnectionDetails.FirstOrDefault(c => c.ConnectionName == selectedConnectionName);
                if (connectionToRemove != null)
                {
                    // Call RemoveAdditionalOrganization to remove the connection
                    RemoveAdditionalOrganization(connectionToRemove);

                    // Refresh the connections list box
                    RefreshConnectionsList();
                }
            }
            else
            {
                MessageBox.Show("Please select a connection to remove.");
            }
        }

        private void SetActiveButton_Click(object sender, EventArgs e)
        {
            if (connectionsListBox.SelectedItem != null)
            {
                // Set the selected connection as active
                activeConnectionName = connectionsListBox.SelectedItem.ToString();
                ResetActiveConnection();
                UpdateActiveConnectionLabel();
                WriteOutput($"Active connection set to: {activeConnectionName}");
            }
            else
            {
                WriteOutput("Error: Please select a connection to set as active.");
            }
        }

        private void ResetActiveConnection()
        {
            var activeConn = connections.Where(c => c.IsActive == true).First();
            activeConn.IsActive = false;
            var newActiveConn = connections.Where(c => c.ConnectionDetails.ConnectionName == activeConnectionName).First();
            newActiveConn.IsActive = true;
        }

        // Update the active connection label
        private void UpdateActiveConnectionLabel()
        {
            activeConnectionLabel.Text = string.IsNullOrEmpty(activeConnectionName)
                ? "Active: None"
                : $"Active: {activeConnectionName}";
        }

        protected override void ConnectionDetailsUpdated(NotifyCollectionChangedEventArgs e)
        {
            RefreshConnectionsList();
        }

        private void WriteOutput(string message)
        {
            outputTextBox.AppendText($"{message}{Environment.NewLine}");
        }
    }

    public class Connection
    {
        public ConnectionDetail ConnectionDetails { get; set; }
        public bool IsActive { get; set; }
    }
}