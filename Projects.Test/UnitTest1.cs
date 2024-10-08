using Xunit;

namespace Projects.Test
{
    public class ReactionControllerTests
    {
        private static IController _controller;
        private static IGui _gui;
        private static string _displayText = string.Empty;
        private static int _randomNumber;

        static ReactionControllerTests()
        {
            // Initialize your controller and gui here
            _controller = new EnhancedReactionController();
            _gui = new DummyGui();
            _gui.Connect(_controller);
            _controller.Connect(_gui, new RndGenerator());
            _gui.Init();
        }

        [Fact]
        public void SimpleTest()
        {
            // IDLE
            DoReset("Insert Coin");
            DoGoStop("Insert Coin");
            DoTicks(1, "Insert Coin");

            // Coin inserted with inactive player
            DoInsertCoin("Press GO!");
            DoInsertCoin("Press GO!");
            DoTicks(5, "Press GO!");
            DoTicks(100, "Insert Coin");

            /*NEW GAME-------------------------------------*/
            DoInsertCoin("Press GO!");
            DoGoStop("Wait...");
            DoGoStop("Insert Coin");

            /*NEW GAME-------------------------------------*/
            _randomNumber = 100;
            DoInsertCoin("Press GO!");
            DoGoStop("Wait...");
            DoTicks(_randomNumber - 1, "Wait...");
            DoTicks(1, "0.00");  // Testing timer and ticks
            DoTicks(1, "0.01");
            DoGoStop("0.01");  // Hits button immediately
            DoTicks(300, "Wait...");  // Waits for timer to run before next round
            DoGoStop("Insert Coin");  // Cheats in second round
        }

        private void DoReset(string expectedMessage)
        {
            _controller.Init();
            Assert.Equal(expectedMessage, _displayText);
        }

        private void DoGoStop(string expectedMessage)
        {
            _controller.GoStopPressed();
            Assert.Equal(expectedMessage, _displayText);
        }

        private void DoInsertCoin(string expectedMessage)
        {
            _controller.CoinInserted();
            Assert.Equal(expectedMessage, _displayText);
        }

        private void DoTicks(int n, string expectedMessage)
        {
            for (int t = 0; t < n; t++) _controller.Tick();
            Assert.Equal(expectedMessage, _displayText);
        }

        private class DummyGui : IGui
        {
            private IController _controller;

            public void Connect(IController controller)
            {
                _controller = controller;
            }

            public void Init()
            {
                _displayText = "?reset?";
            }

            public void SetDisplay(string msg)
            {
                _displayText = msg;
            }
        }

        private class RndGenerator : IRandom
        {
            public int GetRandom(int from, int to)
            {
                return _randomNumber;
            }
        }
    }
}