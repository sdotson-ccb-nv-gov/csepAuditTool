﻿using csepAuditTool.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Configuration;
using System.Runtime.InteropServices.ObjectiveC;

namespace csepAuditTool.DAL
{
    internal class AccelaDataModel
    {
        private OdbcConnection? _odbcConnection { get; set; }
        public void BuildOdbcConnection()
        {
            var connStr = ConfigurationManager.ConnectionStrings["accelaSqlConnectionString"].ConnectionString;
            //connStr += "Uid=testUser;Pwd=testUser;"; OR ENCRYPT STRING
            _odbcConnection = new OdbcConnection(connStr);
            _odbcConnection.Open();
        }
        public AccelaDataModel() { }

        public List<OutgoingRowModel> GetMatches(IncomingRowModel incomingRowValues, OutgoingLocateRequestModel outgoingLocateRequest)
        {
            if (_odbcConnection == null) BuildOdbcConnection();

            //var mySingleRow = outgoingLocateRequest.SingleRowModel();

            var matchedResults = new List<OutgoingRowModel>();

            var query1Text = "SELECT p.B1_ALT_ID [Cannabis_Agent_Card_Number]     ,CASE WHEN p.B1_APPL_STATUS IS NOT NULL AND p.B1_APPL_STATUS IN ('Pending Background','Additional Info Required-Bkgrd','Board Review','Approved','Final Review','Registered','Waiting for Background','Pend Bkgrd Results-Temp Issued','Fingerprint Return-W/Temp') THEN 'A' ELSE 'I' END [Cannabis_Agent_Card_Status]     ,FORMAT(ex.EXPIRATION_DATE,'MM/dd/yyyy') [Cannabis_Agent_Card_Expiration_Date]     ,c.B1_LNAME [Agent_Last_Name]     ,c.B1_FNAME [Agent_First_Name]     ,SUBSTRING(c.B1_MNAME,0,1) [Agent_Middle_Initial]     ,'' [Modifier_of_the_Agent]     ,REPLACE(c.B1_SOCIAL_SECURITY_NUMBER,'-','') [Agent_Social_Security_Number]     ,FORMAT(c.B1_BIRTH_DATE,'MM/dd/yyyy') [Agent_Date_of_Birth]     ,eye.B1_CHECKLIST_COMMENT [Agent_Eye_Color]     ,hair.B1_CHECKLIST_COMMENT [Agent_Hair_Color]     ,ht.B1_CHECKLIST_COMMENT [Agent_Height]     ,wt.B1_CHECKLIST_COMMENT [Agent_Weight]     ,g.B1_CHECKLIST_COMMENT [Agent_Gender]     ,mail.G7_ADDRESS1 [Agent_Mailing_Address_Line_1]     ,mail.G7_ADDRESS2 [Agent_Mailing_Address_Line_2]     ,mail.G7_CITY [Agent_Mailing_Address_City]     ,mail.G7_STATE [Agent_Mailing_Address_State]     ,mail.G7_ZIP [Agent_Mailing_Address_Zip_Code]     ,mail.[G7_ZIP_PLUS_4] [Agent_Mailing_Address_Zip_+_4]     ,FORMAT(mail.LAST_UPDATE,'MM/dd/yyyy') [Date_of_Last_Agent_Mailing_Address_Update]     ,home.G7_ADDRESS1 [Agent_Residential_Address_Line_1]     ,home.G7_ADDRESS2 [Agent_Residential_Address_Line_2]     ,home.G7_CITY [Agent_Residential_Address_City]     ,home.G7_STATE [Agent_Residential_Address_State]     ,home.G7_ZIP [Agent_Residential_Address_Zip_Code]     ,home.[G7_ZIP_PLUS_4] [Agent_Residential_Address_Zip_+_4]     ,FORMAT(home.LAST_UPDATE,'MM/dd/yyyy') [Date_of_Last_Agent_Residential_Address_Update]     ,c.B1_PHONE3 [Agent_Home_Phone_Number]     ,c.B1_PHONE2 [Agent_Mobile_Phone_Number]     ,c.B1_EMAIL [Email_Address]     ,entName.B1_CHECKLIST_COMMENT [Entity_Name]     ,'' [Entity_Mailing_Address_Line_1]     ,'' [Entity_Mailing_Address_Line_2]     ,'' [Entity_Mailing_Address_City]     ,'' [Entity_Mailing_Address_County]     ,'' [Entity_Mailing_Address_State]     ,'' [Entity_Mailing_Address_Zip_Code]     ,'' [Entity_Mailing_Address_Zip_+_4]     ,'' [Business_Telephone_Number]     ,'' [Business_Telephone_Extension_Number] FROM B1PERMIT p JOIN B3CONTACT c ON 1=1     AND c.SERV_PROV_CODE=p.SERV_PROV_CODE     AND c.B1_PER_ID1=p.B1_PER_ID1     AND c.B1_PER_ID2=p.B1_PER_ID2     AND c.B1_PER_ID3=p.B1_PER_ID3 LEFT JOIN B1_EXPIRATION ex ON 1=1     AND ex.SERV_PROV_CODE=p.SERV_PROV_CODE     AND ex.B1_PER_ID1=p.B1_PER_ID1     AND ex.B1_PER_ID2=p.B1_PER_ID2     AND ex.B1_PER_ID3=p.B1_PER_ID3 LEFT JOIN BCHCKBOX eye ON 1=1     AND eye.SERV_PROV_CODE=p.SERV_PROV_CODE     AND eye.B1_PER_ID1=p.B1_PER_ID1     AND eye.B1_PER_ID2=p.B1_PER_ID2     AND eye.B1_PER_ID3=p.B1_PER_ID3     AND eye.B1_CHECKBOX_DESC='Natural Eye Color' LEFT JOIN BCHCKBOX hair ON 1=1     AND hair.SERV_PROV_CODE=p.SERV_PROV_CODE     AND hair.B1_PER_ID1=p.B1_PER_ID1     AND hair.B1_PER_ID2=p.B1_PER_ID2     AND hair.B1_PER_ID3=p.B1_PER_ID3     AND hair.B1_CHECKBOX_DESC='Natural Hair Color' LEFT JOIN BCHCKBOX ht on 1=1     AND ht.SERV_PROV_CODE=p.SERV_PROV_CODE     AND ht.B1_PER_ID1=p.B1_PER_ID1     AND ht.B1_PER_ID2=p.B1_PER_ID2     AND ht.B1_PER_ID3=p.B1_PER_ID3     AND ht.B1_CHECKBOX_DESC='Height' LEFT JOIN BCHCKBOX wt ON 1=1     AND wt.SERV_PROV_CODE=p.SERV_PROV_CODE     AND wt.B1_PER_ID1=p.B1_PER_ID1     and wt.B1_PER_ID2=p.B1_PER_ID2     and wt.B1_PER_ID3=p.B1_PER_ID3     AND wt.B1_CHECKBOX_DESC='Weight' LEFT JOIN BCHCKBOX g ON 1=1     AND g.SERV_PROV_CODE=p.SERV_PROV_CODE     AND g.B1_PER_ID1=p.B1_PER_ID1     AND g.B1_PER_ID2=p.B1_PER_ID2     AND g.B1_PER_ID3=p.B1_PER_ID3     AND g.B1_CHECKBOX_DESC='Gender' LEFT JOIN BCHCKBOX entName ON 1=1     AND entName.SERV_PROV_CODE=p.SERV_PROV_CODE     AND entName.B1_PER_ID1=p.B1_PER_ID1     AND entName.B1_PER_ID2=p.B1_PER_ID2     AND entName.B1_PER_ID3=p.B1_PER_ID3     AND entName.B1_CHECKBOX_DESC='If you are an Employee, please list the name of the establishment you will be working in, if known'  OUTER APPLY (     SELECT TOP 1 ma.G7_ADDRESS1         ,ma.G7_ADDRESS2         ,ma.G7_CITY         ,ma.G7_STATE         ,ma.G7_ZIP         ,'' G7_ZIP_PLUS_4         ,CASE WHEN ma.AUDIT_MOD_DATE IS NOT NULL THEN ma.AUDIT_MOD_DATE ELSE ma.REC_DATE END [LAST_UPDATE]     FROM XRECORD_CONTACT_ENTITY xm     LEFT JOIN G7CONTACT_ADDRESS ma ON 1=1         AND ma.SERV_PROV_CODE=p.SERV_PROV_CODE         AND ma.G7_ENTITY_TYPE=xm.ENT_TYPE         AND ma.RES_ID=xm.ENT_ID1         AND ma.G7_ADDRESS_TYPE='Mailing'     WHERE 1=1         AND xm.B1_PER_ID1=p.B1_PER_ID1         AND xm.B1_PER_ID2=p.B1_PER_ID2         AND xm.B1_PER_ID3=p.B1_PER_ID3         AND xm.REC_STATUS='A'         AND ma.G7_ADDRESS1 IS NOT NULL ) mail OUTER APPLY (     SELECT TOP 1 ma.G7_ADDRESS1         ,ma.G7_ADDRESS2         ,ma.G7_CITY         ,ma.G7_STATE         ,ma.G7_ZIP         ,'' G7_ZIP_PLUS_4         ,CASE WHEN ma.AUDIT_MOD_DATE IS NOT NULL THEN ma.AUDIT_MOD_DATE ELSE ma.REC_DATE END [LAST_UPDATE]     FROM XRECORD_CONTACT_ENTITY xm     LEFT JOIN G7CONTACT_ADDRESS ma ON 1=1         AND ma.SERV_PROV_CODE=p.SERV_PROV_CODE         AND ma.G7_ENTITY_TYPE=xm.ENT_TYPE         AND ma.RES_ID=xm.ENT_ID1         AND ma.G7_ADDRESS_TYPE='Home'     WHERE 1=1         AND xm.B1_PER_ID1=p.B1_PER_ID1         AND xm.B1_PER_ID2=p.B1_PER_ID2         AND xm.B1_PER_ID3=p.B1_PER_ID3         AND xm.REC_STATUS='A'         AND ma.G7_ADDRESS1 IS NOT NULL ) home WHERE 1=1     AND p.SERV_PROV_CODE='NVMED'     AND c.B1_CONTACT_TYPE='Employee'     AND p.B1_APPL_STATUS IS NOT NULL     AND (         c.B1_SOCIAL_SECURITY_NUMBER=?         AND         SUBSTRING(c.B1_FNAME,1,4)=?     )";
            //DO NOT ALTER QUERY RETURNING FIELDS, COLUMN NAMES USED FOR MATCH TO OBJECT
            var search1Command = _odbcConnection.CreateCommand();
            search1Command.CommandText = query1Text;
            search1Command.Parameters.Add("@B1_SOCIAL_SECURITY_NUMBER", OdbcType.VarChar).Value = incomingRowValues.SsnWithDashes;
            search1Command.Parameters.Add("@B1_FNAME", OdbcType.VarChar).Value = incomingRowValues.FirstNameFirst4;
            var dr1 = search1Command.ExecuteReader();
            if (dr1 != null && dr1.HasRows)
            {
                //var count = dr1.FieldCount;
                while (dr1.Read())
                {
                    var outgoingRow = new OutgoingRowModel();
                    for (var i = 0; i < dr1.FieldCount; i++)
                    {
                        var test = dr1.GetName(i);
                        var thisColumn = outgoingLocateRequest.SingleRowModel().FirstOrDefault(p => p.ColNam == dr1.GetName(i));
                        if (thisColumn == null) continue;
                        var newColumn = new ColumnsModel(i, thisColumn.ColLen, thisColumn.ColNam, dr1.GetValue(i).ToString() ?? "");
                        outgoingRow.RowCols.Add(newColumn);
                    }
                    matchedResults.Add(outgoingRow);
                }
                dr1.Close();
            }
            search1Command.Dispose();
            if (matchedResults.Count > 0) return matchedResults;

            if (incomingRowValues.LastName == "Skywalker" && incomingRowValues.FirstName == "Anakin")
            {
                var t = "";
            }
            var query2Text = "SELECT p.B1_ALT_ID [Cannabis_Agent_Card_Number]     ,CASE WHEN p.B1_APPL_STATUS IS NOT NULL AND p.B1_APPL_STATUS IN ('Pending Background','Additional Info Required-Bkgrd','Board Review','Approved','Final Review','Registered','Waiting for Background','Pend Bkgrd Results-Temp Issued','Fingerprint Return-W/Temp') THEN 'A' ELSE 'I' END [Cannabis_Agent_Card_Status]     ,FORMAT(ex.EXPIRATION_DATE,'MM/dd/yyyy') [Cannabis_Agent_Card_Expiration_Date]     ,c.B1_LNAME [Agent_Last_Name]     ,c.B1_FNAME [Agent_First_Name]     ,SUBSTRING(c.B1_MNAME,0,1) [Agent_Middle_Initial]     ,'' [Modifier_of_the_Agent]     ,REPLACE(c.B1_SOCIAL_SECURITY_NUMBER,'-','') [Agent_Social_Security_Number]     ,FORMAT(c.B1_BIRTH_DATE,'MM/dd/yyyy') [Agent_Date_of_Birth]     ,eye.B1_CHECKLIST_COMMENT [Agent_Eye_Color]     ,hair.B1_CHECKLIST_COMMENT [Agent_Hair_Color]     ,ht.B1_CHECKLIST_COMMENT [Agent_Height]     ,wt.B1_CHECKLIST_COMMENT [Agent_Weight]     ,g.B1_CHECKLIST_COMMENT [Agent_Gender]     ,mail.G7_ADDRESS1 [Agent_Mailing_Address_Line_1]     ,mail.G7_ADDRESS2 [Agent_Mailing_Address_Line_2]     ,mail.G7_CITY [Agent_Mailing_Address_City]     ,mail.G7_STATE [Agent_Mailing_Address_State]     ,mail.G7_ZIP [Agent_Mailing_Address_Zip_Code]     ,mail.[G7_ZIP_PLUS_4] [Agent_Mailing_Address_Zip_+_4]     ,FORMAT(mail.LAST_UPDATE,'MM/dd/yyyy') [Date_of_Last_Agent_Mailing_Address_Update]     ,home.G7_ADDRESS1 [Agent_Residential_Address_Line_1]     ,home.G7_ADDRESS2 [Agent_Residential_Address_Line_2]     ,home.G7_CITY [Agent_Residential_Address_City]     ,home.G7_STATE [Agent_Residential_Address_State]     ,home.G7_ZIP [Agent_Residential_Address_Zip_Code]     ,home.[G7_ZIP_PLUS_4] [Agent_Residential_Address_Zip_+_4]     ,FORMAT(home.LAST_UPDATE,'MM/dd/yyyy') [Date_of_Last_Agent_Residential_Address_Update]     ,c.B1_PHONE3 [Agent_Home_Phone_Number]     ,c.B1_PHONE2 [Agent_Mobile_Phone_Number]     ,c.B1_EMAIL [Email_Address]     ,entName.B1_CHECKLIST_COMMENT [Entity_Name]     ,'' [Entity_Mailing_Address_Line_1]     ,'' [Entity_Mailing_Address_Line_2]     ,'' [Entity_Mailing_Address_City]     ,'' [Entity_Mailing_Address_County]     ,'' [Entity_Mailing_Address_State]     ,'' [Entity_Mailing_Address_Zip_Code]     ,'' [Entity_Mailing_Address_Zip_+_4]     ,'' [Business_Telephone_Number]     ,'' [Business_Telephone_Extension_Number] FROM B1PERMIT p JOIN B3CONTACT c ON 1=1     AND c.SERV_PROV_CODE=p.SERV_PROV_CODE     AND c.B1_PER_ID1=p.B1_PER_ID1     AND c.B1_PER_ID2=p.B1_PER_ID2     AND c.B1_PER_ID3=p.B1_PER_ID3 LEFT JOIN B1_EXPIRATION ex ON 1=1     AND ex.SERV_PROV_CODE=p.SERV_PROV_CODE     AND ex.B1_PER_ID1=p.B1_PER_ID1     AND ex.B1_PER_ID2=p.B1_PER_ID2     AND ex.B1_PER_ID3=p.B1_PER_ID3 LEFT JOIN BCHCKBOX eye ON 1=1     AND eye.SERV_PROV_CODE=p.SERV_PROV_CODE     AND eye.B1_PER_ID1=p.B1_PER_ID1     AND eye.B1_PER_ID2=p.B1_PER_ID2     AND eye.B1_PER_ID3=p.B1_PER_ID3     AND eye.B1_CHECKBOX_DESC='Natural Eye Color' LEFT JOIN BCHCKBOX hair ON 1=1     AND hair.SERV_PROV_CODE=p.SERV_PROV_CODE     AND hair.B1_PER_ID1=p.B1_PER_ID1     AND hair.B1_PER_ID2=p.B1_PER_ID2     AND hair.B1_PER_ID3=p.B1_PER_ID3     AND hair.B1_CHECKBOX_DESC='Natural Hair Color' LEFT JOIN BCHCKBOX ht on 1=1     AND ht.SERV_PROV_CODE=p.SERV_PROV_CODE     AND ht.B1_PER_ID1=p.B1_PER_ID1     AND ht.B1_PER_ID2=p.B1_PER_ID2     AND ht.B1_PER_ID3=p.B1_PER_ID3     AND ht.B1_CHECKBOX_DESC='Height' LEFT JOIN BCHCKBOX wt ON 1=1     AND wt.SERV_PROV_CODE=p.SERV_PROV_CODE     AND wt.B1_PER_ID1=p.B1_PER_ID1     and wt.B1_PER_ID2=p.B1_PER_ID2     and wt.B1_PER_ID3=p.B1_PER_ID3     AND wt.B1_CHECKBOX_DESC='Weight' LEFT JOIN BCHCKBOX g ON 1=1     AND g.SERV_PROV_CODE=p.SERV_PROV_CODE     AND g.B1_PER_ID1=p.B1_PER_ID1     AND g.B1_PER_ID2=p.B1_PER_ID2     AND g.B1_PER_ID3=p.B1_PER_ID3     AND g.B1_CHECKBOX_DESC='Gender' LEFT JOIN BCHCKBOX entName ON 1=1     AND entName.SERV_PROV_CODE=p.SERV_PROV_CODE     AND entName.B1_PER_ID1=p.B1_PER_ID1     AND entName.B1_PER_ID2=p.B1_PER_ID2     AND entName.B1_PER_ID3=p.B1_PER_ID3     AND entName.B1_CHECKBOX_DESC='If you are an Employee, please list the name of the establishment you will be working in, if known'  OUTER APPLY (     SELECT TOP 1 ma.G7_ADDRESS1         ,ma.G7_ADDRESS2         ,ma.G7_CITY         ,ma.G7_STATE         ,ma.G7_ZIP         ,'' G7_ZIP_PLUS_4         ,CASE WHEN ma.AUDIT_MOD_DATE IS NOT NULL THEN ma.AUDIT_MOD_DATE ELSE ma.REC_DATE END [LAST_UPDATE]     FROM XRECORD_CONTACT_ENTITY xm     LEFT JOIN G7CONTACT_ADDRESS ma ON 1=1         AND ma.SERV_PROV_CODE=p.SERV_PROV_CODE         AND ma.G7_ENTITY_TYPE=xm.ENT_TYPE         AND ma.RES_ID=xm.ENT_ID1         AND ma.G7_ADDRESS_TYPE='Mailing'     WHERE 1=1         AND xm.B1_PER_ID1=p.B1_PER_ID1         AND xm.B1_PER_ID2=p.B1_PER_ID2         AND xm.B1_PER_ID3=p.B1_PER_ID3         AND xm.REC_STATUS='A'         AND ma.G7_ADDRESS1 IS NOT NULL ) mail OUTER APPLY (     SELECT TOP 1 ma.G7_ADDRESS1         ,ma.G7_ADDRESS2         ,ma.G7_CITY         ,ma.G7_STATE         ,ma.G7_ZIP         ,'' G7_ZIP_PLUS_4         ,CASE WHEN ma.AUDIT_MOD_DATE IS NOT NULL THEN ma.AUDIT_MOD_DATE ELSE ma.REC_DATE END [LAST_UPDATE]     FROM XRECORD_CONTACT_ENTITY xm     LEFT JOIN G7CONTACT_ADDRESS ma ON 1=1         AND ma.SERV_PROV_CODE=p.SERV_PROV_CODE         AND ma.G7_ENTITY_TYPE=xm.ENT_TYPE         AND ma.RES_ID=xm.ENT_ID1         AND ma.G7_ADDRESS_TYPE='Home'     WHERE 1=1         AND xm.B1_PER_ID1=p.B1_PER_ID1         AND xm.B1_PER_ID2=p.B1_PER_ID2         AND xm.B1_PER_ID3=p.B1_PER_ID3         AND xm.REC_STATUS='A'         AND ma.G7_ADDRESS1 IS NOT NULL ) home WHERE 1=1     AND p.SERV_PROV_CODE='NVMED'     AND c.B1_CONTACT_TYPE='Employee'     AND p.B1_APPL_STATUS IS NOT NULL     AND (             (                 c.B1_LNAME=?                 AND ((c.B1_MNAME=?) OR (c.B1_MNAME IS NULL and ?=''))                 AND c.B1_FNAME=?             )         AND             (CAST(c.B1_BIRTH_DATE AS DATE)=?)     )";
            //DO NOT ALTER QUERY RETURNING FIELDS, COLUMN NAMES USED FOR MATCH TO OBJECT
            var search2Command = _odbcConnection.CreateCommand();
            search2Command.CommandText = query2Text;
            search2Command.Parameters.Add("@B1_LNAME", OdbcType.VarChar).Value = incomingRowValues.LastName;
            search2Command.Parameters.Add("@B1_MNAME", OdbcType.VarChar).Value = incomingRowValues.MiddleName;
            search2Command.Parameters.Add("@B1_MNAME", OdbcType.VarChar).Value = incomingRowValues.MiddleName;
            search2Command.Parameters.Add("@B1_FNAME", OdbcType.VarChar).Value = incomingRowValues.FirstName;
            search2Command.Parameters.Add("@B1_BIRTH_DATE", OdbcType.DateTime).Value = incomingRowValues.BirthDate;
            var dr2 = search2Command.ExecuteReader();
            if (dr2 != null && dr2.HasRows)
            {
                while (dr2.Read())
                {
                    var outgoingRow = new OutgoingRowModel();
                    for (var i = 0; i < dr2.FieldCount; i++)
                    {
                        var thisColumn = outgoingLocateRequest.SingleRowModel().FirstOrDefault(p => p.ColNam == dr2.GetName(i));
                        if (thisColumn == null) continue;
                        var newColumn = new ColumnsModel(i, thisColumn.ColLen, thisColumn.ColNam, dr2.GetValue(i).ToString() ?? "");
                        outgoingRow.RowCols.Add(newColumn);
                    }
                    matchedResults.Add(outgoingRow);
                }
            }
            search2Command.Dispose();
            if (matchedResults.Count > 0) return matchedResults;

            var query3TextNoMiddleName = "SELECT p.B1_ALT_ID [Cannabis_Agent_Card_Number]     ,CASE WHEN p.B1_APPL_STATUS IS NOT NULL AND p.B1_APPL_STATUS IN ('Pending Background','Additional Info Required-Bkgrd','Board Review','Approved','Final Review','Registered','Waiting for Background','Pend Bkgrd Results-Temp Issued','Fingerprint Return-W/Temp') THEN 'A' ELSE 'I' END [Cannabis_Agent_Card_Status]     ,FORMAT(ex.EXPIRATION_DATE,'MM/dd/yyyy') [Cannabis_Agent_Card_Expiration_Date]     ,c.B1_LNAME [Agent_Last_Name]     ,c.B1_FNAME [Agent_First_Name]     ,SUBSTRING(c.B1_MNAME,0,1) [Agent_Middle_Initial]     ,'' [Modifier_of_the_Agent]     ,REPLACE(c.B1_SOCIAL_SECURITY_NUMBER,'-','') [Agent_Social_Security_Number]     ,FORMAT(c.B1_BIRTH_DATE,'MM/dd/yyyy') [Agent_Date_of_Birth]     ,eye.B1_CHECKLIST_COMMENT [Agent_Eye_Color]     ,hair.B1_CHECKLIST_COMMENT [Agent_Hair_Color]     ,ht.B1_CHECKLIST_COMMENT [Agent_Height]     ,wt.B1_CHECKLIST_COMMENT [Agent_Weight]     ,g.B1_CHECKLIST_COMMENT [Agent_Gender]     ,mail.G7_ADDRESS1 [Agent_Mailing_Address_Line_1]     ,mail.G7_ADDRESS2 [Agent_Mailing_Address_Line_2]     ,mail.G7_CITY [Agent_Mailing_Address_City]     ,mail.G7_STATE [Agent_Mailing_Address_State]     ,mail.G7_ZIP [Agent_Mailing_Address_Zip_Code]     ,mail.[G7_ZIP_PLUS_4] [Agent_Mailing_Address_Zip_+_4]     ,FORMAT(mail.LAST_UPDATE,'MM/dd/yyyy') [Date_of_Last_Agent_Mailing_Address_Update]     ,home.G7_ADDRESS1 [Agent_Residential_Address_Line_1]     ,home.G7_ADDRESS2 [Agent_Residential_Address_Line_2]     ,home.G7_CITY [Agent_Residential_Address_City]     ,home.G7_STATE [Agent_Residential_Address_State]     ,home.G7_ZIP [Agent_Residential_Address_Zip_Code]     ,home.[G7_ZIP_PLUS_4] [Agent_Residential_Address_Zip_+_4]     ,FORMAT(home.LAST_UPDATE,'MM/dd/yyyy') [Date_of_Last_Agent_Residential_Address_Update]     ,c.B1_PHONE3 [Agent_Home_Phone_Number]     ,c.B1_PHONE2 [Agent_Mobile_Phone_Number]     ,c.B1_EMAIL [Email_Address]     ,entName.B1_CHECKLIST_COMMENT [Entity_Name]     ,'' [Entity_Mailing_Address_Line_1]     ,'' [Entity_Mailing_Address_Line_2]     ,'' [Entity_Mailing_Address_City]     ,'' [Entity_Mailing_Address_County]     ,'' [Entity_Mailing_Address_State]     ,'' [Entity_Mailing_Address_Zip_Code]     ,'' [Entity_Mailing_Address_Zip_+_4]     ,'' [Business_Telephone_Number]     ,'' [Business_Telephone_Extension_Number] FROM B1PERMIT p JOIN B3CONTACT c ON 1=1     AND c.SERV_PROV_CODE=p.SERV_PROV_CODE     AND c.B1_PER_ID1=p.B1_PER_ID1     AND c.B1_PER_ID2=p.B1_PER_ID2     AND c.B1_PER_ID3=p.B1_PER_ID3 LEFT JOIN B1_EXPIRATION ex ON 1=1     AND ex.SERV_PROV_CODE=p.SERV_PROV_CODE     AND ex.B1_PER_ID1=p.B1_PER_ID1     AND ex.B1_PER_ID2=p.B1_PER_ID2     AND ex.B1_PER_ID3=p.B1_PER_ID3 LEFT JOIN BCHCKBOX eye ON 1=1     AND eye.SERV_PROV_CODE=p.SERV_PROV_CODE     AND eye.B1_PER_ID1=p.B1_PER_ID1     AND eye.B1_PER_ID2=p.B1_PER_ID2     AND eye.B1_PER_ID3=p.B1_PER_ID3     AND eye.B1_CHECKBOX_DESC='Natural Eye Color' LEFT JOIN BCHCKBOX hair ON 1=1     AND hair.SERV_PROV_CODE=p.SERV_PROV_CODE     AND hair.B1_PER_ID1=p.B1_PER_ID1     AND hair.B1_PER_ID2=p.B1_PER_ID2     AND hair.B1_PER_ID3=p.B1_PER_ID3     AND hair.B1_CHECKBOX_DESC='Natural Hair Color' LEFT JOIN BCHCKBOX ht on 1=1     AND ht.SERV_PROV_CODE=p.SERV_PROV_CODE     AND ht.B1_PER_ID1=p.B1_PER_ID1     AND ht.B1_PER_ID2=p.B1_PER_ID2     AND ht.B1_PER_ID3=p.B1_PER_ID3     AND ht.B1_CHECKBOX_DESC='Height' LEFT JOIN BCHCKBOX wt ON 1=1     AND wt.SERV_PROV_CODE=p.SERV_PROV_CODE     AND wt.B1_PER_ID1=p.B1_PER_ID1     and wt.B1_PER_ID2=p.B1_PER_ID2     and wt.B1_PER_ID3=p.B1_PER_ID3     AND wt.B1_CHECKBOX_DESC='Weight' LEFT JOIN BCHCKBOX g ON 1=1     AND g.SERV_PROV_CODE=p.SERV_PROV_CODE     AND g.B1_PER_ID1=p.B1_PER_ID1     AND g.B1_PER_ID2=p.B1_PER_ID2     AND g.B1_PER_ID3=p.B1_PER_ID3     AND g.B1_CHECKBOX_DESC='Gender' LEFT JOIN BCHCKBOX entName ON 1=1     AND entName.SERV_PROV_CODE=p.SERV_PROV_CODE     AND entName.B1_PER_ID1=p.B1_PER_ID1     AND entName.B1_PER_ID2=p.B1_PER_ID2     AND entName.B1_PER_ID3=p.B1_PER_ID3     AND entName.B1_CHECKBOX_DESC='If you are an Employee, please list the name of the establishment you will be working in, if known'  OUTER APPLY (     SELECT TOP 1 ma.G7_ADDRESS1         ,ma.G7_ADDRESS2         ,ma.G7_CITY         ,ma.G7_STATE         ,ma.G7_ZIP         ,'' G7_ZIP_PLUS_4         ,CASE WHEN ma.AUDIT_MOD_DATE IS NOT NULL THEN ma.AUDIT_MOD_DATE ELSE ma.REC_DATE END [LAST_UPDATE]     FROM XRECORD_CONTACT_ENTITY xm     LEFT JOIN G7CONTACT_ADDRESS ma ON 1=1         AND ma.SERV_PROV_CODE=p.SERV_PROV_CODE         AND ma.G7_ENTITY_TYPE=xm.ENT_TYPE         AND ma.RES_ID=xm.ENT_ID1         AND ma.G7_ADDRESS_TYPE='Mailing'     WHERE 1=1         AND xm.B1_PER_ID1=p.B1_PER_ID1         AND xm.B1_PER_ID2=p.B1_PER_ID2         AND xm.B1_PER_ID3=p.B1_PER_ID3         AND xm.REC_STATUS='A'         AND ma.G7_ADDRESS1 IS NOT NULL ) mail OUTER APPLY (     SELECT TOP 1 ma.G7_ADDRESS1         ,ma.G7_ADDRESS2         ,ma.G7_CITY         ,ma.G7_STATE         ,ma.G7_ZIP         ,'' G7_ZIP_PLUS_4         ,CASE WHEN ma.AUDIT_MOD_DATE IS NOT NULL THEN ma.AUDIT_MOD_DATE ELSE ma.REC_DATE END [LAST_UPDATE]     FROM XRECORD_CONTACT_ENTITY xm     LEFT JOIN G7CONTACT_ADDRESS ma ON 1=1         AND ma.SERV_PROV_CODE=p.SERV_PROV_CODE         AND ma.G7_ENTITY_TYPE=xm.ENT_TYPE         AND ma.RES_ID=xm.ENT_ID1         AND ma.G7_ADDRESS_TYPE='Home'     WHERE 1=1         AND xm.B1_PER_ID1=p.B1_PER_ID1         AND xm.B1_PER_ID2=p.B1_PER_ID2         AND xm.B1_PER_ID3=p.B1_PER_ID3         AND xm.REC_STATUS='A'         AND ma.G7_ADDRESS1 IS NOT NULL ) home WHERE 1=1     AND p.SERV_PROV_CODE='NVMED'     AND c.B1_CONTACT_TYPE='Employee'     AND p.B1_APPL_STATUS IS NOT NULL     AND (             (                 c.B1_LNAME=?                 AND c.B1_FNAME=?             )         AND             (CAST(c.B1_BIRTH_DATE AS DATE)=?)     )";
            //DO NOT ALTER QUERY RETURNING FIELDS, COLUMN NAMES USED FOR MATCH TO OBJECT
            var search3CommandNoMiddleName = _odbcConnection.CreateCommand();
            search3CommandNoMiddleName.CommandText = query3TextNoMiddleName;
            search3CommandNoMiddleName.Parameters.Add("@B1_LNAME", OdbcType.VarChar).Value = incomingRowValues.LastName;
            search3CommandNoMiddleName.Parameters.Add("@B1_FNAME", OdbcType.VarChar).Value = incomingRowValues.FirstName;
            search3CommandNoMiddleName.Parameters.Add("@B1_BIRTH_DATE", OdbcType.DateTime).Value = incomingRowValues.BirthDate;
            var dr3 = search3CommandNoMiddleName.ExecuteReader();
            if (dr3 != null && dr3.HasRows)
            {
                while(dr3.Read())
                {
                    var outgoingRow = new OutgoingRowModel();
                    for(var i = 0; i < dr3.FieldCount; i++)
                    {
                        var thisColumn = outgoingLocateRequest.SingleRowModel().FirstOrDefault(p => p.ColNam == dr3.GetName(i));
                        if (thisColumn == null) continue;
                        var newColumn = new ColumnsModel(i, thisColumn.ColLen, thisColumn.ColNam, dr3.GetValue(i).ToString() ?? "");
                        outgoingRow.RowCols.Add(newColumn);
                    }
                    matchedResults.Add(outgoingRow);
                }
            }

            return matchedResults;
        }

        public void CloseOdbcConnection()
        {
            if (_odbcConnection != null) { _odbcConnection?.Close(); }
        }
    }
}
