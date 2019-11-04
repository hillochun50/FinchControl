using System;
using System.Collections.Generic;
using FinchAPI;
using System.IO;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control
    // Description: 
    // Application Type: Console
    // Author: 
    // Dated Created: 
    // Last Modified: 
    //
    // **************************************************

    class Program
    {

        public enum Command
        {
            NONE,
            MOVEFORWARD,
            MOVEBACKWARD,
            STOPMOTORS,
            WAIT,
            TURNRIGHT,
            TURNLEFT,
            LEDON,
            LEDOFF,
            DONE

        }


        static void Main(string[] args)
        {
            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();


        }

        static void DisplayMenuScreen()
        {
            //
            // instantiate a Finch object
            //
            //Finch finchRobot;
            //finchRobot = new Finch();
            Finch finchRobot = new Finch();

            bool finchRobotConnected = false;
            bool quitApplication = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("a) Connect Finch Robot");
                Console.WriteLine("b) Talent Show");
                Console.WriteLine("c) Data Recorder");
                Console.WriteLine("d) Alarm System");
                Console.WriteLine("e) User Programming");
                Console.WriteLine("f) Disconnect Finch Robot");
                Console.WriteLine("q) Quit");
                Console.Write("Enter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        finchRobotConnected = DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        if (finchRobotConnected)
                        {
                            DisplayTalentShow(finchRobot);
                        }
                        else
                        {
                            Console.WriteLine("The Finch robot is not connect. Please return to the Main Menu and connect.");
                            DisplayContinuePrompt();
                        }
                        break;

                    case "c":
                        DisplayDataRecorder(finchRobot);
                        break;

                    case "d":
                        DisplayAlarmSystem(finchRobot);
                        break;

                    case "e":
                        DisplayWriteUserProgrammingData(commands);
                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }


            } while (!quitApplication);
        }

        static List<Command> DisplayReadUserProgrammingData()
        {
            string dataPath = @"Data\Data.txt";
            List<Command> commands = new List<Command>();
            string[] commandsString;

            DisplayScreenHeader("Read Data from File");

            Console.WriteLine("Ready to read from the data file.");
            DisplayContinuePrompt();

            commandsString = File.ReadAllLines(dataPath);

            //
            // create list of Command enum
            //

            Command command;
            foreach  (string commandString in commandsString)
            {
                Enum.TryParse(commandString, out command);
                commands.Add(command);

            }

            DisplayContinuePrompt();


            return commands; 
        }

        static void DisplayWriteUserProgrammingData(List<Command> commands)
        {
            string dataPath = @"Data\Data.txt";

            List<string> commandString = new List<string>();

            DisplayScreenHeader("Write Data to File");

            //
            // create a list of command strings
            //
            foreach (Command command in commands)
            {
                commandString.Add(command.ToString());
            }

            Console.WriteLine("Ready to write to the data file.");
            DisplayContinuePrompt();

            File.WriteAllLines(dataPath, commandString.ToArray());

            DisplayContinuePrompt();
        }

        #region USER PROGRAMMING

        static void DispalyFinchCommands(List<Command> commands)
        {
            DisplayScreenHeader("Finch Commands");

            Console.WriteLine("Commands");
            Console.WriteLine();

            foreach (Command command in commands)
            {
                Console.WriteLine(command);
            }

            DisplayContinuePrompt();
        }

        static void DisplayGetFinchCommands(List<Command> commands)
        {
            string userResponse;
            Command command = Command.NONE;

            DisplayScreenHeader("Finch Robot Commands");

            while (command != Command.DONE)
            {

                // validate!!
                Console.Write("Enter Command:");
                userResponse = Console.ReadLine().ToUpper();
                Enum.TryParse(userResponse, out command);

                commands.Add(command);

            }

            // echo command list

            DisplayContinuePrompt();
        }

        static (int motorSpeed, int ledBrightness, int waitSeconds) DisplayGetCommandParameters()
        {
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameters;

            DisplayScreenHeader("Command Parameters");

            Console.Write("Enter Motor Speed [0 - 255]:");
            commandParameters.motorSpeed = int.Parse(Console.ReadLine());

            Console.Write("Enter LED Brightness [0 - 255]:");
            commandParameters.ledBrightness = int.Parse(Console.ReadLine());

            Console.Write("Enter Wait Command in Seconds:");
            commandParameters.waitSeconds = int.Parse(Console.ReadLine());

            // todo echo values to user


            DisplayContinuePrompt();

            return commandParameters;
        }

        static void DisplayUserProgramming(Finch finchRobot)
        {
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameter;
            commandParameter.motorSpeed = 0;
            commandParameter.ledBrightness = 0;
            commandParameter.waitSeconds = 0;



            List<Command> commands = new List<Command>();
            string menuChoice;
            bool quitApplication = false;

            do
            {
                DisplayScreenHeader("User Programming");

                //
                // get user menu choice
                //
                Console.WriteLine("a) Set Command Parameters");
                Console.WriteLine("b) Add Commands");
                Console.WriteLine("c) View Commands");
                Console.WriteLine("d) Execute Commands");
                Console.WriteLine("e) Write commands from data file");
                Console.WriteLine("q) Return to Main Menu");
                Console.Write("Enter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParameter = DisplayGetCommandParameters();
                        break;

                    case "b":
                        DisplayGetFinchCommands(commands);
                        break;

                    case "c":
                        DisplayGetFinchCommands(commands);
                        break;

                    case "d":
                        DisplayExecuteFinchCommands(finchRobot, commands, commandParameter);
                        break;

                    case "e":
                        DisplayUserProgramming(finchRobot);
                        break;

                    case "f":
                        DisplayWriteUserProgrammingData(commands);
                        break;

                    case "q":
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("Please enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }


            } while (!quitApplication);
        }


        static void DisplayExecuteFinchCommands(
            Finch finchRobot, 
            List<Command> commands,
            (int motorSpeed, int ledBrightness, int waitSeconds) commandParameter)
        {
            int motorSpeed = commandParameter.motorSpeed;
            int ledBrightness = commandParameter.ledBrightness;
            int waitSeconds = commandParameter.waitSeconds;

            DisplayScreenHeader("Execute Finch Commands");

            //info and pause
            Console.ReadKey();

            foreach (Command command in commands)
            {
                switch (command)
                {
                    case Command.NONE:
                        break;
                    case Command.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        break;
                    case Command.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        break;
                    case Command.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        break;
                    case Command.WAIT:
                        finchRobot.wait(waitSeconds * 1000);
                        break;
                    case Command.TURNRIGHT:

                        break;
                    case Command.TURNLEFT:
                        break;
                    case Command.LEDON:
                        break;
                    case Command.LEDOFF:
                        break;
                    case Command.DONE:
                        break;
                    default:
                        break;
                }
            }
        }

            #endregion


            #region TALENT SHOW
            static void DisplayTalentShow(Finch finchRobot)
        {
            DisplayScreenHeader("Talent Show");

            Console.WriteLine("The Finch robot will not show off its talent!!");
            DisplayContinuePrompt();

            for (int lightLevel = 0; lightLevel < 255; lightLevel++)
            {
                finchRobot.setLED(lightLevel, lightLevel, lightLevel);
            }

            DisplayContinuePrompt();
        }
        #endregion       

        #region DATA RECORDER
        static void DisplayDataRecorder(Finch finchRobot)
        {
            double frequency;
            int numberOfDataPoints;

            DisplayScreenHeader("Data Recoder");

            // tell user what is going happen

            frequency = DisplayGetDataRecorderFrequency(finchRobot);
            numberOfDataPoints = DisplayGetNumberOfDataPoints(finchRobot);

            //
            // instantiate (create) array
            //
            double[] temperatures = new double[numberOfDataPoints];

            // warn the user before recording
            DisplayGetDataReadings(numberOfDataPoints, frequency, temperatures, finchRobot);

            DisplayDataRecorderData(temperatures);

            DisplayMainMenuPrompt();
        }

        static void DisplayDataRecorderData(double[] temperatures)
        {
            DisplayScreenHeader("Temperatures");

            // provide some info to the user
            Console.WriteLine("Data Set");
            Console.WriteLine();

            for (int index = 0; index < temperatures.Length; index++)
            {
                Console.WriteLine($"Temperature {index + 1}: {temperatures[index]}");
            }

            DisplayContinuePrompt();
        }

        static void DisplayGetDataReadings(
            int numberOfDataPoints,
            double frequencyOfDataPoints,
            double[] temperatures,
            Finch finchRobot)
        {
            DisplayScreenHeader("Get Temperature Recordings");

            // prompt the user
            DisplayContinuePrompt();

            //
            // get temperatures
            //
            for (int index = 0; index < numberOfDataPoints; index++)
            {
                temperatures[index] = finchRobot.getTemperature();
                int milleSeconds = (int)(frequencyOfDataPoints * 1000);
                finchRobot.wait(milleSeconds);
                Console.WriteLine($"Temperature {index + 1}: {temperatures[index]}");
            }

            DisplayContinuePrompt();
        }

        static double DisplayGetDataRecorderFrequency(Finch finchRobot)
        {
            double frequency;
            //string userResponse;

            DisplayScreenHeader("Get Frequency of Recordings");

            Console.Write("Enter frequency [seconds]");
            //userResponse = Console.ReadLine();
            //double.TryParse(userResponse, out frequency);
            double.TryParse(Console.ReadLine(), out frequency);

            DisplayMainMenuPrompt();

            return frequency;
        }

        static int DisplayGetNumberOfDataPoints(Finch finchRobot)
        {
            int numberOfDataPoints;

            DisplayScreenHeader("Get Number of Data Points");

            Console.Write("Enter the number of data points.");
            int.TryParse(Console.ReadLine(), out numberOfDataPoints);

            DisplayMainMenuPrompt();

            return numberOfDataPoints;
        }
        #endregion

        #region ALARM SYSTEM
        static void DisplayAlarmSystem(Finch finchRobot)
        {
            string alarmType;
            int maxSeconds;
            double threshold;
            bool thresholdExceeded;

            DisplayScreenHeader("Alarm System");

            alarmType = DisplayGetAlarmType();
            maxSeconds = DisplayGetMaxSeconds();
            threshold = DisplayGetThreshold(finchRobot, alarmType);

            // warn the user and pause

            thresholdExceeded = MonitorLightLevels(finchRobot, threshold, maxSeconds);

            if (thresholdExceeded)
            {
                if (alarmType == "string")
                {
                    Console.WriteLine("Maximum Light Level Exceeded");
                }
                else
                {
                    Console.WriteLine("Maximum Temperature Exceeded");
                }
            }
            else
            {
                Console.WriteLine("Maximum Monitoring Time Exceeded");
            }

            DisplayMainMenuPrompt();
        }

        static bool MonitorLightLevels(Finch finchRobot, double threshold, int maxSeconds)
        {
            bool thresholdExceeded = false;
            double seconds = 0;
            int currentLightLevel;

            while (!thresholdExceeded && seconds <= maxSeconds)
            {
                currentLightLevel = finchRobot.getLeftLightSensor();

                DisplayScreenHeader("Monitor Light Levels");
                Console.WriteLine($"Maximum Light Level: {(int)threshold}");
                Console.WriteLine($"Current Light Level: {currentLightLevel}");

                if (currentLightLevel >= threshold) thresholdExceeded = true;

                finchRobot.wait(500);
                seconds += 0.5;
            }

            return thresholdExceeded;
        }

        static double DisplayGetThreshold(Finch finchRobot, string alarmType)
        {
            double threshold = 0;

            DisplayScreenHeader("Threshold Value");

            switch (alarmType)
            {
                case "light":
                    Console.WriteLine($"Current Light Level: {finchRobot.getLeftLightSensor()}");
                    Console.Write("Enter Maximum Light Level:");
                    threshold = double.Parse(Console.ReadLine());
                    break;

                case "temperature":

                    break;

                default:
                    throw new FormatException();
                    break;
            }

            DisplayContinuePrompt();

            return threshold;
        }

        static int DisplayGetMaxSeconds()
        {
            // validate user input!
            Console.Write("Maximum Seconds: ");
            return int.Parse(Console.ReadLine());
        }

        static string DisplayGetAlarmType()
        {
            // todo - validate user input!
            Console.Write("Alarm Type [light or temperature]:");
            return Console.ReadLine();
        }

        #endregion

        #region FINCH ROBOT MANAGMENT

        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("About to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("The Finch robot is now disconnect.");

            DisplayContinuePrompt();
        }

        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("About to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            if (robotConnected)
            {
                finchRobot.setLED(0, 255, 0);
                finchRobot.noteOn(15000);
                finchRobot.wait(1000);
                finchRobot.noteOff();

                Console.WriteLine("The Finch robot is now connected.");
            }
            else
            {
                Console.WriteLine("Unable to connect to the Finch robot.");
            }

            DisplayContinuePrompt();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// display welcome screen
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display closing screen
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display main menu prompt
        /// </summary>
        static void DisplayMainMenuPrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the Main Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
