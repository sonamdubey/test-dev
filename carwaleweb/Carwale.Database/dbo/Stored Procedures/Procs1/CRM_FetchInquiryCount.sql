IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_FetchInquiryCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_FetchInquiryCount]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 4th Sept 2014
-- Description:	To fetch the count of exposure ,callback requests,phone calls of new car inquiries
-- =============================================
CREATE PROCEDURE [dbo].[CRM_FetchInquiryCount] 
	@ModelId		INT,
	@CityId			INT,
	@Exposure		INT OUTPUT,
	@CallbackReq	INT OUTPUT
AS
BEGIN
	DECLARE @NoOfDays DECIMAL = DATEDIFF(DD,'2014-08-28',GETDATE()) --number of days between 28th aug and current date for calculating average
	DECLARE @TempExposure DECIMAL, @TempCallbackReq DECIMAL
	DECLARE @StartDate DATETIME = '2014-08-28' 
	DECLARE @EndDate DATETIME = DATEADD(MINUTE, -1, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)) --yesterday's date till 11.59pm

	SELECT @TempExposure = CAST (((COUNT(NCPI.Id)/@noOfDays)*30) AS decimal(38,2)),
	@TempCallbackReq = CAST (((COUNT(DISTINCT CASE WHEN NCPI.CustomerId <> -1 THEN NCPI.CustomerId ELSE NULL END)/@noOfDays)*30) AS decimal(38,2))
	FROM NewCarPurchaseInquiries NCPI WITH (NOLOCK)
	INNER JOIN vwMMV VM WITH (NOLOCK) ON VM.VersionId = NCPI.CarVersionId
	LEFT JOIN NewPurchaseCities NPC WITH (NOLOCK) ON NPC.InquiryId = NCPI.Id
	LEFT JOIN Cities C WITH (NOLOCK) ON C.ID = NPC.CityId
	WHERE VM.ModelId = @ModelId
	AND C.ID = CASE WHEN @CityId IS NULL THEN C.ID ELSE @CityId END
	AND NCPI.RequestDateTime BETWEEN @StartDate AND @EndDate
	--AND DATEDIFF(DD,NCPI.RequestDateTime,GETDATE()) <= 30

	SET @Exposure = ROUND(@TempExposure,0)
	SET @CallbackReq = ROUND(@TempCallbackReq,0)
END
