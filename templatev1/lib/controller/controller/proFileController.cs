using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.controller
{
    public class proFileController : abstractController
    {
        private new Database _db;

        private string sqlStr;
        private string accountType, UID;
        private DateTime dateOfBirth, createDate;

        private string jobTitle,
            dept,
            email,
            fName,
            lName,
            sex,
            phone,
            payment,
            caddress,
            dfwaddress,
            waddress1,
            waddress2,
            corp,
            city,
            IsLM,
            status,
            province;

        private bool NGDateOfBirth;
        private int dfadd;

        AccountController accountController;

        //For department manager view or modify the user account.
        public proFileController(string uid, string type, Database db = null)
        {
            _db = db ?? new Database();
            UID = uid;
            accountType = type;
            UserInfo();
            Log.LogMessage(LogLevel.Debug, "proFileController", $"UserInfo: UserID = {UID}");
        }

        public proFileController(AccountController accountController, Database db = null)
        {
            this.accountController = accountController;
            _db = db ?? new Database();
            UID = accountController.GetUid();
            Log.LogMessage(LogLevel.Debug, "proFileController", $"UserInfo: UserID = {UID}");
        }


        //Get the default address value for customer user from the database and set the value.
        private void GetDfAdd()
        {
            DataTable dt = new DataTable();
            string query = "SELECT dfadd FROM customer_dfadd WHERE customerID = @customerID";
            dt = _db.ExecuteDataTable(query, new Dictionary<string, object> { { "@customerID", UID } });
            Log.LogMessage(LogLevel.Debug, "proFileController", $"GetDfAdd: DFAdd = {dfadd}");
            dfadd = int.Parse(dt.Rows[0]["dfadd"].ToString());
        }

        //Set the account type.
        public void setType(string AccType)
        {
            accountType = AccType;
            UserInfo();
        }


        private void UserInfo()
        {
            DataTable dt = new DataTable();

            if (accountType.Equals("Staff")) //Staff info
            {
                sqlStr =
                    "SELECT jobTitle, name, emailAddress, firstName, lastName, status, sex, phoneNumber, dateOfBirth, createDate " +
                    "FROM staff S, department D, staff_account SA WHERE S.deptID = D.deptID AND S.staffID = @uid AND SA.staffID = @uid";
            }
            else //Customer info
            {
                GetDfAdd();
                sqlStr =
                    "SELECT emailAddress, firstName, lastName, isLM, status, sex, phoneNumber, dateOfBirth, createDate, paymentMethod, province, " +
                    "city, companyAddress, warehouseAddress, company, warehouseAddress2 " +
                    "FROM customer_account CA, customer C WHERE CA.customerID = @uid AND C.customerID = @uid";
            }


            dt = _db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@uid", UID } });
            Log.LogMessage(LogLevel.Debug, "proFileController", $"UserInfo: SQL = {sqlStr}");

            //Set user data to gobal variable
            if (accountType.Equals("Staff"))
            {
                jobTitle = dt.Rows[0]["jobTitle"].ToString();
                dept = dt.Rows[0]["name"].ToString();
            }
            else
            {
                waddress1 = dt.Rows[0]["warehouseAddress"].ToString();
                waddress2 = dt.Rows[0]["warehouseAddress2"].ToString();
                dfwaddress = dfadd == 1 ? waddress1 : waddress2;
                IsLM = dt.Rows[0]["isLM"].ToString();
                payment = dt.Rows[0]["paymentMethod"].ToString();
                caddress = dt.Rows[0]["companyAddress"].ToString();
                corp = dt.Rows[0]["company"].ToString();
                city = dt.Rows[0]["city"].ToString();
                province = dt.Rows[0]["province"].ToString();
            }

            email = dt.Rows[0]["emailAddress"].ToString();
            fName = dt.Rows[0]["firstName"].ToString();
            lName = dt.Rows[0]["lastName"].ToString();
            sex = dt.Rows[0]["sex"].ToString();
            phone = dt.Rows[0]["phoneNumber"].ToString();
            createDate = (DateTime)dt.Rows[0]["createDate"];
            status = dt.Rows[0]["status"].ToString();

            //If the date of birth is not provided
            if (string.IsNullOrEmpty(dt.Rows[0]["dateOfBirth"].ToString()) && accountType.Equals("Customer"))
            {
                NGDateOfBirth = true;
                dateOfBirth = DateTime.Now;
            }
            else
                dateOfBirth = (DateTime)dt.Rows[0]["dateOfBirth"];
        }

        //Return value to the profile or the info panel in user management.
        public dynamic getUserInfo()
        {
            dynamic UserInfo = new ExpandoObject();
            UserInfo.accountType = accountType;
            UserInfo.jobTitle = jobTitle;
            UserInfo.dept = dept;
            UserInfo.email = email;
            UserInfo.fName = fName;
            UserInfo.lName = lName;
            UserInfo.sex = sex;
            UserInfo.phone = phone;
            UserInfo.dateOfBirth = dateOfBirth;
            UserInfo.createDate = createDate;
            UserInfo.payment = payment;
            UserInfo.caddress = caddress + ", " + city + ", " + province;
            UserInfo.NGDateOfBirth = NGDateOfBirth;
            UserInfo.corp = corp;
            UserInfo.waddress = dfwaddress + ", " + city + ", " + province;
            UserInfo.status = status;
            UserInfo.IsLM = IsLM;
            return UserInfo;
        }

        //Return the address for customer user.
        public dynamic getAddinfo()
        {
            dynamic AddInfo = new ExpandoObject();
            DataTable dt = new DataTable();

            sqlStr =
                "SELECT province, city, companyAddress, warehouseAddress, warehouseAddress2 FROM customer WHERE customerID = @customerID";
            dt = _db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@customerID", UID } });
            AddInfo.province = province;
            AddInfo.city = city;
            AddInfo.corpAdd = caddress;
            AddInfo.wAdd1 = waddress1;
            AddInfo.wAdd2 = waddress2;
            AddInfo.dfvalue = dfadd;

            return AddInfo;
        }

        //Values for listbox
        public List<string> GetCity(string province)
        {
            DataTable dt = new DataTable();
            sqlStr = "SELECT city FROM location WHERE province = @province";
            dt = _db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@province", province } });
            List<string> city = new List<string>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                city.Add(dt.Rows[i]["city"].ToString());
            }

            return city;
        }

        public List<string> getpriovince()
        {
            DataTable dt = new DataTable();
            sqlStr = "SELECT DISTINCT province FROM location";
            dt = _db.ExecuteDataTable(sqlStr);
            List<string> priovince = new List<string>();

            for (int i = 0; i <= (dt.Rows.Count - 1); i++)
            {
                priovince.Add(dt.Rows[i]["province"].ToString());
            }

            return priovince;
        }

        //Check whether the email or phone has registered an account.
        public bool CheckEmailPhone(string data)
        {
            try
            {
                string sqlCmd =
                    "SELECT emailAddress, phoneNumber FROM customer C, customer_account CA WHERE Status = 'active' AND c.customerID = CA.customerID AND (phoneNumber = @data OR emailAddress = @data) " +
                    "UNION ALL SELECT emailAddress, phoneNumber FROM staff S, staff_account SA WHERE status = 'active' AND s.staffID = sa.staffID AND(phoneNumber = @data OR emailAddress = @data)";
                var parameters = new Dictionary<string, object>
                {
                    { "@data", data }
                };

                DataTable dt = _db.ExecuteDataTable(sqlCmd, parameters);

                return dt.Rows.Count < 1;
            }
            catch (Exception e)
            {
                Log.LogException(new Exception($"Error in CheckEmailPhone. {e.Message}"), "proFileController");
                return false; //Something went wrong.
            }
        }

        //Update the user's info in the database.
        public bool ModifyUserInfo(dynamic info)
        {
            try
            {
                string sqlCmd;
                var parameters = new Dictionary<string, object>
                {
                    { "@fName", info.fName },
                    { "@lName", info.lName },
                    { "@sex", info.sex },
                    { "@phone", info.phone },
                    { "@DFB", info.DFB },
                    { "@UID", UID }
                };

                if (accountType.Equals("Customer"))
                {
                    sqlCmd =
                        "UPDATE customer SET firstName = @fName, lastName = @lName, sex = @sex, phoneNumber = @phone, paymentMethod = @pay, dateofBirth = @DFB, company = @corp WHERE customerID = @UID";
                    parameters.Add("@pay", info.pay);
                    parameters.Add("@corp", info.corp);
                }
                else
                {
                    sqlCmd =
                        "UPDATE staff SET firstName = @fName, lastName = @lName, sex = @sex, phoneNumber = @phone, dateofBirth = @DFB WHERE staffID = @UID";
                }

                _db.ExecuteNonQueryCommand(sqlCmd, parameters);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "profile controller", $"Error modifying user info: {e.Message}");
                return false; //Something went wrong.
            }
        }

        //Manager Update the user's info in the database.
        public bool MgmtModifyUserInfo(dynamic info)
        {
            try
            {
                string sqlCmd;
                var parameters = new Dictionary<string, object>
                {
                    { "@fName", info.fName },
                    { "@lName", info.lName },
                    { "@sex", info.sex },
                    { "@phone", info.phone },
                    { "@Email", info.email },
                    { "@DFB", info.DFB },
                    { "@UID", UID },
                    { "@IsLM", info.IsLM }
                };

                if (accountType.Equals("Customer"))
                {
                    sqlCmd = "UPDATE customer_account SET isLM = @IsLM WHERE customerID = @UID";

                    _db.ExecuteNonQueryCommand(sqlCmd, parameters);

                    sqlCmd =
                        "UPDATE customer SET firstName = @fName, lastName = @lName, sex = @sex" +
                        ", phoneNumber = @phone, emailAddress = @email, dateOfBirth = @DFB WHERE customerID = @UID";
                }
                else
                {
                    sqlCmd =
                        "UPDATE staff SET firstName = @fName, lastName = @lName, sex = @sex, phoneNumber = @phone, dateofBirth = @DFB, emailAddress = @email WHERE staffID = @UID";
                }


                _db.ExecuteNonQueryCommand(sqlCmd, parameters);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "profile controller", $"Error modifying user info: {e.Message}");
                return false; //Something went wrong.
            }
        }

        //Update the address in the database.
        public bool ModifyAddress(dynamic Addinfo)
        {
            try
            {
                string sqlCmd1 =
                    "UPDATE customer SET province = @province, city = @city, companyAddress = @corpAdd, warehouseAddress = @wAdd1, warehouseAddress2 = @wAdd2 WHERE customerID = @UID";
                var parameters1 = new Dictionary<string, object>
                {
                    { "@province", Addinfo.province },
                    { "@city", Addinfo.city },
                    { "@corpAdd", Addinfo.corpAdd },
                    { "@wAdd1", Addinfo.wAdd1 },
                    { "@wAdd2", Addinfo.wAdd2 },
                    { "@UID", UID }
                };
                _db.ExecuteNonQueryCommand(sqlCmd1, parameters1);

                string sqlCmd2 = "UPDATE customer_dfadd SET dfadd = @dfvalue WHERE customerID = @UID";
                var parameters2 = new Dictionary<string, object>
                {
                    { "@dfvalue", Addinfo.dfvalue },
                    { "@UID", UID }
                };
                _db.ExecuteNonQueryCommand(sqlCmd2, parameters2);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "profile controller", $"Error modifying address: {e.Message}");
                return false; //Something went wrong.
            }
        }

        // upload avatar
        public bool UploadUserAvatar(string filename, string userID)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + "\\Upload\\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(filename);
                string newfileName =
                    "usr_" + userID + "_" + Guid.NewGuid().ToString("N") + "_" + fileName;
                string newFilePath = path + newfileName;

                if (!File.Exists(filename))
                {
                    throw new FileNotFoundException($"The source file {filename} does not exist.");
                }

                if (File.Exists(newFilePath))
                {
                    throw new IOException($"The destination file {newFilePath} already exists.");
                }

                File.Move(filename, newFilePath);

                string sqlCmd = "SELECT id FROM resource WHERE id = (SELECT MAX(id) FROM resource)";
                DataTable dt = _db.ExecuteDataTable(sqlCmd);
                int imageID = int.Parse(dt.Rows[0]["id"].ToString()) + 1;

                var parameters = new Dictionary<string, object> { { "@id", imageID } };
                _db.ExecuteNonQueryCommand("INSERT INTO resource VALUES(@id, @name, @type, @path)", parameters);
                _db.ExecuteNonQueryCommand("UPDATE customer SET imageID = @id WHERE customerID = @UID",
                    new Dictionary<string, object> { { "@id", imageID.ToString() }, { "@UID", userID } });

                Log.LogMessage(LogLevel.Debug, "profile Controller",
                    $"UploadUserAvatar: New file path = {newFilePath}");

                return true;
            }
            catch (Exception e)
            {
                Log.LogException(e, "profile Controller - Upload User Avatar");
                return false;
            }
        }

        public bool DeleteUserAvatar(string userID)
        {
            string checkAvatarSql = "SELECT imageId FROM customer WHERE customerID = @UID";
            var checkAvatarParams = new Dictionary<string, object> { { "@UID", userID } };
            DataTable dt = _db.ExecuteDataTable(checkAvatarSql, checkAvatarParams);

            if (dt.Rows.Count > 0)
            {
                // Avatar exists, delete it
                string deleteAvatarSql = "DELETE FROM resource WHERE id = @id";
                var deleteAvatarParams = new Dictionary<string, object> { { "@id", dt.Rows[0]["imageID"].ToString() } };
                _db.ExecuteNonQueryCommand(deleteAvatarSql, deleteAvatarParams);
                // Set IMGUploaded to false
                return false;
            }

            Log.LogMessage(LogLevel.Debug, "profile - Controller - Delete User Avatar",
                $"Delete User Avatar: UserID = {userID}");
            return true;
        }

        public bool CheckUserAvatar(string userID)
        {
            try
            {
                string checkAvatarSql = "SELECT imageId FROM customer WHERE customerID = @UID";
                var checkAvatarParams = new Dictionary<string, object> { { "@UID", userID } };
                DataTable dt = _db.ExecuteDataTable(checkAvatarSql, checkAvatarParams);

                // if it returns a value = null, it means the user has no avatar
                return dt.Rows.Count != 0;
            }
            catch (Exception e)
            {
                Log.LogException(new Exception($"Error in CheckUserAvatar. {e.Message}"), "proFile Controller");

                return false; //Something went wrong.
            }
        }

        public string GetUserAvatar(string userID)
        {
            try
            {
                string checkAvatarSql = "SELECT imageId FROM customer WHERE customerID = @UID";
                var checkAvatarParams = new Dictionary<string, object> { { "@UID", userID } };
                DataTable dt = _db.ExecuteDataTable(checkAvatarSql, checkAvatarParams);
                int imageID;
                if (int.TryParse(dt.Rows[0]["imageID"].ToString(), out imageID))
                {
                    // Avatar exists, return the path
                    string getAvatarSql = "SELECT path FROM resource WHERE id = @id";
                    var getAvatarParams = new Dictionary<string, object> { { "@id", imageID } };
                    DataTable dt2 = _db.ExecuteDataTable(getAvatarSql, getAvatarParams);
                    return dt2.Rows[0]["path"].ToString();
                }

                return null;
            }
            catch (Exception e)
            {
                Log.LogException(new Exception($"Error in GetUserAvatar. {e.Message}"), "proFile Controller");

                return null; //Something went wrong.
            }
        }
    }
}
