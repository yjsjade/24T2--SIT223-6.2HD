﻿// See https://aka.ms/new-console-template for more information
using System;
using System.Timers;
using Microsoft.VisualBasic;
using Timer = System.Timers.Timer;


namespace _54HD
{
    internal class Tester
    {
        const string TOP_LEFT_JOINT = "┌";
        const string TOP_RIGHT_JOINT = "┐";
        const string BOTTOM_LEFT_JOINT = "└";
        const string BOTTOM_RIGHT_JOINT = "┘";
        const string TOP_JOINT = "┬";
        const string BOTTOM_JOINT = "┴";
        const string LEFT_JOINT = "├";
        const string JOINT = "┼";
        const string RIGHT_JOINT = "┤";
        const char HORIZONTAL_LINE = '─';
        const char PADDING = ' ';
        const string VERTICAL_LINE = "│";

        static private IController contoller;
        static private IGui gui;

        static void Main(string[] args)
        {
            bool isInteractive = !Console.IsInputRedirected;

            if (isInteractive)
            {
                DisplayMenu();
            } else
            {
                Console.WriteLine("Running in non - interactive mode.");
            }


            // Create a time for Tick event
            Timer timer = new Timer(10);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;

            // Connect GUI with the Controller and vice versa
            contoller = new EnhancedReactionController();
            gui = new Gui();
            gui.Connect(contoller);
            contoller.Connect(gui, new RandomGenerator());

            //Reset the GUI
            gui.Init();
            // Start the timer
            timer.Enabled = true;

            if (isInteractive)
            {
                RunInteractiveMenu();
            } 
            else
            {
                Console.WriteLine("No user input required in non-interactive mode. Starting automatic game mode.");
                while (true)
                {
                    contoller.Tick();
                    System.Threading.Thread.Sleep(10); 
                }
            }
        }

        static void DisplayMenu()
        {
            // Make a menu
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0}{1}{2}", TOP_LEFT_JOINT, new string(HORIZONTAL_LINE, 50), TOP_RIGHT_JOINT);
            Console.WriteLine("{0}{1}{2}", VERTICAL_LINE, new string(' ', 50), VERTICAL_LINE);
            Console.WriteLine("{0}{1}{2}", VERTICAL_LINE, new string(' ', 50), VERTICAL_LINE);
            Console.WriteLine("{0}{1}{2}", VERTICAL_LINE, new string(' ', 50), VERTICAL_LINE);
            Console.WriteLine("{0}{1}{2}", LEFT_JOINT, new string(HORIZONTAL_LINE, 50), RIGHT_JOINT);
            Console.WriteLine("{0}{1}{2}", VERTICAL_LINE, new string(' ', 50), VERTICAL_LINE);
            Console.WriteLine("{0}{1}{2}", VERTICAL_LINE, new string(' ', 50), VERTICAL_LINE);
            Console.WriteLine("{0}{1}{2}", VERTICAL_LINE, new string(' ', 50), VERTICAL_LINE);
            Console.WriteLine("{0}{1}{2}", VERTICAL_LINE, new string(' ', 50), VERTICAL_LINE);
            Console.WriteLine("{0}{1}{2}", VERTICAL_LINE, new string(' ', 50), VERTICAL_LINE);
            Console.WriteLine("{0}{1}{2}", BOTTOM_LEFT_JOINT, new string(HORIZONTAL_LINE, 50), BOTTOM_RIGHT_JOINT);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(5, 6);
            Console.Write("{0,-20}", "- For Insert Coin press SPACE");
            Console.SetCursorPosition(5, 7);
            Console.Write("{0,-20}", "- For Go/Stop action press ENTER");
            Console.SetCursorPosition(5, 8);
            Console.Write("{0,-20}", "- For Exit press ESC");
        }

        static void RunInteractiveMenu()
        {
            // Run the menu
            bool quitePressed = false;
            while (!quitePressed)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        contoller.GoStopPressed();
                        break;
                    case ConsoleKey.Spacebar:
                        contoller.CoinInserted();
                        break;
                    case ConsoleKey.Escape:
                        quitePressed = true;
                        break;
                }
            }
        }

        // This event occurs every 10 msec
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            contoller.Tick();
        }

        // Internal implementation of Random Generator
        private class RandomGenerator : IRandom
        {
            Random rnd = new Random(100);

            public int GetRandom(int from, int to)
            {
                return rnd.Next(to - from) + from;
            }
        }

        // Internal implementation of GUI
        private class Gui : IGui
        {
            private IController controller;
            public void Connect(IController controller)
            {
                this.controller = controller;
            }

            public void Init()
            {
                SetDisplay("Start your game!");
            }

            public void SetDisplay(string text)
            {
                PrintUserInterface(text);
            }

            private void PrintUserInterface(string text)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.SetCursorPosition(15, 2);
                Console.Write("{0,-20}", text);
                Console.SetCursorPosition(0, 10);
            }
        }
    }
}