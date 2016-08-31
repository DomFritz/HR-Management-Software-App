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
                    writer.WriteElementString("PLZ", employee.Addresses.Zip.ToString());
                    writer.WriteElementString("Straße", employee.Addresses.Street);
                    writer.WriteElementString("Ort", employee.Addresses.City);
                    writer.WriteElementString("Staat", employee.Addresses.State);

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return Path.Combine(appDataPath, string.Format("{0}.{1}", "Alle Mitarbeiter", "xml"));
        }
    }
}
