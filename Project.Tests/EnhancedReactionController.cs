using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _54HD
{
    internal class EnhancedReactionController : IController
    {
        //constant variables
        private const int minimumWait = 100;
        private const int maximumWait = 250;
        private const int autoEnd = 200;
        private const int waitTime = 300;
        private const double ticksPerSecond = 100;
        private const int gameNumber = 3;
        private const int endDelay = 500;

        //instance variables 
        private IGui gui { get; set; }
        private IRandom random { get; set; }
        private int Ticks { get; set; }
        private int Games { get; set; }
        private int TotalReactionTime { get; set; }
        private States _state;


        //connects the controller to the IGui and IRandom interface
        public void Connect(IGui gui, IRandom random)
        {
            this.gui = gui;
            this.random = random;
            Init();
        }

        //initialises the controller at the start of the program
        public void Init()
        {
            _state = new CoinInsertState(this);
        }

        /* handles the different events for when a certain action occurs,
        such as coin is inserted or the Go/stop button is pressed */
        public void CoinInserted()
        {
            _state.CoinInserted();
        }

        public void GoStopPressed()
        {
            _state.GoStopPressed();
        }

        public void Tick()
        {
            _state.Tick();
        }

        //method to change the current state of the machine to a new state
        private void SetState(States state)
        {
            _state = state;
        }

        //the base class of all the different concrete state classes
        private abstract class States
        {
            protected EnhancedReactionController _controller;

            public States(EnhancedReactionController controller)
            {
                _controller = controller;
            }

            public abstract void CoinInserted();
            public abstract void GoStopPressed();
            public abstract void Tick();
        }

        //state of the game when its waiting for a coin to be inserted
        private class CoinInsertState : States
        {
            public CoinInsertState(EnhancedReactionController controller) : base(controller)
            {
                _controller.gui.SetDisplay("Insert Coin");
                _controller.TotalReactionTime = 0;
                _controller.Games = 0;
            }

            public override void CoinInserted()
            {
                _controller.SetState(new ReadyState(_controller));
            }
            public override void GoStopPressed() { }
            public override void Tick() { }
        }

        //state of the game when its waiting for the button to be pressed
        private class ReadyState : States
        {
            public ReadyState(EnhancedReactionController controller) : base(controller)
            {
                _controller.gui.SetDisplay("Press GO!");
                _controller.Ticks = 0;
            }

            public override void CoinInserted() { }
            public override void GoStopPressed()
            {
                _controller.SetState(new WaitState(_controller));
            }
            public override void Tick()
            {
                _controller.Ticks++;
                if (_controller.Ticks == 100)
                {
                    _controller.SetState(new CoinInsertState(_controller));
                }
            }
        }

        //state of the game when its waiting for the random amount of time to finish
        private class WaitState : States
        {
            private int delay;

            public WaitState(EnhancedReactionController controller) : base(controller)
            {
                _controller.gui.SetDisplay("Wait...");
                _controller.Ticks = 0;
                _controller.Games++;
                delay = _controller.random.GetRandom(minimumWait, maximumWait);
            }

            public override void CoinInserted() { }
            public override void GoStopPressed()
            {
                _controller.SetState(new CoinInsertState(_controller));
            }
            public override void Tick()
            {
                _controller.Ticks++;
                if (_controller.Ticks == delay)
                {
                    _controller.SetState(new GameState(_controller));
                }
            }
        }

        //state of the game where it awaits a button press to calculate the reaction time
        private class GameState : States
        {
            public GameState(EnhancedReactionController controller) : base(controller)
            {
                _controller.gui.SetDisplay("0.00");
                _controller.Ticks = 0;
            }

            public override void CoinInserted() { }
            public override void GoStopPressed()
            {
                _controller.SetState(new ResultsState(_controller));
            }
            public override void Tick()
            {
                _controller.Ticks++;
                _controller.gui.SetDisplay((_controller.Ticks / ticksPerSecond).ToString("0.00"));
                if (_controller.Ticks == autoEnd)
                {
                    _controller.SetState(new ResultsState(_controller));
                }
            }
        }

        //state of the game where it showcases the result of the round
        private class ResultsState : States
        {
            public ResultsState(EnhancedReactionController controller) : base(controller)
            {
                _controller.TotalReactionTime += _controller.Ticks;
                _controller.Ticks = 0;
            }

            public override void CoinInserted() { }
            public override void GoStopPressed()
            {
                CurrentGame();
            }
            public override void Tick()
            {
                _controller.Ticks++;
                if (_controller.Ticks == waitTime)
                {
                    CurrentGame();
                }
            }
            private void CurrentGame()
            {
                if (_controller.Games == gameNumber)
                {
                    _controller.SetState(new GameOverState(_controller));
                }
                else
                {
                    _controller.SetState(new WaitState(_controller));
                }
            }
        }

        //state of the game where it showcases the final and average result of the 3 rounds
        private class GameOverState : States
        {
            public GameOverState(EnhancedReactionController controller) : base(controller)
            {
                _controller.gui.SetDisplay("Average: " + (_controller.TotalReactionTime / ticksPerSecond / _controller.Games).ToString("0.00"));
                _controller.Ticks = 0;
            }

            public override void CoinInserted() { }
            public override void GoStopPressed()
            {
                _controller.SetState(new CoinInsertState(_controller));
            }
            public override void Tick()
            {
                _controller.Ticks++;
                if (_controller.Ticks == endDelay)
                {
                    _controller.SetState(new CoinInsertState(_controller));
                }
            }
        }
    }
}
