﻿using KSR.Model;
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
        private string chosenMetricFeature;
        private string chosenExtractFeature;
        private string trainingSetString;
        private string k;
        private int _k;
        private List<List<Reuter>> allReuters;
        #endregion

        #region Properties
        public List<Reuter> Reuters
        {
            get { return reuters; }
            set { this.reuters = value; }
        }

        public string ChosenMetricFeature
        {
            get { return chosenMetricFeature; }
            set { this.chosenMetricFeature = value;
                chosenMetricFeature = chosenMetricFeature.Substring(38, chosenMetricFeature.Length - 38);
            }
        }

        public string ChosenExtractFeature
        {
            get { return chosenExtractFeature; }
            set
            {
                this.chosenExtractFeature = value;
                chosenExtractFeature = chosenExtractFeature.Substring(38, chosenExtractFeature.Length - 38);
            }
        }

        public string TrainingSetString
        {
            get { return trainingSetString; }
            set { this.trainingSetString = value;
            }
        }

        public int TrainingSet
        {
            get { return int.Parse(TrainingSetString); }
        }

        public List<List<Reuter>> AllReuters
        {
            get { return allReuters; }
            set { this.allReuters = value; }
        }

        public string K
        {
            get { return k; }
            set { this.k = value;
                _k = int.Parse(K);
            }
        }

        public int getK
        { get { return _k; } }
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
            Reuters = await Model.Reuter.GetReutersFromFileAsync(path,ChosenExtractFeature);
            AllReuters = TrainingPatterns.SetTrainingAndTestSet(TrainingSet, Reuters);
            if (ChosenMetricFeature.Equals("Euclidean Metric"))
            {
                await EuclideanMetric.CalculateAsync(AllReuters);
                MessageBox.Show("Done");
            }
            else if (ChosenMetricFeature.Equals("Manhattan Metric"))
            {
                await ManhattanMetric.CalculateAsync(AllReuters);
                MessageBox.Show("Done");
            }
        }
        #endregion
    }
}