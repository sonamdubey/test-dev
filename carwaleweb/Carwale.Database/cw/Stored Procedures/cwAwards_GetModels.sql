IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[cwAwards_GetModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[cwAwards_GetModels]
GO

	
-- =============================================
-- Author:		Vikas
-- Create date: 24-01-2013
-- Description:	To get all models except the deleted ones for a particular make
-- =============================================
CREATE PROCEDURE [cw].[cwAwards_GetModels] 
	-- Add the parameters for the stored procedure here
	@MakeId NUMERIC
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
   
	SELECT ID AS Value, Name AS Text FROM CarModels WHERE IsDeleted = 0 AND Futuristic = 0 and CarMakeId = @MakeId ORDER BY Text
    	
END

