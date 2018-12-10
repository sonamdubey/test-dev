IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FeedbackCalling_SavePushedData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FeedbackCalling_SavePushedData]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 30th Sept 2016
-- Description:	To save i/p for the api which fetches data that is to be pushed
-- =============================================
CREATE PROCEDURE [dbo].[TC_FeedbackCalling_SavePushedData]
@DealerId INT,
@ToDate DATETIME,
@FromDate DATETIME,
@FeedbackDealerId INT,
@CreatedBy INT
AS
BEGIN
	
	SELECT Id FROM TC_FeedbackCalling_PushedData WITH (NOLOCK)
	WHERE DealerId = @DealerId AND CAST(ToDate AS DATE) = CAST(@ToDate AS DATE) AND CAST(FromDate AS DATE) = CAST(@FromDate AS DATE) AND FeedbackDealerId = @FeedbackDealerId

	IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO TC_FeedbackCalling_PushedData(DealerId,Todate,FromDate,FeedbackDealerId,CreatedBy,CreatedOn)
		VALUES (@DealerId,@ToDate,@FromDate,@FeedbackDealerId,@CreatedBy,GETDATE())
	END
END

