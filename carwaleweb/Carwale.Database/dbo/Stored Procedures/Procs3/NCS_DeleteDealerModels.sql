IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_DeleteDealerModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_DeleteDealerModels]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 22 Nov 2013
-- Description:	to delete dealer models
-- =============================================
CREATE PROCEDURE [dbo].[NCS_DeleteDealerModels]
	-- Add the parameters for the stored procedure here
	@Id VARCHAR(MAX),
	@DeletedBy INT,
	@DeletedOn DATETIME
AS
 BEGIN
		DELETE FROM NCS_DealerModels WHERE Id IN (SELECT LISTMEMBER FROM fnSplitCSV(@Id))
END