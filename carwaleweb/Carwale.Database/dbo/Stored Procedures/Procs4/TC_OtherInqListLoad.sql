IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_OtherInqListLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_OtherInqListLoad]
GO

	-- =====================================================================================================
--- Author:  Vivek Gupta
--- Create date:   12-02-2013  
--- Description: This Proc Returns Other Inquiries(Service,Loan,Grievance) That Comes to Dealers Website
--- [TC_OtherInqListLoad] 5,NULL,NULL,NULL,1,100,'2014-10-01 13:01:38.470','2014-10-10 13:01:38.470'
-- Modifed By : Tejashree Patil on 30 Jun 2014, Fetched PreferedDate and added JOIN with TC_InquirySource.
-- Modifed By : Tejashree Patil on 7 Oct 2014, Added condition TC_InquiryTypeId <> 5 in first SELECT query
-- ===================================================================================================== 
CREATE PROCEDURE [dbo].[TC_OtherInqListLoad]
(
  @BranchId        BIGINT,  
  @CustomerName    VARCHAR(100), 
  @CustomerMobile  VARCHAR(50), 
  @CustomerEmail   VARCHAR(100),
  @FromIndex INT , 
  @ToIndex INT,
  @FromRequestdate AS DATETIME,
  @ToRequestdate AS DATETIME
)
AS
BEGIN

WITH Cte1 
AS (
		 SELECT
		 TC.CustomerName, 
		 TC.Email, 
		 TC.Mobile, 
		 O.Comments,
		 IT.InquiryType AS InqType,
		 O.CreatedDate AS RequestedDate,
		 O.PreferredDateTime AS PreferedDate-- Modifed By : Tejashree Patil on 30 Jun 2014
	 
		 FROM			TC_CustomerDetails AS TC WITH(NOLOCK)
			   JOIN	    TC_OtherRequests AS O WITH(NOLOCK)
														     ON TC.Id = O.TC_CustomerId 
			   JOIN		TC_InquiryType AS IT WITH(NOLOCK)
															 ON IT.TC_InquiryTypeId = O.TC_InquiryTypeId-- Modifed By : Tejashree Patil on 30 Jun 2014
         WHERE			TC.BranchId=@BranchId
                AND      ( ( @FromRequestdate IS NULL )      OR ( O.CreatedDate >= @FromRequestdate ) )
                AND      (( @ToRequestdate IS NULL ) OR (O.CreatedDate <= @ToRequestdate) )
                AND      ( ( @CustomerName IS NULL )         OR ( TC.CustomerName LIKE '%' + @CustomerName + '%' ) ) 
                AND      ( ( @CustomerMobile IS NULL )       OR ( TC.Mobile = @CustomerMobile ) ) 
                AND      ( ( @CustomerEmail IS NULL )        OR ( TC.Email = @CustomerEmail ) )
				AND		 O.TC_InquiryTypeId <> 5-- Modifed By : Tejashree Patil on 7 Oct 2014
		 UNION  ALL
         
         SELECT
		 TC.CustomerName, 
		 TC.Email, 
		 TC.Mobile, 
		 S.Comments,
		 CASE S.TypeOfService 
		 WHEN 1
		 THEN 'Regular Car Maintenance'
		 WHEN 2
		 THEN 'Specific Repair/Maintenance'
		 ELSE
		 IT.InquiryType
		 END AS InqType,
		 S.CreatedDate AS RequestedDate,
		 S.PreferredDate AS PreferedDate-- Modifed By : Tejashree Patil on 30 Jun 2014

		 FROM			 TC_CustomerDetails TC WITH(NOLOCK)
				JOIN     TC_ServiceRequests S WITH(NOLOCK)
															 ON TC.Id = S.TC_CustomerId 
			    JOIN		TC_InquiryType AS IT WITH(NOLOCK)
															 ON IT.TC_InquiryTypeId = 5-- Modifed By : Tejashree Patil on 30 Jun 2014
         WHERE           TC.BranchId=@BranchId
                AND      ( ( @FromRequestdate IS NULL )      OR ( S.CreatedDate >= @FromRequestdate ) )
                AND      (( @ToRequestdate IS NULL ) OR (S.CreatedDate <= @ToRequestdate) )
                AND      ( ( @CustomerName IS NULL )         OR ( TC.CustomerName LIKE '%' + @CustomerName + '%' ) ) 
                AND      ( ( @CustomerMobile IS NULL )       OR ( TC.Mobile = @CustomerMobile ) ) 
                AND      ( ( @CustomerEmail IS NULL )        OR ( TC.Email = @CustomerEmail ) ) 
                
                ),
     Cte2
     AS (SELECT *, 
                      ROW_NUMBER() 
                      OVER ( 
                          ORDER BY RequestedDate DESC ) RowNumber 
               FROM   Cte1)
         
              SELECT * 
              INTO   #TblTemp 
              FROM   Cte2 

		      SELECT * 
		      FROM   #TblTemp 
		      WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
		      
		      SELECT COUNT(*) AS RecordCount 
		      FROM   #TblTemp 

		      DROP TABLE #TblTemp  
END




------------------------------------------------------------------------------------------

