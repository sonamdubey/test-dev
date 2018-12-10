IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_GetPrepushdataSUCCESS]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_GetPrepushdataSUCCESS]
GO

	CREATE PROCEDURE CRM_GetPrepushdataSUCCESS (@OID      smallint, 
                                    @fromdate datetime, 
                                    @todate   datetime) 
AS 
  BEGIN 
      SET @OID=9 
      SET @fromdate='2012-01-01 00:50:48.760' 
      SET @todate = '2012-07-12 10:50:48.760' 

      CREATE TABLE #templeads 
        ( 
           LeadId       bigint, 
           LeadStatusId smallint 
        ) 

      INSERT INTO #templeads 
                  (LeadId, 
                   LeadStatusId) 
      SELECT CL.ID, 
             CL.LeadStatusId 
      FROM   CRM_Customers AS CC WITH (nolock) 
             JOIN OLM_RegionCities AS OC WITH (nolock) 
               ON CC.CityId = OC.CityId 
             JOIN CRM_Leads AS CL WITH (nolock) 
               ON CL.CNS_CustId = CC.Id 
             JOIN CRM_CarBasicData AS CBD WITH (nolock) 
               ON CL.ID = CBD.LeadId 
             JOIN vwMMV AS VM WITH (nolock) 
               ON CBD.VersionId = VM.VersionId 
                  AND VM.MakeId = 15 
             --JOIN CRM_PrePushData AS CPD WITH (nolock)  
             --  ON CPD.LeadId = CL.Id  
             --     AND CPD.Result = 'SUCCESS'  
             JOIN NCS_Dealers AS ND WITH (nolock) 
               ON ND.CityId = OC.CityId 
             JOIN NCS_SubDealerOrganization AS NSD WITH (nolock) 
               ON ND.ID = NSD.DId 
                  AND NSD.OID = @OID 
      WHERE  CL.CreatedOnDatePart BETWEEN @fromdate AND @todate 

      SELECT Count(DISTINCT CL.LeadId) AS TotalLeads, 
             CL.LeadStatusId 
      FROM   #templeads AS CL 
             JOIN CRM_PrePushData AS CPD WITH (nolock) 
               ON CPD.LeadId = CL.LeadId 
                  AND CPD.Result = 'SUCCESS' 
      GROUP  BY CL.LeadStatusId 

      DROP TABLE #templeads 
  END 