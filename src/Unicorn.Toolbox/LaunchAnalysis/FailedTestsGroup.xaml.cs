﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Unicorn.Toolbox.LaunchAnalysis
{
    /// <summary>
    /// Interaction logic for FailedTestsGroup.xaml
    /// </summary>
    public partial class FailedTestsGroup : UserControl
    {
        public Dictionary<string, IEnumerable<TestResult>> FailedResults { get; protected set; }

        public FailedTestsGroup(IEnumerable<TestResult> tests)
        {
            InitializeComponent();

            textErrorMessage.Text = tests.First().ErrorMessage;
            labelFoundFailedTests.Content += tests.Count().ToString();

            Height = 100;
            Width = 400;

            var uniqueSuites = new HashSet<string>(tests.Select(r => r.TestListName));

            FailedResults = new Dictionary<string, IEnumerable<TestResult>>();

            foreach (var suite in uniqueSuites)
            {
                var matchingResults = tests.Where(r => r.TestListName.Equals(suite));
                FailedResults.Add(suite, matchingResults);
            }
        }

        private void LabelFoundFailedTests_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var window = new WindowTestsByMessage();
            window.gridResults.ItemsSource = FailedResults;
            window.ShowDialog();
        }
    }
}