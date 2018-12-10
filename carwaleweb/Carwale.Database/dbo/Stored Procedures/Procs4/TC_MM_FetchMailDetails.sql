IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MM_FetchMailDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MM_FetchMailDetails]
GO

	
-- =============================================
-- Author:		Kritika Choudhary
-- Create date: 10-10-2016
-- Description:	Fetch organization name and Mapped l3 emailId
-- exec [TC_MM_FetchMailDetails] 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_MM_FetchMailDetails] 
@DealerId INT,
@L3Mail VARCHAR(20)=NULL OUTPUT,
@Organization VARCHAR(30)=NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;	

	SELECT OU.LoginId + '@carwale.com' AS L3Mail,D.Organization
	FROM DCRM_ADM_UserDealers UD WITH (NOLOCK)
	JOIN OprUsers OU WITH (NOLOCK) ON OU.Id = UD.UserId
	JOIN Dealers D WITH (NOLOCK) ON D.ID = UD.DealerId
	WHERE D.ID = @DealerId and UD.RoleId=3 --sales field for L3
END

