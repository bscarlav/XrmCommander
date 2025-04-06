﻿using System.ComponentModel.Composition;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace XrmCommander
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "XrmCommander"),
        ExportMetadata("Description", "A powerful tool designed to streamline your D365 / Power Platform operations by providing centralized control over multiple environments. With XrmCommander, you can effortlessly execute commands, navigate records and admin pages—all while seamlessly switching between connections and browser profiles. Whether you're querying data, or configuring environments, XrmCommander puts you in full command of your D365 / Power Platform ecosystem."),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAVFBMVEX///8AAADq6uqIiIjGxsZiYmI+Pj55eXktLS0VFRWqqqry8vK4uLgPDw/BwcGhoaEgICCAgIDb29tPT09sbGz4+PiRkZHU1NQYGBixsbGbm5tEREScHqHcAAAA8ElEQVQ4y83Ty26FIBCA4XHkojDIVRR8//csPSan2li6PT8Jq2/BTALAB6QS75QUGCc6uRkGX4gUq6if8kMDMmepuH58wNgAt9ZWW9VjvAEzdzINJNYpNTD2ljCeYNX/gBxZH9Dk7NoDsEWTEIAptKGOFbeqsYYLAO3NwYAJGTeRorSGlKErACjOFZBGoRh5si5x9wsEMWRIe8VJOKYOEY87KJOrKx1ZMFFittzneAXaz6JNypa1BMKNwrKuhD8gnENoovI631e7wxv4cw20m1v+DTC83qHZcgtvU/y9aomd5AD7PnXaHZCXnfzyCV/zCzeME+KQ0wHKAAAAAElFTkSuQmCC"),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "iVBORw0KGgoAAAANSUhEUgAAAFAAAABQCAYAAACOEfKtAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAEsmlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSfvu78nIGlkPSdXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQnPz4KPHg6eG1wbWV0YSB4bWxuczp4PSdhZG9iZTpuczptZXRhLyc+CjxyZGY6UkRGIHhtbG5zOnJkZj0naHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyc+CgogPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9JycKICB4bWxuczpBdHRyaWI9J2h0dHA6Ly9ucy5hdHRyaWJ1dGlvbi5jb20vYWRzLzEuMC8nPgogIDxBdHRyaWI6QWRzPgogICA8cmRmOlNlcT4KICAgIDxyZGY6bGkgcmRmOnBhcnNlVHlwZT0nUmVzb3VyY2UnPgogICAgIDxBdHRyaWI6Q3JlYXRlZD4yMDI1LTAyLTI0PC9BdHRyaWI6Q3JlYXRlZD4KICAgICA8QXR0cmliOkV4dElkPmIyMGQ0OGFhLWM5ODQtNGZkMS05ZTA0LWE3MWIwMmUwZTM3ZDwvQXR0cmliOkV4dElkPgogICAgIDxBdHRyaWI6RmJJZD41MjUyNjU5MTQxNzk1ODA8L0F0dHJpYjpGYklkPgogICAgIDxBdHRyaWI6VG91Y2hUeXBlPjI8L0F0dHJpYjpUb3VjaFR5cGU+CiAgICA8L3JkZjpsaT4KICAgPC9yZGY6U2VxPgogIDwvQXR0cmliOkFkcz4KIDwvcmRmOkRlc2NyaXB0aW9uPgoKIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PScnCiAgeG1sbnM6ZGM9J2h0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvJz4KICA8ZGM6dGl0bGU+CiAgIDxyZGY6QWx0PgogICAgPHJkZjpsaSB4bWw6bGFuZz0neC1kZWZhdWx0Jz5YUk0gLSAxPC9yZGY6bGk+CiAgIDwvcmRmOkFsdD4KICA8L2RjOnRpdGxlPgogPC9yZGY6RGVzY3JpcHRpb24+CgogPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9JycKICB4bWxuczpwZGY9J2h0dHA6Ly9ucy5hZG9iZS5jb20vcGRmLzEuMy8nPgogIDxwZGY6QXV0aG9yPkJsYWtlIFNjYXJsYXZhaTwvcGRmOkF1dGhvcj4KIDwvcmRmOkRlc2NyaXB0aW9uPgoKIDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PScnCiAgeG1sbnM6eG1wPSdodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvJz4KICA8eG1wOkNyZWF0b3JUb29sPkNhbnZhIChSZW5kZXJlcikgZG9jPURBR2Ytay1uUllJIHVzZXI9VUFGU0hRTHFXc2sgYnJhbmQ9QkFGbHRxeEo3SlUgdGVtcGxhdGU9PC94bXA6Q3JlYXRvclRvb2w+CiA8L3JkZjpEZXNjcmlwdGlvbj4KPC9yZGY6UkRGPgo8L3g6eG1wbWV0YT4KPD94cGFja2V0IGVuZD0ncic/PkPR8uUAAAkrSURBVHic7Zt5bExdGIefqXaUSauoWkpQxNIykdResU2DWotoMtSSqN2o2EVraxANvhahEUopoaKhCRFrRS1JqQi1NCaSopYiUaVVM/f7YzI3vWamZnqnX9vPfZImzjvnnHnPz9nPOypBEAQUqo1HbTtQ31EElIkioEwUAWWiCCgTRUCZKALKRBFQJoqAMvGsnPj58ydms7m2fKkXeHh4oFarxbQnQHJyMlu2bKG4uLjWHKtPtGjRgs2bNzN//nxUR44cEWbNmlXbPtVLMjIyUHXp0kUoKCiobV/qJaGhoag8PDwE67zXq1cvIiMja9mtuk1mZiaPHj0CwMvLC8/Ki4ZWq2Xjxo215Fr9wGg0igJWVFQ43sYcOHAAg8HA48ePRdvjx48xGAzs379ftFVUVJCYmEhsbCwvX76sQdfrJp72jMePH2fBggUAnDt3DqPRiEqlIiIigsLCQgCaNGmCXq8nKSmJVatWAXDr1i1yc3P/I9frBnYFrLyoFBUVUVJSAsD79+9Fu7W3Vc5rNBqpqKjAy8urRpyti9gdwrGxsUyaNInAwEAOHjyIn58ffn5+pKSkEBgYyKRJkzAYDABs2LCB8PBwOnToQFpa2l8lHoAKEN9EoqOjSUtLq0V36j4zZszg2LFjYloyhN+/f8+NGzf+a5/qFe/evZOkJT1QwXWU2xiZKALKRDIHDhw4kJiYmNrypV5w8OBBbt++LaYlAnbq1AnlZqZqrl27JhFQGcIyUQSUiSKgTBQBZaIIKBOXBPzy5Qvp6em8ePGipvypd7gk4JgxY5g+fTo9evQgISGhpnyqVzgtYElJCXfu3AHAZDIRFxdHeHi45I7wb8RpAX18fJgwYYLEduXKFXr37s3169fd7lh9waUhfObMGVavXi2xFRUVodPp2LJly18Z1eCSgJ6enmzfvp0LFy7QvHlz0W42m4mPjyc8PJyioiK3O1mXsfsm8idGjx5NXl4eUVFR4rwIlnOiVqvlxIkT6HS6P9bz4sULfv36BUDnzp3FmJPy8nLxzUWj0dC+fXtKSkrEBy0rarWa1q1bo9FobOo2Go2UlZUB0LFjRxo1aiT5vLi4mA8fPgDg7+9PQECAs82XUO19YLt27cjOzmbFihUS+8ePHxk5ciRxcXGYTKYq69i5cyfBwcEEBwdz9OhR0Z6amiraMzMzAbh+/bpos/516dIFX19fwsLCyMnJkdQ9ZcoUMd/hw4dtvjsmJkb8fNeuXdWVQd5G2svLi8TERLKysmjatKloN5vNJCQkMGLECN6+feuwfHx8PI0bNwYsYppMJgRBYO/evYDlP2n+/PlV+mA2m8nJyUGn04kP3r9T+Q0D4PPnz1y8eNGpNv4Jt5xExo4dS15eHv369ZPYs7Oz0Wq1XL582W65wMBAlixZAsDz58/Jysri2rVrPHnyBIC4uDi8vb1tyiUlJVFRUcGnT5/E3lNWViZ58K/MvXv3eP78uZg+e/Ys5eXlrjfUDm47yrVv356bN2+ybNkyib24uJhRo0axbt06u0N69erVNGvWDIDExERSUlIAy5zo6G6yQYMGeHp60qxZM2JjY/Hx8QEs897vtGrVCoD09HTRZv13y5YtXWylLW49C6vVanbt2kVmZiZNmjQR7WazmW3btjF06FDevHkjKdO0aVNxa3T79m3Onj0LwKZNm5x6Y3769Cnfvn0DwM/Pz+bzwYMHAxbRBEGgsLCQmzdvEhAQQLdu3arX0ErUyGXCxIkTycvLIzQ0VGK/desWw4YNs8m/ePFi/P39AcspJzg4mKioKIf1Z2RkYDAYiI6OJiwsDOvP/aZOnWqTt1OnTrRs2RKj0cjdu3fJyMjAbDYzefJkcRWWQ43dxnTs2JG0tDRxkbBSUFAghopYyc/P59OnT2L6w4cPYq+yR3Z2Nnv27OH48eN8+fKFBg0aEB8fz+TJk23yfv78mSlTpgCW0DTrqq7X690SkVtjAmZkZNC/f3++f/8usS9atEics6ysWbMGQRAICgoCLFuhHTt2OKx7wIABzJo1SxziWq2WDRs22M1bWlrKzJkzAcswvnPnDp07d2bQoEE2vlUHtwtYVlbGwoULmTp1Kl+/fhXt3t7epKSkiFsUK5cuXeLq1asAbN26leHDhwPwzz//8Pr1a7vfMW3aNFJTU5kzZw4ADx484OTJk3bzCoJAnz59CAkJ4e3bt5hMJmbMmIFKpcIdv/R1q4AFBQUMGDDAZjvRtWtX7t69y9y5cyV2s9nM2rVrAWjbti2RkZHigvL9+3fi4+Or/L7169eLp5C1a9dSWlrqMK+1F3p4eBAdHe1aw6rAbQKePHmS3r178/DhQ4ldr9dz//59tFqt3TJ5eXkAGAwG1Go14eHh9O3bF4C0tDSHm2OANm3asHTpUgAKCwvZuXOnw7x6vR5/f38iIiLo0KGDq81ziGwBf/z4QUxMDHq9XtIDvL29OXToEOnp6XbPquXl5cTFxQHg6+srPuirVCrWr18PWFZka/CmI1auXClebCQmJtpsk6y0adOGjx8/kpWV5Xojq6BalwlW8vPziYqKkoQBA3Tv3p1Tp07Rs2dPh2UbNmxod+MLMG7cOJv5afz48XbnLD8/P7ur6YMHD/7of1VD3lmq3QNTU1MJDQ21EW/atGnk5uZWKd7/CZd7YGlpKfPmzZMcjQAaNWrE/v37xcn6b8ElAZ88eUJkZCS//zAnODiY06dP06NHD6fqKSoqYvfu3W45CbgTHx8f5s2bR0hIiNNlnBbQZDKh0+lsIjRnz57Nvn37bC4sq2LmzJkOb2hqm/Pnz/Pq1StUKpVT+Z2eA8vKyiSTtUaj4dixYxw+fNgl8YA6/a78+vVrl04oTguo0WhITk7G39+fsLAw7t+/z/Tp06vl5I4dO8TLg7qERqMhISHB7rbLEUqUvov8HqWvxMbIRBFQJoqAMlEElIkioEwUAWWiUqvVws+fPwHLM587Xqr+zzx79kwM6VOr1aiGDRsm/M3haXLQ6XSocnJyhCFDhohBPgrO4evrS3Z2NipBEITc3FySk5N59eoV1uGsYJ+GDRsSFBTE8uXLCQkJsQhY207VZ5RVWCaKgDJRBJSJIqBMFAFloggoE0VAmSgCykQRUCaKgDL5F17CTdTuRau3AAAAAElFTkSuQmCC"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class XrmCommanderPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new XrmCommanderControl();
        }
    }
}