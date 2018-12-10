IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMGetNewCarModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMGetNewCarModels]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,19th Nov, 2013>
-- Description:	<Description,Gives all new car Models>
-- =============================================
CREATE PROCEDURE [dbo].[TC_TMGetNewCarModels]
	-- Add the parameters for the stored procedure here
	@MakeId SMALLINT
AS
BEGIN
	SELECT ID AS Value, Name AS Text 
	FROM CarModels WITH (NOLOCK)
	WHERE IsDeleted = 0 
	AND Futuristic = 0 
	AND  CarMakeId =@MakeId 
	AND New = 1 
	ORDER BY Text  	
END
