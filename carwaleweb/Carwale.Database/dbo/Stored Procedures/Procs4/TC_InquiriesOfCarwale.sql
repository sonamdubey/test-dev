IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiriesOfCarwale]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiriesOfCarwale]
GO

	-- Author:		Surendra Chouksey
-- Create date: 28 August,2012
-- Description:	This SP is used in inqapi to retrive carwale inquiries
--execute TC_InquiriesOfCarwale 'apiuser6','apiuser6','2012-02-28 17:46:35.607'
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquiriesOfCarwale]
(
@UserId VARCHAR(50),
@Password VARCHAR(50),
@RequestTime DATETIME
)	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @BranchId BIGINT=NULL
	DECLARE @LastRequestedTime DATETIME=NULL
	DECLARE @Getdate DATETIME=GETDATE()
	
	-- Checking user's existence based on i/p
	SELECT @BranchId=DealerId,@LastRequestedTime=LastRequestedTime FROM TC_APIUsers WITH(NOLOCK)
	WHERE UserId=@UserId AND Password=@Password AND IsActive=1
	
	IF(@BranchId IS NULL) -- Unauthorised user
	BEGIN
		--PRINT 'not exists'
		RETURN 1
	END
	
	
	IF(@LastRequestedTime IS NOT NULL) -- request came withing 30 mins
	BEGIN		
		IF(cast(DATEDIFF(MINUTE,@LastRequestedTime, @Getdate) as smallint) <30)
		BEGIN			
			--PRINT 'req withing 30 mins'
			RETURN 2 -- request come within 30 mins
		END	
	END	
	
	-- fetching all inquiries for logged in dealers of Source 1(carwale)
	BEGIN
		SELECT S.regno,
			   C.email,
			   C.mobile,
			   C.customername,
			   I.createddate 'InquiryDate'
		FROM   tc_inquiries I WITH(NOLOCK)
			   INNER JOIN tc_buyerinquiries B WITH(NOLOCK)
					   ON I.tc_inquiriesid = B.tc_inquiriesid
						  AND I.sourceid = 1
			   INNER JOIN tc_stock S WITH(NOLOCK)
					   ON B.stockid = S.id
			   INNER JOIN tc_customerdetails C WITH(NOLOCK)
					   ON I.tc_customerid = C.id
		WHERE  I.branchid = @BranchId
			   AND I.createddate > @RequestTime
		ORDER BY I.createddate

		--updating lastrequestedtime 
		UPDATE tc_apiusers
		SET    lastrequestedtime = @Getdate
		WHERE  userid = @UserId
			   AND password = @Password
			   AND isactive = 1  
		
		RETURN 3
	END	
		
END


