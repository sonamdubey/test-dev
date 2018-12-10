IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Support_GetTicketHistory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Support_GetTicketHistory]
GO

	-- =============================================
-- Author:		Komal Manjare
-- Create date: 29 August 2016
-- Description:	Get ticket details from log with image details uploaded.
-- =============================================
CREATE PROCEDURE [dbo].[Support_GetTicketHistory]
	@TicketId INT

AS
BEGIN

	 SELECT STL.Comment, STS.Status AS TktStatus,STL.CreatedOn,OU.UserName AS CreatedBy,
	 STL.Id AS TicketLogId,SAFD.HostUrl,SAFD.OriginalImgPath
     FROM Support_TicketLog STL(NOLOCK) 
     INNER JOIN Support_TicketStatus STS (NOLOCK) ON STS.Id=STL.Status 
     INNER JOIN OprUsers OU (NOLOCK) ON OU.Id = STL.CreatedBy AND OU.IsActive=1
	 LEFT JOIN Support_AttachedFileDetails SAFD(NOLOCK) ON SAFD.TicketLogId=STL.Id
     WHERE TicketId =@TicketId
END

