IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_OtherInqSendtoExcel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_OtherInqSendtoExcel]
GO

	
-- =====================================================================================================
--- Author:  Vivek Gupta
--- Create date:   18-02-2013  
--- Description: This Proc Returns Other Inquiries(Service,Loan,Grievance)List That Comes to Dealers Website
--- [TC_OtherInqSendtoExcel] 1028,NULL,NULL,NULL,'2012-02-12 13:01:38.470','2013-02-19 19:01:38.470'
-- ===================================================================================================== 
CREATE PROCEDURE [dbo].[TC_OtherInqSendtoExcel]
(
  @BranchId        BIGINT,  
  @CustomerName    VARCHAR(100), 
  @CustomerMobile  VARCHAR(50), 
  @CustomerEmail   VARCHAR(100),
  @FromRequestdate AS DATETIME,
  @ToRequestdate AS DATETIME
)
AS
BEGIN
		 SELECT 
		 TC.CustomerName, 
		 TC.Email, 
		 TC.Mobile, 
		 O.Comments,
		 CASE O.TC_InquiryTypeId
		 WHEN 8
		 THEN 'Loan Request'
		 WHEN 10
		 THEN 'Grievance Request'
		 END AS InqType,
		 O.CreatedDate AS RequestedDate
		 
		 FROM			TC_CustomerDetails AS TC WITH(NOLOCK)
			   JOIN	    TC_OtherRequests AS O WITH(NOLOCK)
														     ON TC.Id = O.TC_CustomerId 
         WHERE			TC.BranchId=@BranchId
                AND      ( ( @FromRequestdate IS NULL )      OR ( O.CreatedDate >= @FromRequestdate ) )
                AND      (( @ToRequestdate IS NULL ) OR (O.CreatedDate <= @ToRequestdate) )
                AND      ( ( @CustomerName IS NULL )         OR ( TC.CustomerName LIKE '%' + @CustomerName + '%' ) ) 
                AND      ( ( @CustomerMobile IS NULL )       OR ( TC.Mobile = @CustomerMobile ) ) 
                AND      ( ( @CustomerEmail IS NULL )        OR ( TC.Email = @CustomerEmail ) )
         
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
		 END AS InqType,
		 S.CreatedDate AS RequestedDate
		 FROM			 TC_CustomerDetails TC WITH(NOLOCK)
				JOIN     TC_ServiceRequests S WITH(NOLOCK)
															 ON TC.Id = S.TC_CustomerId 
         WHERE           TC.BranchId=@BranchId
                AND      ( ( @FromRequestdate IS NULL )      OR ( S.CreatedDate >= @FromRequestdate ) )
                AND      (( @ToRequestdate IS NULL ) OR (S.CreatedDate <= @ToRequestdate) )
                AND      ( ( @CustomerName IS NULL )         OR ( TC.CustomerName LIKE '%' + @CustomerName + '%' ) ) 
                AND      ( ( @CustomerMobile IS NULL )       OR ( TC.Mobile = @CustomerMobile ) ) 
                AND      ( ( @CustomerEmail IS NULL )        OR ( TC.Email = @CustomerEmail ) ) 

END





/****** Object:  StoredProcedure [dbo].[TC_OtherInqListLoad]    Script Date: 02/20/2013 16:10:09 ******/
SET ANSI_NULLS ON
