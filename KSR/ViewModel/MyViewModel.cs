using KSR.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private List<Reut> reuters;
        #endregion

        #region Properties
        public List<Reut> Reuters
        {
            get { return reuters; }
            set { this.reuters = value; }
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
            Reuters = Reut.GetReutersFromFile(path);
        }
        #endregion
    }
}
