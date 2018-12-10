IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetFeedbackPercentage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetFeedbackPercentage]
GO

	-- =============================================
-- Author:		<Author,,> Chetan Kane
-- Modified:	Vaibhav K
--Summary:		Changed the query to get data 
-- Create date: <Create Date,,>
-- Description:	<Description,,>EXEC DCRM_GetFeedbackPercentage 5
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetFeedbackPercentage] 
	-- Add the parameters for the stored procedure here
	@DealerId INT,
	@From DATETIME,
	@To DATETIME
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @DealerIs INT
	SET @DealerIs = (SELECT COUNT(Id) FROM DCRM_BuyerFeedBackNew WHERE DealerId = @DealerId)
	IF (@DealerIs <> 0)
	BEGIN
		-- Insert statements for procedure here
		DECLARE @CityCount FLOAT
		DECLARE @CityId INT
		
		--get dealers city & feedback count for that city
		SELECT @CityCount=COUNT(DBF.Id), @CityId = D.CityId FROM DCRM_BuyerFeedBackNew DBF INNER JOIN Dealers D ON D.ID=DBF.DealerId  
		WHERE CityId=(SELECT CityId FROM Dealers WHERE ID=@DealerId) GROUP BY D.CityId;
		
		--Get all feedback details for that dealer passed
		SELECT DBFN.Id AS FId,DBFN.ReceivedDealerCall,DBFN.ReceivedDealerCallOn,DBFN.DealerResponseSatisfaction,DBFN.FoundCarInterestedIn, 
		C.Name,C.email,C.Mobile,CT.Name AS City FROM DCRM_BuyerFeedBackNew DBFN 
		INNER JOIN Customers C ON DBFN.CustomerId=C.Id 
		LEFT JOIN Cities CT ON C.CityId = CT.ID WHERE DealerId = @DealerId 
		AND DBFN.FeedBackDate BETWEEN @From AND @To
		
		---Get the percentage for all dealers in that city
		SELECT 
		ROUND ((SELECT COUNT(DBF.Id) 
				FROM DCRM_BuyerFeedBackNew DBF 
				INNER JOIN Dealers D ON D.ID=DBF.DealerId  
				WHERE CityId=@CityId and ReceivedDealerCallOn=1 AND FeedBackDate BETWEEN @From AND @To)/@CityCount * 100 , 0 )  AS CityReceivedOnFirstDay,
		ROUND ((SELECT COUNT(DBF.Id) 
				FROM DCRM_BuyerFeedBackNew DBF 
				INNER JOIN Dealers D ON D.ID=DBF.DealerId  
				WHERE CityId=@CityId and ReceivedDealerCall = 0 AND FeedBackDate BETWEEN @From AND @To)/@CityCount * 100 , 0)  AS CityInqNotAnsed,
		ROUND ((SELECT COUNT(DBF.Id) 
				FROM DCRM_BuyerFeedBackNew DBF 
				INNER JOIN Dealers D ON D.ID=DBF.DealerId  
				WHERE CityId=@CityId and DealerResponseSatisfaction = 1 AND FeedBackDate BETWEEN @From AND @To)/@CityCount * 100 , 0)  AS CityCustmerSatified,
		ROUND ((SELECT COUNT(DBF.Id) 
				FROM DCRM_BuyerFeedBackNew DBF 
				INNER JOIN Dealers D ON D.ID=DBF.DealerId  
				WHERE CityId=@CityId and FoundCarInterestedIn <> 1 AND FeedBackDate BETWEEN @From AND @To)/@CityCount * 100 , 0)  AS CityStillLookingForCar
	END
END