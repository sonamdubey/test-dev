IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateCarIsArchived]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateCarIsArchived]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 20 Nov 2013
-- Description:	Proc to update the isarchived flag in usedcarsellinquiries
-- =============================================
CREATE PROCEDURE UpdateCarIsArchived
	-- Add the parameters for the stored procedure here
	@ProfileId BIGINT	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE CustomerSellInquiries
	SET IsArchived = 1
	WHERE ID = @ProfileId
END

