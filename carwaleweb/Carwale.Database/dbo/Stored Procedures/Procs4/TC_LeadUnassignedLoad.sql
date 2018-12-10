IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LeadUnassignedLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LeadUnassignedLoad]
GO

	
-- =============================================     
--- Author:  Surendra    
-- Create date:   10-01-2013  
-- Description:    
--execute [TC_LeadUnassignedLoad] 1297,1,10,'Totti',null,null,null,null,null'2013-01-16 00:00:00.000','2013-03-16 00:00:00.000' 
-- Modifed By: Nilesh utture on 14th Oct, 2013 Added cond. (@FromDate IS NULL AND @ToDate IS NULL) for NewCarInquiries
-- Modified By : Vivek Gupta on 13-08-2014, Added parameter @ModelId to get Un Asssigned leads model wise.
-- Modified By : Vivek Gupta on 11-09-2014, changed from inner join to left join in stock for buyer inquiries
-- =============================================     
CREATE PROCEDURE [dbo].[TC_LeadUnassignedLoad] 
  -- Add the parameters for the stored procedure here     
  @BranchId        BIGINT, 
  @FromIndex       INT, 
  @ToIndex         INT, 

  --@Type TINYINT , 
  @CustomerName    VARCHAR(100),   
  @CustomerMobile  VARCHAR(50), 
  @CustomerEmail   VARCHAR(100),
  @TC_LeadInquiryTypeId TINYINT=NULL,
  @FromDate DATETIME=NULL,
  @ToDate   DATETIME=NULL,
  @ModelId INT = NULL
  
AS 
  BEGIN 
      --  Lead Scheduling for verifications
    
	IF (@TC_LeadInquiryTypeId=1)
	BEGIN
      WITH CTE AS 
             (  SELECT DISTINCT CarDetails,
                                TC_LeadId,
                                TCIL.LatestInquiryDate
                 FROM TC_InquiriesLead  AS TCIL WITH(NOLOCK)
                  JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK) ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
				  LEFT JOIN TC_Stock AS S WITH(NOLOCK) ON S.ID = TCBI.StockId
                 WHERE  TCIL.BranchId	= @BranchId             
					AND  TC_LeadInquiryTypeId=@TC_LeadInquiryTypeID 
					AND  ((@FromDate IS NULL AND @ToDate IS NULL) OR (TCBI.CreatedOn BETWEEN @FromDate AND @ToDate))
					AND  (@ModelId IS NULL OR S.VersionId IN(SELECT V.ID FROM CarVersions V WITH(NOLOCK) WHERE V.CarModelId = @ModelId)) 
			 ),     
      CT 
           AS (
           SELECT C.CustomerName ,C.Email, C.Mobile, L.LeadCreationDate, L.TC_LeadId,
           'Used Buy' AS LeadType,
            CarDetails,
			ROW_NUMBER() OVER ( ORDER BY CTE.LatestInquiryDate DESC ) rownumber 
			FROM 
		  TC_Lead L WITH(NOLOCK)  
		  INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON L.TC_CustomerId = C.Id
		  INNER JOIN CTE              ON CTE.TC_LeadId=L.TC_LeadId 
               WHERE 
				L.TC_LeadStageId IS NULL 
				AND L.BranchId = @BranchId
                AND ( ( @CustomerName IS NULL ) 
                       OR ( C.CustomerName LIKE '%' + @CustomerName + '%' ) ) 
                AND ( ( @CustomerMobile IS NULL ) 
                       OR ( C.Mobile = @CustomerMobile ) ) 
                AND ( ( @CustomerEmail IS NULL ) 
                       OR ( C.Email = @CustomerEmail ) )
                       
            )
      SELECT * 
      INTO   #tblTempBuyer
      FROM   CT  ORDER BY LeadCreationDate DESC;

      SELECT * 
      FROM   #tblTempBuyer
      WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
     

      SELECT COUNT(*) AS RecordCount 
      FROM   #tblTempBuyer

      DROP TABLE #tblTempBuyer
      END
	IF (@TC_LeadInquiryTypeId=2)
     BEGIN 
      --  Lead Scheduling for verifications
    
	  WITH CTE AS 
             (  SELECT DISTINCT CarDetails,
                                TC_LeadId,
                                TCIL.LatestInquiryDate
                 FROM TC_InquiriesLead  AS TCIL WITH(NOLOCK)
                    JOIN TC_SellerInquiries AS TCBI WITH(NOLOCK) ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
                   WHERE BranchId	= @BranchId             
					AND  TC_LeadInquiryTypeId=@TC_LeadInquiryTypeID 
					AND  ((@FromDate IS NULL AND @ToDate IS NULL) OR (TCBI.CreatedOn BETWEEN @FromDate AND @ToDate))
			 ),     
      CT 
           AS (
           SELECT C.CustomerName ,C.Email, C.Mobile, L.LeadCreationDate, L.TC_LeadId,
           'Used Sell' AS LeadType,
            CarDetails,
			ROW_NUMBER() OVER ( ORDER BY CTE.LatestInquiryDate DESC ) rownumber 
			FROM 
		  TC_Lead L WITH(NOLOCK)  
		  INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON L.TC_CustomerId = C.Id
		  INNER JOIN CTE              ON CTE.TC_LeadId=L.TC_LeadId 
               WHERE 
				L.TC_LeadStageId IS NULL 
				AND L.BranchId = @BranchId
                AND ( ( @CustomerName IS NULL ) 
                       OR ( C.CustomerName LIKE '%' + @CustomerName + '%' ) ) 
                AND ( ( @CustomerMobile IS NULL ) 
                       OR ( C.Mobile = @CustomerMobile ) ) 
                AND ( ( @CustomerEmail IS NULL ) 
                       OR ( C.Email = @CustomerEmail ) )
                       
            )
      SELECT * 
      INTO   #tblTempSeller 
      FROM   CT  ORDER BY LeadCreationDate DESC;

      SELECT * 
      FROM   #tblTempSeller
      WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
     

      SELECT COUNT(*) AS RecordCount 
      FROM   #tblTempSeller 

      DROP TABLE #tblTempSeller 
      END

    IF (@TC_LeadInquiryTypeId=3)
BEGIN 
      --  Lead Scheduling for verifications
    
     WITH CTE AS 
             (  SELECT DISTINCT CarDetails,
                                TC_LeadId,
                                TCIL.LatestInquiryDate
                 FROM TC_InquiriesLead  AS TCIL WITH(NOLOCK)
                  JOIN TC_NewCarInquiries AS TCBI WITH(NOLOCK) ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
                 WHERE BranchId	= @BranchId             
					AND  TC_LeadInquiryTypeId=@TC_LeadInquiryTypeID 
					AND  ((@FromDate IS NULL AND @ToDate IS NULL) OR (TCBI.CreatedOn BETWEEN @FromDate AND @ToDate))-- Modifed By: Nilesh utture on 14th Oct, 2013
			 ),     
      CT 
           AS (
           SELECT C.CustomerName ,C.Email, C.Mobile, L.LeadCreationDate, L.TC_LeadId,
           'New Buy' AS LeadType,
            CarDetails,
			ROW_NUMBER() OVER ( ORDER BY CTE.LatestInquiryDate DESC ) rownumber 
			FROM 
		  TC_Lead L WITH(NOLOCK)  
		  INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON L.TC_CustomerId = C.Id
		  INNER JOIN CTE              ON CTE.TC_LeadId=L.TC_LeadId 
               WHERE 
				L.TC_LeadStageId IS NULL 
				AND L.BranchId = @BranchId
                AND ( ( @CustomerName IS NULL ) 
                       OR ( C.CustomerName LIKE '%' + @CustomerName + '%' ) ) 
                AND ( ( @CustomerMobile IS NULL ) 
                       OR ( C.Mobile = @CustomerMobile ) ) 
                AND ( ( @CustomerEmail IS NULL ) 
                       OR ( C.Email = @CustomerEmail ) )
                       
            )
      SELECT * 
      INTO   #tblTempNewCar 
      FROM   CT  ORDER BY LeadCreationDate DESC;

      SELECT * 
      FROM   #tblTempNewCar
      WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
     

      SELECT COUNT(*) AS RecordCount 
      FROM   #tblTempNewCar 

      DROP TABLE #tblTempNewCar
      END
  
  END 
