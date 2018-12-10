IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ProcessedDealerPhoto]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ProcessedDealerPhoto]
GO

	-- =============================================
-- Author:        <Khushaboo Patil>
-- Create date: <10/06/2015>
-- Description:    <get replicated dealer photo>
-- =============================================
CREATE PROCEDURE [dbo].[TC_ProcessedDealerPhoto]
@Id    INT
AS
BEGIN

    SELECT ProfilePhotoHostUrl AS HostUrl,ProfilePhotoUrl,ProfilePhotoStatusId AS StatusId
    FROM Dealers WHERE ID= @Id
END