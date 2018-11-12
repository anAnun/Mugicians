using Organizer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Organizer.Services
{
    public class OrganizerService : IOrganizerService
    {
        //ConfigurationManager is a class
        //it has a static property called '"ConnectionStrings".
        //we "index" into that using square brackets.
        // then we get the ConnectionString property of that

        // in C# parlance, this is a 'field'

        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]
            .ConnectionString;

        public List<OrganizerModel> GetAll()
        {
            //1 create and open a sql connection
            //2 create a sql command
            //3 execute the command which will give us a sqldatareader
            //4 we'll use the sql datareader to loop over all of the rows coming back from the query

            //1
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                //2
                var command = con.CreateCommand();
                // 'var' is exactly the same as: SqlCommand command = con.CreateCommand();

                command.CommandText = "SongNames_GetAll";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                //3
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var results = new List<OrganizerModel>();

                    while (reader.Read())
                    {
                        //this loop will happen once for every row coming out of the database
                        var song = new OrganizerModel();
                        song.Id = (int)reader["id"];
                        song.SongName = (string)reader["SongName"];
                        song.DateCreated = (DateTime)reader["DateCreated"];
                        song.DateModified = (DateTime)reader["DateModified"];
                        song.Lyrics = reader["Lyrics"]as string ;                
                        #region OTHER WAY TO DO IT
                        //this is the exact same as above but uses C# 'object initializer" syntax
                        //var car2 = new Car
                        //{
                        //Id = (int)reader["id"] etc....
                        //}
                        #endregion
                        results.Add(song);
                    }

                    return results;

                }
            }

                // no more con (thanks to the'using'
        }

        public OrganizerModel GetById(int Id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                var command = con.CreateCommand();
                command.CommandText = "SongNames_GetById";
                
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", Id); 

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var result = new OrganizerModel();
                   
                    while (reader.Read())
                    {
                        
                        result.Id = (int)reader["id"];
                        result.SongName = (string)reader["SongName"];
                        result.DateCreated = (DateTime)reader["DateCreated"];
                        result.DateModified = (DateTime)reader["DateModified"];
                        result.Lyrics = reader["Lyrics"] as string;
                    }
                    return result;
                }
            }
        }

        public int Create(OrganizerCreateModel model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SongNames_CreateUpdateDelete";

                cmd.Parameters.AddWithValue("@SongName", model.SongName);
                cmd.Parameters.AddWithValue("@Lyrics", model.Lyrics);

                cmd.Parameters.Add("@id", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@id"].Value;
                
            }
        }

        //public IHttpActionResult Get(string fileName)
        //{
        //    var model = File.ReadAllBytes(fileName);

        //    var result = new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new StreamContent(new MemoryStream(model))
        //    };
        //    result.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
        //    return ResponseMessage(result);
        //}

        //public IHttpActionResult Post(string fileName)
        //{
        //    var model = File.ReadAllBytes(fileName);
        //    var result = new HttpResponseMessage(HttpStatusCode.Ok)
        //    {
        //        Content = new StreamContent(new MemoryStream(model))
        //    };
        //    result.Content.Headers.ContentType = new MediaTypeHeaderValue("audio/wav");
        //    return HttpResponseMessage(result);
        //}

        //public OrganizerCreateFileModel GetById(int Id)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionString))
        //    {
        //        con.Open();
        //        var command = con.CreateCommand();
        //        command.CommandText = "AudioFiles_GetById";

        //        command.CommandType = System.Data.CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@SongId", SongId);

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            var result = new OrganizerCreateFileModel();

        //            while (reader.Read())
        //            {
        //                result.SongId = (int)reader["SongId"];
        //                result.AudioFile = (string)reader["AudioFile"];
        //            }
        //            return result;
        //        }
        //    }

        //}

        public int CreateAudioFile(int songId, string fileName)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "AudioFiles_Create";

                cmd.Parameters.AddWithValue("@AudioFile", fileName);
                cmd.Parameters.AddWithValue("@SongId", songId);

                cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                return (int)cmd.Parameters["@Id"].Value;
            }
        }
        public void UpdateFile(OrganizerUpdateFileModel model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "AudioFiles_Update";
                cmd.Parameters.AddWithValue("@AudioFile", model.AudioFile);
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@SongId", model.SongId);
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(OrganizerUpdateModel model)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SongNames_Update";
                cmd.Parameters.AddWithValue("@SongName", model.SongName);
                cmd.Parameters.AddWithValue("@Lyrics", model.Lyrics);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.ExecuteNonQuery();
            }
        }
        public void Delete(int Id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SongNames_Delete";
                cmd.Parameters.AddWithValue("@Id", Id);

                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteFile(int Id)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "AudioFiles_Delete";
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
            }
        }
        public List<SongFileModel> GetAllFiles(int songId)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                var command = con.CreateCommand();

                command.CommandText = "AudioFiles_Get";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@songId", songId);


                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var results = new List<SongFileModel>();

                    while (reader.Read())
                    {
                        var song = new SongFileModel();

                        song.Id = (int)reader["Id"];
                        song.AudioFile = (string)reader["AudioFile"];
                        song.SongId = (int)reader["SongId"];
                        song.Description = reader["Description"] as string ?? default(string);

                        results.Add(song);
                    }

                    return results;

                }
            }
            
        }
    }
}