using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongestPeriodWithUI
{
    public static class LongestPeriod
    {
        public static int ProjectID { get; set; }
        public static int FirstEmpID { get; set; }
        public static int SecondEmpID { get; set; }
        public static int MaxDays { get; set; }

        public static void FindEmployeesThatHaveWorkedTheLongestTogether(string filePath)
        {
            Dictionary<int, Dictionary<int, List<DateTime[]>>> employeesWorkInfo = new Dictionary<int, Dictionary<int, List<DateTime[]>>>();
            Dictionary<int, List<DateTime[]>> employeesAndTheirWorkDates = new Dictionary<int, List<DateTime[]>>();

            StreamReader streamReader = new StreamReader(filePath);
            using (streamReader)
            {
                string line = streamReader.ReadLine();
                string[] arguments = line.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                int empID = int.Parse(arguments[0]);
                int projectID = int.Parse(arguments[1]);
                DateTime dateFrom = DateTime.Parse(arguments[2]);
                DateTime dateTo = DateTime.Today;
                if (arguments[3].TrimEnd() != "NULL")
                {
                    dateTo = DateTime.Parse(arguments[3]);
                }

                List<DateTime[]> dates = new List<DateTime[]>();
                dates.Add(new DateTime[] { dateFrom, dateTo });
                employeesAndTheirWorkDates.Add(empID, dates);
                employeesWorkInfo.Add(projectID, employeesAndTheirWorkDates);

                while (true)
                {
                    line = streamReader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    arguments = line.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    empID = int.Parse(arguments[0]);
                    projectID = int.Parse(arguments[1]);
                    dateFrom = DateTime.Parse(arguments[2]);
                    dateTo = DateTime.Today;
                    if (arguments[3].TrimEnd() != "NULL")
                    {
                        dateTo = DateTime.Parse(arguments[3]);
                    }

                    if (employeesWorkInfo.ContainsKey(projectID))
                    {
                        if (employeesWorkInfo[projectID].ContainsKey(empID))
                        {
                            employeesWorkInfo[projectID][empID].Add(new DateTime[] { dateFrom, dateTo });
                        }
                        else
                        {
                            dates = new List<DateTime[]>();
                            dates.Add(new DateTime[] { dateFrom, dateTo });
                            employeesWorkInfo[projectID].Add(empID, dates);
                        }
                    }
                    else
                    {
                        employeesAndTheirWorkDates = new Dictionary<int, List<DateTime[]>>();
                        dates = new List<DateTime[]>();
                        dates.Add(new DateTime[] { dateFrom, dateTo });
                        employeesAndTheirWorkDates.Add(empID, dates);
                        employeesWorkInfo.Add(projectID, employeesAndTheirWorkDates);
                    }
                }
            }

            int projectIDOnWhichTheTwoEmployeesHaveWorkedOnTheMost = 0;
            int firstEmployeeID = 0;
            int secondEmployeeID = 0;
            int maxDaysTwoEmployeesHaveWorkedOnTheSameProject = 0;

            for (int i = 0; i < employeesWorkInfo.Count; i++)
            {
                Dictionary<int, List<DateTime[]>> currentProjectEmployeesWorkInfo = employeesWorkInfo.ElementAt(i).Value;
                for (int j = 0; j < currentProjectEmployeesWorkInfo.Count - 1; j++)
                {
                    for (int k = j + 1; k < currentProjectEmployeesWorkInfo.Count; k++)
                    {
                        int daysWorkedTogether = 0;
                        for (int m = 0; m < currentProjectEmployeesWorkInfo.ElementAt(j).Value.Count; m++)
                        {
                            for (int n = 0; n < currentProjectEmployeesWorkInfo.ElementAt(k).Value.Count; n++)
                            {
                                if ((currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[0] >=
                                    currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[0]) &&
                                    (currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[1] <=
                                    currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[1]))
                                {
                                    daysWorkedTogether += (currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[1] - currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[0]).Days + 1;
                                }
                                else if ((currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[0] <=
                                    currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[0]) &&
                                    (currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[1] >=
                                    currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[1]))
                                {
                                    daysWorkedTogether += (currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[1] - currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[0]).Days + 1;
                                }
                                else if ((currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[1] >=
                                    currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[0]) &&
                                    (currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[0] <=
                                    currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[0]))
                                {
                                    daysWorkedTogether += (currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[1] - currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[0]).Days + 1;
                                }
                                else if ((currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[1] >=
                                    currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[1]) &&
                                    (currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[0] <=
                                    currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[1]))
                                {
                                    daysWorkedTogether += (currentProjectEmployeesWorkInfo.ElementAt(k).Value.ElementAt(n)[1] - currentProjectEmployeesWorkInfo.ElementAt(j).Value.ElementAt(m)[0]).Days + 1;
                                }
                            }
                        }

                        if (daysWorkedTogether > maxDaysTwoEmployeesHaveWorkedOnTheSameProject)
                        {
                            maxDaysTwoEmployeesHaveWorkedOnTheSameProject = daysWorkedTogether;
                            firstEmployeeID = currentProjectEmployeesWorkInfo.ElementAt(j).Key;
                            secondEmployeeID = currentProjectEmployeesWorkInfo.ElementAt(k).Key;
                            projectIDOnWhichTheTwoEmployeesHaveWorkedOnTheMost = employeesWorkInfo.ElementAt(i).Key;

                            MaxDays = maxDaysTwoEmployeesHaveWorkedOnTheSameProject;
                            FirstEmpID = firstEmployeeID;
                            SecondEmpID = secondEmployeeID;
                            ProjectID = projectIDOnWhichTheTwoEmployeesHaveWorkedOnTheMost;
                        }
                    }
                }
            }
        }
    }
}
