﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdoNet.Models;


namespace Ado.net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecoverDataController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RecoverDataController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("Recovery")]
        [HttpPost]
        public ActionResult Recovery()
        {
            SqlConnection connString;
            SqlCommand cmd;
            SqlDataAdapter adap;
            DataTable dtb;
            connString = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            try
            {
                dtb = new DataTable();
                cmd = new SqlCommand("select * from studentBackup ", connString);
                connString.Open();
                adap = new SqlDataAdapter(cmd);
                adap.Fill(dtb);
                List<Student> students = new List<Student>();
                foreach (DataRow dr in dtb.Rows)
                {

                    cmd = new SqlCommand("insert into student values ('" + dr["Student_ID"] + "','" + dr["gender"] + "','" + dr["NationalITy"] + "','" + dr["PlaceOfBirth"] + "','" + dr["StageID"] + "', '" + dr["GradeID"] + "','" + dr["SectionID"] + "' ,'" + dr["Topic"] + "' ,'" + dr["Semester"] + "' , '" + dr["Relation"] + "' , '" + dr["raisedhands"] + "','" + dr["VisITedResources"] + "','" + dr["AnnouncementsView"] + "','" + dr["Discussion"] + "', '" + dr["ParentAnsweringSurvey"] + "', '" + dr["ParentschoolSatisfaction"] + "', '" + dr["StudentAbsenceDays"] + "', '" + dr["Student_Marks"] + "', '" + dr["Class"] + "'  )", connString);
                    cmd.ExecuteNonQuery();
                    students.Add(new Student
                    {
                        StudentID = dr["Student_ID"].ToString(),

                        Gender = dr["gender"].ToString(),

                        Nationality = dr["NationalITy"].ToString(),

                        PlaceOfBirth = dr["PlaceOfBirth"].ToString(),

                        StageID = dr["StageID"].ToString(),

                        GradeID = dr["GradeID"].ToString(),

                        SectionID = dr["SectionID"].ToString(),

                        Topic = dr["Topic"].ToString(),

                        Semester = dr["Semester"].ToString(),

                        Relation = dr["Relation"].ToString(),

                        RaisedHands = Convert.ToInt32(dr["raisedhands"]),

                        VisitedResources = Convert.ToInt32(dr["VisITedResources"]),

                        AnnouncementsView = Convert.ToInt32(dr["AnnouncementsView"]),

                        Discussion = Convert.ToInt32(dr["Discussion"]),

                        ParentAnsweringSurvey = dr["ParentAnsweringSurvey"].ToString(),

                        ParentSchoolSatisfaction = dr["ParentschoolSatisfaction"].ToString(),

                        StudentAbsenceDays = dr["StudentAbsenceDays"].ToString(),

                        StudentMarks = Convert.ToInt32(dr["Student_Marks"]),

                        Class = dr["Class"].ToString()
                    });

                }
                cmd = new SqlCommand("TRUNCATE TABLE StudentBackup", connString);
                cmd.ExecuteNonQuery();              
                return Ok(students);
            }
            catch (Exception ef)
            {
                return BadRequest(ef.Message);
            }
            finally { connString.Close(); }
        }


    }

}
