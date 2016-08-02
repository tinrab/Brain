using System.Collections.Generic;

namespace Brain.Data
{
    public class DataTable
    {
        public DataTable()
        {
            Rows = new List<DataRow>();
        }

        public DataTable(DataTable dataTable)
        {
            Rows = new List<DataRow>();

            for (int i = 0; i < dataTable.Length; i++) {
                Rows.Add(new DataRow(dataTable[i]));
            }
        }

        public IList<DataRow> Rows { get; set; }

        public DataRow this[int index]
        {
            get { return Rows[index]; }
            set { Rows[index] = value; }
        }

        public int Length
        {
            get { return Rows.Count; }
        }

        public void AddRow(params double[] row)
        {
            Rows.Add(new DataRow(row));
        }

        public void DeleteColumn(int column)
        {
            for (var i = 0; i < Rows.Count; i++) {
                var row = Rows[i];
                var newValues = new double[row.Length - 1];
                var k = 0;

                for (var j = 0; j < row.Length; j++) {
                    if (j != column) {
                        newValues[k] = row.Values[j];
                        k++;
                    }
                }

                Rows[i].Values = newValues;
            }
        }

        public double[] SelectColumn(int column)
        {
            var rows = new double[Rows.Count];

            for (var i = 0; i < rows.Length; i++) {
                rows[i] = Rows[i].Values[column];
            }

            return rows;
        }
    }
}