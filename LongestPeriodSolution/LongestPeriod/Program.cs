using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LongestPeriod
{
    public class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Dictionary<int, List<DateTime[]>>> employeesWorkInfo = new Dictionary<int, Dictionary<int, List<DateTime[]>>>();
            Dictionary<int, List<DateTime[]>> employeesAndTheirWorkDates = new Dictionary<int, List<DateTime[]>>();

            List<int[]> employeesPairsWorkdays = new List<int[]>();
            SortedSet<int> employeeIds = new SortedSet<int>();
            Dictionary<int[], HashSet<int>> employeesPairsAndProjectIds = new Dictionary<int[], HashSet<int>>();

            StreamReader streamReader = new StreamReader("../../textDocument.txt");
            using (streamReader)
            {
                string line = streamReader.ReadLine();
                string[] arguments = line.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                int empID = int.Parse(arguments[0]);
                int projectID = int.Parse(arguments[1]);
                DateTime dateFrom = DateTime.Parse(arguments[2]);
                DateTime dateTo = DateTime.Today;
                employeeIds.Add(empID);
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
                    if (String.IsNullOrEmpty(line))
                    {
                        break;
                    }

                    arguments = line.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
                    empID = int.Parse(arguments[0]);
                    projectID = int.Parse(arguments[1]);
                    dateFrom = DateTime.Parse(arguments[2]);
                    dateTo = DateTime.Today;
                    employeeIds.Add(empID);
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

            for (int i = 0; i < employeeIds.Count - 1; i++)
            {
                for (int j = i + 1; j < employeeIds.Count; j++)
                {
                    int[] arrayOfEmployeeIdsAndWorkdays = new int[3];
                    arrayOfEmployeeIdsAndWorkdays[0] = employeeIds.ElementAt(i);
                    arrayOfEmployeeIdsAndWorkdays[1] = employeeIds.ElementAt(j);
                    arrayOfEmployeeIdsAndWorkdays[2] = 0;
                    employeesPairsWorkdays.Add(arrayOfEmployeeIdsAndWorkdays);
                    employeesPairsAndProjectIds.Add(arrayOfEmployeeIdsAndWorkdays, new HashSet<int>());
                }
            }

            int employeesPairWhoWorkedTheMostTogetherIndex = 0;
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

                        int firstEmpId = currentProjectEmployeesWorkInfo.ElementAt(j).Key;
                        int secondEmpId = currentProjectEmployeesWorkInfo.ElementAt(k).Key;
                        int smallerEmpId = firstEmpId < secondEmpId ? firstEmpId : secondEmpId;
                        int biggerEmpId = firstEmpId > secondEmpId ? firstEmpId : secondEmpId;
                        int currentPairIndex = 0;

                        for (int t = 0; t < employeesPairsWorkdays.Count; t++)
                        {
                            if (employeesPairsWorkdays.ElementAt(t)[0] == smallerEmpId &&
                                employeesPairsWorkdays.ElementAt(t)[1] == biggerEmpId)
                            {
                                employeesPairsWorkdays.ElementAt(t)[2] += daysWorkedTogether;
                                currentPairIndex = t;
                                break;
                            }
                        }

                        for (int r = 0; r < employeesPairsAndProjectIds.Count; r++)
                        {
                            if (employeesPairsAndProjectIds.ElementAt(r).Key[0] == smallerEmpId &&
                                employeesPairsAndProjectIds.ElementAt(r).Key[1] == biggerEmpId)
                            {
                                employeesPairsAndProjectIds.ElementAt(r).Value.Add(employeesWorkInfo.ElementAt(i).Key);
                                break;
                            }
                        }
                        
                        if (employeesPairsWorkdays.ElementAt(currentPairIndex)[2] > maxDaysTwoEmployeesHaveWorkedOnTheSameProject)
                        {
                            maxDaysTwoEmployeesHaveWorkedOnTheSameProject = employeesPairsWorkdays.ElementAt(currentPairIndex)[2];
                            firstEmployeeID = currentProjectEmployeesWorkInfo.ElementAt(j).Key;
                            secondEmployeeID = currentProjectEmployeesWorkInfo.ElementAt(k).Key;
                            employeesPairWhoWorkedTheMostTogetherIndex = currentPairIndex;
                        }
                    }
                }
            }

            Console.WriteLine(String.Format("EmployeeID #1: {0}, EmployeeID #2: {1}, Days: {2}", firstEmployeeID, secondEmployeeID, maxDaysTwoEmployeesHaveWorkedOnTheSameProject));
            Console.Write("Project IDs: ");
            for (int i = 0; i < employeesPairsAndProjectIds.ElementAt(employeesPairWhoWorkedTheMostTogetherIndex).Value.Count; i++)
            {
                Console.Write(employeesPairsAndProjectIds.ElementAt(employeesPairWhoWorkedTheMostTogetherIndex).Value.ElementAt(i) + " ");
            }

            Console.WriteLine();
        }
    }
}
