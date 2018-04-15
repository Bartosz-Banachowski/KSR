using KSR.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace KSR.ViewModel
{
    public class MyViewModel : INotifyPropertyChanged
    {
        #region Constructors
        public MyViewModel()
        {
            Load_Files = new DelegateCommand(LoadReuters);
        }
        #endregion

        #region ICommand
        public ICommand Load_Files { get; }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName_)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName_));
        }
        #endregion

        #region Private
        private List<Reuter> reuters;
        private string chosenExtractFeature;
        private string trainingSetString;
        private List<List<Reuter>> allReuters;
        #endregion

        #region Properties
        public List<Reuter> Reuters
        {
            get { return reuters; }
            set { this.reuters = value; }
        }

        public string ChosenExtractFeature
        {
            get { return chosenExtractFeature; }
            set { this.chosenExtractFeature = value;
                chosenExtractFeature = chosenExtractFeature.Substring(38, chosenExtractFeature.Length - 38);
            }
        }

        public string TrainingSetString
        {
            get { return trainingSetString; }
            set { this.trainingSetString = value;
            }
        }

        public int TrainingSetPercent
        {
            get { return int.Parse(TrainingSetString); }
        }

        public List<List<Reuter>> AllReuters
        {
            get { return allReuters; }
            set { this.allReuters = value; }
        }
        #endregion

        #region Functions
        private void LoadReuters()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Sgm File(*.sgm)| *.sgm";
            openFileDialog.Multiselect = true;
            openFileDialog.ShowDialog();
            string[] path = openFileDialog.FileNames;
            ChooseExtract(path);  
        }

        public async void ChooseExtract(string[] path)
        {
            Reuters = await Model.Reuter.GetReutersFromFileAsync(path);
            AllReuters = TrainingPatterns.SetTrainingAndTestSet(TrainingSetPercent, Reuters);

            if (ChosenExtractFeature.Equals("Euclidean Metric"))
            {
                await EuclideanMetric.CalculateAsync(AllReuters);
                MessageBox.Show("Done");
            }
            else if (ChosenExtractFeature.Equals("Manhattan Metric"))
            {
                await ManhattanMetric.CalculateAsync(AllReuters);
                MessageBox.Show("Done");
            }
        }
        #endregion
    }
}