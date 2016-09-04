using HRManagement_ServiceApplication.WcfEmployeeDatabaseService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HRManagement_ServiceApplication
{
    /// <summary>
    /// The class to export the employees. The button to export them will not show if no employees are available.
    /// </summary>
    public class EmployeeXMLWriter
    {
        /// <summary>
        /// This method writes all employees of the employee list in the main window into a xml file.
        /// </summary>
        /// <param name="employees"></param>
        public static string WriteEmployeeXML(List<Employee> employees)
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), EmployeeOverviewViewModel.ApplicationName); // write it to appdata - next to the database
            string xmlFilePath = Path.Combine(appDataPath, "Alle Mitarbeiter.xml");

            using (XmlWriter writer = XmlWriter.Create(xmlFilePath))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Alle_Mitarbeiter");

                foreach (Employee employee in employees)
                {
                    writer.WriteStartElement("Mitarbeiter");

                    writer.WriteElementString("ID", employee.Id.ToString());
                    writer.WriteElementString("Vorname", employee.FirstName);
                    writer.WriteElementString("Nachname", employee.LastName);
                    writer.WriteElementString("Alter", employee.Age.ToString());

                    for (int addressCounter = 0; addressCounter < employee.Addresses.Count(); addressCounter++) // write all available addresses to the XML-file
                    {
                        if (employee.Addresses.Count() > 1) // if more than one address - with counter, otherwise without a counter in the xml-elements
                        {
                            writer.WriteStartElement(string.Format("Addresse_{0}", addressCounter + 1));
                        }
                        else
                        {
                            writer.WriteStartElement("Addresse");
                        }

                        var employeeAddresses = employee.Addresses;

                        writer.WriteElementString("PLZ", employeeAddresses[addressCounter].Zip.ToString());
                        writer.WriteElementString("Straße", employeeAddresses[addressCounter].Street);
                        writer.WriteElementString("Ort", employeeAddresses[addressCounter].City);
                        writer.WriteElementString("Staat", employeeAddresses[addressCounter].State);

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return xmlFilePath; // return the path to show the user if the export was successfull
        }
    }
}
