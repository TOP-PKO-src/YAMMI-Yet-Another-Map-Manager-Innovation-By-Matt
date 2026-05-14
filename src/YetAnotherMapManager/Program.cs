using System;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager;

internal static class Program
{
  private static string version = "V0.6.2";

  [STAThread]
  private static void Main()
  {
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    Form mainForm = (Form) new MainForm();
    Form form = mainForm;
    form.Text = $"{form.Text} [{Program.version}]";
    Application.Run(mainForm);
  }
}
