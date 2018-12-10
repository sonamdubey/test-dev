IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[BrandWiseLeadPushReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[BrandWiseLeadPushReport]
GO

	--=============================================
-- Author:	   Manish Chourasiya
-- Create date: 17-07-2015
-- Description:	Get the leads pushed to any brand
-- =============================================
CREATE PROCEDURE [reports].[BrandWiseLeadPushReport]
	-- Add the parameters for the stored procedure here
	 @MakeId INT,
     @StartDate DATETIME,
     @EndDate DATETIME
AS
 BEGIN 
         set @StartDate = convert(datetime,convert(varchar(10),@startDate,120)+ ' 00:00:00')	
	      set @EndDate = convert(datetime,convert(varchar(10),@endDate,120)+ ' 23:59:59');

      
          SELECT   CONVERT(VARCHAR,t.EntryDate) EntryDate,
		           t.PQId,
				   t.CustomerName,
				   t.Email,
				   t.Mobile,
				   T.City,
				   V.MakeId,
				   V.Make,
				   V.ModelId,
				   T.ModelName,
				   V.VersionId,
				   V.Version,
				   P.AssignedDealerID,
				   D.Name DealerName,
				   t.PushStatus,
				   'Success' AS FinalStatus
			FROM ThirdPartyLeadQueue AS T  WITH(NOLOCK)
			JOIN PQDealerAdLeads     AS P  WITH(NOLOCK)  ON T.PQId=P.PQId 
														AND T.MakeId=@MakeId 
			                                            AND P.PQID<>0 
														AND LEN(T.PushStatus)=36
														AND (LEN(T.PushStatus)-LEN(REPLACE(T.PushStatus, '-', '')))=4
			LEFT JOIN VWMMV          AS V                ON V.VersionId=P.VersionId
			LEFT JOIN NCS_Dealers    AS D  WITH(NOLOCK)  ON D.id = p.AssignedDealerID
			WHERE T.EntryDate BETWEEN @StartDate AND @EndDate
UNION ALL
       SELECT      CONVERT(VARCHAR,t.EntryDate) EntryDate,
	               t.PQId,
				   t.CustomerName,
				   t.Email,
				   t.Mobile,
				   T.City,
				   V.MakeId,
				   V.Make,
				   V.ModelId,
				   T.ModelName,
				   V.VersionId,
				   V.Version,
				   P.AssignedDealerID,
				   D.Name DealerName,
				   t.PushStatus,
				   CASE WHEN  LEN(T.PushStatus)=36
						  AND (LEN(T.PushStatus)-LEN(REPLACE(T.PushStatus, '-', '')))=4 THEN 'Success'
                        WHEN   T.PushStatus LIKE '%ERROR%' THEN 'Error'
						WHEN   T.PushStatus LIKE '%Exists%' THEN 'Duplicate'
						WHEN   T.PushStatus ='-1' OR T.PushStatus IS NULL  THEN 'Unknown' end as 'FinalStatus'
			FROM ThirdPartyLeadQueue AS T  WITH(NOLOCK)
			JOIN PQDealerAdLeads     AS P  WITH(NOLOCK)   ON T.PQId=P.PQId 
														 AND T.MakeId=@MakeId 
			                                             AND P.PQID<>0 
														 AND LEN(T.PushStatus)<>36
														 AND (LEN(T.PushStatus)-LEN(REPLACE(T.PushStatus, '-', '')))<>4
			LEFT JOIN VWMMV          AS V                ON V.VersionId=P.VersionId
			LEFT JOIN NCS_Dealers    AS D  WITH(NOLOCK)  ON D.id = p.AssignedDealerID
			WHERE  T.EntryDate BETWEEN @StartDate AND @EndDate
			    AND  T.PQID NOT IN (SELECT PQID 
			                        FROM  ThirdPartyLeadQueue 
								    WHERE  LEN(PushStatus)=36
									AND (LEN(PushStatus)-LEN(REPLACE(PushStatus, '-', '')))=4 
									AND  MAKEID=@MakeId
									AND EntryDate BETWEEN @StartDate AND @EndDate
							      )

END 