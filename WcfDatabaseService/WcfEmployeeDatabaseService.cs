using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Collections.ObjectModel;

namespace WcfDatabaseService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "IWcfEmployeeDatabaseService" in both code and config file together.
    public class WcfEmployeeDatabaseService : IWcfEmployeeDatabaseService
    {
        private const string mApplicationName = "HR Management Service Application";

        private SQLiteConnection mDatabaseConnection = null;

        public WcfEmployeeDatabaseService()
        {
            ConnectToDatabase(); // when creating the service, connect to the database
        }

        // The method to connect to the database.
        // The database will be saved in the Appdata-Folder for this application.
        // The connection type is a sqlite-connection, the dll's needed will be loaded with a nugget package.
        // If first start and no database available, the database will be created and  the table-creation script will be fired,
        private void ConnectToDatabase()
        {
            string folderPathToDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), mApplicationName);
            string database = Path.Combine(folderPathToDatabase, string.Format("{0}.{1}", "database", "sqlite"));
            var connectionString = string.Format(@"data source='{0}'", database);

            if (!Directory.Exists(folderPathToDatabase) && !File.Exists(database)) // if there is no available database
            {
                // create the directory in the appdata-folder
                Directory.CreateDirectory(folderPathToDatabase);
                // now create the database and connect with it
                SQLiteConnection.CreateFile(database);

                mDatabaseConnection = new SQLiteConnection(connectionString);
                mDatabaseConnection.Open();
                var databaseCommand = mDatabaseConnection.CreateCommand();

                // in the next steps, the script for creating the data tables will be executed.
                FileInfo file = new FileInfo(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"Scripts\Create_Tables.sql"));
                string script = file.OpenText().ReadToEnd();
                databaseCommand = new SQLiteCommand(script, mDatabaseConnection);
                databaseCommand.ExecuteNonQuery();
                databaseCommand.Dispose();
            }
            else // if there is already an available database with data
            {
                mDatabaseConnection = new SQLiteConnection(connectionString);
                mDatabaseConnection.Open();
            }
        }

        // This method checks if the address is already available in the database.
        private Address IsAddressAlreadyAvailable(Address a)
        {
            var alreadyAvailableAddress = ReadAllAddresses().FirstOrDefault(x => x.City.Equals(a.City) && x.State.Equals(a.State) && x.Street.Equals(a.Street) && x.Zip == a.Zip);
            return alreadyAvailableAddress;
        }

        #region database-inserts
        // Create a new employee in the database and the address for him. If this address is already existing, do not create a new one, use this.
        public int InsertEmployeeAndAddressIfNotAvailable(Employee e)
        {
            // if no valid connection available
            if(mDatabaseConnection == null)
            {
                return -1;
            }

            var alreadyAvailableAddress = IsAddressAlreadyAvailable(e.Addresses);
            if (alreadyAvailableAddress != null)
            {
                e.Addresses = alreadyAvailableAddress;
            }
            else
            {
                InsertAddress(e.Addresses);
            }

            try
            {
                var databaseCommand = new SQLiteCommand(mDatabaseConnection);

                databaseCommand.CommandText = "INSERT INTO Employee VALUES (@Id, @FirstName, @LastName, @Age, @MainAddressId)";
                databaseCommand.Parameters.AddWithValue("Id", e.Id.ToString());
                databaseCommand.Parameters.AddWithValue("FirstName", e.FirstName);
                databaseCommand.Parameters.AddWithValue("LastName", e.LastName);
                databaseCommand.Parameters.AddWithValue("Age", e.Age);
                databaseCommand.Parameters.AddWithValue("MainAddressId", e.Addresses.Id.ToString());

                databaseCommand.CommandType = System.Data.CommandType.Text;
                if(mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    mDatabaseConnection.Open();
                }

                int retVal = databaseCommand.ExecuteNonQuery();
                databaseCommand.Dispose();
                return retVal;
            }
            catch(Exception)
            {
                throw; // problem with the database...
            }
            finally
            {
                if(mDatabaseConnection != null)
                {
                    mDatabaseConnection.Close();
                }
            }
        }

        // This method will insert the address into the database if it isn't available at this time, otherwise nothing happens.
        public int InsertAddress(Address a)
        {
            // if no valid connection available
            if (mDatabaseConnection == null)
            {
                return -1;
            }

            var alreadyAvailableAddress = IsAddressAlreadyAvailable(a);
            if (alreadyAvailableAddress != null)
            {
                return 0; // the address is already available in the database
            }
            else
            {
                try
                {
                    var newCommand = new SQLiteCommand(mDatabaseConnection);

                    newCommand.CommandText = "INSERT INTO Address VALUES (@Id, @Street, @Zip, @City, @State)";
                    newCommand.Parameters.AddWithValue("Id", a.Id.ToString());
                    newCommand.Parameters.AddWithValue("Street", a.Street);
                    newCommand.Parameters.AddWithValue("Zip", a.Zip);
                    newCommand.Parameters.AddWithValue("City", a.City);
                    newCommand.Parameters.AddWithValue("State", a.State);

                    newCommand.CommandType = System.Data.CommandType.Text;
                    if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                    {
                        mDatabaseConnection.Open();
                    }

                    int retVal = newCommand.ExecuteNonQuery();
                    newCommand.Dispose();
                    return retVal;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    if (mDatabaseConnection != null)
                    {
                        mDatabaseConnection.Close();
                    }
                }
            }
        }

        #endregion

        #region read all employees and addresses

        /// <summary>
        /// This method reads all addresses of the database and returns them.
        /// </summary>
        /// <returns>an collection of addresses</returns>
        public ObservableCollection<Address> ReadAllAddresses()
        {
            ObservableCollection<Address> allAddresses = new ObservableCollection<Address>();

            try
            {
                var newCommand = new SQLiteCommand(mDatabaseConnection);

                newCommand.CommandText = "SELECT * FROM Address";
                newCommand.CommandType = System.Data.CommandType.Text;
                if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    mDatabaseConnection.Open();
                }

                var reader = newCommand.ExecuteReader();
                while (reader.Read())
                {
                    Address a = new Address();
                    a.Id = Guid.Parse(reader["Id"].ToString());
                    a.Street = reader["Street"].ToString();
                    int zip = 0;
                    if (Int32.TryParse(reader["Zip"].ToString(), out zip))
                    {
                        a.Zip = zip;
                    }
                    a.City = reader["City"].ToString();
                    a.State = reader["State"].ToString();

                    allAddresses.Add(a);
                }

                newCommand.Dispose();
                return allAddresses;
            }
            catch (Exception)
            {
                throw;
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
                var employeeCommand = new SQLiteCommand(mDatabaseConnection);
                employeeCommand.CommandText = "SELECT * FROM Employee";
                employeeCommand.CommandType = System.Data.CommandType.Text;

                var allAddresses = this.ReadAllAddresses();
                if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    mDatabaseConnection.Open();
                }

                var reader = employeeCommand.ExecuteReader();
                while(reader.Read())
                {
                    Employee e = new Employee();
                    e.Id = Guid.Parse(reader["Id"].ToString());
                    e.FirstName = reader["FirstName"].ToString();
                    e.LastName = reader["LastName"].ToString();
                    int age = -1;
                    if(Int32.TryParse(reader["Age"].ToString(), out age))
                    {
                        e.Age = age;
                    }
                    e.Addresses = allAddresses.FirstOrDefault(x => x.Id.Equals(Guid.Parse(reader["MainAddressId"].ToString())));

                    allEmployees.Add(e);
                }

                reader.Close();
                employeeCommand.Dispose();
                return allEmployees;
            }
            catch(Exception)
            {
                throw;
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
                var newCommand = new SQLiteCommand(mDatabaseConnection);

                newCommand.CommandText = "UPDATE Employee SET FirstName=@FirstName, LastName=@LastName, Age=@Age, MainAddressId=@MainAddressId WHERE Id=@Id";
                newCommand.Parameters.AddWithValue("FirstName", e.FirstName);
                newCommand.Parameters.AddWithValue("LastName", e.LastName);
                newCommand.Parameters.AddWithValue("Age", e.Age);
                newCommand.Parameters.AddWithValue("MainAddressId", e.Addresses.Id.ToString());
                newCommand.Parameters.AddWithValue("Id", e.Id.ToString());
                newCommand.CommandType = System.Data.CommandType.Text;

                if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    mDatabaseConnection.Open();
                }

                int retVal = newCommand.ExecuteNonQuery();
                newCommand.Dispose();
                return retVal;
            }
            catch (Exception)
            {
                throw;
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
        private int UpdateAddress(Address a)
        {
            try
            {
                var newCommand = new SQLiteCommand(mDatabaseConnection);

                newCommand.CommandText = "UPDATE Address SET Street=@Street, Zip=@Zip, City=@City, State=@State WHERE Id=@Id";
                newCommand.Parameters.AddWithValue("Street", a.Street);
                newCommand.Parameters.AddWithValue("Zip", a.Zip);
                newCommand.Parameters.AddWithValue("City", a.City);
                newCommand.Parameters.AddWithValue("State", a.State);
                newCommand.Parameters.AddWithValue("Id", a.Id.ToString());
                newCommand.CommandType = System.Data.CommandType.Text;

                if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    mDatabaseConnection.Open();
                }

                int retVal = newCommand.ExecuteNonQuery();
                newCommand.Dispose();
                return retVal;
            }
            catch (Exception)
            {
                throw;
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
        /// <returns>true if updated successfully, false if not</returns>
        public bool UpdateEmployeeAndAddress(Employee e)
        {
            if (UpdateAddress(e.Addresses) == 0 || UpdateEmployee(e) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

        #region deleting employees and addresses of the database.

        /// <summary>
        /// This method deletes the given address if it isn't as foreign key in any other employee.
        /// </summary>
        /// <param name="The address to delete"></param>
        public void DeleteAddressWhenNotUsedByOtherEmployee(Address a)
        {
            var employees = ReadAllEmployees();
            if(employees.Any(x => x.Addresses.Id.Equals(a.Id)))
            {
                return; // another employee has this address - because of the foreign key it can't be deleted
            }

            try
            {
                var newCommand = new SQLiteCommand(mDatabaseConnection);

                newCommand.CommandText = "DELETE FROM Address WHERE Id=@Id";
                newCommand.Parameters.AddWithValue("Id", a.Id.ToString());
                newCommand.CommandType = System.Data.CommandType.Text;

                if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    mDatabaseConnection.Open();
                }

                newCommand.ExecuteNonQuery();
                newCommand.Dispose();
            }
            catch (Exception)
            {
                throw;
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
                var newCommand = new SQLiteCommand(mDatabaseConnection);

                newCommand.CommandText = "DELETE FROM Employee WHERE Id=@Id";
                newCommand.Parameters.AddWithValue("Id", e.Id.ToString());
                newCommand.CommandType = System.Data.CommandType.Text;

                if (mDatabaseConnection.State != System.Data.ConnectionState.Open)
                {
                    mDatabaseConnection.Open();
                }

                var removedItems =  newCommand.ExecuteNonQuery();
                newCommand.Dispose();
                return removedItems;
            }
            catch (Exception)
            {
                throw;
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
    }
}
