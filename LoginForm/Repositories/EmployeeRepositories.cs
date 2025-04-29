
using ERP.Models;
using ERP.Data;
using ERP.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Data;

namespace ERP.Repositories
{
    
    public class EmployeeRepositories:IEmployeeRepositories
    {
        private readonly DbHelper _dbHelper;

        public EmployeeRepositories(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public bool Login(string Email, string Password)
        {
           

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM hrm.employees WHERE Email = @Email AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password);

                con.Open();

                int count = (int)cmd.ExecuteScalar(); // Returns the number of matching rows
                con.Close();

                return count > 0; // Login successful if count > 0
            }

        }

        // -------------  Employees -----------------------
        public bool AddDept(departments departments)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("insert into hrm.departments(DepartmentName) values(@DepartmentName)", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DepartmentName", departments.DepartmentName);
                

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }
        public IEnumerable<departments> GetDept()
        {
            var departments = new List<departments>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DispDepts", con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    departments.Add(new departments
                    {
                        DepartmentID = (int)reader["DepartmentID"],
                        DepartmentName = reader["DepartmentName"].ToString()

                    });
                }
                con.Close();
            }
            return departments;
        }
        public bool DeleteDept(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("delete from hrm.departments where DepartmentID=@id", con);
               // cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                con.Close();
                return rows > 0;
            }
        }
        public bool AddRole(roles roles)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("insert into hrm.roles(role_name) values(@role_name)", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@role_name", roles.role_name);


                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public IEnumerable<roles> GetRoles()
        {
            var roles = new List<roles>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DispRoles", con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    roles.Add(new roles
                    {
                        role_id = (int)reader["role_id"],
                        role_name = reader["role_name"].ToString()

                    });
                }
                con.Close();
            }
            return roles;
        }

        public bool DeleteRole(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("delete from hrm.roles where role_id=@id", con);
                // cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                con.Close();
                return rows > 0;
            }
        }

        public employees GetEmployeesById(int id)
        {
            employees employees = null;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_GetEmpById", con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    employees = new employees
                    {
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        Name = reader["Name"].ToString(),
                        Password = reader["Password"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        role_id = Convert.ToInt32(reader["role_id"]),
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        Salary = Convert.ToDecimal(reader["Salary"]),
                        Hobbies = reader["Hobbies"] != DBNull.Value ? reader["Hobbies"].ToString().Split(',').ToList() : new List<string>(),
                        profileimg = reader["profileimg"] as byte[]
                    };
                }
                con.Close();
            }

            return employees;
        }

        public employees getSessionData(string Email)
        {
            employees employee = null;

            string query = "SELECT * FROM hrm.employees WHERE Email = @Email";

            using (SqlConnection connection = _dbHelper.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", Email);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read(); // Assuming email is unique, we can use Read() to get the first (and only) record
                    employee = new employees
                    {
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        role_id =Convert.ToInt32(reader["role_id"]),
                        EmployeeID =Convert.ToInt32(reader["EmployeeID"]),
                        Name = reader["Name"].ToString()


                    };
                }

                connection.Close();
            }

            return employee; // Returns null if no employee is found
        }
        public bool AddEmp(employees employees, string selectedHobbies, byte[] profileimg)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_AddEmp", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", employees.Name);
                cmd.Parameters.AddWithValue("@Password", employees.Password);
                cmd.Parameters.AddWithValue("@Email", employees.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", employees.PhoneNumber);
                cmd.Parameters.AddWithValue("@role_id", employees.role_id);
                cmd.Parameters.AddWithValue("@DepartmentID", employees.DepartmentID);
                cmd.Parameters.AddWithValue("@Salary", employees.Salary);
                cmd.Parameters.AddWithValue("@Hobbies", (object)selectedHobbies ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@profileimg", profileimg ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@contenttype", employees.contenttype);


                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public List<employees> GetAllEmployees()
        {
            List<employees> employees = new List<employees>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_DispEmp", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employees.Add(
                        new employees
                        {
                            EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                            Name = reader["Name"].ToString(),
                            Password = reader["Password"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            role_id = Convert.ToInt32(reader["role_id"]),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            Salary = Convert.ToDecimal(reader["Salary"]),
                            Hobbies = reader["Hobbies"] != DBNull.Value ? reader["Hobbies"].ToString().Split(',').ToList() : new List<string>(),
                            profileimg = reader["profileimg"] as byte[]

                        });
                }
            }
            return employees;
        }
       
        public bool DeleteEmp(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteEmp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                con.Close();
                return rows > 0;
            }
        }

        public bool UpdateEmp(employees employees, string selectedHobbies, byte[] profileimg)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                string query = profileimg != null ? "sp_UpdateEmp" : "sp_UpdateEmp1";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", employees.Name);
                cmd.Parameters.AddWithValue("@Password", employees.Password);
                cmd.Parameters.AddWithValue("@Email", employees.Email);
                cmd.Parameters.AddWithValue("@PhoneNumber", employees.PhoneNumber);
                cmd.Parameters.AddWithValue("@role_id", employees.role_id);
                cmd.Parameters.AddWithValue("@DepartmentID", employees.DepartmentID);
                cmd.Parameters.AddWithValue("@Salary", employees.Salary);
                cmd.Parameters.AddWithValue("@selectedHobbies", selectedHobbies);


                cmd.Parameters.AddWithValue("@id", employees.EmployeeID);

                if (profileimg != null)
                {
                    cmd.Parameters.AddWithValue("@profileimg", profileimg ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@contenttype", employees.contenttype);
                }
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }


        // ------------- Emp Services  ----------------

        public bool AddFaq(FAQ Faq)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("insert into hrm.FAQ (Questions,Answers) values(@Questions,@Answers)", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Questions", Faq.Questions);
                cmd.Parameters.AddWithValue("@Answers", Faq.Answers);
                
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public List<FAQ> DispFaq()
        {
            List<FAQ> Faq = new List<FAQ>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from hrm.FAQ", con);
              //  cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Faq.Add(
                        new FAQ
                        {
                            Questions = reader["Questions"].ToString(),
                            Answers = reader["Answers"].ToString()
                        });
                }
            }
            return Faq;
        }

        public bool AddLeaveRequests(leave_requests leave_Requests)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("insert into hrm.leave_requests (EmployeeID,LeaveType,StartDate,EndDate) values (@EmployeeID,@LeaveType,@StartDate,@EndDate)", con);
                // cmd.CommandType = CommandType.StoredProcedure;

                DateTime minSqlDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
               /* DateTime appliedAt = leave_Requests.Applied_at < minSqlDate ? minSqlDate : leave_Requests.Applied_at;*/
                DateTime startDate = leave_Requests.StartDate < minSqlDate ? minSqlDate : leave_Requests.StartDate;
                DateTime endDate = leave_Requests.EndDate < minSqlDate ? minSqlDate : leave_Requests.EndDate;

                cmd.Parameters.AddWithValue("@EmployeeID",leave_Requests.EmployeeID);
                cmd.Parameters.AddWithValue("@LeaveType", leave_Requests.LeaveType);
                cmd.Parameters.AddWithValue("@StartDate", leave_Requests.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", leave_Requests.EndDate);

                //cmd.Parameters.AddWithValue("@Applied_at",DateTime.Now);


                con.Open();
                
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public List<leave_requests> DisplayLeaveRequests()
        {
            List<leave_requests> leave_Requests = new List<leave_requests>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from hrm.leave_requests where Status='pending' ", con);
                //  cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    leave_Requests.Add(
                        new leave_requests
                        {
                            LeaveID =Convert.ToInt32(reader["LeaveID"]),
                            EmployeeID =Convert.ToInt32(reader["EmployeeID"]),
                            LeaveType = reader["LeaveType"].ToString(),
                            StartDate =Convert.ToDateTime(reader["StartDate"]),
                            EndDate =Convert.ToDateTime(reader["EndDate"]),
                            Status = reader["Status"].ToString(),
                            Applied_at = Convert.ToDateTime(reader["Applied_at"]),

                        });
                }
            }
            return leave_Requests;
        }

        public bool UpdateLeaveRequests(leave_requests leave_Requests)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
              
                SqlCommand cmd = new SqlCommand("Update hrm.leave_requests set Status=@Status where EmployeeID=@EmployeeID And LeaveID=@LeaveID ", con);
                //cmd.CommandType = CommandType.StoredProcedure;

               
                cmd.Parameters.AddWithValue("@EmployeeID", leave_Requests.EmployeeID);
                cmd.Parameters.AddWithValue("@LeaveID", leave_Requests.LeaveID);
                cmd.Parameters.AddWithValue("@Status",leave_Requests.Status);

                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public List<feedback> DisplayFeedback()
        {
            List<feedback> feedbacks = new List<feedback>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from hrm.feedback", con);
                //  cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    feedbacks.Add(
                        new feedback
                        {
                            feedback_id = Convert.ToInt32(reader["feedback_id"]),
                            customer_id = Convert.ToInt32(reader["customer_id"]),
                            productid = Convert.ToInt32(reader["productid"]),
                            rating = Convert.ToDecimal(reader["rating"]),
                            comments = reader["comments"].ToString(),
                            submitted_at = Convert.ToDateTime(reader["submitted_at"])
                           
                        });
                }
            }
            return feedbacks;
        }


        public employee_attendence GetTodayAttendence(int EmployeeID)
        {
            employee_attendence employee_Attendence = null;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from hrm.employee_attendence where EmployeeID=@EmployeeID and Date=@Date", con);

               // cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    employee_Attendence = new employee_attendence
                    {
                        AttendenceID = Convert.ToInt32(reader["AttendenceID"]),
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        Date = Convert.ToDateTime(reader["Date"]),
                        Status = reader["Status"].ToString(),
                        ShiftStart = reader["ShiftStart"] != DBNull.Value
                             ? Convert.ToDateTime(reader["ShiftStart"]) : DateTime.MinValue,
                        ShiftEnd = reader["ShiftEnd"] != DBNull.Value
                           ? Convert.ToDateTime(reader["ShiftEnd"]) : DateTime.MinValue,

                    };
                }
                con.Close();
            }

            return employee_Attendence;
        }
        public bool PunchIN(employee_attendence employee_Attendence)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {

                con.Open();


                SqlCommand cmd = new SqlCommand("Update hrm.employee_attendence set Status=@Status,ShiftStart=@ShiftStart where EmployeeID=@EmployeeID and Date=@Date ", con);
                //cmd.CommandType = CommandType.StoredProcedure;

               
                    cmd.Parameters.AddWithValue("@EmployeeID", employee_Attendence.EmployeeID);
                      cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@Status", "punch in");
                    cmd.Parameters.AddWithValue("@ShiftStart", DateTime.Now);
               


                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public bool PunchOUT(employee_attendence employee_Attendence)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {

                con.Open();


                SqlCommand cmd = new SqlCommand( "Update hrm.employee_attendence set Status=@Status,ShiftEnd=@ShiftEnd where EmployeeID=@EmployeeID And Date=@Date ", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@EmployeeID", employee_Attendence.EmployeeID);
                      cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                    cmd.Parameters.AddWithValue("@Status", "punch out");
                    cmd.Parameters.AddWithValue("@ShiftEnd", DateTime.Now);
                
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public List<employee_attendence> PunchHistory(int EmployeeID)
        {
            List<employee_attendence> employee_Attendences = new List<employee_attendence>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from hrm.employee_attendence where EmployeeID=@EmployeeID ", con);
                 // cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);
            //    cmd.Parameters.AddWithValue("@Date", DateTime.Now.Date);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    employee_Attendences.Add(
                        new employee_attendence
                        {
                            Date = Convert.ToDateTime(reader["Date"]),
                            Status = reader["Status"].ToString(),
                            ShiftStart = reader["ShiftStart"] != DBNull.Value ? Convert.ToDateTime(reader["ShiftStart"]) : (DateTime?)null,
                            ShiftEnd = reader["ShiftEnd"] != DBNull.Value ? Convert.ToDateTime(reader["ShiftEnd"]) : (DateTime?)null

                        });
                }
            }
            return employee_Attendences;
        }

      
        public bool AddCampaign(campaign camp)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("insert into crm.campaign(campaign_name,campaign_type,start_date,end_date,campaignIMG,discount) values (@campaign_name,@campaign_type,@start_date,@end_date,@campaignIMG,@discount)", con);
               // cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@campaign_name", camp.campaign_name);
                cmd.Parameters.AddWithValue("@campaign_type", camp.campaign_type);
                cmd.Parameters.AddWithValue("@start_date", camp.start_date);
                cmd.Parameters.AddWithValue("@end_date", camp.end_date);
               cmd.Parameters.AddWithValue("@campaignIMG", camp.campaignIMG);
               cmd.Parameters.AddWithValue("@discount", camp.discount);
              // cmd.Parameters.AddWithValue("@status", camp.status);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        public List<campaign> DispCampaign()
        {
            List<campaign> campaigns = new List<campaign>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from crm.campaign", con);
                //cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    campaigns.Add(
                        new campaign
                        {
                            campaign_id = Convert.ToInt32(reader["campaign_id"]),
                            campaign_name = reader["campaign_name"].ToString(),
                            campaign_type = reader["campaign_type"].ToString(),
                            start_date = Convert.ToDateTime(reader["start_date"]),
                            end_date = Convert.ToDateTime(reader["end_date"]),
                            campaignIMG = reader["campaignIMG"].ToString(),
                            discount = Convert.ToDecimal(reader["discount"]),
                            status = reader["status"].ToString(),

                        });
                }
            }
            return campaigns;
        }

        public bool EditCampaignStatus(int campaign_id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("update crm.campaign SET status = CASE \r\n                WHEN status = 'Active' THEN 'Deactive'\r\n                ELSE 'Active'\r\n             END where campaign_id=@campaign_id", con);
                cmd.Parameters.AddWithValue("@campaign_id", campaign_id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        // Handloom

        public bool AddHandloom(Handloom handloom)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("insert into inventory.Handloom(handloomname,LOC) values (@handloomName,@LOC)", con);
              //  cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@handloomName", handloom.handloomname);
                cmd.Parameters.AddWithValue("@LOC", handloom.LOC);
               
                con.Open();
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }
        public Handloom GetHandloomByID(int id)
        {
            Handloom handloom = null;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from inventory.Handloom where handloomID=@id", con);

             //  cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    handloom = new Handloom
                    {
                        handloomID = Convert.ToInt32(reader["handloomID"]),
                        handloomname = reader["handloomname"].ToString(),
                        LOC = reader["LOC"].ToString(),
                       };
                }
                con.Close();
            }

            return handloom;
        }

        public List<Handloom> GetAllHandloom()
        {
            List<Handloom> handlooms = new List<Handloom>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from inventory.Handloom", con);
               // cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    handlooms.Add(
                        new Handloom
                        {
                            handloomID = Convert.ToInt32(reader["handloomID"]),
                            handloomname = reader["handloomname"].ToString(),
                            LOC = reader["LOC"].ToString(),
                          
                        });
                }
            }
            return handlooms;
        }

        public bool DeleteHandloom(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("delete from inventory.Handloom where handloomID=@id", con);
              //  cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                con.Close();
                return rows > 0;
            }
        }

        public bool UpdateHandloom(Handloom handloom)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
               
                SqlCommand cmd = new SqlCommand("update inventory.Handloom set handloomname=@handloomname,LOC=@LOC where handloomID=@id", con);
               // cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@handloomname", handloom.handloomname);
                cmd.Parameters.AddWithValue("@LOC", handloom.LOC);
              
                cmd.Parameters.AddWithValue("@id", handloom.handloomID);

               
                int rows = cmd.ExecuteNonQuery();
                con.Close();

                return rows > 0;
            }
        }

        // Customers
        public List<Customer> DispCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from crm.customers", con);
                // cmd.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    customers.Add(
                        new Customer
                        {
                            customer_id = Convert.ToInt32(reader["customer_id"]),
                            firstName = reader["firstName"].ToString(),
                            lastName = reader["lastName"].ToString(),
                            email = reader["email"].ToString(),
                            phone = reader["phone"].ToString(),
                            password = reader["password"].ToString(),
                           


                        });
                }
            }
            return customers;
        }

        public bool DeleteCustomers(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("delete from crm.customers where customer_id=@id", con);
                //  cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                int rows = cmd.ExecuteNonQuery();

                con.Close();
                return rows > 0;
            }
        }

        public List<PayableSalaryViewModel> GetMonthlyPayrollData()
        {
            List<PayableSalaryViewModel> list = new List<PayableSalaryViewModel>();

            DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-1);
            DateTime toDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddDays(-1);
            int totalDays = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
            DateTime payDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 5);
            //DateTime payDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"
            SELECT e.EmployeeID, e.Name, e.Salary,
                (SELECT COUNT(*) FROM hrm.employee_attendence a 
                 WHERE a.EmployeeID = e.EmployeeID 
                 AND a.Date BETWEEN @fromDate AND @toDate 
                 AND a.Status = 'punch out') AS PresentDays,
                (SELECT COUNT(*) FROM hrm.payroll p 
                 WHERE p.EmployeeID = e.EmployeeID 
                 AND p.PayDate = @payDate) AS PaidFlag
            FROM hrm.employees e", con);

                cmd.Parameters.AddWithValue("@fromDate", fromDate);
                cmd.Parameters.AddWithValue("@toDate", toDate);
                cmd.Parameters.AddWithValue("@payDate", payDate);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int presentDays = Convert.ToInt32(reader["PresentDays"]);
                    decimal salary = Convert.ToDecimal(reader["Salary"]);
                    decimal payable = Math.Round((salary * presentDays) / totalDays, 2);

                    list.Add(new PayableSalaryViewModel
                    {
                        EmployeeID = Convert.ToInt32(reader["EmployeeID"]),
                        Name = reader["Name"].ToString(),
                        MonthlySalary = salary,
                        PresentDays = presentDays,
                        TotalDaysInMonth = totalDays,
                        PayableAmount = payable,
                        IsPaid = Convert.ToInt32(reader["PaidFlag"]) > 0
                    });
                }
            }

            return list;
        }

        public void PayEmployeeSalary(int employeeId, decimal amount)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand(@"INSERT INTO hrm.payroll(EmployeeID, Amount, PayDate, PayType)
                                          VALUES(@empId, @amount, @payDate, @payType)", con);

                cmd.Parameters.AddWithValue("@empId", employeeId);
                cmd.Parameters.AddWithValue("@amount", amount);
                cmd.Parameters.AddWithValue("@payDate", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 5));
                cmd.Parameters.AddWithValue("@payType", "Manual");

                cmd.ExecuteNonQuery();
            }
        }

        public void PayAllSalaries()
        {
            var list = GetMonthlyPayrollData();
            foreach (var emp in list.Where(x => !x.IsPaid))
            {
                PayEmployeeSalary(emp.EmployeeID, emp.PayableAmount);
            }
        }
    }
}
