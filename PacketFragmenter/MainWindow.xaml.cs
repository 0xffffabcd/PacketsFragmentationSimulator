using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using PacketFragmenter.Classes;

namespace PacketFragmenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow
    {
        private const int Header = 20;
        private const int MinimumMTU = 30;
        private ObservableCollection<Router> _routers;

        public MainWindow()
        {
            InitializeComponent();
            _routers = new ObservableCollection<Router>();
            routersListBox.DataContext = _routers;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            SetupTestData();
        }

        private void AddRouterButtonClick(object sender, RoutedEventArgs e)
        {
            string routerName = routerNameTB.Text.Trim();
            int mtu;

            if ((routerName.Length <= 0)
                || !int.TryParse(routerMTUTB.Text.Trim(), out mtu)
                || (mtu < MinimumMTU))
                return;

            _routers.Add(new Router(routerName, mtu));
            routerNameTB.Text = "T";
            routerMTUTB.Clear();
        }

        private void FragmentDataButtonClick(object sender, RoutedEventArgs e)
        {
            if (routersListBox.Items.Count <= 2)
                return;

            var solution = DoFragment(int.Parse(dataSizeTextBox.Text));

            resultsRTB.Document.Blocks.Clear();
            foreach (var solItem in solution)
            {
                var titleRun = new Run(string.Format("Routeur : {0} (MTU : {1})", solItem.Key.Name, solItem.Key.MTU))
                                   {
                                       FontSize = 14,
                                       FontWeight = FontWeights.Bold,
                                       TextDecorations = TextDecorations.Underline
                                   };
                resultsRTB.Document.Blocks.Add(new Paragraph(titleRun));
                resultsRTB.Document.Blocks.Add(Helpers.FormatFragmentsList(solItem.Value));
            }
        }

        private Dictionary<Router, List<Fragment>> DoFragment(int testDataLength)
        {
            var solution = new Dictionary<Router, List<Fragment>>
                                                              {
                                                                  {
                                                                      _routers[0],
                                                                      Helpers.FragmentData(
                                                                          new Fragment(testDataLength, false, 0),
                                                                          _routers[0].MTU)
                                                                      }
                                                              };

            for (int index = 1; index < _routers.Count; index++)
            {
                Router prevRouter = _routers[index - 1];
                Router currRouter = _routers[index];
                if (currRouter.MTU == 0)
                {
                    continue;
                }
                solution.Add(currRouter, new List<Fragment>());
                for (int i = 0; i < solution[prevRouter].Count; i++)
                {
                    if ((solution[prevRouter][i].Length + Header) > currRouter.MTU)
                    {
                        solution[currRouter].AddRange(Helpers.FragmentData(solution[prevRouter][i], currRouter.MTU));
                    }
                    else
                    {
                        solution[currRouter].Add(solution[prevRouter][i]);
                    }
                }
            }

            return solution;
        }

        private void SetupTestData()
        {
            //Note : Test data
            /*
            var testRouters = new[]
                                   {
                                       new Router("Client A", 4000),
                                       new Router("T1", 1000),
                                       new Router("T2", 500),
                                       new Router("T3", 300),
                                       new Router("T4", 500),
                                       new Router("Client B", 0)
                                   };
            */

            //Note: Test with the wikipedia example https://secure.wikimedia.org/wikipedia/en/wiki/IPv4#Fragmentation

            var testRouters = new[]
                                   {
                                       new Router("Client A", 5000),
                                       new Router("T1", 2500),
                                       new Router("T2", 1500),
                                       new Router("Client B", 0)
                                   };

            foreach (var testRouter in testRouters)
            {
                _routers.Add(testRouter);
            }
        }

        private void EmptyListButtonClick(object sender, RoutedEventArgs e)
        {
            _routers.Clear();
        }
    }
}