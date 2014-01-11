using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ICTEAS.WinForms.Controls
{

    public class F2EventArgs:EventArgs
    {
        public int row;
        public int column;
        
        public F2EventArgs(int row,int col)
        {
           
            this.row = row;
            this.column = col;
        }
    }


    public class ComboSelectionChangeCommitedEventArgs : EventArgs
    {
        public int row;
        public int column;

        public ComboSelectionChangeCommitedEventArgs()
        {

            
        }
    }

    public class DataGridViewCheckChangedEventArgs : EventArgs
    {
        public bool state;
        public DataGridViewCheckChangedEventArgs(bool state)
        {

            this.state = state;
        }
    }
    public class DataGridViewCellTextChangedEventArgs : EventArgs
    {
        public bool _TextChanged = false;
        public string _NewText;
        public string _OldText;
        public int _RowIndex;
        public int _ColumnIndex;
        public DataGridViewCellTextChangedEventArgs(bool Changed,string OldText,string NewText,int RowIndex,int ColumnIndex)
        {
            _TextChanged = Changed;
            _NewText = NewText;
            _OldText = OldText;
            _RowIndex = RowIndex;
            _ColumnIndex = ColumnIndex;
        }
 
    }

    public class DataGridViewEditingControlTextChanged :EventArgs
    {
        public int RowIndex;
        public int ColumnIndex;
        public string Text;
        public DataGridViewEditingControlTextChanged(int _rowIndex, int _columnIndex, string _text)
        {
            RowIndex = _rowIndex;
            ColumnIndex = _columnIndex;
            Text = _text;
        }

    }


    public class DataGridViewFilterChanged : EventArgs
    {
        private string _CurrentFilter = "";
        public string CurrentFilter
        {
            get { return _CurrentFilter; }
        }

        private int _NoOfRows = 0;
        public int CurrentRowNumber
        {
            get { return _NoOfRows; }
        }

        private bool _IsFiltered = false;
        public bool IsFiltered
        {
            get { return _IsFiltered; }
        }
        public DataGridViewFilterChanged(string _CurrentFilter, int _CurrentRowNumber, bool _IsFiltered)
        {
            this._CurrentFilter = _CurrentFilter;
            this._NoOfRows = _CurrentRowNumber;
            this._IsFiltered = _IsFiltered;
        }

    }
     
}
