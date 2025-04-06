using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace XrmCommander
{
    public class CommandExecutor
    {
        private TextBox outputTextBox;
        private TabControl outputTabControl;
        private DataGridView outputDataGridView;
        private CommandParser commandParser;
        private List<Connection> connections;
        private bool previewMode = true;
        public CommandExecutor(CommandParser commandParser, TextBox outputTextBox, TabControl outputTabControl,
            DataGridView outputDataGridView)
        {
            this.commandParser = commandParser;
            this.outputTextBox = outputTextBox;
            this.outputTabControl = outputTabControl;
            this.outputDataGridView = outputDataGridView;
        }
        public void ExecuteParsedCommand(List<Connection> connections, ParsedCommand parsedCommand)
        {
            this.connections = connections;

            switch (parsedCommand.Action)
            {
                case "Fetch":
                    FetchRecords(parsedCommand.Value, parsedCommand.Environment);
                    break;
                case "Navigate":
                    NavigateToTable(parsedCommand.Table, parsedCommand.Environment, "");
                    break;
                case "NavigateId":
                    NavigateToTable(parsedCommand.Table, parsedCommand.Environment, parsedCommand.Value);
                    break;
                case "Query":
                    QueryRecords(parsedCommand.Environment, parsedCommand.Table, parsedCommand.Fields, parsedCommand.ConditionAttribute, parsedCommand.Operator, parsedCommand.ConditionValue);
                    break;
                case "Update":
                    //UpdateRecords(parsedCommand.Target, parsedCommand.Environment, parsedCommand.Updates);
                    break;
                case "Help":
                    ShowHelp();
                    break;
                case "AdminCenter":
                    GoToAdminCenter(parsedCommand.Environment);
                    break;
                case "MakerPortal":
                    GoToMakerPortal(parsedCommand.Environment, "");
                    break;
                case "MakerPortalApps":
                    GoToMakerPortal(parsedCommand.Environment, "apps");
                    break;
                case "MakerPortalFlows":
                    GoToMakerPortal(parsedCommand.Environment, "logicflows");
                    break;
                case "MakerPortalSolutions":
                    GoToMakerPortal(parsedCommand.Environment, "solutions");
                    break;
                case "MakerPortalTables":
                    GoToMakerPortal(parsedCommand.Environment, "entities");
                    break;
                case "Preview":
                    SetPreview(parsedCommand.Value);
                    break;
                case "TotalRecords":
                    TotalTableRecords(parsedCommand.Environment, parsedCommand.Table);
                    break;
                case "PluginTrace":
                    GoToPluginTraceLogs(parsedCommand.Environment);
                    break;
                case "EntityTypeCode":
                    GetEntityTypeCode(parsedCommand.Environment, parsedCommand.Table);
                    break;
                case "ListAttributesForTable":
                    ListAttributesForTable(parsedCommand.Environment, parsedCommand.Table);
                    break;
                case "AdvancedFind":
                    GoToAdvancedFind(parsedCommand.Environment);
                    break;
                case "GoToTableMetadata":
                    GoToTableMetadata(parsedCommand.Environment, parsedCommand.Table);
                    break;
                default:
                    WriteOutput($"Action '{parsedCommand.Action}' is not supported.");
                    break;
            }
        }

        public void SetPreview(string previewValue)
        {
            this.previewMode = (previewValue.ToLower() == "enable") ? true : false;
        }

        public void ShowHelp()
        {
            ClearOutput();

            var commands = commandParser.GetCommands().OrderBy(c => c.Guide);
            foreach (var command in commands)
            {
                WriteOutput(command.Guide + " - " + command.Description);
            }
        }

        public void GetEntityTypeCode(string environment, string table)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            if (conn == null) return;

            RetrieveEntityRequest request = new RetrieveEntityRequest();
            request.LogicalName = table.ToLower();
            var resp = (RetrieveEntityResponse)conn.GetCrmServiceClient().Execute(request);
            WriteOutput($"Type Code for {table} is { resp.EntityMetadata.ObjectTypeCode}");
        }

        public void ListAttributesForTable(string environment, string table)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            if (conn == null) return;

            RetrieveEntityRequest request = new RetrieveEntityRequest();
            request.LogicalName = table.ToLower();
            request.EntityFilters = EntityFilters.Attributes;
            var resp = (RetrieveEntityResponse)conn.GetCrmServiceClient().Execute(request);

            var results = new DataTable();
            results.Columns.Add("Display Name");
            results.Columns.Add("Logical Name");
            results.Columns.Add("Schema Name");
            results.Columns.Add("Data Type");

            foreach (var field in resp.EntityMetadata.Attributes)
            {
                var dr = results.NewRow();
                dr.SetField("Display Name", field.DisplayName?.UserLocalizedLabel?.Label);
                dr.SetField("Logical Name", field.LogicalName);
                dr.SetField("Schema Name", field.SchemaName);

                var dataType = "";
                switch (field.AttributeType)
                {
                    case AttributeTypeCode.BigInt:
                        dataType = "Big Int";
                        break;
                    case AttributeTypeCode.Boolean:
                        dataType = "Boolean";
                        break;
                    case AttributeTypeCode.CalendarRules:
                        dataType = "Calendar Rules";
                        break;
                    case AttributeTypeCode.Customer:
                        dataType = "Customer";
                        break;
                    case AttributeTypeCode.DateTime:
                        dataType = "Date/Time";
                        break;
                    case AttributeTypeCode.Decimal:
                        dataType = "Decimal";
                        break;
                    case AttributeTypeCode.Double:
                        dataType = "Double";
                        break;
                    case AttributeTypeCode.EntityName:
                        dataType = "EntityName";
                        break;
                    case AttributeTypeCode.Integer:
                        dataType = "Integer";
                        break;
                    case AttributeTypeCode.Lookup:
                        dataType = "Lookup";
                        break;
                    case AttributeTypeCode.ManagedProperty:
                        dataType = "Managed Property";
                        break;
                    case AttributeTypeCode.Memo:
                        dataType = "Memo";
                        break;
                    case AttributeTypeCode.Money:
                        dataType = "Money";
                        break;
                    case AttributeTypeCode.Owner:
                        dataType = "Owner";
                        break;
                    case AttributeTypeCode.PartyList:
                        dataType = "PartyList";
                        break;
                    case AttributeTypeCode.Picklist:
                        dataType = "Picklist";
                        break;
                    case AttributeTypeCode.State:
                        dataType = "State";
                        break;
                    case AttributeTypeCode.Status:
                        dataType = "Status";
                        break;
                    case AttributeTypeCode.String:
                        dataType = "String";
                        break;
                    case AttributeTypeCode.Uniqueidentifier:
                        dataType = "Uniqueidentifier";
                        break;
                    case AttributeTypeCode.Virtual:
                        dataType = "Virtual";
                        break;
                }
                dr.SetField("Data Type", dataType);
                results.Rows.Add(dr);
            }

            WriteOutput($"Retrieve attributes from {table} in {environment}...");
            WriteOutput("Results displayed in the Table Output tab.");

            outputDataGridView.DataSource = results;
            outputDataGridView.Tag = "tablemetadata" + "|" + conn.ConnectionName + "|" + resp.EntityMetadata.MetadataId;
            outputTabControl.SelectedTab = outputTabControl.TabPages[1];
        }

        public void QueryRecords(string environment, string table, string fields, string conditionField, string conditionOperator, string conditionValue)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            if (conn == null) return;

            List<string> fieldArr = fields.Split(',').ToList();

            QueryExpression qe = new QueryExpression(table);
            qe.ColumnSet = new ColumnSet();
            qe.ColumnSet.AddColumn(table + "id");
            foreach (var field in fieldArr) qe.ColumnSet.AddColumn(field.Trim());
            qe.Criteria = new FilterExpression(LogicalOperator.And);
            qe.Criteria.AddCondition(conditionField, ConditionOperator.Equal, conditionValue);

            var entities = conn.GetCrmServiceClient().RetrieveMultiple(qe).Entities;

            var results = new DataTable();
            results.Columns.Add(table + "id");
            foreach (var field in fieldArr) results.Columns.Add(field);
            foreach (var entity in entities)
            {
                var dr = results.NewRow();
                dr.SetField(table + "id", entity.Id);
                foreach (var field in fieldArr)
                {
                    dr.SetField(field, GetValueFromAttribute(entity, field));
                }
                results.Rows.Add(dr);
            }

            WriteOutput($"Querying {table} in {environment} where {conditionField} = {conditionValue}...");
            WriteOutput("Results displayed in the Table Output tab.");

            outputDataGridView.DataSource = results;
            outputDataGridView.Tag = "record"+"|"+conn.ConnectionName+"|"+table;
            outputTabControl.SelectedTab = outputTabControl.TabPages[1]; 

            if (entities.Count == 0)
                WriteOutput("No records found.");
        }

        public void FetchRecords(string fetchxml, string environment)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            if (conn == null) return;

            XDocument xmlDoc = XDocument.Parse(fetchxml);

            string table = xmlDoc.Descendants("entity")
                                  .FirstOrDefault()?
                                  .Attribute("name")?
                                  .Value;

            if (string.IsNullOrEmpty(table))
            {
                WriteOutput("Not valid FetchXml.");
                return;
            }

            List<string> fieldArr = xmlDoc.Descendants("attribute")
                                            .Select(attr => attr.Attribute("name")?.Value)
                                            .Where(name => !string.IsNullOrEmpty(name))
                                            .ToList();

            var entities = conn.GetCrmServiceClient().RetrieveMultiple(new FetchExpression(fetchxml)).Entities;

            var results = new DataTable();
            results.Columns.Add(table + "id");
            foreach (var field in fieldArr) results.Columns.Add(field);
            foreach (var entity in entities)
            {
                var dr = results.NewRow();
                dr.SetField(table + "id", entity.Id);
                foreach (var field in fieldArr)
                {
                    dr.SetField(field, GetValueFromAttribute(entity, field));
                }
                results.Rows.Add(dr);
            }

            WriteOutput($"Executing fetch in {environment}...");
            WriteOutput("Results displayed in the Table Output tab.");

            outputDataGridView.DataSource = results;
            outputDataGridView.Tag = "record" + "|" + conn.ConnectionName + "|" + table;
            outputTabControl.SelectedTab = outputTabControl.TabPages[1];

            if (entities.Count == 0)
                WriteOutput("No records found.");
        }
        public void NavigateToTable(string target, string environment, string value)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            if (conn == null) return;

            string url = GetNavigateUrl(conn, target, value);
            if (!string.IsNullOrEmpty(url))
            {
                conn.OpenUrlWithBrowserProfile(new Uri(url));
                WriteOutput($"Navigated to '{target}' in '{conn.ConnectionName}'");
            }
            else
            {
                WriteOutput($"Unable to generate a URL for '{target}' in {environment}'");
            }
        }
                
        public void GoToTableMetadata(string environment, string target)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            if (conn == null) return;

            string url = GetMakerUrl(conn.EnvironmentId, "entities");
            if (!string.IsNullOrEmpty(url))
            {
                var id = GetEntityId(environment, target);
                conn.OpenUrlWithBrowserProfile(new Uri(url + "/" + id));
            }
        }

        public void NavigateToTableFieldsMetadata(string target, string environment)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            if (conn == null) return;

            string url = GetMakerUrl(conn.EnvironmentId, "entities");
            if (!string.IsNullOrEmpty(url))
            {
                conn.OpenUrlWithBrowserProfile(new Uri(url + "/" + target + "/fields"));
            }
        }

        public void GoToAdminCenter(string environment)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            conn.OpenUrlWithBrowserProfile(new Uri("https://admin.powerplatform.microsoft.com/"));
            WriteOutput($"Navigated to Admin Center");
        }

        public void TotalTableRecords(string environment, string table)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            var req = new RetrieveTotalRecordCountRequest();
            req.EntityNames = new string[] { table };

            var res = (RetrieveTotalRecordCountResponse)conn.GetCrmServiceClient().Execute(req);
            foreach (var count in res.EntityRecordCountCollection)
            {
                WriteOutput($"{table} has {count.Value} records.");
            }
        }

        public void GoToMakerPortal(string environment, string target)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            conn.OpenUrlWithBrowserProfile(new Uri(GetMakerUrl(conn.EnvironmentId, target)));
            WriteOutput($"Navigated to Maker Portal");
        }

        public void GoToPluginTraceLogs(string environment)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            conn.OpenUrlWithBrowserProfile(new Uri($"{conn.WebApplicationUrl}_root/homepage.aspx?etc=4619&sitemappath=Settings%7cCustomizations%7cnav_plugintrace&pagemode=iframe"));
            WriteOutput($"Navigated to Plugin Trace Logs");
        }   
        
        public void GoToAdvancedFind(string environment)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            conn.OpenUrlWithBrowserProfile(new Uri($"{conn.WebApplicationUrl}main.aspx?pagetype=advancedfind"));
            WriteOutput($"Navigated to Advanced Find");
        }

        public string GetNavigateUrl(ConnectionDetail conn, string target, string id)
        {
            if (!String.IsNullOrEmpty(id))
                return $"{conn.WebApplicationUrl}main.aspx?pagetype=entityrecord&id={id}&etn={target}";

            return $"{conn.WebApplicationUrl}main.aspx?pagetype=entityrecord&etn={target}";
        }

        private Guid? GetEntityId(string environment, string table)
        {
            ClearOutput();

            var conn = GetConnectionOrDefault(environment);
            if (conn == null) return null;

            RetrieveEntityRequest request = new RetrieveEntityRequest();
            request.LogicalName = table.ToLower();
            var resp = (RetrieveEntityResponse)conn.GetCrmServiceClient().Execute(request);
            return resp.EntityMetadata.MetadataId;
        }

        private void WriteOutput(string message)
        {
            outputTextBox.AppendText($"{message}{Environment.NewLine}");
            outputTabControl.SelectedTab = outputTabControl.TabPages[0];
        }

        private void ClearOutput()
        {
            outputTextBox.Text = "";
        }

        private string GetMakerUrl(string environment, string target)
        {
            string url = "";
            if (previewMode == true)
                url = $"https://make.preview.powerapps.com/environments/{environment}/{target}";
            else
                url = $"https://make.powerapps.com/environments/{environment}/{target}";

            return url;
        }

        private ConnectionDetail GetConnectionOrDefault(string environment)
        {
            if (String.IsNullOrEmpty(environment))
            {
                return GetActiveConnection();
            }

            var conn = GetConnectionForEnvironment(environment);
            if (conn == null)
            {
                WriteOutput($"Could not find connection named {environment}");
                return null;
            }

            return conn;
        }

        private ConnectionDetail GetActiveConnection()
        {
            Connection conn = connections.Where(c => c.IsActive == true).FirstOrDefault();
            if (conn == null)
            {
                WriteOutput($"Can't find Active Connection'");
                return null;
            }

            return conn.ConnectionDetails;
        }

        private ConnectionDetail GetConnectionForEnvironment(string environment)
        {
            Connection conn = connections.Where(acd => acd.ConnectionDetails.ConnectionName.ToLower() == environment.ToLower()).FirstOrDefault();
            if (conn == null)
            {
                WriteOutput($"Can't find Connection named '{environment}'");
                return null;
            }

            return conn.ConnectionDetails;
        }

        private string GetValueFromAttribute(Entity entity, string field)
        {
            if (!entity.Attributes.Contains(field))
                return "null";

            if (entity[field] is Money)
                return entity.GetAttributeValue<Money>(field).Value.ToString();
            if (entity[field] is EntityReference)
                return entity.GetAttributeValue<EntityReference>(field).Id.ToString();
            if (entity[field] is OptionSetValue)
                return entity.GetAttributeValue<OptionSetValue>(field).Value.ToString();
            if (entity[field] is DateTime)
                return entity.GetAttributeValue<DateTime>(field).ToString();

            return entity[field].ToString();
        }
    }
}
