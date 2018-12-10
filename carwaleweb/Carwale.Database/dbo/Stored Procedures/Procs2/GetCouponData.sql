IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCouponData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCouponData]
GO

	
-- =============================================
-- Author:		Avishkar
-- Create date: 17-12-2014
-- Description:	DB_CheckList
-- =============================================
CREATE PROCEDURE [dbo].[GetCouponData]
	-- Add the parameters for the stored procedure here
	
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;



    CREATE TABLE #temp
    (
		Parameters Varchar(100) ,
		Value int,
		CurrDate datetime,
		SortOrder smallint,
    )
    
    INSERT INTO #temp(Parameters,Value,SortOrder,CurrDate)
	-- No. of Coupons Generated Today	:
	SELECT 'Coupons Generated Today',count(*) ,1 as SortOrder,getdate()
	FROM OfferCouponCodes WITH (NOLOCK) 
	WHERE CONVERT(DATE,GeneratedOn) = CONVERT(DATE,GETDATE())

	INSERT INTO #temp(Parameters,Value,SortOrder,CurrDate)
	-- No. of Coupons Generated total	:
	SELECT 'Total Coupons Generated',count(*) ,2 as SortOrder,getdate()
	FROM OfferCouponCodes WITH (NOLOCK) 

	
	 INSERT INTO #temp(Parameters,Value,SortOrder,CurrDate)
	-- No. of offers claimed today		:
	SELECT 'Offers Claimed Today',count(*) ,3 as SortOrder,getdate()
	FROM TC_NewCarBooking  WITH (NOLOCK) 
	WHERE IsOfferClaimed =1 
	AND CONVERT(date, RequestedDate) = CONVERT(DATE,GETDATE())

	 INSERT INTO #temp(Parameters,Value,SortOrder,CurrDate)
	-- No. of offers claimed today		:
	SELECT 'Total Offers Claimed ',count(*) ,4 as SortOrder,getdate()
	FROM TC_NewCarBooking  WITH (NOLOCK) 
	WHERE IsOfferClaimed =1 
	
	INSERT INTO #temp(Parameters,Value,SortOrder,CurrDate)
	SELECT 'Coupons Generated Today - Desktop ',count(*) ,5 as SortOrder,getdate()
	FROM OfferCouponCodes as OC  WITH (NOLOCK) 
	JOIN NewCarPurchaseInquiries as NP  WITH (NOLOCK)  on NP.Id=OC.ReferenceId
	WHERE CONVERT(DATE,GeneratedOn) = CONVERT(DATE,GETDATE())
	AND SourceId=1	
	GROUP BY SourceId

	INSERT INTO #temp(Parameters,Value,SortOrder,CurrDate)
	SELECT 'Coupons Generated Today - Mobile',count(*) ,6 as SortOrder,getdate()
	FROM OfferCouponCodes as OC  WITH (NOLOCK) 
	JOIN NewCarPurchaseInquiries as NP  WITH (NOLOCK)   on NP.Id=OC.ReferenceId
	WHERE CONVERT(DATE,GeneratedOn) = CONVERT(DATE,GETDATE())
	AND SourceId=43	
	GROUP BY SourceId

	INSERT INTO #temp(Parameters,Value,SortOrder,CurrDate)
	SELECT 'Coupons Generated Today - Android',count(*) ,7 as SortOrder,getdate()
	FROM OfferCouponCodes as OC  WITH (NOLOCK) 
	JOIN NewCarPurchaseInquiries as NP  WITH (NOLOCK)   on NP.Id=OC.ReferenceId
	WHERE CONVERT(DATE,GeneratedOn) = CONVERT(DATE,GETDATE())
	AND SourceId=74	
	GROUP BY SourceId

	INSERT INTO #temp(Parameters,Value,SortOrder,CurrDate)
	SELECT 'Coupons Generated Today - iOS',count(*) ,8 as SortOrder,getdate()
	FROM OfferCouponCodes as OC  WITH (NOLOCK)  
	JOIN NewCarPurchaseInquiries as NP   WITH (NOLOCK)  on NP.Id=OC.ReferenceId
	WHERE CONVERT(DATE,GeneratedOn) = CONVERT(DATE,GETDATE())
	AND SourceId=83	
	GROUP BY SourceId	

	insert into DealerOfferData 
    (
		Parameters ,
		Value ,
		CurrDate 
	)
	SELECT Parameters,Value,CurrDate
	FROM #temp
	
	SELECT Parameters,Value,CurrDate
	FROM #temp
		
	DROP TABLE #temp
 
END


