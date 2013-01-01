using System;
using System.Xml;
using System.Net;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using System.Data.Odbc;
using System.Collections;
using NUnit.Framework;
using System.Configuration;

namespace WCIRB.LSRMFBridgeTests
{
    /// <summary>
    /// NUnitBase is the parent of all the NUnits generated
    /// by the CodeSmith NUnit template; it is the place where
    /// you would declare any functionality and data 
    /// (methods, instance variables, constants, etc.)
    /// that can be used by all NUnits
    /// </summary>
    public class NUnitBase
    {
        // constants are protected to enable access from the child classes
        #region Protected Constants

        //protected const string STAGING_CONNECTION_STRING = "Server=sf53;uid=LSRUser;pwd=wcirblsr;database=StagingDB";
        //protected const string CAD_CONNECTION_STRING = "Server=db2test1;Database=TPE_DB;UID=tpe_r;PWD=tpe";
        //protected const string LSR_CONNECTION_STRING = "Server=sf53;uid=LSRUser;pwd=wcirblsr;database=LSR";

        //protected const string STAGING_CONNECTION_STRING = "Server=BANGAVERM6TEMP1;uid=sa;pwd=pwd;database=StagingDB";
        //protected const string CAD_CONNECTION_STRING = "Server=db2test1;Database=TPE_DB;UID=tpe_r;PWD=tpe";
        //protected const string LSR_CONNECTION_STRING = "Server=BANGAVERM6TEMP1;uid=sa;pwd=pwd;database=LSRDB";

        protected const string CAD_DATABASE_PREFIX = "CAR_TPE";

        protected const int WAIT_FOR_REPLICATION_TIME = 30000;

        #endregion Protected Constants

        #region Private Properties

        private SqlConnection stagingConnection;
        private SqlConnection lsrConnection;
        
        #endregion Private Properties

        #region Public Properties

        /// <summary>
        /// SqlConnection to Staging database;
        /// gets initialized and opened from the constructor
        /// </summary>
        public SqlConnection StagingConnection
        {
            get
            {
                return stagingConnection;
            }
            set
            {
                stagingConnection = value;
            }
        }

        /// <summary>
        /// SqlConnection to LSR database;
        /// gets initialized and opened from the constructor
        /// </summary>
        public SqlConnection LSRConnection
        {
            get
            {
                return lsrConnection;
            }
            set
            {
                lsrConnection = value;
            }
        }

        #endregion Public Properties

        #region Constructor

        /// <summary>
        /// Default constructor;
        /// This is initially constructor for all the NUnits, since it
        /// gets called by default from the child classes
        /// </summary>
        public NUnitBase()
        {
            this.StagingConnection = new SqlConnection();
            this.LSRConnection = new SqlConnection();

            this.StagingConnection.ConnectionString = ConfigurationSettings.AppSettings["ConStrStaggingDB"].ToString();
            this.LSRConnection.ConnectionString = ConfigurationSettings.AppSettings["ConStrLSRDB"].ToString();

            //Open connections
            this.StagingConnection.Open();
          //  this.LSRConnection.Open();
        }

        #endregion Constructor

        #region Protected Methods

        #region readDataFromLSR

        /// <summary>
        /// Executes given SQL Select query on LSR database and returns
        /// SqlDataReader with the results; note that tableName argument
        /// is optional and is only used for logging purposes
        /// </summary>
        /// <param name="SQLReadCommand">SQL select statement</param>
        /// <param name="tableName">Name of the table against which SELECT statement is 
        /// executed [optional], pass in null if no table name needs to be logged </param>
        /// <returns>SqlDataReader with table results</returns>
        protected SqlDataReader readDataFromLSR(string SQLReadCommand, string tableName)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SQLReadCommand, this.LSRConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                //error occurred connecting to the database
                Console.Error.WriteLine("Exception was thrown when tried to execute: " + SQLReadCommand);

                // log table name if it's given
                if (tableName != null)
                    Assert.Fail("LSR SELECT statement on " + tableName + " resulted in Exception:\n" + e.Message);
                else
                    Assert.Fail("LSR SELECT statement resulted in Exception:\n" + e.Message);
            }

            // this statement will never be reached but we include it to avoid compiler errors
            return null;
        }

        #endregion 

        #region readDataFromStaging

        /// <summary>
        /// Executes given SQL Select query on Staging database and returns
        /// SqlDataReader with the results; note that tableName argument
        /// is optional and is only used for logging purposes
        /// </summary>
        /// <param name="SQLReadCommand">SQL select statement</param>
        /// <param name="tableName">Name of the table against which SELECT statement is 
        /// executed [optional], pass in null if no table name needs to be logged </param>
        /// <returns>SqlDataReader with table results</returns>
        protected SqlDataReader readDataFromStaging(string SQLReadCommand, string tableName)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(SQLReadCommand, this.StagingConnection);
                SqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                //error occurred connecting to the database
                Console.Error.WriteLine("Exception was thrown when tried to execute: " + SQLReadCommand);

                // log table name if it's given
                if (tableName != null)
                    Assert.Fail("Staging SELECT statement on " + tableName + " resulted in Exception:\n" + e.Message);
                else
                    Assert.Fail("Staging SELECT statement resulted in Exception:\n" + e.Message);
            }

            // this statement will never be reached but we include it to avoid compiler errors
            return null;
        }

        #endregion 

        #region executeTransactionOnStaging

        /// <summary>
        /// Executes given SQL Insert, Update, or Delete command on LSR database; transaction
        /// is commited only if there are no exceptions thrown and no more than 1 row 
        /// is effected by the given command; tableName argument is optional and is only
        /// used for logging purposes
        /// </summary>
        /// <param name="SQLReadCommand">SQL Insert, Update, or Delete statement</param>
        /// <param name="tableName">Name of the table against which SELECT statement is 
        /// executed [optional], pass in null if no table name needs to be logged </param>
        /// <returns>the number of rows effected</returns>
        protected int executeTransactionOnStaging(string sqlCommand, string tableName)
        {
            try
            {
                SqlTransaction CADTrans = StagingConnection.BeginTransaction();

                SqlCommand cmd = new SqlCommand(sqlCommand, this.StagingConnection, CADTrans);
                int retVal = cmd.ExecuteNonQuery();

                //if (retVal > 1)
                //{
                //    CADTrans.Rollback();
                //    Console.Error.WriteLine("LSR non-query effects " + retVal + " rows: " + sqlCommand);

                //    // log table name if it's given
                //    if (tableName != null)
                //        Assert.Fail("LSR non-query on " + tableName + " effects " + retVal + " rows. See logs.");
                //    else
                //        Assert.Fail("LSR non-query effects " + retVal + " rows. See logs.");

                //}
                //else
                //{
                    CADTrans.Commit();
                    return retVal;
                //}
            }
            catch (Exception e)
            {
                //error occurred connecting to the database
                Console.Error.WriteLine("Exception was thrown when tried to execute: " + sqlCommand);

                // log table name if it's given
                if (tableName != null)
                    Assert.Fail("LSR non-query statement on " + tableName + " resulted in Exception:\n" + e.Message);
                else
                    Assert.Fail("LSR non-query statement resulted in Exception:\n" + e.Message);
            }
            
            // this statement will never be reached but we include it to avoid compiler errors
            return 0;
        }

        #endregion

        #region executeTransactionOnLSR

        /// <summary>
        /// Executes given SQL Insert, Update, or Delete command on LSR database; transaction
        /// is commited only if there are no exceptions thrown and no more than 1 row 
        /// is effected by the given command; tableName argument is optional and is only
        /// used for logging purposes
        /// </summary>
        /// <param name="SQLReadCommand">SQL Insert, Update, or Delete statement</param>
        /// <param name="tableName">Name of the table against which SELECT statement is 
        /// executed [optional], pass in null if no table name needs to be logged </param>
        /// <returns>the number of rows effected</returns>
        protected int executeTransactionOnLSR(string sqlCommand, string tableName)
        {
            try
            {
                SqlTransaction LSRTrans = LSRConnection.BeginTransaction();

                SqlCommand cmd = new SqlCommand(sqlCommand, this.LSRConnection, LSRTrans);
                int retVal = cmd.ExecuteNonQuery();

                //if (retVal > 1)
                //{
                //    LSRTrans.Rollback();
                //    Console.Error.WriteLine("LSR non-query effects " + retVal + " rows: " + sqlCommand);

                //    // log table name if it's given
                //    if (tableName != null)
                //        Assert.Fail("LSR non-query on " + tableName + " effects " + retVal + " rows. See logs.");
                //    else
                //        Assert.Fail("LSR non-query effects " + retVal + " rows. See logs.");

                //}
                //else
                //{
                LSRTrans.Commit();
                return retVal;
                //}
            }
            catch (Exception e)
            {
                //error occurred connecting to the database
                Console.Error.WriteLine("Exception was thrown when tried to execute: " + sqlCommand);

                // log table name if it's given
                if (tableName != null)
                    Assert.Fail("LSR non-query statement on " + tableName + " resulted in Exception:\n" + e.Message);
                else
                    Assert.Fail("LSR non-query statement resulted in Exception:\n" + e.Message);
            }
            // this statement will never be reached but we include it to avoid compiler errors
            return 0;
        }

        #endregion

        #region executeLSRPrerequisite

        /// <summary>
        /// Executes given LSR prerequisite SQL statement and checks if either there is 
        /// at least one row returned or there is no row returned based on wantRow flag.
        /// </summary>
        /// <param name="sqlStmt">SQL prerequisite statement to be executed on LSR</param>
        /// <param name="wantRow">if true, checks if there is at least one row returned;
        /// if false, checks if there is no row returned</param>
        protected void executeLSRPrerequisite(string sqlStmt, bool wantRow)
        {
            SqlDataReader reader = readDataFromLSR(sqlStmt, null);
            bool hasRow = reader.HasRows;
            reader.Close();

            // if prereq is successful
            if (((wantRow == true) && (hasRow == true)) || ((wantRow == false) && (hasRow == false)))
            {
                Console.Out.WriteLine("LSR Prerequisite Query " + sqlStmt + " Successful");
            }
            // prereq fails
            else
            {
                if ((wantRow == true) && (hasRow == false))
                {
                    Console.Error.WriteLine("LSR Prerequisite Query returned no rows on: " + sqlStmt);
                    Assert.Fail("LSR Prerequisite Query Failed - No Rows Returned: " + sqlStmt);
                }
                else if ((wantRow == false) && (hasRow == true))
                {
                    Console.Error.WriteLine("LSR Prerequisite Query returned rows: " + sqlStmt);
                    Assert.Fail("LSR Prerequisite Query Failed - Rows Returned: " + sqlStmt);
                }
            }
        }

        #endregion

        #region getNumberOfLSRRowsEffected

        /// <summary>
        /// Runs a SELECT query against the given LSR table with given where clause and
        /// returns the number of rows returned; used before executing updates and deletes
        /// on LSR
        /// </summary>
        /// <param name="table">LSR table to query</param>
        /// <param name="whereClause">where clause to use in SELECT query</param>
        /// <returns>number of rows returned by the SELECT query</returns>
        protected int getNumberOfLSRRowsEffected(string table, string whereClause)
        {
            string query = "SELECT * FROM " + table + " " + whereClause;

            SqlDataReader reader = readDataFromLSR(query, table);

            int count = 0;

            if (reader.HasRows)
            {
                while (reader.Read())
                    count++;
            }

            reader.Close();

            return count;
        }

        #endregion

        #region CompareData

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ouputList"></param>
        /// <param name="targetList"></param>
        /// <returns></returns>
        protected string CompareData(Hashtable ouputList, Hashtable targetList)
        {
            Object[] keys = new Object[ouputList.Count];
            //Array keys = new Array; // = null;
            ouputList.Keys.CopyTo(keys, 0);
            //ICollection keys = ouputList.Keys;
            string outputString = string.Empty;

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys.GetValue(i).ToString().IndexOf("Date") != -1)
                {
                    if (!Convert.ToDateTime(targetList[keys.GetValue(i)]).Date.Equals(Convert.ToDateTime(ouputList[keys.GetValue(i)]).Date))
                    {
                        outputString = outputString + "Source Value:" + ouputList[keys.GetValue(i)].ToString() + "and Target Value: " + targetList[keys.GetValue(i)].ToString() + Environment.NewLine;
                    }
                }
                else
                {
                    if (!targetList[keys.GetValue(i)].ToString().Equals(ouputList[keys.GetValue(i)].ToString()))
                    {
                        outputString = outputString + "Source Value:" + ouputList[keys.GetValue(i)].ToString() + " and Target Value: " + targetList[keys.GetValue(i)].ToString() + Environment.NewLine;
                    }
                }
            }
            return outputString;
        }

        #endregion

        #region InsertDataInTableFromXml

        protected int InsertDataInTableFromXml(string fileName, string tableName, SqlConnection conn)
        {
            XmlDocument testdataDoc = new XmlDocument();
            SqlCommand cmdInsert = new SqlCommand();
            int test = 0;
            
            testdataDoc.Load(fileName);

            cmdInsert.Connection = conn;
            cmdInsert.CommandType = CommandType.StoredProcedure;
            cmdInsert.CommandText = TestConstants.CONST_SP_INSERTSOURCEDATA;
            cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_DOC_PATH, testdataDoc.OuterXml);
            cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_TABLE_NAME, tableName);
          //  cmdInsert.Parameters[TestConstants.CONST_SP_INPUT_TABLE_NAME].Value = tableName;
            //cmdInsert.ExecuteNonQuery();
            if (conn.State != ConnectionState.Open)
            { conn.Open(); }
            cmdInsert.ExecuteNonQuery();
            //test = Int32.Parse(cmdInsert.ExecuteScalar().ToString());
            return test;
            //cmdInsert.Parameters.Add(TestConstants.CONST_SP_INPUT_TABLE_NAME, testdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_EMPLOYERBAC).Name);

        }
        #endregion

        #region InsertDataInTablesFromXml

        protected Hashtable InsertDataInTablesFromXml(String fileName, SqlConnection conn)
        {
            XmlDocument testdataDoc = new XmlDocument();
            SqlCommand cmdInsert = new SqlCommand();
            

            testdataDoc.Load(fileName);

            cmdInsert.Connection = conn;
            cmdInsert.CommandType = CommandType.StoredProcedure;
            cmdInsert.CommandText = TestConstants.CONST_SP_INSERTSOURCEDATA;
            cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_DOC_PATH, testdataDoc.OuterXml);
            cmdInsert.Parameters.AddWithValue(TestConstants.CONST_SP_INPUT_TABLE_NAME, string.Empty);

            //Loop through all the nodes in the xml and call the
            //insert source data stored procedure by passing the respective table
            //name
            int nodeCount = testdataDoc.SelectSingleNode("Inserts").ChildNodes.Count;
            Hashtable tableList = new Hashtable();

            for (int node = 0; node < nodeCount; node++)
            {
                string nodeName = testdataDoc.SelectSingleNode("Inserts").ChildNodes[node].Name;
                cmdInsert.Parameters[TestConstants.CONST_SP_INPUT_TABLE_NAME].Value = "LSR." + nodeName;
                string key = Convert.ToString(cmdInsert.ExecuteNonQuery());
                tableList.Add(nodeName, key);

            }
            //cmdInsert.Parameters.Add(TestConstants.CONST_SP_INPUT_TABLE_NAME, testdataDoc.SelectSingleNode(TestConstants.CONST_XPATH_EMPLOYERBAC).Name);
            return tableList;
            
        }
        #endregion

        #region GetOutputTestData

        protected Hashtable GetOutputTestData(string fileName, string nodeName)
        {
            //Get the data from target XML file
            Hashtable outputDataList = new Hashtable();
            XmlDocument insurerOutputdataDoc = new XmlDocument();
            //XmlAttributeCollection columnCollection = new XmlAttributeCollection();

            insurerOutputdataDoc.Load(fileName);
            XmlAttributeCollection columnCollection = insurerOutputdataDoc.SelectSingleNode(nodeName).Attributes;
            int columnCount = columnCollection.Count;
            if (columnCount > 0)
            {
                for (int count = 0; count < columnCount; count++)
                {
                    outputDataList.Add(columnCollection.Item(count).Name, columnCollection.Item(count).Value);
                }
            }

            return outputDataList;
        }

        #endregion

        #region GetOutputData
        protected Hashtable GetOutputData(string tablename, string keyValue)
        {
            //Get the data from the Insurer target table
            Hashtable targetDataList = new Hashtable();            
            
            try
            {
               
                SqlDataReader readData = this.readDataFromStaging("SELECT * FROM CDC_" + tablename + "WHERE " + tablename + "GID = '" + keyValue + "'", tablename);
                string insurerName = String.Empty;

                if (readData.HasRows)
                {
                    readData.Read();
                    int columnCount = readData.FieldCount;
                    for (int count = 0; count < columnCount; count++)
                    {
                        targetDataList.Add(readData.GetName(count), readData.GetValue(count));
                    }
                }
                if (readData != null)
                {
                    readData.Close();
                    readData = null;
                }
            }
            catch (Exception exp)
            {
                throw exp;
            }
            return targetDataList;
        }

        #endregion

        #region Read Mapping Flat Files and return Array list

        protected ArrayList GetDataFromFlatFile(string FlatfileName)
        {
            ArrayList targetTable = new ArrayList();
            Hashtable tblOutput = new Hashtable();

           // StreamReader sr = new StreamReader(FlatfileName);
            string temp = String.Empty;
            using (StreamReader sr = new StreamReader(FlatfileName))
            {

                while ((temp = sr.ReadLine()) != null)
                {
                    string[] strFields = null;
                    try
                    {
                        strFields = temp.Split("|".ToCharArray());

                        for (int i = 0; i < strFields.Length; i++)
                        {
                            tblOutput.Add("Column" + i, strFields[i].ToString());
                        }
                        targetTable.Add(tblOutput);
                    }
                    catch (Exception objException)
                    {
                        throw (objException);
                    }

                }
            }
            return targetTable;
        }


        #endregion

        #endregion Protected Methods
    }
}
