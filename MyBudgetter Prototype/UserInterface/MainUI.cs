﻿using Core;
using Core.Chunk;
using Core.Data;
using Core.Model;

namespace MyBudgetter_Prototype.UserInterface
{
    public class MainUI
    {
        public MainUI()
        {
            var yearNum = 2023;

            while (true)
            {
                Console.Write("1. Add Income\n2. Add Expense\n3. Read week\n4. Read month\n5. Update record\n6. Delete record\n7. Output data chunk\n8. Exit\nPlease select one: ");

                int decision;
                int.TryParse(Console.ReadLine(), out decision);

                DateTime? dateTime;
                string category, source, userInput, frequencyInput;
                double amount;
                int frequency;
                Week week;
                Month month;
                Year year;

                switch (decision)
                {
                    // Add income
                    case 1:
                        dateTime = GetDate();
                        if (!dateTime.HasValue) break;

                        Console.Write("Category: ");
                        category = Console.ReadLine();

                        Console.Write("Amount: $");
                        double.TryParse(Console.ReadLine(), out amount);

                        Console.Write("Frequency:\n  1. Daily\n  2. Weekly\n  3. BiWeekly\n  4. Monthly\n  5. Yearly\n(Optional, you may leave this blank): ");
                        frequencyInput = Console.ReadLine();
                        int.TryParse(frequencyInput, out frequency);

                        Console.Write("Source (optional): ");
                        source = Console.ReadLine();
                        if (source == "")
                        {
                            source = null;
                        }

                        var income = new Income()
                        {
                            Category = category,
                            Date = dateTime.Value,
                            Amount = amount,
                            Source = source
                        };

                        if (frequency > 0 && frequency < 6)
                        {
                            frequency = frequency - 1;
                            income.Frequency = (Frequency)frequency;
                        }

                        Database.InsertIncome(income);

                        break;

                    // Add expense
                    case 2:
                        dateTime = GetDate();
                        if (!dateTime.HasValue) break;

                        Console.Write("Category: ");
                        category = Console.ReadLine();

                        Console.Write("Amount: $");
                        double.TryParse(Console.ReadLine(), out amount);

                        Console.Write("Frequency:\n  1. Daily\n  2. Weekly\n  3. BiWeekly\n  4. Monthly\n  5. Yearly\n(Optional, you may leave this blank): ");
                        frequencyInput = Console.ReadLine();
                        int.TryParse(frequencyInput, out frequency);

                        // Notes
                        Console.Write("Notes (optional): ");
                        var notes = Console.ReadLine();

                        // Tags
                        var tags = GetTags();

                        var expense = new Expense()
                        {
                            Category = category,
                            Date = dateTime.Value,
                            Amount = amount,
                            Notes = notes,
                            Tags = tags
                        };

                        if (frequency > 0 && frequency < 6)
                        {
                            frequency = frequency - 1;
                            expense.Frequency = (Frequency)frequency;
                        }

                        Database.InsertExpense(expense);
                        break;

                    // Read week
                    case 3:
                        dateTime = GetDate();
                        if (!dateTime.HasValue) break;

                        week = Database.GetWeek(Calendar.GetWeekRange(dateTime.Value));

                        Calendar.DisplayWeek(week);
                        break;
                       
                    // Read month
                    case 4:
                        Console.Write("Which month would you like to view:\n 1. January\n 2. February\n 3. March\n 4. April\n 5. May\n 6. June\n 7. July \n 8. August\n 9. September\n 10. October\n 11. November\n 12. December\nPlease select: ");
                        string monthValue = Console.ReadLine();

                        int monthInt;

                        int.TryParse(monthValue, out monthInt);

                        if (monthInt < 0 || monthInt > 13)
                        {
                            Console.WriteLine("\nInvalid month\n");
                            break;
                        }

                        month = Database.GetMonth(monthInt);

                        monthInt -= 1;
                        Console.WriteLine($"\nFor the month of {(CalendarEnum) monthInt}\n");

                        foreach(var _week in month.Weeks)
                        {
                            if (_week.Income.Count == 0 && _week.Expenses.Count == 0) continue;
                            Calendar.DisplayWeek(_week);
                        }

                        break;

                    // Update record
                    case 5:
                        Console.Write("Would you like to update:\n 1. Income Record\n 2. Expense Record\n\n > ");
                        var inputDecision = Console.ReadLine();
                        int inputDecisionInt;

                        int.TryParse(inputDecision, out inputDecisionInt);
                        int ID;

                        switch (inputDecisionInt)
                        {
                            case 1:
                                Console.Write("Enter the ID: ");
                                inputDecision = Console.ReadLine();
                                
                                int.TryParse(inputDecision, out ID);

                                income = Database.GetIncomeRecord(ID);

                                if (income is null)
                                {
                                    Console.WriteLine("\nThat record does not exist...\n\n");
                                    break;
                                }

                                Console.WriteLine($"This is the data associated:\n" +
                                    $" Category: {income.Category}\n" +
                                    $" Date: {income.Date}\n" +
                                    $" Amount: ${income.Amount}\n" +
                                    $" Frequency: {income.Frequency}\n" +
                                    $" Source: {income.Source}\n");

                                Console.WriteLine("Please update this record, if you don't want to change something just leave the field blank:");

                                Console.Write("Category (leave blank to not change): ");
                                category = Console.ReadLine();

                                if (category != "")
                                {
                                    income.Category = category;
                                }

                                Console.Write("Amount (leave blank to not change): $");
                                var amountInput = Console.ReadLine();

                                if (amountInput != "")
                                {
                                    double.TryParse(amountInput, out amount);
                                    income.Amount = amount;
                                }

                                Console.Write("Frequency:\n  1. Daily\n  2. Weekly\n  3. BiWeekly\n  4. Monthly\n  5. Yearly\n (leave blank to not change): ");
                                frequencyInput = Console.ReadLine();
                                if (frequencyInput != "")
                                {
                                    int.TryParse(frequencyInput, out frequency);
                                    if (frequency > 0 && frequency < 6)
                                    {
                                        frequency = frequency - 1;
                                        income.Frequency = (Frequency)frequency;
                                    }
                                }

                                Console.Write("Source (leave blank to not change): ");
                                source = Console.ReadLine();
                                
                                if (source != "")
                                {
                                    income.Source = source;
                                }

                                Database.UpdateIncomeRecord(income);

                                break;

                            case 2:
                                Console.Write("Enter the ID: ");
                                inputDecision = Console.ReadLine();

                                int.TryParse(inputDecision, out ID);

                                expense = Database.GetExpenseRecord(ID);

                                if (expense is null)
                                {
                                    Console.WriteLine("\nThat record does not exist...\n\n");
                                    break;
                                }

                                Console.Write($"This is the data associated:\n" +
                                    $" Category: {expense.Category}\n" +
                                    $" Date: {expense.Date}\n" +
                                    $" Amount: ${expense.Amount}\n" +
                                    $" Frequency: {expense.Frequency}\n" +
                                    $" Notes: {expense.Notes}\n" +
                                    $" Tags: ");

                                if (expense.Tags is not null)
                                    foreach(var tag in expense.Tags)
                                    {
                                        if (tag == expense.Tags[expense.Tags.Count - 1])
                                        {
                                            Console.Write(tag + "\n");
                                            continue;
                                        }
                                        Console.Write($"{tag}, ");
                                    }
                                Console.WriteLine();

                                Console.WriteLine("Please update this record, if you don't want to change something just leave the field blank:");

                                Console.Write("Category (leave blank to not change): ");
                                category = Console.ReadLine();

                                if (category != "")
                                {
                                    expense.Category = category;
                                }

                                Console.Write("Amount (leave blank to not change): $");
                                amountInput = Console.ReadLine();

                                if (amountInput != "")
                                {
                                    double.TryParse(amountInput, out amount);
                                    expense.Amount = amount;
                                }

                                Console.Write("Frequency:\n  1. Daily\n  2. Weekly\n  3. BiWeekly\n  4. Monthly\n  5. Yearly\n (leave blank to not change): ");
                                frequencyInput = Console.ReadLine();
                                if (frequencyInput != "")
                                {
                                    int.TryParse(frequencyInput, out frequency);
                                    if (frequency > 0 && frequency < 6)
                                    {
                                        frequency = frequency - 1;
                                        expense.Frequency = (Frequency)frequency;
                                    }
                                }

                                Console.Write("Notes (optional): ");
                                notes = Console.ReadLine();

                                if (notes != "")
                                {
                                    expense.Notes = notes;
                                }

                                if (expense.Tags is null) expense.Tags = new List<string>();
                                Console.Write("Here are all the tags:");
                                foreach (var tag in expense.Tags)
                                {
                                    if (tag == expense.Tags[expense.Tags.Count - 1])
                                    {
                                        Console.Write(tag + "\n");
                                        continue;
                                    }
                                    Console.Write($"{tag}, ");
                                }

                                Console.WriteLine("\nType \"remove [tag_name]\" to remove a tag, or just write a tag name to add a tag (enter nothing to exit)\n");
                                while (true)
                                {
                                    Console.Write(" > ");
                                    userInput = Console.ReadLine();

                                    if (userInput == "") break;

                                    string[] inputs = userInput.Split(" ");

                                    if (inputs.Length == 1)
                                    {
                                        // create tag
                                        bool tagExistsAlready = false;
                                        // check if it exists
                                        foreach (var tag in expense.Tags)
                                        {
                                            if (tag.ToLower() == inputs[0].ToLower())
                                            {
                                                tagExistsAlready = true;
                                                break;
                                            }
                                        }

                                        if (!tagExistsAlready)
                                        {
                                            expense.Tags.Add(inputs[0]);
                                        }
                                    }
                                    
                                    if (inputs.Length == 2 && inputs[0].ToLower() == "remove")
                                    {
                                        var tagToRemove = inputs[1];
                                        var tagsToRemove = new List<string>();
                                        foreach(var tag in expense.Tags)
                                        {
                                            if (tag.ToLower() == tagToRemove.ToLower())
                                            {
                                                tagsToRemove.Add(tag);
                                            }
                                        }

                                        foreach(var tag in tagsToRemove)
                                        {
                                            expense.Tags.Remove(tag);
                                        }
                                    }
                                }

                                Database.UpdateExpenseRecord(expense);

                                break;
                        }

                        break;

                    // Delete record
                    case 6:
                        Console.Write("Which would you like to delete from:\n 1. Income Record\n 2. Expense Record\n\n > ");
                        inputDecision = Console.ReadLine();

                        int.TryParse(inputDecision, out inputDecisionInt);

                        Console.Write("Enter the ID: ");
                        inputDecision = Console.ReadLine();

                        int.TryParse(inputDecision, out ID);

                        if (inputDecisionInt == 1) Database.Delete(ID, "IncomeRecord");
                        if (inputDecisionInt == 2) Database.Delete(ID, "ExpenseRecord");

                        break;

                    // Output data chunk
                    case 7:
                        DateTime startDate, endDate;

                        Console.Write("Please insert the start date (format: dd/MM/yyyy): ");
                        userInput = Console.ReadLine();

                        // Try to parse the user input into a DateTime
                        if (DateTime.TryParseExact(userInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime userDate))
                        {
                            // Successfully parsed
                            startDate = userDate;
                        }
                        else
                        {
                            // Parsing failed
                            Console.WriteLine("Invalid date format. Please enter a valid date.");
                            break;
                        }

                        Console.Write("Please insert the end date (format: dd/MM/yyyy): ");
                        userInput = Console.ReadLine();

                        // Try to parse the user input into a DateTime
                        if (DateTime.TryParseExact(userInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out userDate))
                        {
                            // Successfully parsed
                            endDate = userDate;
                        }
                        else
                        {
                            // Parsing failed
                            Console.WriteLine("Invalid date format. Please enter a valid date.");
                            break;
                        }

                        ChunkWriter.WriteToJSON(startDate, endDate);

                        break;

                    case 8:
                        System.Environment.Exit(0);
                        break;

                    default:
                        break;
                }

                Console.WriteLine();
            }
        }

        public static DateTime? GetDate()
        {
            Console.Write("Please insert the date (format: dd/MM/yyyy): ");
            string userInput = Console.ReadLine();

            // Try to parse the user input into a DateTime
            if (DateTime.TryParseExact(userInput, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime userDate))
            {
                // Successfully parsed
                return userDate;
            }
            else
            {
                // Parsing failed
                Console.WriteLine("Invalid date format. Please enter a valid date.");
                return null;
            }
        }
        public static List<string> GetTags()
        {
            var tags = new List<string>();
            Console.WriteLine("Enter a tag associated with this expense (optional, enter nothing to continue): ");
            while (true)
            {

                Console.Write(" > ");
                var tag = Console.ReadLine();
                if (tag == "")
                {
                    break;
                }

                tags.Add(tag);
            }

            return tags;
        }
    }
}
