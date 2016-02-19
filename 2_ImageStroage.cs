/*
  Before coding, create table and Creating Stored Procedures, check this: http://www.codeproject.com/Articles/354639/Storing-and-Retrieving-Images-from-SQL-Server-Us
  will very helpful.
  
  Stored Procedures In ADO:
  
  CREATE proc [dbo].[ReadAllImage] as 
  SELECT * FROM ImageData 
  GO
  
  
  CREATE proc [dbo].[ReadAllImageIDs] as 
  SELECT ImageID FROM ImageData 
  GO
  
  CREATE proc [dbo].[ReadImage] @imgId int as 
  SELECT ImageData FROM ImageData 
  WHERE ImageID=@imgId 
  GO 
  
  CREATE proc [dbo].[SaveImage] @img image as
  INSERT INTO ImageData(ImageData)
  VALUES (@img)
  GO
*/

using System.IO;
using System.Data;
using System.Data.SqlClient;

/////////////////////SAVE IMAGE
SqlConnection con = new SqlConnection(DBHandler.GetConnectionString());
try
 {
   OpenFileDialog fop = new OpenFileDialog();
   fop.InitialDirectory = @"C:\"; 
   fop.Filter = "[JPG,JPEG]|*.jpg";
   if (fop.ShowDialog() == DialogResult.OK)
   {
     FileStream FS = new FileStream(@fop.FileName, FileMode.Open, FileAccess.Read);
     byte[] img = new byte[FS.Length];
     FS.Read(img, 0, Convert.ToInt32(FS.Length));

     if (con.State == ConnectionState.Closed)
       con.Open();
     SqlCommand cmd = new SqlCommand("SaveImage", con);
     cmd.CommandType = CommandType.StoredProcedure;
     cmd.Parameters.Add("@img", SqlDbType.Image).Value = img;
     cmd.ExecuteNonQuery();
     loadImageIDs();
     MessageBox.Show("Image Save Successfully!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
   }
   else
   {
     MessageBox.Show("Please Select a Image to save!!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
   }

 }
 catch (Exception ex)
 {
   MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
 }
 finally
 {
   if (con.State == ConnectionState.Open)
     con.Close();
 }
 
 ////////////////// READ IMAGE
 if (cmbImageID.SelectedValue != null)
{
    if (picImage.Image != null)
        picImage.Image.Dispose();

    SqlConnection con = new SqlConnection(DBHandler.GetConnectionString());
    SqlCommand cmd = new SqlCommand("ReadImage", con);
    cmd.CommandType = CommandType.StoredProcedure; 
    cmd.Parameters.Add("@imgId", SqlDbType.Int).Value = 
              Convert.ToInt32(cmbImageID.SelectedValue.ToString());
    SqlDataAdapter adp = new SqlDataAdapter(cmd);
    DataTable dt = new DataTable();
    try
    {
        if (con.State == ConnectionState.Closed)
            con.Open();
        adp.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            MemoryStream ms = new MemoryStream((byte[])dt.Rows[0]["ImageData"]);
            picImage.Image = Image.FromStream(ms);
            picImage.SizeMode = PictureBoxSizeMode.StretchImage;
            picImage.Refresh();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Error", 
              MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
        if (con.State == ConnectionState.Open)
            con.Close();
    }
}
else
{
    MessageBox.Show("Please Select a Image ID to Display!!", 
       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}

//////////////// LOAD IDS
private void loadImageIDs()
{
    #region Load Image Ids
    SqlConnection con = new SqlConnection(DBHandler.GetConnectionString());
    SqlCommand cmd = new SqlCommand("ReadAllImageIDs", con);
    cmd.CommandType = CommandType.StoredProcedure;
    SqlDataAdapter adp = new SqlDataAdapter(cmd);
    DataTable dt = new DataTable();
    try
    {
        if (con.State == ConnectionState.Closed)
            con.Open();
        adp.Fill(dt);
        if (dt.Rows.Count > 0)
        {
            cmbImageID.DataSource = dt;
            cmbImageID.ValueMember = "ImageID";
            cmbImageID.DisplayMember = "ImageID";
            cmbImageID.SelectedIndex = 0;
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    finally
    {
        if (con.State == ConnectionState.Open)
            con.Close();
    }
    #endregion
}
