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
    public class EmployeeXMLWriter
    {
        /// <summary>
        /// This method writes all employees into a xml file.
        /// </summary>
        /// <param name="employees"></param>
        public static string WriteEmployeeXML(List<Employee> employees)
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HR Management Service Application");
            using (XmlWriter writer = XmlWriter.Create(Path.Combine(appDataPath, string.Format("{0}.{1}", "Alle Mitarbeiter", "xml"))))
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

                    for(int addressCounter = 0; addressCounter < employee.Addresses.Count(); addressCounter++) // write all available addresses to the XML-file
                    {
                        if(employee.Addresses.Count() > 1)
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

            return Path.Combine(appDataPath, string.Format("{0}.{1}", "Alle Mitarbeiter", "xml"));
        }
    }
}
