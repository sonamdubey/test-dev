IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_FetchCEOData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_FetchCEOData]
GO

	CREATE PROCEDURE [dbo].[AP_FetchCEOData]
	@DateFrom				DateTime,
	@DateTo					DateTime,
	@Valuations				Numeric OutPut,
	@LastValuations			Numeric OutPut,
	@Recommendations		Numeric OutPut,
	@CurrentUCProfileView	Numeric OutPut,
	@LastUCProfileView		Numeric OutPut,
	@CurrentReviewView		Numeric OutPut,
	@LastReviewView			Numeric OutPut,
	@CurrentAnswersView		Numeric	OutPut,
	@LastAnswersView		Numeric OutPut,
	@TotalDealers			Numeric Output,
	@CurrentActiveDealers	Numeric OutPut,
	@AvgLiveStockI			Numeric OutPut,
	@AvgLiveStockD			Numeric OutPut,
	@TotalRegUsers			Numeric OutPut
 AS
	
BEGIN

	SET NOCOUNT ON
	SELECT	@LastUCProfileView = IsNull(SUM(UCProfileView),0),
			@LastReviewView = IsNull(SUM(ReviewView),0),
			@LastAnswersView = IsNull(SUM(AnswersView),0),
			@LastValuations	= IsNull(SUM(Valuations),0)
	FROM CeoReport
	WHERE ReportMonth < @DateFrom
	
	SELECT @Valuations = rows FROM sysindexes WHERE id = OBJECT_ID('CarValuations') AND indid < 2

	SELECT @Recommendations = COUNT(Id) FROM RecommendCarSearchData 
			WHERE EntryDateTime >= @DateFrom AND EntryDateTime < @DateTo

	SELECT @CurrentReviewView = SUM(Viewed) FROM CustomerReviews AS CurrentReviewView
		
	SELECT @CurrentAnswersView = SUM(Viewed) FROM AskUsQuestions AS CurrentAnswersView
	
	SELECT @TotalDealers = IsNull(COUNT(ID),0) FROM Dealers 
			WHERE Status = 0 AND JoiningDate < @DateTo
	
	SELECT @CurrentActiveDealers = IsNull(Count(ID),0) From Dealers WHERE ID IN
			(SELECT DealerId FROM SellInquiries 
				WHERE DateName(Month, LastUpdated) = DateName(Month, @DateFrom) 
					 AND DateName(Year, LastUpdated) = DateName(year,@DateTo))

	SELECT @TotalRegUsers = IsNull(COUNT(ID),0) FROM Customers 
			WHERE RegistrationDateTime < @DateTo

	SELECT

		@CurrentUCProfileView = ((SELECT SUM(ViewCount) FROM CustomerSellInquiries) +
			(SELECT SUM(ViewCount) FROM SellInquiries)), 
		
		@AvgLiveStockI = IsNull((SELECT COUNT(ID) FROM CustomerSellInquiries WHERE
				PackageType = 2 AND ((
				(EntryDate BETWEEN @DateFrom AND @DateTo) AND
				(ClassifiedExpiryDate BETWEEN @DateFrom AND @DateTo)
				) OR ((EntryDate BETWEEN @DateFrom AND @DateTo) AND
				(ClassifiedExpiryDate NOT BETWEEN @DateFrom AND @DateTo)
				) OR ((EntryDate NOT BETWEEN @DateFrom AND @DateTo)
				AND (ClassifiedExpiryDate BETWEEN @DateFrom AND @DateTo)))), 0),

		@AvgLiveStockD = IsNull((SELECT COUNT(ID) FROM SellInquiries WHERE
			DealerId Not IN (SELECT ID FROM Dealers WHERE Status = 1) AND
				((EntryDate <= @DateTo AND StatusId = 1 )
				OR (EntryDate <= @DateTo AND StatusId <> 1 
				AND LastUpdated >= @DateFrom))), 0)

END