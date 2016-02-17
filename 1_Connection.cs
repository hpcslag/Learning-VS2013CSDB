using System;
using System.Data;
using System.Data.SqlClient;

/*
  before using mdf file, you need create database in your project:
  
  1. Create a Windows Forms project that's named SampleDatabaseWalkthrough.
  2. See Creating Solutions and Projects.
  3. On the menu bar, choose Project, Add New Item.
  4. The Add New Item dialog box appears so that you can add items that are appropriate in a Windows Form project.
  5. In the list of item templates, scroll down until Service-based Database appears, and then choose it.
  6. Item Templates dialog box
  7. Name the database SampleDatabase, and then choose the Add button.
  8. If the Data Sources window isn't open, open it by choosing the Shift-Alt-D keys or, on the menu bar, choosing View, Other Windows, Data Sources.
  9. In the Data Sources window, choose the Add New Data Source link.
  10.In the Data Source Configuration Wizard, choose the Next button four times to accept the default settings, and then choose the Finish button.
  
  Reference here: https://msdn.microsoft.com/en-us/library/ms233763.aspx
*/

class Program
{
    static System.Data.SqlClient.SqlConnection con;
    static void Main()
    {
        con = new System.Data.SqlClient.SqlConnection();
        con.ConnectionString = @"Data Source=.\SQLEXPRESS;
                          AttachDbFilename=c:\to\your\path\Database1.mdf;
                          Integrated Security=True;
                          Connect Timeout=30;
                          User Instance=True"; 
        con.Open();
        System.Console.WriteLine("Connection opened");
        con.Close();
        System.Console.WriteLine("Connection closed");
        System.Console.ReadLine();
    }
}
