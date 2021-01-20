using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteApp
{
    class DAtaAccess
    {
        public const string dbpath = "Customer.db";  //เป็นการประการไปเลยว่า dbpath คือการใช้ไฟล์ที่ชื่อ Customer.db เพื่อลดขั้นตอนการเขียนชื่อซ้ำไปซ้ำมา และป้องกันการเขียนชื่อไฟล์ผิดในบรรทัดอื่นๆ
        private static object createTable;

        public static void InitializeDatabase() //สร้างตาราง
        {
            //INTEGER   TINYINT     = 1 byte = 127
            //          SMALLINT    = 2 byte = 32767
            //          MEDIUMINT   = 3 byte = 8388607
            //          INT(11)     = 4 byte = 2147483647
            //          BIGINT      = 8 byte = 9223372036854775807
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS CustomerTable (uid INTEGER PRIMARY KEY, " +
                    "first_Name NVARCHAR(50) NULL, " +
                    "last_Name NVARCHAR(50) NULL, " +
                    "email NVARCHAR(50) NULL)";
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();

            }
        }
        public static void AddData(string firstName, string lastName,string email)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO CustomerTable VALUES (NULL, @first_Name,@last_Name,@email);";
                insertCommand.Parameters.AddWithValue("@first_Name", firstName);
                insertCommand.Parameters.AddWithValue("@last_Name", lastName);
                insertCommand.Parameters.AddWithValue("@email", email);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }
        public static List<String> GetData() //คือการแสดงผลข้อมูลที่อยู่ใน dataBase ออกมาทั้งหมด
        {
            List<String> entries = new List<string>();

            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT first_Name,last_Name,email from CustomerTable", db); //หลัง SELECT คือใส่ชื่อ field หากมีหลาย field ข้องใส่ (,) เอาไว้ด้วย แต่ถ้าอยากได้ทั้งหมดเลยก็ใส่ (*)
               

                SqliteDataReader query = selectCommand.ExecuteReader();
                
                while (query.Read())
                {
                    entries.Add($"{query.GetString(0)} {query.GetString(1)} ({query.GetString(2)})"); //มันใช้งานเหมือน ArrayList คือกำหนดให้ส่งค่ากลับออกมาเป็นแต่ละ column (field) โดยเรียงตามที่เราเรียง Opject [0][1][2]
                }

                db.Close();
            }

            return entries;
        }
    }
}
