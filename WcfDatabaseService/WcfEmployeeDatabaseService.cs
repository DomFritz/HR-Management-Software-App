using System;
using System.Linq;
using System.Data.SQLite;
using System.IO;
using System.Collections.ObjectModel;
using HRManagement_ServiceApplication_HRItems;

namespace WcfDatabaseService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IWcfEmployeeDatabaseService" in both code and config file together.
    public class WcfEmployeeDatabaseService : IWcfEmployeeDatabaseService
    {
        private SQLiteConnection mDatabaseConnection = null;

        public WcfEmployeeDatabaseService()
        {
            ConnectToDatabase();
        }

        // this method returns the application name - it should be only changed once if necessary
        public string GetApplicationName()
        {
            return "HR Management Software";
        }

        // The method to connect to the database.
        // The database will be saved in the Appdata-Folder for this application.
        // The connection type is a sqlite-connection, the dll's needed will be loaded with a nugget package.
        // If first start and no database available, the database will be created and  the table-creation script will be fired,
        private void ConnectToDatabase()
        {
            if(mDatabaseConnection != null)
            {
                return; // connection already exists
            }

            string folderPathToDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetApplicationName());
            string database = Path.Combine(folderPathToDatabase, string.Format("{0}.sqlite", GetApplicationName()));
            var connectionString = string.Format(@"data source='{0}'", database);

            if (!Directory.Exists(folderPathToDatabase) || !File.Exists(database)) // if there exists no available database
            {
                // create the directory in the appdata-folder
                Directory.CreateDirectory(folderPathToDatabase);
                // now create the database and connect with it
                SQLiteConnection.CreateFile(database);

                mDatabaseConnection = new SQLiteConnection(connectionString);
                mDatabaseConnection.Open();

                // in the next steps, the script for creating the data tables will be executed.
                FileInfo file = new FileInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"Scripts\Create_Tables.sql"));
                using (var databaseCommand = new SQLiteCommand(file.OpenText().ReadToEnd(), mDatabaseConnection))
                {
                    databaseCommand.ExecuteNonQuery();
                }
            }
            else // if there is already an available database with data or the empty tables
            {
                mDatabaseConnection = new SQLiteConnection(connectionString);
                mDatabaseConnection.Open();
            }
        }

        #region database-inserts

        // Create a new employee in the database and the address for him. If this address is already existing, do not create a new one, use this.
        public int InsertEmployeeAndAddresses(Employee e)
        {
            // if no valid connection available
            if (mDatabaseConnection == null)
            {
                return -1;
            }

            try
            {
                int retVal = -1;

                using (var databaseCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    databaseCommand.CommandText = "INSERT INTO Employee VALUES (@Id, @FirstName, @LastName, @Age)";
                    databaseCommand.Parameters.AddWithValue("Id", e.Id.ToString());
                    databaseCommand.Parameters.AddWithValue("FirstName", e.FirstName);
                    databaseCommand.Parameters.AddWithValue("LastName", e.LastName);
                    databaseCommand.Parameters.AddWithValue("Age", e.Age);

                    databaseCommand.CommandType = System.Data.CommandType.Text;
                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    retVal = databaseCommand.ExecuteNonQuery();
                }

                foreach (var address in e.Addresses)
                {
                    if (InsertAddress(address, e.Id) == -1)
                    {
                        return -1;
                    }
                }

                return retVal;
            }
            catch (Exception)
            {
                return -1; // problem with database
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        // This method will insert the address into the database if it isn't available at this time, otherwise nothing happens.
        public int InsertAddress(Address a, Guid employeeId)
        {
            // if no valid connection available
            if (mDatabaseConnection == null)
            {
                return -1;
            }

            try
            {
                using (var newCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    newCommand.CommandText = "INSERT INTO Address VALUES (@Id, @Street, @Zip, @City, @State, @EmployeeId)";
                    newCommand.Parameters.AddWithValue("Id", a.Id.ToString());
                    newCommand.Parameters.AddWithValue("Street", a.Street);
                    newCommand.Parameters.AddWithValue("Zip", a.Zip);
                    newCommand.Parameters.AddWithValue("City", a.City);
                    newCommand.Parameters.AddWithValue("State", a.State);
                    newCommand.Parameters.AddWithValue("EmployeeId", employeeId.ToString());

                    newCommand.CommandType = System.Data.CommandType.Text;
                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    return newCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }

        }

        #endregion

        #region read all employees and addresses

        /// <summary>
        /// This method reads all addresses of the database and returns them.
        /// </summary>
        /// <returns>an collection of addresses</returns>
        public ObservableCollection<Address> ReadAllAddresses(Employee e = null)
        {
            ObservableCollection<Address> allAddresses = new ObservableCollection<Address>();

            try
            {
                using (var newCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    // if employee is set, we only choose the addresses for the employee
                    if (e == null)
                    {
                        newCommand.CommandText = "SELECT * FROM Address";
                    }
                    else
                    {
                        newCommand.CommandText = "SELECT * FROM Address WHERE EmployeeId=@EmployeeId";
                        newCommand.Parameters.AddWithValue("EmployeeId", e.Id.ToString());
                    }

                    newCommand.CommandType = System.Data.CommandType.Text;
                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    var reader = newCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Address a = new Address();

                        try
                        {
                            a.Id = Guid.Parse(reader["Id"].ToString());
                        }
                        catch (FormatException)
                        {
                            a.Id = Guid.NewGuid(); // if invalid Guid was manually added in the sql file directly
                        }

                        a.Street = reader["Street"].ToString();
                        int zip = 0;
                        if (Int32.TryParse(reader["Zip"].ToString(), out zip))
                        {
                            a.Zip = zip;
                        }
                        a.City = reader["City"].ToString();
                        a.State = reader["State"].ToString();
                        a.EmployeeId = Guid.Parse(reader["EmployeeId"].ToString());

                        allAddresses.Add(a);
                    }

                    reader.Close();
                }

                return allAddresses;
            }
            catch (Exception)
            {
                return allAddresses; // return empty collection if problems happen
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        /// <summary>
        /// This method reads all employees of the database and returns them.
        /// </summary>
        /// <returns>an observable collection with all employees</returns>
        public ObservableCollection<Employee> ReadAllEmployees()
        {
            ObservableCollection<Employee> allEmployees = new ObservableCollection<Employee>();

            try
            {
                using (var employeeCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    employeeCommand.CommandText = "SELECT * FROM Employee";
                    employeeCommand.CommandType = System.Data.CommandType.Text;

                    var allAddresses = this.ReadAllAddresses();
                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    var reader = employeeCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Employee e = new Employee();

                        try
                        {
                            e.Id = Guid.Parse(reader["Id"].ToString()); // if invalid Guid was manually added in the sql file directly
                        }
                        catch (FormatException)
                        {
                            e.Id = Guid.NewGuid();
                        }

                        e.FirstName = reader["FirstName"].ToString();
                        e.LastName = reader["LastName"].ToString();
                        int age = -1;
                        if (Int32.TryParse(reader["Age"].ToString(), out age))
                        {
                            e.Age = age;
                        }
                        e.Addresses = allAddresses.Where(x => x.EmployeeId.Equals(e.Id)).ToList();

                        allEmployees.Add(e);
                    }

                    reader.Close();
                }

                return allEmployees;
            }
            catch (Exception)
            {
                return allEmployees; // return empty list if problems happen
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        #endregion

        # region update methods for employees and addresses

        /// <summary>
        /// This method updates the dataset in the database with the values given in the arguments.
        /// </summary>
        /// <param name="The Employee to update"></param>
        /// <returns>1 if a dataset was updated, 0 if nothing happened</returns>
        private int UpdateEmployee(Employee e)
        {
            try
            {
                using (var newCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    newCommand.CommandText = "UPDATE Employee SET FirstName=@FirstName, LastName=@LastName, Age=@Age WHERE Id=@Id";
                    newCommand.Parameters.AddWithValue("FirstName", e.FirstName);
                    newCommand.Parameters.AddWithValue("LastName", e.LastName);
                    newCommand.Parameters.AddWithValue("Age", e.Age);
                    newCommand.Parameters.AddWithValue("Id", e.Id.ToString());
                    newCommand.CommandType = System.Data.CommandType.Text;

                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    return newCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        /// <summary>
        /// This method updates the dataset in the database with the values given in the arguments.
        /// </summary>
        /// <param name="The Address to update"></param>
        /// <returns>1 if a dataset was updated, 0 if nothing happened</returns>
        public int UpdateAddress(Address a, Guid employeeId)
        {
            try
            {
                using (var newCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    newCommand.CommandText = "UPDATE Address SET Street=@Street, Zip=@Zip, City=@City, State=@State, EmployeeId=@EmployeeId WHERE Id=@Id";
                    newCommand.Parameters.AddWithValue("Street", a.Street);
                    newCommand.Parameters.AddWithValue("Zip", a.Zip);
                    newCommand.Parameters.AddWithValue("City", a.City);
                    newCommand.Parameters.AddWithValue("State", a.State);
                    newCommand.Parameters.AddWithValue("EmployeeId", employeeId.ToString());
                    newCommand.Parameters.AddWithValue("Id", a.Id.ToString());
                    newCommand.CommandType = System.Data.CommandType.Text;

                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    return newCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        /// <summary>
        /// This method calls the update methods for employees and the address.
        /// </summary>
        /// <param name="The Employee to update"></param>
        /// <returns>1 if updated successfully, -1 if not</returns>
        public int UpdateEmployeeAndAddresses(Employee e)
        {
            foreach (var address in e.Addresses)
            {
                if (ReadAllAddresses(e).Contains(address)) // check if there is already an address for this employee
                {
                    if (UpdateAddress(address, e.Id) == -1)
                    {
                        return -1;
                    }
                }
                else
                {
                    if (InsertAddress(address, e.Id) == -1)
                    {
                        return -1;
                    }
                }
            }

            if (UpdateEmployee(e) == 0) // update the employee and check if it was successfull
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        #endregion

        #region deleting employees and addresses of the database.

        /// <summary>
        /// Delete all addresses of the employee with the Employee-ID given.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns>1 or more if all successfully deleted, 0 if not successful and -1 if error</returns>
        public int DeleteAddressesOfEmployee(Guid employeeId)
        {
            try
            {
                using (var newCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    newCommand.CommandText = "DELETE FROM Address WHERE EmployeeId=@EmployeeId";
                    newCommand.Parameters.AddWithValue("EmployeeId", employeeId.ToString());
                    newCommand.CommandType = System.Data.CommandType.Text;

                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    return newCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        /// <summary>
        /// This method deletes the given address if it isn't as foreign key in any other employee.
        /// </summary>
        /// <param name="int">The count of deleted addresses</param>
        public int DeleteAddress(Address a)
        {
            try
            {
                using (var newCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    newCommand.CommandText = "DELETE FROM Address WHERE Id=@Id";
                    newCommand.Parameters.AddWithValue("Id", a.Id.ToString());
                    newCommand.CommandType = System.Data.CommandType.Text;

                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    return newCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        /// <summary>
        /// This method deletes the given employee. It must be called AFTER deleting the address because otherwise we have a problem with the foreign key of the address.
        /// </summary>
        /// <param name="The Employee to delete in the database."></param>
        /// <returns>1 if successfull and 0 if not</returns>
        public int DeleteEmployee(Employee e)
        {
            try
            {
                foreach (var address in e.Addresses)
                {
                    if (DeleteAddress(address) == -1) // delete all addresses before deleting the employee - otherwise foreign key exception
                    {
                        return -1;
                    }
                }

                using (var newCommand = new SQLiteCommand(mDatabaseConnection))
                {
                    newCommand.CommandText = "DELETE FROM Employee WHERE Id=@Id";
                    newCommand.Parameters.AddWithValue("Id", e.Id.ToString());
                    newCommand.CommandType = System.Data.CommandType.Text;

                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    return newCommand.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                return -1;
            }
            finally
            {
                if (mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        #endregion

        #region database check

        private string GetDatabaseConnectionName()
        {
            string folderPathToDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetApplicationName());
            return Path.Combine(folderPathToDatabase, string.Format("{0}.sqlite", GetApplicationName()));
        }

        /// <summary>
        /// Checks if the database is available and can be connected.
        /// </summary>
        /// <returns></returns>
        public bool CheckDatabaseAvailability()
        {
            var database = GetDatabaseConnectionName();

            if (!File.Exists(database))
            {
                return false;
            }

            var connectionString = string.Format(@"data source='{0}'", database);
            using (var databaseConnection = new SQLiteConnection(connectionString))
            {
                databaseConnection.Open();

                if (databaseConnection.State != System.Data.ConnectionState.Open)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
