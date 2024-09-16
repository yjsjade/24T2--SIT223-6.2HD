// See https://aka.ms/new-console-template for more information
using System;

namespace _54HD
{
    internal class Tester
    {
        private static IController controller;
        private static IGui gui;
        private static string displayText;
        private static int randomNumber;
        private static int passed = 0;

        static void Main(string[] args)
        {
            // run simple test
            SimpleTest();
            Console.WriteLine("\n=====================================\nSummary: {0} tests passed out of 42", passed);
            Console.ReadKey();
        }

        private static void SimpleTest()
        {
            //Construct a ReactionController
            controller = new EnhancedReactionController();
            gui = new DummyGui();

            //Connect them to each other
            gui.Connect(controller);
            controller.Connect(gui, new RndGenerator());

            //Reset the components()
            gui.Init();

            //Test the SimpleReactionController
            //IDLE
            DoReset('A', controller, "Insert coin");
            DoGoStop('B', controller, "Insert coin");
            DoTicks('C', controller, 1, "Insert coin");

            //coinInserted with inactive player
            DoInsertCoin('D', controller, "Press GO!");
            DoInsertCoin('E', controller, "Press GO!");
            DoTicks('F', controller, 5, "Press GO!");
            DoTicks('G', controller, 100, "Insert coin");

            /*NEW GAME-------------------------------------
            Game where player cheats in the first round*/
            DoInsertCoin('H', controller, "Press GO!");
            DoGoStop('I', controller, "Wait...");
            DoGoStop('J', controller, "Insert coin");

            /*NEW GAME-------------------------------------
            Game where player cheats in the second round*/
            randomNumber = 100;
            DoInsertCoin('K', controller, "Press GO!");
            DoGoStop('L', controller, "Wait...");
            DoTicks('M', controller, randomNumber - 1, "Wait...");

            DoTicks('N', controller, 1, "0.00");                        //testing timer and ticks
            DoTicks('O', controller, 1, "0.01");

            DoGoStop('P', controller, "0.01");                          //hits button immediately
  
            DoTicks('Q', controller, 300, "Wait...");                   //waits for timer to run before next round
            DoGoStop('R', controller, "Insert coin");                   //cheats in second round

            /*NEW GAME-------------------------------------
            Game where player plays properly*/
            randomNumber = 120;
            DoInsertCoin('S', controller, "Press GO!");
            DoGoStop('T', controller, "Wait...");
            DoTicks('U', controller, randomNumber + 98, "0.98");
            DoGoStop('V', controller, "0.98");                          //round 1 results

            randomNumber = 100;
            DoGoStop('W', controller, "Wait...");
            DoTicks('X', controller, randomNumber + 68, "0.68");
            DoGoStop('Y', controller, "0.68");                          //round 2 results

            randomNumber = 150;
            DoGoStop('Z', controller, "Wait...");
            DoTicks('a', controller, randomNumber + 88, "0.88");
            DoGoStop('b', controller, "0.88");                          //round 3 results

            DoGoStop('c', controller, "Average: 0.85");
            DoTicks('d', controller, 500, "Insert coin");               //time runs out before restart game

            /*NEW GAME-------------------------------------
            Game where player runs out of time*/
            randomNumber = 100;
            DoInsertCoin('e', controller, "Press GO!");
            DoGoStop('f', controller, "Wait...");
            DoTicks('g', controller, randomNumber + 50, "0.50");
            DoGoStop('h', controller, "0.50");                          //round 1 results

            randomNumber = 110;
            DoTicks('i', controller, 300, "Wait...");                   //waits for timer to run out before starting next round
            DoTicks('j', controller, randomNumber + 55, "0.55");
            DoGoStop('k', controller, "0.55");                          //round 2 results

            randomNumber = 150;
            DoGoStop('l', controller, "Wait...");
            DoTicks('m', controller, randomNumber + 58, "0.58");
            DoGoStop('n', controller, "0.58");                          //round 3 results

            DoGoStop('o', controller, "Average: 0.54");
            DoGoStop('p', controller, "Insert coin");                   //press button to skip results screen
        }

        private static void DoReset(char ch, IController controller, string msg)
        {
            try
            {
                controller.Init();
                GetMessage(ch, msg);
            }
            catch (Exception exception)
            {
                Console.WriteLine("test {0}: failed with exception {1})", ch, msg, exception.Message);
            }
        }

        private static void DoGoStop(char ch, IController controller, string msg)
        {
            try
            {
                controller.GoStopPressed();
                GetMessage(ch, msg);
            }
            catch (Exception exception)
            {
                Console.WriteLine("test {0}: failed with exception {1})", ch, msg, exception.Message);
            }
        }

        private static void DoInsertCoin(char ch, IController controller, string msg)
        {
            try
            {
                controller.CoinInserted();
                GetMessage(ch, msg);
            }
            catch (Exception exception)
            {
                Console.WriteLine("test {0}: failed with exception {1})", ch, msg, exception.Message);
            }
        }

        private static void DoTicks(char ch, IController controller, int n, string msg)
        {
            try
            {
                for (int t = 0; t < n; t++) controller.Tick();
                GetMessage(ch, msg);
            }
            catch (Exception exception)
            {
                Console.WriteLine("test {0}: failed with exception {1})", ch, msg, exception.Message);
            }
        }

        private static void GetMessage(char ch, string msg)
        {
            if (msg.ToLower() == displayText.ToLower())
            {
                Console.WriteLine("test {0}: passed successfully", ch);
                passed++;
            }
            else
                Console.WriteLine("test {0}: failed with message ( expected {1} | received {2})", ch, msg, displayText);
        }

        private class DummyGui : IGui
        {

            private IController controller;

            public void Connect(IController controller)
            {
                this.controller = controller;
            }

            public void Init()
            {
                displayText = "?reset?";
            }

            public void SetDisplay(string msg)
            {
                displayText = msg;
            }
        }

        private class RndGenerator : IRandom
        {
            public int GetRandom(int from, int to)
            {
                return randomNumber;
            }
        }
    }
}