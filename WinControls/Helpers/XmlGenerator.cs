using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;
using System.Windows.Forms;

namespace ICTEAS.WinForms.Helpers
{
    public class XmlGenerator : IDisposable
    {

        private DataTable _dTable;
        private string _FileName;
        private XmlWriter _Writer;
        private ArrayList _ColumnsList;
        private ArrayList _LocalColumnList;
        private int _ColumnCount = 0;


        public XmlGenerator(DataTable _dTable, string _FileName)
        {
            this._dTable = _dTable;
            this._FileName = _FileName;
 
        }


        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion

        public bool CreateFile()
        {
            if (!DoCreateColumnsList())
                return false;
            _LocalColumnList = new ArrayList();
            XmlWriterSettings _Settings = new XmlWriterSettings();
            _Settings.Indent = true;
            _Settings.OmitXmlDeclaration = false;
            _Settings.IndentChars = "\t";

            _Writer = XmlWriter.Create(_FileName, _Settings);

            _Writer.WriteStartElement("XmlStore");//1
            _Writer.WriteAttributeString("Version", null, "1.0");//2

            _Writer.WriteStartElement("schema");//3
            _Writer.WriteAttributeString("datanodename", null, "record");//4

            IEnumerator _enumerate = _ColumnsList.GetEnumerator();
            int index=0;
            while (_enumerate.MoveNext())
            {
                _Writer.WriteStartElement("field");
                _Writer.WriteAttributeString("name", null, GetColumnName(index));
                _Writer.WriteAttributeString("title", null, _enumerate.Current.ToString());
                _Writer.WriteAttributeString("width", null, Convert.ToString(0));
                _Writer.WriteEndElement();
                _LocalColumnList.Add(GetColumnName(index));
                index++;
            }


            IEnumerator _enumerateLocalColumns;
            //foreach (DataRow _Row in _dTable.Rows)
            //{
            //    index = 0;
            //    _enumerateLocalColumns = _LocalColumnList.GetEnumerator();
            //    _Writer.WriteStartElement("record");
            //    while (_enumerateLocalColumns.MoveNext())
            //    {
            //        _Writer.WriteAttributeString(_enumerate.Current.ToString(), null, _Row[index].ToString());
            //        index++;
            //    }

            //    _Writer.WriteEndElement();
            //}

            foreach (DataRow _Row in _dTable.Rows)
            {
                index = 0;
                _enumerateLocalColumns = _ColumnsList.GetEnumerator();
                _Writer.WriteStartElement("record");
                while (_enumerateLocalColumns.MoveNext())
                {
                    _Writer.WriteAttributeString(_LocalColumnList[index].ToString(), null, _Row[index].ToString());
                    index++;
                }

                _Writer.WriteEndElement();
            }



            _Writer.WriteEndElement();//4
            _Writer.WriteEndElement();//3

            //_Writer.WriteEndElement();
            //_Writer.WriteEndElement();//1

            _Writer.Flush();
            _Writer.Close();
            return true;
        }


        private string GetColumnName(int index)
        {
            string RetStr="";
            if (index < 10)
            {
                RetStr = "0" + index.ToString();
                RetStr = "col" + RetStr;
            }
            else
            {
                RetStr = "col" + index.ToString();
            }
            return RetStr;

        }

        private bool DoCreateColumnsList()
        {
            try
            {
                _ColumnsList = new ArrayList();
                foreach (DataColumn _column in _dTable.Columns)
                {
                    _ColumnsList.Add(_column.ColumnName);
                    _ColumnCount++;
                }

                return true;
            }
            catch(Exception Ex)
            {
                MessageBox.Show("Error : " + Ex.Message.ToString(), "Component Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

        }
    }
}
