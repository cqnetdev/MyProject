using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DBUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread t1 = new Thread(new ThreadStart(TestMethod1));
            Thread t2 = new Thread(new ThreadStart(TestMethod2));
            Thread t3 = new Thread(new ThreadStart(TestMethod3));
            Thread t4 = new Thread(new ThreadStart(TestMethod4));
            t1.IsBackground = true;
            t2.IsBackground = true;
            t3.IsBackground = true;
            t4.IsBackground = true;
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            Console.ReadKey();
        }

        private static DBUtilityMSSQL dbHelper1;
        private static DBUtilityMSSQL dbHelper2;
        private static DBUtilityMSSQL dbHelper3;
        private static DBUtilityMSSQL dbHelper4;

        private static string Str1 = "";
        private static string Str2 = "";
        private static string Str3 = "";
        private static string Str4 = "";

        public static void InitMSSQLConn()
        {
            dbHelper1 = new DBUtilityMSSQL(DBUtilityMSSQL.MSSQLConnectionStringDB1);
            dbHelper2 = new DBUtilityMSSQL(DBUtilityMSSQL.MSSQLConnectionStringDB2);
            dbHelper3 = new DBUtilityMSSQL(DBUtilityMSSQL.MSSQLConnectionStringDB3);
            dbHelper4 = new DBUtilityMSSQL(DBUtilityMSSQL.MSSQLConnectionStringDB4);
        }

        private static void TestMethod1()
        {
            InitMSSQLConn();
            DataSet ds = dbHelper1.Query("SELECT TOP 5 Name  FROM sys.objects WHERE type='u'");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rowsCount = dbHelper1.GetSingle(" select count(1) from " + dt.Rows[i]["Name"].ToString()).ToString();
                    Console.WriteLine("Thread1:数据库【SWLM_HUAXU】表【" + dt.Rows[i]["Name"].ToString() + "】中有【" + rowsCount + "】条记录");
                    Str1 += ","+dt.Rows[i]["Name"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Str1))
            {
                System.Diagnostics.Debug.WriteLine("SWLM_HUAXU:" + Str1.Substring(1));
            }
        }

        private static void TestMethod2()
        {
            InitMSSQLConn();
            DataSet ds = dbHelper2.Query("SELECT TOP 5 Name  FROM sys.objects WHERE type='u'");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rowsCount = dbHelper2.GetSingle(" select count(1) from " + dt.Rows[i]["Name"].ToString()).ToString();
                    Console.WriteLine("Thread2:数据库【SWLM_LOG】表【" + dt.Rows[i]["Name"].ToString() + "】中有【" + rowsCount + "】条记录");
                    Str2 += "," + dt.Rows[i]["Name"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Str2))
            {
                System.Diagnostics.Debug.WriteLine("SWLM_LOG:" + Str2.Substring(1));
            }
        }

        private static void TestMethod3()
        {
            InitMSSQLConn();
            DataSet ds = dbHelper3.Query("SELECT TOP 5 Name  FROM sys.objects WHERE type='u'");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rowsCount = dbHelper3.GetSingle(" select count(1) from " + dt.Rows[i]["Name"].ToString()).ToString();
                    Console.WriteLine("Thread3:数据库【SWLM_DATA】表【" + dt.Rows[i]["Name"].ToString() + "】中有【" + rowsCount + "】条记录");
                    Str3 += "," + dt.Rows[i]["Name"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Str3))
            {
                System.Diagnostics.Debug.WriteLine("SWLM_DATA:" + Str3.Substring(1));
            }
        }

        private static void TestMethod4()
        {
            InitMSSQLConn();
            DataSet ds = dbHelper4.Query("SELECT TOP 5 Name  FROM sys.objects WHERE type='u'");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string rowsCount = dbHelper4.GetSingle(" select count(1) from " + dt.Rows[i]["Name"].ToString()).ToString();
                    Console.WriteLine("Thread4:数据库【Test】表【" + dt.Rows[i]["Name"].ToString() + "】中有【" + rowsCount + "】条记录");
                    Str4 += "," + dt.Rows[i]["Name"].ToString();
                }
            }
            if (!string.IsNullOrEmpty(Str4))
            {
                System.Diagnostics.Debug.WriteLine("Test:" + Str4.Substring(1));
            }
        }
    }
}
