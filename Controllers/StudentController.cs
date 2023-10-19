﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using AdoNet.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Collections.Specialized.BitVector32;
using CsvHelper.Configuration;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using AdoNet.Models;

namespace AdoNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _configuration;



        public StudentController(IConfiguration configuration)

        {

            _configuration = configuration;

        }
        SqlConnection connString;
        SqlCommand cmd;
        SqlDataAdapter adap;
        DataTable dtb;



        [Route("GetAllStudents")]

        [HttpGet]

        public async Task<IActionResult> GetAllStudents()

        {

            DataTable dt = new DataTable();

            SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

            SqlCommand cmd = new SqlCommand("SELECT * FROM student", con);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(dt);



            List<Student> lstprofiles = new List<Student>();

            foreach (DataRow dr in dt.Rows)

            {

                lstprofiles.Add(new Student

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

            return Ok(lstprofiles);

        }



        [Route("CreateStudent")]

        [HttpPost]

        public async Task<IActionResult> CreateStudent(Student obj)

        {

            try

            {

                SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

                SqlCommand cmd = new SqlCommand("insert into student values ('" + obj.StudentID + "','" + obj.Gender + "','" + obj.Nationality + "','" + obj.PlaceOfBirth + "','" + obj.StageID + "', '" + obj.GradeID + "','" + obj.SectionID + "' ,'" + obj.Topic + "' ,'" + obj.Semester + "' , '" + obj.Relation + "' , '" + obj.RaisedHands + "','" + obj.VisitedResources + "','" + obj.AnnouncementsView + "','" + obj.Discussion + "', '" + obj.ParentAnsweringSurvey + "', '" + obj.ParentSchoolSatisfaction + "', '" + obj.StudentAbsenceDays + "', '" + obj.StudentMarks + "', '" + obj.Class + "' )", con);

                con.Open();

                cmd.ExecuteNonQuery();

                con.Close();

                return Ok("Success");

            }

            catch (Exception ex)

            {

                throw ex;

            }

        }

        [Route("FindById")] //GET
        [HttpGet]
        public async Task<IActionResult> FindById(string id)
        {
            connString = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            dtb = new DataTable();
            cmd = new SqlCommand("select * from student where Student_ID=@StudentID", connString);
            cmd.Parameters.AddWithValue("@StudentID ", id);
            try
            {
                connString.Open();
                adap = new SqlDataAdapter(cmd);
                adap.Fill(dtb);
                DataRow dr = dtb.Rows[0];
                Student lstprofile = new Student

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

                };

                if (dtb.Rows.Count > 0)
                {
                    return Ok(lstprofile);
                }
                else
                    return Ok("Not Found!");
            }
            catch (Exception ef)
            {
                return Ok(ef.Message);
            }
            finally { connString.Close(); }
        }


        [Route("Delete/{id}")]
        [HttpDelete] //DELETE
        public ActionResult Delete(string id)
        {
            try
            {
                connString = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                cmd = new SqlCommand("delete from student where Student_ID=@StudentID" + "", connString);
                cmd.Parameters.AddWithValue("@StudentID", id);
                connString.Open();
                int x = cmd.ExecuteNonQuery();
                if (x > 0)
                {
                    return Ok(new { Message = "Record Deleted!" });
                }
                return BadRequest(new { Message = "Record Not found!" });
            }
            catch (Exception ef)
            {
                return BadRequest(ef.Message);
            }
            finally { connString.Close(); }
        }

        [HttpPut]

        [Route("UpdateStudent/{id}")]

        public async Task<IActionResult> UpdateStudent(string id, [FromBody] Student updatedStudent)
        {

            if (updatedStudent == null || string.IsNullOrWhiteSpace(id))

            {

                return BadRequest("Invalid data or ID.");

            }



            connString = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));



            try

            {

                connString.Open();



                // Use a parameterized query to update the student's record

                string query = "UPDATE Student SET " +
                    "Student_ID = @StudentID, " +
                    "Gender = @Gender, " +
                    "Nationality = @Nationality, " +
                    "PlaceOfBirth = @PlaceOfBirth, " +
                    "StageID = @StageID, " +
                    "GradeID = @GradeID, " +
                    "SectionID = @SectionID, " +
                    "Topic = @Topic, " +
                    "Semester = @Semester, " +
                    "Relation = @Relation, " +
                    "RaisedHands = @RaisedHands, " +
                    "VisitedResources = @VisitedResources, " +
                    "AnnouncementsView = @AnnouncementsView, " +
                    "Discussion = @Discussion, " +
                    "ParentAnsweringSurvey = @ParentAnsweringSurvey, " +
                    "ParentSchoolSatisfaction = @ParentSchoolSatisfaction, " +
                    "StudentAbsenceDays = @StudentAbsenceDays, " +
                    "Student_Marks = @StudentMarks, " +
                    "Class = @Class " +
                    "WHERE Student_ID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, connString);
                cmd.Parameters.AddWithValue("@StudentID", id);
                cmd.Parameters.AddWithValue("@Gender", updatedStudent.Gender);
                cmd.Parameters.AddWithValue("@Nationality", updatedStudent.Nationality);
                cmd.Parameters.AddWithValue("@PlaceOfBirth", updatedStudent.PlaceOfBirth);
                cmd.Parameters.AddWithValue("@StageID", updatedStudent.StageID);
                cmd.Parameters.AddWithValue("@GradeID", updatedStudent.GradeID);
                cmd.Parameters.AddWithValue("@SectionID", updatedStudent.SectionID);
                cmd.Parameters.AddWithValue("@Topic", updatedStudent.Topic);
                cmd.Parameters.AddWithValue("@Semester", updatedStudent.Semester);
                cmd.Parameters.AddWithValue("@Relation", updatedStudent.Relation);
                cmd.Parameters.AddWithValue("@RaisedHands", updatedStudent.RaisedHands);
                cmd.Parameters.AddWithValue("@VisitedResources", updatedStudent.VisitedResources);
                cmd.Parameters.AddWithValue("@AnnouncementsView", updatedStudent.AnnouncementsView);
                cmd.Parameters.AddWithValue("@Discussion", updatedStudent.Discussion);
                cmd.Parameters.AddWithValue("@ParentAnsweringSurvey", updatedStudent.ParentAnsweringSurvey);
                cmd.Parameters.AddWithValue("@ParentSchoolSatisfaction", updatedStudent.ParentSchoolSatisfaction);
                cmd.Parameters.AddWithValue("@StudentAbsenceDays", updatedStudent.StudentAbsenceDays);
                cmd.Parameters.AddWithValue("@StudentMarks", updatedStudent.StudentMarks);
                cmd.Parameters.AddWithValue("@Class", updatedStudent.Class);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok("Student updated successfully.");
                }
                else
                {
                    return NotFound("Student not found.");
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Internal server error: " + ex.Message);

            }

            finally

            {

                connString.Close();

            }

        }//update action
        [Route("ImportCsv")]
        [HttpPost]
        public IActionResult ImportCsv(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("No file uploaded.");
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                IEnumerable<Student> csvData = csv.GetRecords<Student>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var csvStudent in csvData)
                    {
                        if (!IsDuplicateStudent(connection, csvStudent.StudentID))
                        {
                            InsertCsvStudent(connection, csvStudent);
                        }
                    }

                    connection.Close();
                }
            }

            return Ok("CSV data imported successfully.");
        }
        private bool IsDuplicateStudent(SqlConnection connection, string studentId)
        {
            using (var command = new SqlCommand("SELECT COUNT(*) FROM student WHERE Student_ID = @Student_ID", connection))
            {
                command.Parameters.AddWithValue("@Student_ID", studentId);
                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

        private void InsertCsvStudent(SqlConnection connection, Student csvStudent)
        {
            using (var command = new SqlCommand("INSERT INTO student VALUES (@Student_ID, @Gender, @NationalITy, @PlaceofBirth, @StageID, @GradeID, @SectionID, @Topic, @Semester, @Relation, @raisedhands, @VisITedResources, @AnnouncementsView, @Discussion, @ParentAnsweringSurvey, @ParentschoolSatisfaction, @StudentAbsenceDays, @Student_Marks, @Class)", connection))
            {
                command.Parameters.AddWithValue("@Student_ID", csvStudent.StudentID);
                command.Parameters.AddWithValue("@Gender", csvStudent.Gender);
                command.Parameters.AddWithValue("@NationalITy", csvStudent.Nationality);
                command.Parameters.AddWithValue("@PlaceofBirth", csvStudent.PlaceOfBirth);
                command.Parameters.AddWithValue("@StageID", csvStudent.StageID);
                command.Parameters.AddWithValue("@GradeID", csvStudent.GradeID);
                command.Parameters.AddWithValue("@SectionID", csvStudent.SectionID);
                command.Parameters.AddWithValue("@Topic", csvStudent.Topic);
                command.Parameters.AddWithValue("@Semester", csvStudent.Semester);
                command.Parameters.AddWithValue("@Relation", csvStudent.Relation);
                command.Parameters.AddWithValue("@raisedhands", csvStudent.RaisedHands);
                command.Parameters.AddWithValue("@VisITedResources", csvStudent.VisitedResources);
                command.Parameters.AddWithValue("@AnnouncementsView", csvStudent.AnnouncementsView);
                command.Parameters.AddWithValue("@Discussion", csvStudent.Discussion);
                command.Parameters.AddWithValue("@ParentAnsweringSurvey", csvStudent.ParentAnsweringSurvey);
                command.Parameters.AddWithValue("@ParentschoolSatisfaction", csvStudent.ParentSchoolSatisfaction);
                command.Parameters.AddWithValue("@StudentAbsenceDays", csvStudent.StudentAbsenceDays);
                command.Parameters.AddWithValue("@Student_Marks", csvStudent.StudentMarks);
                command.Parameters.AddWithValue("@Class", csvStudent.Class);
                command.ExecuteNonQuery();
            }
        }














        //-------------------------------------------------------------------------------->

    }//controller class
}//Namespace

