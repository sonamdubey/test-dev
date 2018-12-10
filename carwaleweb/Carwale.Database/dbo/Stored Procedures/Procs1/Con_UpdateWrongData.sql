IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_UpdateWrongData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_UpdateWrongData]
GO

	-- ===============================================================================
-- Author:		Yuga Hatolkar
-- Create date: 12/08/2014
-- Description:	SP to update the Suggested value (Wrong Content Report System)
-- ===============================================================================
CREATE PROCEDURE [dbo].[Con_UpdateWrongData] 
	-- Add the parameters for the stored procedure here
	@Id INT,
	@SuggestedValue FLOAT,
	@ResolvedBy INT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here
	-- print @SuggestedValue
	UPDATE Con_WrongDataLog SET SuggestedValue = @SuggestedValue, ResolvedOn = GETDATE() , IsResolved =1, ResolvedBy = @ResolvedBy WHERE Id = @Id;
	--print @SuggestedValue
	
	
END

