IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AP_GetExpiredCampaignDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AP_GetExpiredCampaignDetails]
GO

	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <6 th Apr 2015>
-- Description:	<Automated process to get PQ and DN campaign data if it end>
-- =============================================
CREATE PROCEDURE [dbo].[AP_GetExpiredCampaignDetails]
	@Id VARCHAR(MAX)
AS
BEGIN
	UPDATE PQ_DealerSponsored SET IsMailerSent = 0 WHERE TotalGoal <> TotalCount AND IsMailerSent = 1
	IF @Id = '0'
	BEGIN 
		SELECT DISTINCT DS.Id AS CampaignId,ISNULL(D.FirstName,'') + ' ' + ISNULL(D.LastName,'') + '('+ CONVERT(VARCHAR,DS.DealerId) +')'--+', '+ CT.Name 
		AS DealerDetails, CONVERT(VARCHAR(11),DS.EndDate,106)  EndDate, DS.TotalGoal , OU.UserName ,OU.LoginId +'@Carwale.com' AS ExecMailId
		FROM PQ_DealerSponsored DS WITH(NOLOCK)
		INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = DS.DealerId
		INNER JOIN OprUsers OU WITH(NOLOCK) ON OU.Id = DS.UpdatedBy
		WHERE (TotalCount = DS.TotalGoal OR CONVERT(DATE,DS.EndDate) =CONVERT(DATE,GETDATE()-1)) AND IsMailerSent = 0


		SELECT DISTINCT M.Name + '-' +ISNULL(D.FirstName,'') + ' ' + ISNULL(D.LastName,'') + '('+ CONVERT(VARCHAR,DN.TcDealerId) +')'
		+','+ CT.Name AS DealerDetails ,  CONVERT(VARCHAR(11),PackageEndDate ,106) PackageEndDate
		FROM Dealer_NewCar DN WITH(NOLOCK)
		INNER JOIN CarMakes M WITH(NOLOCK) ON M.ID = DN.MakeId
		INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = DN.TcDealerId
		INNER JOIN Cities CT WITH(NOLOCK) ON CT.ID = DN.CityId
		WHERE CONVERT(DATE,PackageEndDate) = CONVERT(DATE,GETDATE()-1)
	END
	ELSE
	BEGIN
		UPDATE PQ_DealerSponsored SET IsMailerSent = 1 WHERE Id in(SELECT ListMember FROM fnSplitCSV(@Id))
	END
END
