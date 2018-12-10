IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetCarVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetCarVersions]
GO

	-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <04/09/2012>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [cw].[GetCarVersions] 
	-- Add the parameters for the stored procedure here
	@ModelId SMALLINT,
	@VersionCond varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (@VersionCond='New')	
	SELECT ID AS Value, Name AS Text 
	FROM CarVersions
	WHERE CarModelId = @ModelId AND
	IsDeleted = 0 
	AND Futuristic = 0 
	AND New = 1 
	ORDER BY Text  
	
	ELSE
	IF (@VersionCond='Used')	
	SELECT Id AS Value,Name AS Text 
	FROM CarVersions 
	WHERE  CarModelId = @ModelId AND
	IsDeleted = 0 AND Futuristic = 0 
	AND Used = 1 
	ORDER BY Name
	
	ELSE
	IF (@VersionCond='All')	
	SELECT Id AS Value,Name AS Text 
	FROM CarVersions
	WHERE  CarModelId = @ModelId
	ORDER BY Name
END


