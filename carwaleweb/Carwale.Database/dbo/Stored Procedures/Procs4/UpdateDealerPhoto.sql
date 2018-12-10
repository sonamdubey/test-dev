IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateDealerPhoto]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateDealerPhoto]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 16 Dec 2015
-- Description:	Proc to update image url bsed on imageId for Dealer Microsite photos
-- =============================================
CREATE PROCEDURE [dbo].[UpdateDealerPhoto]
	-- Add the parameters for the stored procedure here
	@Id Int,
	@HostUrl VARCHAR(50),
	@OriginalImgPath VARCHAR(250)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    UPDATE Microsite_Images SET HostUrl = @HostUrl, OriginalImgPath = @OriginalImgPath WHERE ID = @Id

END