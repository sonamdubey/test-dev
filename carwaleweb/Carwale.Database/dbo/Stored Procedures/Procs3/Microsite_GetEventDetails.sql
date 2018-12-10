IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetEventDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetEventDetails]
GO

	-- =============================================
-- Author:		Komal Manjare	
-- Create date: 06-October-2016
-- Description:	get event data for dealer website
-- Modifier -- Kartik Rathod on 17 oct 2016,added @EventId and @ImageId
-- exec [Microsite_GetEventDetails] 6
-- Modified By : Komal Manjare on 27-10-2016 added Getevent further details
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_GetEventDetails] 
@DealerId INT,
@EventId INT = 0,
@ImageId INT = 0
AS
BEGIN

	
	SELECT MDE.Id AS EventId,MDE.EventName,CONVERT(VARCHAR,MDE.EventStartDate,106) AS EventStartDate,MDE.IsActive,MEI.Id  AS Images_ImageId ,MEI.HostUrl AS Images_HostUrl,
			MEI.OriginalImgPath AS Images_OriginalImgPath , MDE.DealerId ,
			CONVERT(VARCHAR,MDE.EventEndDate,106) AS EventEndDate,MDE.EventVenue,MDE.EventDescription
	FROM Microsite_DealerEvents MDE WITH(NOLOCK)
	LEFT JOIN Microsite_EventImages MEI WITH(NOLOCK) ON MDE.Id = MEI.EventId AND MEI.IsActive = 1
	WHERE MDE.DealerId=@DealerId AND (@EventId = 0 OR MDE.Id = @EventId) AND (@ImageId = 0 OR MEI.Id = @ImageId) AND ISNULL(MDE.IsDeleted,0) = 0
	Order by MDE.Id DESC
END




