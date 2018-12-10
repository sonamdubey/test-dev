IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[UsedCarRecommendationDailyInsertJob]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[UsedCarRecommendationDailyInsertJob]
GO

	-- =============================================
-- Author:		Manish
-- Create date: 18-07-2013
-- Description: SP for inserting used car recommendation for sending mail to the customer in the table " UCAlert.RecommendUsedCarAlert"
-- Modified by: Manish on 25-04-2014 for removing old thead of the emailer.
-- =============================================
CREATE PROCEDURE  [UCAlert].[UsedCarRecommendationDailyInsertJob]
AS
BEGIN

		EXEC UCAlert.RecommendCarAlertForLastDay
		EXEC UCAlert.RecommendCarAlertForSecondMail
		EXEC [UCAlert].[RemoveOldThreadForUsedCarMailer]
END



