using HMA2.Dtos;
using HMA2.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebMatrix.Data;

namespace HMA2.Controllers
{
    public class VersionController : Controller
    {
        #region Utilities

        private bool CreateFiles(string filePath, string fileName, string fileBase64String)
        {
            try
            {
                string fullPath = Server.MapPath(filePath);
                if (!Directory.Exists(fullPath.Replace(fileName, "").Replace("/", "")))
                {
                    Directory.CreateDirectory(fullPath.Replace(fileName, "").Replace("/", ""));
                }

                using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs))
                    {
                        var base64string = fileBase64String.Split(',')[1];
                        var fileBinary = Convert.FromBase64String(base64string);

                        bw.Write(fileBinary);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetVersions(int versionType)
        {
            SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.ToString());
            connection.Open();

            List<VersionDto> versions = new List<VersionDto>();

            string sqlString =
                "SELECT v.[Id], v.[VersionTypeId], v.[VersionName], vt.[VersionTypeName], f.[FileName], f.[FilePath], vfm.[FileId], v.[Memo], v.[CreatedOn] " +
                "FROM dbo.[Version] as v " +
                "INNER JOIN dbo.[VersionType] as vt on vt.Id = v.[VersionTypeId] " +
                "LEFT JOIN dbo.[Version_File_Mapping] as vfm on vfm.[VersionId] = v.[Id] " +
                "INNER JOIN dbo.[File] as f on f.[Id] = vfm.[FileId] " +
                "WHERE v.[VersionTypeId] = " + versionType + " " + 
                "ORDER BY v.[VersionName] DESC";

            SqlCommand command1 = new SqlCommand(sqlString, connection);
            command1.CommandType = CommandType.Text; ;
            SqlDataReader reader1 = command1.ExecuteReader();

            while (reader1.Read())
            {
                VersionDto version = new VersionDto();
                version.Id = Convert.ToInt32(reader1["Id"].ToString());
                version.VersionTypeId = Convert.ToInt32(reader1["VersionTypeId"].ToString());
                version.VersionName = reader1["VersionName"].ToString();
                version.VersionTypeName = reader1["VersionTypeName"].ToString();
                version.FileName = reader1["FileName"].ToString();
                version.FilePath = reader1["FilePath"].ToString();
                version.FileId = Convert.ToInt32(reader1["FileId"].ToString());
                version.Memo = reader1["Memo"].ToString();
                version.CreatedOn = Convert.ToDateTime(reader1["CreatedOn"].ToString());
                versions.Add(version);
            }

            connection.Close();

            return Json(versions);
        }

        [HttpPost]
        public JsonResult SetVersion(string fileName, string filePath, string fileExtension, int versionTypeId, string versionName, string memo, string fileBase64String)
        {
            try
            {
                var db = Database.Open("DefaultConnection");

                if (!CreateFiles(filePath, fileName, fileBase64String))
                    return Json(false);

                int fileId = 0;
                int versionId = 0;

                var fileSqlString = string.Format(
                    "INSERT INTO dbo.[File] " +
                    "(FileName, FilePath, FileExtension, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) " +
                    "VALUES " +
                    "('{0}', '{1}', '{2}', {3}, {4}, '{5}', {6}, '{7}')",
                    fileName, filePath, fileExtension,
                    1, (int)Users.Admin, DateTime.Now, (int)Users.Admin, DateTime.Now);

                db.Execute(fileSqlString);
                fileId = Convert.ToInt32(db.GetLastInsertId());

                var versionSqlString = string.Format(
                    "INSERT INTO dbo.[Version] " +
                    "(VersionTypeId, VersionName, Memo, IsActive, CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) " +
                    "VALUES " +
                    "('{0}', '{1}', '{2}', {3}, {4}, '{5}', {6}, '{7}')",
                    versionTypeId, versionName, memo,
                    1, (int)Users.Admin, DateTime.Now, (int)Users.Admin, DateTime.Now);

                db.Execute(versionSqlString);
                versionId = Convert.ToInt32(db.GetLastInsertId());

                var mappingSqlString = string.Format(
                    "INSERT INTO dbo.[Version_File_Mapping] " +
                    "(VersionId, FileId) " +
                    "VALUES " +
                    "({0}, {1})", versionId, fileId);

                db.Execute(mappingSqlString);
                db.Close();

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpDelete]
        public JsonResult DeleteVersion(int versionId, int fileId)
        {
            try
            {
                var db = Database.Open("DefaultConnection");

                var mappingSqlString =
                    "DELETE dbo.[Version_File_Mapping] " +
                    "WHERE [VersionId] = " + versionId + " " +
                    "AND [FileId] = " + fileId;

                var fileSqlString =
                    "UPDATE dbo.[File] " +
                    "SET [IsActive] = 0 " +
                    "WHERE [Id] = " + fileId;

                var versionSqlString =
                    "UPDATE dbo.[Version] " +
                    "SET [IsActive] = 0 " +
                    "WHERE [Id] = " + versionId;

                db.Execute(mappingSqlString);
                db.Execute(fileSqlString);
                db.Execute(versionSqlString);
                db.Close();

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files)
        {
            return Content("");
        }

        public ActionResult Remove(string[] fileNames)
        {
            return Content("");
        }

        #endregion
    }
}