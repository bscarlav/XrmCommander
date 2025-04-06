using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace XrmCommander
{
    public class CommandParser
    {
        private static readonly List<Command> CommandPatterns = new List<Command>
        {
            new Command()
            {
                Action="Help",
                Description="List out command guide and descriptions",
                Guide="(help | show help | list help)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(help|show help|list help)$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="Fetch",
                Description="Execute a fetch query",
                Guide="Fetch FETCHXML (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^fetch\s+(?<value><fetch>.*?<\/fetch>)\s+in\s+(?<environment>\w+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^fetch\s+(?<value><fetch>.*?<\/fetch>)$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            { 
                Action="Navigate",
                Description="Navigate to a specific table",
                Guide="(Navigate to | goto | go to | open) TABLE (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+(?<table>\w+)\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+(?<table>\w+)$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="NavigateId",
                Description="Navigate to a specific table record by id",
                Guide="(Navigate to | goto | go to | open) TABLE with id ID (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+(?<table>\w+) with id (?<value>[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}) in (?<environment>\w+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+(?<table>\w+) with id (?<value>[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12})$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="Query",
                Description="Select field(s) from a table row using a simple filter",
                Guide="Select FIELD(S) from TABLE WHERE FIELD IS VALUE (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^select\s+(?<fields>[^ ]+(?:\s*,\s*[^ ]+)*)\s+from\s+(?<table>\w+)\s+where\s+(?<conditionAttribute>\w+)\s+(?<operator>\w+)\s+(?<conditionValue>[^ ]+)\s+in\s+(?<environment>\w+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^select\s+(?<fields>[^ ]+(?:\s*,\s*[^ ]+)*)\s+from\s+(?<table>\w+)\s+where\s+(?<conditionAttribute>\w+)\s+(?<operator>\w+)\s+(?<conditionValue>[^ ]+)$", RegexOptions.IgnoreCase)
                }                
            },
            new Command()
            {
                Action="Preview",
                Description="Enable or disable preview mode in maker portal (make.powerapps.com vs make.preview.powerapps.com)",
                Guide="(enable | disable) preview",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(?<value>\w+)\s+preview", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="MakerPortal",
                Description="Open the Maker Portal for the given or active environment",
                Guide="(open | go to | goto | navigate to) maker portal (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+maker portal\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+maker portal$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="MakerPortalApps",
                Description="Open the Maker Portal Apps section for the given or active environment",
                Guide="(open | go to | goto | navigate to) apps (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+apps\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+apps$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="MakerPortalFlows",
                Description="Open the Maker Portal Flows section for the given or active environment",
                Guide="(open | go to | goto | navigate to) flows (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+flows\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+flows", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="MakerPortalSolutions",
                Description="Open the Maker Portal Solutions section for the given or active environment",
                Guide="(open | go to | goto | navigate to) solutions (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+solutions\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+solutions", RegexOptions.IgnoreCase)
                }
            },
             new Command()
            {
                Action="MakerPortalTables",
                Description="Open the Maker Portal Tables section for the given or active environment",
                Guide="(open | go to | goto | navigate to) tables (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+tables\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+tables", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="AdminCenter",
                Description="Open the Admin Center for the given or active environment",
                Guide="(open | go to | goto | navigate to) admin center (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+admin center\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+admin center$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="TotalRecords",
                Description="Get a total record count for a table",
                Guide="Total TABLE records (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^total\s+(?<table>\w+)\s+records\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^total\s+(?<table>\w+)\s+records$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="PluginTrace",
                Description="Navigate to the plugin trace logs",
                Guide="Open plugin trace logs (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+(plugin trace logs|trace logs)\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+(plugin trace logs|trace logs)$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="EntityTypeCode",
                Description="Get the type code for a table",
                Guide="(get|find|retrieve) TABLE type code (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(get|find|retrieve)\s+(?<table>\w+)\s+(type code|typecode)\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(get|find|retrieve)\s+(?<table>\w+)\s+(type code|typecode)", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="ListAttributesForTable",
                Description="List attributes for a table",
                Guide="(get|list|show) attributes for TABLE (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(get|list|show)\s+(attributes|fields) for\s+(?<table>\w+)\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(get|list|show)\s+(attributes|fields) for\s+(?<table>\w+)$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="AdvancedFind",
                Description="Navigate to the advanced find",
                Guide="Open advanced find (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+advanced find\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+advanced find$", RegexOptions.IgnoreCase)
                }
            },
            new Command()
            {
                Action="GoToTableMetadata",
                Description="Navigate to table metadata",
                Guide="Go to TABLE metadata (in ENVIRONMENT)",
                Regexes=new List<Regex>()
                {
                    new Regex(@"^(goto|go to|navigate to|open)\s+(?<table>\w+)\s+metadata\s+in\s+(?<environment>[^\n]+)$", RegexOptions.IgnoreCase),
                    new Regex(@"^(goto|go to|navigate to|open)\s+(?<table>\w+)\s+metadata$", RegexOptions.IgnoreCase)
                }
            },            
        };

        public ParsedCommand Parse(string input)
        {
            foreach (var command in CommandPatterns)
            {
                foreach (var regex in command.Regexes)
                {
                    var match = regex.Match(input);
                    if (match.Success)
                    {
                        return new ParsedCommand
                        {
                            Action = command.Action,
                            Table = match.Groups["table"]?.Value,
                            Environment = match.Groups["environment"]?.Value,
                            Condition = match.Groups["condition"]?.Value,
                            Updates = match.Groups["updates"]?.Value,
                            ConditionAttribute = match.Groups["conditionAttribute"]?.Value,
                            Operator = match.Groups["operator"]?.Value,
                            ConditionValue = match.Groups["conditionValue"]?.Value,
                            Fields = match.Groups["fields"]?.Value,
                            Value = match.Groups["value"]?.Value,
                        };
                    }
                }
            }

            return null;
        }

        public List<Command> GetCommands()
        {
            return CommandPatterns;
        }
    }

    public class Command
    {
        public string Action { get; set; }
        public string Guide { get; set; }
        public string Description { get; set; }
        public List<Regex> Regexes { get; set; }
    }

    public class ParsedCommand
    {
        public string Action { get; set; }
        public string Environment { get; set; }
        public string Condition { get; set; } 
        public string Updates { get; set; }
        public string Operator { get; set; }
        public string ConditionAttribute { get; set; }
        public string ConditionValue { get; set; }
        public string Fields { get; set; }
        public string Table { get; set; }
        public string Value { get; set; }
    }
}
