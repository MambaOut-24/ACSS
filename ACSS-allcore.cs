using System;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Linq;

namespace CourseScheduleManager
{
    public partial class MainForm : Form
    {
        private string currentPdfPath;
        private string currentDbPath;
        private List<string> checkNodes;
        private Dictionary<string, bool> nodeStatus;
        private SQLiteConnection dbConnection;
        private Button[,] timeGrid;
        private List<Button> selectedCourses;
        private TextBox txtCourseNo, txtCourseName, txtModule, txtTeacher, txtCampus;
        
        public MainForm()
        {
            InitializeComponent();
            InitializeCheckNodes();
            InitializeTimeGrid();
            InitializeSearchBoxes();
            selectedCourses = new List<Button>();
            LoadExistingDatabases();
        }

        private void InitializeCheckNodes()
        {
            checkNodes = new List<string>()
            {
                "课程编号",
                "课程名称",
                "教师",
                "校区",
                "教室",
                "时间"
            };
            nodeStatus = new Dictionary<string, bool>();
        }

        private void InitializeTimeGrid()
        {
            timeGrid = new Button[13,7];
            for(int i = 0; i < 13; i++)
            {
                for(int j = 0; j < 7; j++)
                {
                    timeGrid[i,j] = new Button();
                    timeGrid[i,j].Size = new Size(50, 30);
                    timeGrid[i,j].Location = new Point(j*55, i*35);
                    timeGrid[i,j].Click += TimeGridButton_Click;
                    timeGrid[i,j].BackColor = Color.White;
                    timeGrid[i,j].Text = $"{i+1}-{j+1}";
                    this.Controls.Add(timeGrid[i,j]);
                }
            }
        }

        private void InitializeSearchBoxes()
        {
            // 初始化搜索文本框
            txtCourseNo = new TextBox() { Location = new Point(10, 450), Size = new Size(100, 20) };
            txtCourseName = new TextBox() { Location = new Point(120, 450), Size = new Size(100, 20) };
            txtModule = new TextBox() { Location = new Point(230, 450), Size = new Size(100, 20) };
            txtTeacher = new TextBox() { Location = new Point(340, 450), Size = new Size(100, 20) };
            txtCampus = new TextBox() { Location = new Point(450, 450), Size = new Size(100, 20) };

            this.Controls.AddRange(new Control[] { 
                txtCourseNo, txtCourseName, txtModule, txtTeacher, txtCampus 
            });
        }

        private string GetUniqueDbName(string pdfName)
        {
            string baseName = Path.GetFileNameWithoutExtension(pdfName);
            string dbName = baseName;
            int counter = 1;
            
            while(File.Exists($"{dbName}.db"))
            {
                dbName = $"{baseName}_{counter++}";
            }
            
            return dbName;
        }

        private void LoadExistingDatabases()
        {
            string[] dbFiles = Directory.GetFiles(".", "*.db");
            if(dbFiles.Length > 0)
            {
                currentDbPath = dbFiles[0];
                dbConnection = new SQLiteConnection($"Data Source={currentDbPath};Version=3;");
                dbConnection.Open();
            }
        }

        private void LoadPdfContent(string filePath)
        {
            using (PdfReader reader = new PdfReader(filePath))
            {
                // PDF读取和内容提取逻辑
            }
        }

        private bool ValidateFormat(string content)
        {
            foreach(string node in checkNodes)
            {
                if(!content.Contains(node))
                    return false;
            }
            return true;
        }

        private void CreateDatabase(string pdfName)
        {
            string dbName = GetUniqueDbName(pdfName);
            string connectionString = $"Data Source={dbName}.db;Version=3;";
            using(SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string createTableSql = @"
                    CREATE TABLE IF NOT EXISTS Courses (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CourseNo TEXT,
                        CourseName TEXT,
                        Module TEXT,
                        Teacher TEXT,
                        Campus TEXT,
                        TimeSlot TEXT
                    )";
                using(SQLiteCommand cmd = new SQLiteCommand(createTableSql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void FilterAndDisplayResults()
        {
            // 根据搜索条件和时间网格过滤结果
            // 显示搜索结果并创建对应按钮
        }

        private void CourseButton_DoubleClick(object sender, EventArgs e)
        {
            Button courseBtn = sender as Button;
            if(courseBtn != null)
            {
                // 处理课程按钮的双击事件
                // 更新时间网格和固定显示
            }
        }

        private void TimeGridButton_Click(object sender, EventArgs e)
        {
            Button timeBtn = sender as Button;
            if(timeBtn != null)
            {
                timeBtn.BackColor = timeBtn.BackColor == Color.White ? 
                                  Color.LightBlue : Color.White;
            }
        }
    }
}
