namespace Brain.Data
{
    public class DataRow
    {
        public DataRow(params double[] values)
        {
            Values = values;
        }

        public DataRow(DataRow dataRow)
        {
            Values = new double[dataRow.Length];

            for (int i = 0; i < dataRow.Length; i++) {
                Values[i] = dataRow[i];
            }
        }

        public double this[int index]
        {
            get { return Values[index]; }
            set { Values[index] = value; }
        }

        public double[] Values { get; set; }

        public int Length
        {
            get { return Values == null ? 0 : Values.Length; }
        }
    }
}