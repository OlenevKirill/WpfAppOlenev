using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFAppOlenev
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Agent2.Text = "Посредник \x0Aкладёт на \x0Aстол :";
        }

        static Semaphore tobacco = new Semaphore(0, 1);
        static Semaphore paper = new Semaphore(0, 1);
        static Semaphore matches = new Semaphore(0, 1);
        static Semaphore agent = new Semaphore(1, 1);

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Thread agentThread = new Thread(AgentWork);
            agentThread.Start();
        }

        private void AgentWork()
        {
            Random rand = new Random();
            while (true)
            {
                agent.WaitOne();
                int choice = rand.Next(3);

                Dispatcher.Invoke(() =>
                {
                    switch (choice)
                    {
                        case 0:
                            Agent.Text = "бумагу и спички.";
                            paper.Release();
                            matches.Release();
                            SmokerWithMatches();
                            break;
                        case 1:
                            Agent.Text = "табак и спички.";
                            tobacco.Release();
                            matches.Release();
                            SmokerWithTobacco();
                            break;
                        case 2:
                            Agent.Text = "табак и бумагу.";
                            tobacco.Release();
                            paper.Release();
                            SmokerWithPaper();
                            break;
                    }
                });
            }
        }

        private void UpdateTextBox(string message)
        {
            Dispatcher.Invoke(() => TextBox.Text += message + Environment.NewLine);
        }

        private void SmokerWithMatches()
        {
            UpdateTextBox("Курильщик со спичками курит.");
            agent.Release();
        }

        private void SmokerWithTobacco()
        {
            UpdateTextBox("Курильщик с табаком курит.");
            agent.Release();
        }

        private void SmokerWithPaper()
        {
            UpdateTextBox("Курильщик с бумагой курит.");
            agent.Release();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread smokerWithTobacco = new Thread(SmokerWithTobacco);
            Thread smokerWithPaper = new Thread(SmokerWithPaper);
            Thread smokerWithMatches = new Thread(SmokerWithMatches);

            smokerWithTobacco.Start();
            smokerWithPaper.Start();
            smokerWithMatches.Start();
        }
    }
}