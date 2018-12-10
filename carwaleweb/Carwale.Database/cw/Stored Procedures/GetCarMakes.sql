IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarMakes]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <04/09/2012>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [cw].[GetCarMakes] 
	-- Add the parameters for the stored procedure here
	@MakeCond varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@MakeCond='New')	
	SELECT ID AS Value, Name AS Text 
	FROM CarMakes 
	WHERE IsDeleted = 0 
	AND Futuristic = 0 
	AND New = 1 
	ORDER BY Text  
	
	ELSE
	IF (@MakeCond='Used')	
	SELECT Id AS Value,Name AS Text 
	FROM CarMakes 
	WHERE IsDeleted = 0 AND Futuristic = 0 
	AND Used = 1 
	ORDER BY Name
	
	ELSE
	IF (@MakeCond='All')	
	SELECT Id AS Value,Name AS Text 
	FROM CarMakes 
	ORDER BY Name
END
