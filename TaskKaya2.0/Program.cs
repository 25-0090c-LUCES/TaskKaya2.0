namespace TaskKaya2._0
{

    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    internal class Program
    {
        // USERNAMES AND PASSWORDS ONLY
        static List<string> UserNames = new List<string>();
        static List<string> UserPasswords = new List<string>();
        static List<string> UserLoc = new List<string>();
        static List<string> UserConNum = new List<string>();

        // PARALLEL LISTS FOR JOBS (Replaces Public Class Job)
        static List<string> JobIDs = new List<string>();
        static List<string> JobTitles = new List<string>();
        static List<string> JobBudgets = new List<string>();
        static List<string> JobEmployers = new List<string>();
        static List<string> JobWorkers = new List<string>();
        static List<string> JobStatuses = new List<string>();
        static List<string> JobRatings = new List<string>();
        static List<string> JobLocations = new List<string>();

        // Engine queues and stacks for simple text parsing
        static Queue<string> LiveNotifications = new Queue<string>();
        static Stack<string> TransactionHistory = new Stack<string>();

        static void Main(string[] args)
        {
            LoadData();

            while (true)
            {
                string currentUser = LoginPortal();
                if (currentUser == "") continue;

                RunUniversalDashboard(currentUser);
            }
        }
        static string LoginPortal()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                Console.WriteLine(@"
                     ████████╗ █████╗ ███████╗██╗  ██╗██╗  ██╗ █████╗ ██╗   ██╗ █████╗ 
                     ╚══██╔══╝██╔══██╗██╔════╝██║ ██╔╝██║ ██╔╝██╔══██╗╚██╗ ██╔╝██╔══██╗
                        ██║   ███████║███████╗█████╔╝ █████╔╝ ███████║ ╚████╔╝ ███████║
                        ██║   ██╔══██║╚════██║██╔═██╗ ██╔═██╗ ██╔══██║  ╚██╔╝  ██╔══██║
                        ██║   ██║  ██║███████║██║  ██╗██║  ██╗██║  ██║   ██║   ██║  ██║
                        ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝
");

                Console.ResetColor();

                Console.WriteLine("[1]. Login");
                Console.WriteLine("[2]. Register");
                Console.WriteLine("[0]. Exit");
                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid input numbers only. Please Try Again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                if (choice == 1)
                {
                    int maxAttempts = 3;
                    int attempts = 0;

                    while (attempts < maxAttempts)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("(Enter / to return to main menu.)");
                        Console.WriteLine("===============================================================================");
                        Console.WriteLine("                              LOGIN");
                        Console.WriteLine("===============================================================================");
                        Console.ResetColor();

                        Console.Write("Enter Username: ");
                        string u = Console.ReadLine().Trim();
                        if (u == "/")
                        {
                            break;
                        }
                        if (string.IsNullOrWhiteSpace(u))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Username cannot be empty. Please try again.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }


                        Console.Write("Enter Password: ");
                        string p = Console.ReadLine().Trim();
                        if (p == "/")
                        {
                            break;
                        }
                        if (string.IsNullOrWhiteSpace(p))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Username cannot be empty.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }

                        bool found = false;
                        for (int i = 0; i < UserNames.Count; i++)
                        {
                            if (UserNames[i].Equals(u, StringComparison.OrdinalIgnoreCase) && UserPasswords[i] == p)
                            {
                                found = true;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\n [SUCCESS] LoggedIn Successfully! Welcome back, " + UserNames[i] + "!");
                                Console.ResetColor();
                                Thread.Sleep(1200);
                                return UserNames[i];
                            }
                        }

                        if (!found)
                        {
                            attempts++;
                            int left = maxAttempts - attempts;

                            if (left == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Too many failed attempts. Access temporarily locked.");
                                Console.ResetColor();
                                Thread.Sleep(3000);
                                attempts = 0;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Incorrect username or password. Please try Again.");
                                Console.ResetColor();
                                Thread.Sleep(1200);
                            }
                        }
                    }
                }
                else if (choice == 2)
                {
                    string u = "";
                    string p = "";
                    string l = "";
                    string c = "";
                    bool cancelled = false;

                    // USERNAME
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("(Enter / to return to main menu.)");
                        Console.WriteLine("=========== REGISTER NEW ACCOUNT ===========");

                        Console.Write("Enter Username: ");
                        u = Console.ReadLine().Trim();
                        if (u == "/")
                        {
                            cancelled = true; break;
                        }
                        bool exists = false;
                        for (int i = 0; i < UserNames.Count; i++)
                        {
                            if (UserNames[i].Equals(u, StringComparison.OrdinalIgnoreCase))
                            { exists = true; break; }
                        }

                        if (exists)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Username already exists. Please try again.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(u))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Username cannot be empty. Please try again.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }

                        break;
                    }

                    if (!cancelled)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("(Enter / to return to main menu.)");
                            Console.WriteLine("=========== REGISTER NEW ACCOUNT ===========");
                            Console.WriteLine($"Enter Username: {u}");
                            Console.Write("Enter Password: ");
                            p = Console.ReadLine().Trim();
                            if (p == "/")
                            {
                                cancelled = true; break;
                            }
                            if (string.IsNullOrWhiteSpace(p))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Password cannot be empty. Please try again.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        }
                    }


                    // LOCATION
                    if (!cancelled)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("(Enter / to return to main menu.)");
                            Console.WriteLine("=========== REGISTER NEW ACCOUNT ===========");
                            Console.WriteLine($"Enter Username: {u}");
                            Console.WriteLine($"Enter Password: {p}");
                            Console.Write("Enter Location: ");
                            l = Console.ReadLine().Trim();
                            if (l == "/")
                            {
                                cancelled = true; break;
                            }
                            if (string.IsNullOrWhiteSpace(l))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Location cannot be empty. Please try again.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        }
                    }

                    if (!cancelled)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("(Enter / to return to main menu.)");
                            Console.WriteLine("=========== REGISTER NEW ACCOUNT ===========");
                            Console.WriteLine($"Enter Username: {u}");
                            Console.WriteLine($"Enter Password: {p}");
                            Console.WriteLine($"Enter Location: {l}");
                            Console.Write("Enter Contact Number: ");
                            c = Console.ReadLine().Trim();
                            if (c == "/")
                            {
                                cancelled = true; break;
                            }

                            if (c.Length != 11 || !c.All(char.IsDigit))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Contact number must contain exactly 11 digits. Please try registering again.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            bool contactExists = false;
                            for (int i = 0; i < UserConNum.Count; i++)
                            {
                                if (UserConNum[i] == c) { contactExists = true; break; }
                            }

                            if (contactExists)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Contact number is already registered. Please try registering another number.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        }
                    }
                    if (!cancelled)
                    {
                        UserNames.Add(u);
                        UserPasswords.Add(p);
                        UserLoc.Add(l);
                        UserConNum.Add(c);

                        SaveData();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[SUCCESS] Account created successfully!");
                        Console.WriteLine("You may now log in.");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                    }
                }
                else if (choice == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nThank you for using TaskKaya!");
                    Console.ResetColor();

                    Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice. PLease try again.");
                    Console.ResetColor();

                    Thread.Sleep(1000);
                }
            }

        }
     
        static void RunUniversalDashboard(string username)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================================================");
                Console.WriteLine("  DASHBOARD | USER: " + username.ToUpper());
                Console.WriteLine("===============================================================================");
                Console.ResetColor();

                int alertCount = 0;
                foreach (string notif in LiveNotifications)
                {
                    string[] parts = notif.Split('|');
                    if (parts.Length >= 3 && parts[0].Equals(username, StringComparison.OrdinalIgnoreCase) && parts[2] == "UNREAD")
                    {
                        alertCount++;
                    }
                }

                if (alertCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(" 🔔 [ALERT] You have (" + alertCount + ") unread message(s) in your Inbox!\n");
                    Console.ResetColor();
                }

                Console.WriteLine(" [1] Find a Job / Apply");
                Console.WriteLine(" [2] Post a Job");
                Console.WriteLine(" [3] Review Applicants & Completions");
                Console.WriteLine(" [4] Track My Ongoing Work");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" [5] Check Notifications Inbox (Approved/Finished/Declined)");
                Console.ResetColor();
                Console.WriteLine(" [6] View History Ledger");
                Console.WriteLine(" [0] Logout");
                Console.WriteLine("-------------------------------------------------------------------------------");

                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                switch (choice)
                {
                    case 1: BrowseJobsBoard(username); break;
                    case 2: PostJob(username); break;
                    case 3: ReviewAndVerifyTasks(username); break;
                    case 4: TrackWorkerContracts(username); break;
                    case 5: CheckAndDisplayNotifications(username); break;
                    case 6: ViewMyLedger(username); break;
                    case 0: return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[ERROR] Invalid choice.");
                        Console.ResetColor();
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
        static void PostJob(string username)
        {
            Console.Clear();

            Console.WriteLine("(Enter / to return.)");
            Console.WriteLine("=== POST A JOB ===");

            string title = "";
            while (true)
            {
                Console.Write("Job Title: ");
                title = Console.ReadLine().Trim();

                if (title == "/")
                    return;

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Job title cannot be empty.");
                    Console.ResetColor();
                    Thread.Sleep(1000);

                    Console.Clear();
                    Console.WriteLine("(Enter / to return.)");
                    Console.WriteLine("=== POST A JOB ===");
                    continue;
                }

                break;
            }

            string location = "";
            while (true)
            {
                Console.Write("Job Location: ");
                location = Console.ReadLine().Trim();

                if (location == "/")
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(location))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Job location cannot be empty.");
                    Console.ResetColor();
                    Thread.Sleep(1000);

                    Console.Clear();
                    Console.WriteLine("(Enter / to return.)");
                    Console.WriteLine("=== POST A JOB ===");
                    Console.WriteLine($"Job Title: {title}");
                    continue;
                }

                break;
            }

            string budget = "";
            while (true)
            {
                Console.Write("Budget (PHP): ");
                budget = Console.ReadLine().Trim();
                if (budget == "/")
                {
                    return;
                }

                if (string.IsNullOrWhiteSpace(budget))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Budget cannot be empty.");
                    Console.ResetColor();
                    Thread.Sleep(1000);

                    Console.Clear();
                    Console.WriteLine("(Enter / to return.)");
                    Console.WriteLine("=== POST A JOB ===");
                    Console.WriteLine($"Job Title: {title}");
                    Console.WriteLine($"Job Location: {location}");
                    continue;
                }

                if (!decimal.TryParse(budget, out decimal budgetValue) || budgetValue <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Please enter a valid amount greater than 0.");
                    Console.ResetColor();
                    Thread.Sleep(1000);

                    Console.Clear();
                    Console.WriteLine("(Enter / to return.)");
                    Console.WriteLine("=== POST A JOB ===");
                    Console.WriteLine($"Job Title: {title}");
                    Console.WriteLine($"Job Location: {location}");
                    continue;
                }

                break;
            }

            string newID = "JOB" + new Random().Next(100, 999);

            JobIDs.Add(newID);
            JobTitles.Add(title);
            JobLocations.Add(location);
            JobBudgets.Add(budget);
            JobEmployers.Add(username);
            JobWorkers.Add("None");
            JobStatuses.Add("AVAILABLE");
            JobRatings.Add("N/A");

            SaveData();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[SUCCESS] Job {newID} posted successfully!");
            Console.ResetColor();

            Pause();
        }
        static void BrowseJobsBoard(string username)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== FIND A JOB ===");
                Console.WriteLine("[1] View All Jobs");
                Console.WriteLine("[2] Filter by Location");
                Console.WriteLine("[0] Return");

                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid input numbers only. Please Try Again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                if (choice == 0)
                    return;

                string filterLocation = "";

                if (choice == 1)
                {
                    filterLocation = "";
                }
                else if (choice == 2)
                {
                    Console.Clear();
                    Console.Write("Enter Location: ");
                    filterLocation = Console.ReadLine().Trim();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice. Please Try Again");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                Console.Clear();
                Console.WriteLine("=== AVAILABLE JOBS ===");

                int count = 0;

                for (int i = 0; i < JobIDs.Count; i++)
                {
                    bool available =
                        JobStatuses[i] == "AVAILABLE" &&
                        !JobEmployers[i].Equals(username, StringComparison.OrdinalIgnoreCase);

                    bool locationMatch =
                        choice == 1 ||
                        JobLocations[i].Equals(filterLocation, StringComparison.OrdinalIgnoreCase);

                    if (available && locationMatch)
                    {
                        Console.WriteLine(
                            $"[ID: {JobIDs[i]}] {JobTitles[i]} | " +
                            $"Location: {JobLocations[i]} | " +
                            $"Budget: PHP {JobBudgets[i]} | " +
                            $"Employer: {JobEmployers[i]}"
                        );
                        count++;
                    }
                }

                if (count == 0)
                {
                    Console.WriteLine("\nNo jobs found.");
                    Pause();
                    continue;
                }

                Console.Write("\nEnter Job ID to apply for (or press Enter to cancel): ");
                string targetId = Console.ReadLine().Trim().ToUpper();

                if (targetId == "")
                    continue;

                int matchIndex = -1;

                for (int i = 0; i < JobIDs.Count; i++)
                {
                    if (JobIDs[i] == targetId &&
                        JobStatuses[i] == "AVAILABLE")
                    {
                        matchIndex = i;
                        break;
                    }
                }

                if (matchIndex != -1)
                {
                    bool alreadyApplied = false;
                    foreach (string notif in LiveNotifications)
                    {
                        string[] parts = notif.Split('|');
                        if (parts.Length >= 4 &&
                            parts[0].Equals(JobEmployers[matchIndex], StringComparison.OrdinalIgnoreCase) &&
                            parts[1] == "APPLIED" &&
                            parts[3] == username + ";" + JobIDs[matchIndex])
                        {
                            alreadyApplied = true;
                            break;
                        }
                    }

                    if (alreadyApplied)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[ERROR] You have already applied for this job.");
                        Console.ResetColor();
                    }
                    else
                    {
                        AddNotification(
                            JobEmployers[matchIndex],
                            "APPLIED",
                            username + ";" + JobIDs[matchIndex]
                        );
                        SaveData();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[SUCCESS] Application submitted! Waiting for Employer review.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Job ID not found or unavailable.");
                    Console.ResetColor();
                }

                Pause();
            }
        }
        static string GetUserAverageRating(string workerName)
        {
            int totalStars = 0;
            int jobCount = 0;

            for (int i = 0; i < JobIDs.Count; i++)
            {
                if (JobWorkers[i].Equals(workerName, StringComparison.OrdinalIgnoreCase) && JobStatuses[i] == "COMPLETED")
                {
                    string stars = JobRatings[i];
                    if (!string.IsNullOrEmpty(stars) && stars != "N/A")
                    {
                        totalStars += stars.Length; // Length gives count of '★' characters
                        jobCount++;
                    }
                }
            }

            if (jobCount == 0) return "No performance reviews yet";
            int average = (int)Math.Round((double)totalStars / jobCount);
            return $"{new string('★', average)} ({jobCount} jobs completed)";
        }
        static void ReviewAndVerifyTasks(string username)
        {
            Console.Clear();
            Console.WriteLine("=== REVIEW HUB ===");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- PENDING JOB APPLICANTS ---");
            Console.ResetColor();
            int applicantCount = 0;
            List<string> applicantMapping = new List<string>();

            foreach (string notif in LiveNotifications)
            {
                string[] parts = notif.Split('|');
                if (parts.Length >= 4 && parts[1] == "APPLIED" && parts[0].Equals(username, StringComparison.OrdinalIgnoreCase))
                {
                    string[] payload = parts[3].Split(';');
                    if (payload.Length >= 2)
                    {
                        string workerName = payload[0];
                        string targetJobId = payload[1];

                        int jobIdx = -1;
                        for (int j = 0; j < JobIDs.Count; j++)
                        {
                            if (JobIDs[j] == targetJobId)
                            {
                                jobIdx = j;
                                break;
                            }
                        }

                        if (jobIdx != -1 && JobStatuses[jobIdx] == "AVAILABLE")
                        {
                            string workerScore = GetUserAverageRating(workerName);
                            Console.WriteLine($" -> [{applicantCount + 1}] Job ID: {targetJobId} ({JobTitles[jobIdx]}) | Applicant: {workerName} | Rating: {workerScore}");
                            applicantMapping.Add(parts[3]);
                            applicantCount++;
                        }
                    }
                }
            }
            if (applicantCount == 0) Console.WriteLine("No active entry applications at this time.");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n--- COMPLETIONS PENDING PAYMENT VERIFICATION ---");
            Console.ResetColor();
            int pendingCount = 0;
            for (int i = 0; i < JobIDs.Count; i++)
            {
                if (JobEmployers[i].Equals(username, StringComparison.OrdinalIgnoreCase) && JobStatuses[i] == "PENDING_VERIFICATION")
                {
                    Console.WriteLine(" -> [ID: " + JobIDs[i] + "] " + JobTitles[i] + " | Worker: " + JobWorkers[i] + " | Cost: PHP " + JobBudgets[i]);
                    pendingCount++;
                }
            }
            if (pendingCount == 0) Console.WriteLine("No completed tasks at the moment.");

            if (applicantCount == 0 && pendingCount == 0)
            {
                Pause();
                return;
            }

            Console.Write("\nDo you want to evaluate an Applicant [A] or a Completion Verification [C]? (A/C): ");
            string choiceMode = Console.ReadLine().Trim().ToUpper();

            if (choiceMode == "A" && applicantCount > 0)
            {
                int appIndex;
                while (true)
                {
                    Console.Write("Enter number of applicant choice: ");
                    if (int.TryParse(Console.ReadLine(), out appIndex)) break;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Invalid input numbers only.");
                    Console.ResetColor();
                }
                appIndex--;

                if (appIndex >= 0 && appIndex < applicantMapping.Count)
                {
                    string[] payload = applicantMapping[appIndex].Split(';');
                    string workerName = payload[0];
                    string targetJobId = payload[1];

                    int matchIndex = -1;
                    for (int j = 0; j < JobIDs.Count; j++)
                    {
                        if (JobIDs[j] == targetJobId) { matchIndex = j; break; }
                    }

                    if (matchIndex != -1)
                    {
                        Console.Clear();
                        Console.WriteLine($"Evaluating Applicant for Job: {JobIDs[matchIndex]} ({JobTitles[matchIndex]})");
                        Console.WriteLine($"Applicant Username: {workerName}");
                        Console.WriteLine($"Applicant Rating Baseline: {GetUserAverageRating(workerName)}");
                        Console.WriteLine("-------------------------------------------------------------------------------");
                        Console.WriteLine(" [1] Accept Applicant & Move Contract into Active Production State");
                        Console.WriteLine(" [2] Decline/Reject Applicant application");

                        int decision;
                        while (true)
                        {
                            Console.Write("Enter action choice: ");
                            if (int.TryParse(Console.ReadLine(), out decision)) break;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Invalid input numbers only.");
                            Console.ResetColor();
                        }

                        if (decision == 1)
                        {
                            JobStatuses[matchIndex] = "ONGOING";
                            JobWorkers[matchIndex] = workerName;
                            AddNotification(workerName, "APPROVED", $"Great news! Employer '{username}' has ACCEPTED you for the job '{JobTitles[matchIndex]}' (ID: {JobIDs[matchIndex]}). You can start work now.");
                            RemoveApplicationNotification(username, workerName, targetJobId);
                            AutoDeclineOtherApplicants(username, targetJobId, JobTitles[matchIndex], workerName);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n[SUCCESS] Contract finalized. Worker can now safely track ongoing duties.");
                            Console.ResetColor();
                        }
                        else if (decision == 2)
                        {
                            AddNotification(workerName, "DECLINED", $"Your application for '{JobTitles[matchIndex]}' (ID: {JobIDs[matchIndex]}) was declined by the employer.");
                            RemoveApplicationNotification(username, workerName, targetJobId);
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n[INFO] Application rejected. Other applicants can still be reviewed.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Invalid choice.");
                            Console.ResetColor();
                        }
                        SaveData();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Invalid selection index.");
                    Console.ResetColor();
                }
                Pause();
            }
            else if (choiceMode == "C" && pendingCount > 0)
            {
                Console.Write("Enter Job ID to evaluate completion: ");
                string targetId = Console.ReadLine().Trim().ToUpper();

                int matchIndex = -1;
                for (int i = 0; i < JobIDs.Count; i++)
                {
                    if (JobIDs[i] == targetId &&
                        JobEmployers[i].Equals(username, StringComparison.OrdinalIgnoreCase) &&
                        JobStatuses[i] == "PENDING_VERIFICATION")
                    {
                        matchIndex = i;
                        break;
                    }
                }

                if (matchIndex == -1)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Match system index mismatch or permissions violation.");
                    Console.ResetColor();
                    Pause();
                    return;
                }

                Console.Clear();
                Console.WriteLine("Evaluating Job Output: " + JobIDs[matchIndex] + " (" + JobTitles[matchIndex] + ")");
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine(" [1] Approve Completion & Release Payment");
                Console.WriteLine(" [2] Reject & Request Revision");

                int choice;
                while (true)
                {
                    Console.Write("Enter number of choice: ");
                    if (int.TryParse(Console.ReadLine(), out choice)) break;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[ERROR] Invalid input numbers only.");
                    Console.ResetColor();
                }

                if (choice == 1)
                {
                    JobStatuses[matchIndex] = "COMPLETED";

                    int stars;
                    while (true)
                    {
                        Console.Write("Rate worker performance (1 to 5 Stars): ");
                        if (!int.TryParse(Console.ReadLine(), out stars))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Invalid input numbers only.");
                            Console.ResetColor();
                            continue;
                        }
                        if (stars < 1 || stars > 5)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Please enter a number between 1 and 5 only.");
                            Console.ResetColor();
                            continue;
                        }
                        break;
                    }
                    JobRatings[matchIndex] = new string('★', stars);

                    string stackPayload = JobIDs[matchIndex] + "|" + JobTitles[matchIndex] + "|" + JobEmployers[matchIndex] + "|" + JobWorkers[matchIndex] + "|" + JobBudgets[matchIndex] + "|" + JobRatings[matchIndex];
                    TransactionHistory.Push(stackPayload);

                    AddNotification(JobWorkers[matchIndex], "FINISHED", "Payment Released! '" + username + "' marked your work on '" + JobTitles[matchIndex] + "' as complete. Rating: " + JobRatings[matchIndex]);
                    SaveData();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n[SUCCESS] Job finalized and paid.");
                    Console.ResetColor();
                }
                else if (choice == 2)
                {
                    JobStatuses[matchIndex] = "ONGOING";
                    AddNotification(JobWorkers[matchIndex], "DECLINED", "[!] Work Rejected: '" + username + "' requested edits for job '" + JobTitles[matchIndex] + "'.");
                    SaveData();
                    Console.WriteLine("\n[INFO] Sent back to worker for corrections.");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice.");
                    Console.ResetColor();
                }
                Pause();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[ERROR] Invalid selection mode or no items available in that section.");
                Console.ResetColor();
                Pause();
            }
        }
        static void RemoveApplicationNotification(string employer, string worker, string jobId)
        {
            List<string> retainedNotifs = new List<string>();
            string targetPayload = worker + ";" + jobId;
            foreach (string n in LiveNotifications)
            {
                string[] parts = n.Split('|');
                if (parts.Length >= 4 &&
                    parts[0].Equals(employer, StringComparison.OrdinalIgnoreCase) &&
                    parts[1] == "APPLIED" &&
                    parts[3] == targetPayload)
                {
                    continue;
                }
                retainedNotifs.Add(n);
            }

            LiveNotifications.Clear();
            foreach (string n in retainedNotifs) LiveNotifications.Enqueue(n);
        }
        static void AutoDeclineOtherApplicants(string employer, string jobId, string jobTitle, string acceptedWorker)
        {
            List<string> retainedNotifs = new List<string>();
            List<string> workersToNotify = new List<string>();

            foreach (string n in LiveNotifications)
            {
                string[] parts = n.Split('|');
                bool isOtherApplicant = false;

                if (parts.Length >= 4 &&
                    parts[0].Equals(employer, StringComparison.OrdinalIgnoreCase) &&
                    parts[1] == "APPLIED")
                {
                    string[] payload = parts[3].Split(';');
                    if (payload.Length >= 2 &&
                        payload[1] == jobId &&
                        !payload[0].Equals(acceptedWorker, StringComparison.OrdinalIgnoreCase))
                    {
                        isOtherApplicant = true;
                        workersToNotify.Add(payload[0]);
                    }
                }

                if (isOtherApplicant)
                {
                    continue;
                }
                retainedNotifs.Add(n);
            }

            LiveNotifications.Clear();
            foreach (string n in retainedNotifs) LiveNotifications.Enqueue(n);

            foreach (string worker in workersToNotify)
            {
                AddNotification(worker, "DECLINED", "Sorry! The job '" + jobTitle + "' (ID: " + jobId + ") has already been filled by another applicant.");
            }
        }
        static void TrackWorkerContracts(string username)
        {
            Console.Clear();
            Console.WriteLine("=== TRACK MY ONGOING WORK ===");

            int ongoingCount = 0;
            for (int i = 0; i < JobIDs.Count; i++)
            {
                if (JobWorkers[i].Equals(username, StringComparison.OrdinalIgnoreCase) && JobStatuses[i] == "ONGOING")
                {
                    Console.WriteLine(" -> [ID: " + JobIDs[i] + "] " + JobTitles[i] + " | Earnings: PHP " + JobBudgets[i] + " | Client: " + JobEmployers[i]);
                    ongoingCount++;
                }
            }

            if (ongoingCount == 0)
            {
                Console.WriteLine("\nYou have no active approved jobs currently in active progress state.");
                Pause();
                return;
            }

            Console.Write("\nEnter Job ID to submit for approval: ");
            string targetId = Console.ReadLine().Trim().ToUpper();

            int matchIndex = -1;
            for (int i = 0; i < JobIDs.Count; i++)
            {
                if (JobIDs[i] == targetId && JobWorkers[i].Equals(username, StringComparison.OrdinalIgnoreCase) && JobStatuses[i] == "ONGOING")
                {
                    matchIndex = i;
                    break;
                }
            }

            if (matchIndex != -1)
            {
                JobStatuses[matchIndex] = "PENDING_VERIFICATION";
                AddNotification(JobEmployers[matchIndex], "FINISHED", "Review Required: '" + username + "' completed '" + JobTitles[matchIndex] + "' (ID: " + JobIDs[matchIndex] + ").");
                SaveData();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[SUCCESS] Job submitted! Waiting for employer approval.");
                Console.ResetColor();
                Pause();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERROR] Job ID not found or not yours.");
                Console.ResetColor();
                Pause();
            }
        }
        static void CheckAndDisplayNotifications(string username)
        {
            while (true)
            {
                List<string> personalNotifications = new List<string>();
                int unreadCount = 0;
                foreach (string notif in LiveNotifications)
                {
                    string[] parts = notif.Split('|');
                    if (parts[0].Equals(username, StringComparison.OrdinalIgnoreCase))
                    {
                        personalNotifications.Add(notif);
                        if (parts[2] == "UNREAD") unreadCount++;
                    }
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("===============================================================================");
                Console.WriteLine("    NOTIFICATIONS INBOX | TOTAL: " + personalNotifications.Count + " | UNREAD: " + unreadCount);
                Console.WriteLine("===============================================================================");
                Console.ResetColor();
                Console.WriteLine(" [1] View Folder: Approved/Applied Jobs");
                Console.WriteLine(" [2] View Folder: Jobs Finished / Submission Reviews");
                Console.WriteLine(" [3] View Folder: Declined / Returned Job Tasks");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" [4] Clear ALL My Notifications");
                Console.ResetColor();
                Console.WriteLine(" [0] Return to Main Menu");
                Console.WriteLine("-------------------------------------------------------------------------------");

                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }
                if (choice == 0) return;

                if (choice == 4)
                {
                    List<string> otherUsersNotifs = new List<string>();
                    foreach (string n in LiveNotifications)
                    {
                        string[] parts = n.Split('|');
                        if (!parts[0].Equals(username, StringComparison.OrdinalIgnoreCase))
                        {
                            otherUsersNotifs.Add(n);
                        }
                    }

                    LiveNotifications.Clear();
                    foreach (string n in otherUsersNotifs) LiveNotifications.Enqueue(n);

                    SaveData();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n[SUCCESS] Inbox completely cleared.");
                    Console.ResetColor();
                    Pause();
                    continue;
                }

                string targetedCategoryCode = "";
                if (choice == 1) { targetedCategoryCode = "APPROVED"; }
                else if (choice == 2) { targetedCategoryCode = "FINISHED"; }
                else if (choice == 3) { targetedCategoryCode = "DECLINED"; }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=== FOLDER INBOX: " + targetedCategoryCode + " ===");
                Console.WriteLine("-------------------------------------------------------------------------------");

                int displayCount = 0;
                List<string> updatedNotifs = new List<string>();

                foreach (string n in LiveNotifications)
                {
                    string[] parts = n.Split('|');

                    if (parts[0].Equals(username, StringComparison.OrdinalIgnoreCase) && parts[1] == targetedCategoryCode)
                    {
                        Console.WriteLine(" * " + parts[3]);
                        displayCount++;
                        updatedNotifs.Add(parts[0] + "|" + parts[1] + "|READ|" + parts[3]);
                    }
                    else
                    {
                        updatedNotifs.Add(n);
                    }
                }

                LiveNotifications.Clear();
                foreach (string n in updatedNotifs) LiveNotifications.Enqueue(n);
                SaveData();

                if (displayCount == 0)
                {
                    Console.WriteLine("No notifications in this folder.");
                }

                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.ResetColor();
                Pause();
            }
        }
        static void AddNotification(string target, string category, string message)
        {
            LiveNotifications.Enqueue(target + "|" + category + "|UNREAD|" + message);
            SaveData();
        }
        static void ViewMyLedger(string username)
        {
            Console.Clear();
            Console.WriteLine("=== MY HISTORY LEDGER ===");

            int displayCount = 0;
            foreach (string log in TransactionHistory)
            {
                string[] p = log.Split('|');
                if (p.Length >= 6)
                {
                    bool isEmployer = p[2].Equals(username, StringComparison.OrdinalIgnoreCase);
                    bool isWorker = p[3].Equals(username, StringComparison.OrdinalIgnoreCase);

                    if (isEmployer || isWorker)
                    {
                        string role = isEmployer ? "Employer" : "Worker";
                        Console.WriteLine(" -> ID: " + p[0] + " | Job: " + p[1] + " | Employer: " + p[2] + " | Worker: " + p[3] + " | Paid: PHP " + p[4] + " | Rating: " + p[5] + " | Your Role: " + role);
                        displayCount++;
                    }
                }
            }

            if (displayCount == 0)
            {
                Console.WriteLine("\nNo completed transactions in your history yet.");
            }

            Pause();
        }
        static void SaveData()
        {
            List<string> userLines = new List<string>();
            for (int i = 0; i < UserNames.Count; i++)
                userLines.Add(UserNames[i] + "," + UserPasswords[i] + "," + UserLoc[i] + "," + UserConNum[i]);
            File.WriteAllLines("users_universal.txt", userLines);

            List<string> jobLines = new List<string>();
            for (int i = 0; i < JobIDs.Count; i++)
                jobLines.Add(JobIDs[i] + "|" + JobTitles[i] + "|" + JobLocations[i] +
             "|" + JobBudgets[i] + "|" + JobEmployers[i] +
             "|" + JobWorkers[i] + "|" + JobStatuses[i] +
             "|" + JobRatings[i]);
            File.WriteAllLines("jobs_universal.txt", jobLines);

            File.WriteAllLines("notifications_universal.txt", LiveNotifications.ToArray());
            File.WriteAllLines("history_universal.txt", TransactionHistory.ToArray());
        }
        static void LoadData()
        {
            if (File.Exists("users_universal.txt"))
            {
                UserNames.Clear();
                UserPasswords.Clear();
                UserLoc.Clear();
                UserConNum.Clear();
                foreach (string line in File.ReadAllLines("users_universal.txt"))
                {
                    string[] p = line.Split(',');
                    if (p.Length == 4)
                    {
                        UserNames.Add(p[0]);
                        UserPasswords.Add(p[1]);
                        UserLoc.Add(p[2]);
                        UserConNum.Add(p[3]);
                    }
                }
            }
            if (File.Exists("jobs_universal.txt"))
            {
                JobIDs.Clear();
                JobTitles.Clear();
                JobBudgets.Clear();
                JobEmployers.Clear();
                JobWorkers.Clear();
                JobStatuses.Clear();
                JobRatings.Clear();
                JobLocations.Clear();
                foreach (string line in File.ReadAllLines("jobs_universal.txt"))
                {
                    string[] p = line.Split('|');
                    if (p.Length == 8)
                    {
                        JobIDs.Add(p[0]);
                        JobTitles.Add(p[1]);
                        JobLocations.Add(p[2]);
                        JobBudgets.Add(p[3]);
                        JobEmployers.Add(p[4]);
                        JobWorkers.Add(p[5]);
                        JobStatuses.Add(p[6]);
                        JobRatings.Add(p[7]);
                    }
                }
            }
            if (File.Exists("notifications_universal.txt"))
            {
                LiveNotifications.Clear();
                foreach (string line in File.ReadAllLines("notifications_universal.txt"))
                    if (!string.IsNullOrWhiteSpace(line)) LiveNotifications.Enqueue(line);
            }
            if (File.Exists("history_universal.txt"))
            {
                TransactionHistory.Clear();
                string[] lines = File.ReadAllLines("history_universal.txt");
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                        TransactionHistory.Push(lines[i]);
                }
            }
        }
        static void Pause()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }

}


