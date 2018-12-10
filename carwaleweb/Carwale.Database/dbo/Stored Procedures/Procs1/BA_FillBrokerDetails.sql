IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_FillBrokerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_FillBrokerDetails]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_FillBrokerDetails]
	@BrokerReqId INT
AS
BEGIN
	SET NOCOUNT ON;
	select BL.Password AS Pin, BAR.BrokerMobile AS Mobile, BAR.BrokerName AS Name, BAR.Comments AS Comments,BAR.CityId AS CityId, C.Name AS City, BAR.StateId AS StateId,
         S.Name AS State,  BAR.Email AS Email,BAR.DownloadDate AS DoJ, BAR.DateofActivation AS DoA
          from BA_registerBroker AS BAR WITH (NOLOCK)
		  LEFT JOIN BA_Login AS BL WITH (NOLOCK)  ON BL.BrokerID = @BrokerReqId
		   LEFT JOIN States AS S WITH (NOLOCK) ON S.ID = BAR.StateId 
		   LEFT JOIN Cities AS C WITH (NOLOCK) ON C.ID = BAR.CityId 
		   WHERE BAR.ID = @BrokerReqId
END
