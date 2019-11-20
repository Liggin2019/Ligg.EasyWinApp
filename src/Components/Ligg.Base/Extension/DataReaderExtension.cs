using System;
using System.Data;
using System.Text;

namespace Ligg.Base.Extension
{
    /// <summary>
    /// 用来转换含有NULL值的字段为合适值的辅助类
    /// </summary>
    public sealed class DataReaderExtension
    {
        private DateTime defaultDate;

        public DataReaderExtension(IDataReader reader)
        {
            defaultDate = Convert.ToDateTime("01/01/1900");
            this.reader = reader;
        }

        public int GetInt32(String column)
        {
            int data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (int)0 : (int)reader[column];
            return data;
        }

        public short GetInt16(String column)
        {
            short data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (short)0 : (short)reader[column];
            return data;
        }

        public byte GetByte(String column)
        {
            byte data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (byte)0 : (byte)reader[column];
            return data;
        }

        public float GetFloat(String column)
        {
            float data = (reader.IsDBNull(reader.GetOrdinal(column))) ? 0 : float.Parse(reader[column].ToString());
            return data;
        }

        public double GetDouble(String column)
        {
            double data = (reader.IsDBNull(reader.GetOrdinal(column))) ? 0 : double.Parse(reader[column].ToString());
            return data;
        }

        public decimal GetDecimal(String column)
        {
            decimal data = (reader.IsDBNull(reader.GetOrdinal(column))) ? 0 : decimal.Parse(reader[column].ToString());
            return data;
        }

        public Single GetSingle(String column)
        {
            Single data = (reader.IsDBNull(reader.GetOrdinal(column))) ? 0 : Single.Parse(reader[column].ToString());
            return data;
        }

        public bool GetBoolean(String column)
        {
            bool data = (reader.IsDBNull(reader.GetOrdinal(column))) ? false : (bool)reader[column];
            return data;
        }

        public String GetString(String column)
        {
            String data = (reader.IsDBNull(reader.GetOrdinal(column))) ? null : reader[column].ToString();
            return data;
        }

        public byte[] GetBytes(String column)
        {
            String data = (reader.IsDBNull(reader.GetOrdinal(column))) ? null : reader[column].ToString();
            return Encoding.UTF8.GetBytes(data);
        }

        public Guid GetGuid(String column)
        {
            String data = (reader.IsDBNull(reader.GetOrdinal(column))) ? null : reader[column].ToString();
            Guid guid = Guid.Empty;
            if (data != null)
            {
                guid = new Guid(data);
            }
            return guid;
        }

        public DateTime GetDateTime(String column)
        {
            DateTime data = (reader.IsDBNull(reader.GetOrdinal(column))) ? defaultDate : (DateTime)reader[column];
            return data;
        }

        public bool Read()
        {
            return reader.Read();
        }

        private IDataReader reader;
    }


}
