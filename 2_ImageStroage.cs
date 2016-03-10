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
  Image myImage = Image.FromFile(/*Give file path*/ filePath);
  System.IO.MemoryStream imgMemoryStream = new System.IO.MemoryStream();
  myImage.save(imgMemoryStream,System.DrawingImaging.ImageFormat.Jpeg);
  byte[] imgByteData = imgMemoryStream.GetBuffer();
  
  cmd.CommandText = "UPDATE Images Set image = @myPicture WHERE id = 1";
  cmd.Parameters.Add("@myPicture",SqlType.Image).Value = imgByteData;
  //or cmd.Parameters.Add("@myPicture",SqlDbType.Binary).Value = imgByteData;
  cmd.Prepare();
  cmd.ExcuteNonQuery();
  MessageBox.Show("IMAGE SAVED IN DATABASE");
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
